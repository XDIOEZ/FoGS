using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public enum DisassembleMode { ˮƽ, ��ֱ, ��ը }

    [Header("�������")]
    [Tooltip("���Ʋ��ʱ����ɢ����")]
    public float disassembleAmount = 1f;

    [Tooltip("���ģʽ��\nˮƽ - ˮƽ������ɢ\n��ֱ - ��ֱ����ѵ�\n��ը - ���ı�ը��ɢ")]
    public DisassembleMode currentMode = DisassembleMode.ˮƽ;

    [Tooltip("���/����ʱ�Ĳο����ĵ�")]
    public Transform referencePoint;

    [Header("��������")]
    [Tooltip("�Ƿ���������")]
    public bool useGravity = true;

    [Tooltip("�Ƿ�������ײ��")]
    public bool enableColliders = true;

    [Tooltip("��ײ���Ƿ���Ϊ������")]
    public bool collidersAsTriggers = false;

    [Header("Disassembly Fine-Tuning Settings")]
    [Tooltip("����֮�����ƫ�ƣ�������ο���ľ��������")]
    [SerializeField] private float distanceMultiplier = 1f;

    [Tooltip("����ʼƫ��������������ͳһ��")]
    [SerializeField] private float initialOffset = 0f;

    [Tooltip("���Ӱ����루������ֵ�����尴���ֵ����")]
    [SerializeField] private float maxEffectiveDistance = 10f;

    [Header("Custom Disassembly Settings")]
    [Tooltip("�Զ���ÿ������Ĳ�ⱶ�� (X,Y,Z)")]
    [SerializeField] private Vector3 customDisassembleMultiplier = Vector3.one;

    [Header("Disassembly Speed Settings")]
    [Tooltip("��⶯�����ٶ� (��λ: ��/��)")]
    [SerializeField] private float disassembleSpeed = 1f;





    private List<BuildingItem> buildingItems = new List<BuildingItem>();
    private List<Vector3> originalPositions = new List<Vector3>();
    private List<Quaternion> originalRotations = new List<Quaternion>();
    private List<Vector3> originalScales = new List<Vector3>();
    private bool isDisassembled = false;
    private float currentDisassembleAmount = 0f;

    void Start()
    {
        InitializeBuilding();
    }

    void Update()
    {
        if (isDisassembled)
        {
            UpdateDisassemble();
        }
    }

    [ContextMenu("�Զ�����")]
    public void CustomDisassemble()
    {
        // �Ȼ�ԭ����
        ReassembleBuilding();

        currentMode = DisassembleMode.ˮƽ; // �Զ�����ʵ�����߼�����Ӱ��
        isDisassembled = true;
        currentDisassembleAmount = 0f;
        SetGravity(useGravity);
    }

    [ContextMenu("�������Ŵ�")]
    public void ScaleUpBuilding()
    {
        GetComponentInParent<Transform>().localScale += Vector3.one * 0.1f;
    }


    [ContextMenu("��ʼ������")]
    private void InitializeBuilding()
    {
        ClearAllComponents();

        buildingItems.Clear();
        originalPositions.Clear();
        originalRotations.Clear();
        originalScales.Clear();

        var children = GetComponentsInChildren<Transform>(true);

        foreach (var child in children)
        {
            if (child == transform) continue;

            if (!child.TryGetComponent<MeshRenderer>(out var renderer)) continue;

            var item = child.GetComponent<BuildingItem>();
            if (item == null) item = child.gameObject.AddComponent<BuildingItem>();
            buildingItems.Add(item);

            if (child.childCount == 0)
            {
                if (!child.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb = child.gameObject.AddComponent<Rigidbody>();
                }
                rb.useGravity = useGravity;
                rb.isKinematic = !useGravity;
            }

            if (!child.TryGetComponent<Collider>(out var collider))
            {
                var meshCollider = child.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.enabled = enableColliders;
                meshCollider.isTrigger = collidersAsTriggers;
            }
            else if (collider is MeshCollider meshCollider)
            {
                meshCollider.convex = true;
                meshCollider.enabled = enableColliders;
                meshCollider.isTrigger = collidersAsTriggers;
            }

            originalPositions.Add(child.transform.position);
            originalRotations.Add(child.transform.rotation);
            originalScales.Add(child.transform.localScale);
        }
    }

    [ContextMenu("����������")]
    public void ClearAllComponents()
    {
        var children = GetComponentsInChildren<Transform>(true);

        foreach (var child in children)
        {
            if (child == transform) continue;

            if (child.TryGetComponent<Rigidbody>(out var rb))
            {
                DestroyImmediate(rb);
            }

            if (child.TryGetComponent<BuildingItem>(out var item))
            {
                DestroyImmediate(item);
            }

            if (child.TryGetComponent<MeshCollider>(out var meshCollider))
            {
                meshCollider.isTrigger = false; // ������ȡ��������
                meshCollider.convex = false;    // Ȼ��ȡ��͹��
            }
        }

        buildingItems.Clear();
        originalPositions.Clear();
        originalRotations.Clear();
        originalScales.Clear();
        isDisassembled = false;
    }


    [ContextMenu("ˮƽ���")]
    public void DisassembleHorizontal() => DisassembleBuilding(DisassembleMode.ˮƽ);

    [ContextMenu("��ֱ���")]
    public void DisassembleVertical() => DisassembleBuilding(DisassembleMode.��ֱ);

    [ContextMenu("��ըʽ���")]
    public void DisassembleExplosion() => DisassembleBuilding(DisassembleMode.��ը);

    public void DisassembleBuilding(DisassembleMode mode)
    {
        // �Ȼ�ԭ����
        ReassembleBuilding();

        currentMode = mode;
        isDisassembled = true;
        currentDisassembleAmount = 0f;
        SetGravity(useGravity);
    }


    private void UpdateDisassemble()
    {
        if (currentDisassembleAmount >= disassembleAmount) return;

        currentDisassembleAmount = Mathf.Min(currentDisassembleAmount + Time.deltaTime * disassembleSpeed, disassembleAmount);
        Vector3 center = referencePoint != null ? referencePoint.position : transform.position;

        for (int i = 0; i < buildingItems.Count; i++)
        {
            var item = buildingItems[i];
            Vector3 direction = GetDisassembleDirection(i, center);

            float distanceToCenter = Vector3.Distance(originalPositions[i], center);
            float normalizedDistance = Mathf.Clamp01(distanceToCenter / maxEffectiveDistance);

            // �ж��Ƿ����Զ�����
            if (currentMode == DisassembleMode.ˮƽ && customDisassembleMultiplier != Vector3.one)
            {
                Vector3 customDirection = Vector3.Scale(direction.normalized, customDisassembleMultiplier.normalized);
                Vector3 targetPosition = originalPositions[i] + customDirection * (initialOffset + normalizedDistance * distanceMultiplier);

                item.transform.position = Vector3.Lerp(
                    originalPositions[i],
                    targetPosition,
                    currentDisassembleAmount / disassembleAmount
                );
            }
            else
            {
                Vector3 targetPosition = originalPositions[i] + direction.normalized * (initialOffset + normalizedDistance * distanceMultiplier);

                item.transform.position = Vector3.Lerp(
                    originalPositions[i],
                    targetPosition,
                    currentDisassembleAmount / disassembleAmount
                );
            }
        }
    }





    private Vector3 GetDisassembleDirection(int index, Vector3 center)
    {
        switch (currentMode)
        {
            case DisassembleMode.ˮƽ:
                return (buildingItems[index].transform.position - center).normalized.WithY(0);
            case DisassembleMode.��ֱ:
                return Vector3.up;
            case DisassembleMode.��ը:
                return (buildingItems[index].transform.position - center).normalized;
            default:
                return Vector3.zero;
        }
    }

    [ContextMenu("��ԭ����")]
    public void ReassembleBuilding()
    {
        if (!isDisassembled) return;

        isDisassembled = false;
        SetGravity(false);

        for (int i = 0; i < buildingItems.Count; i++)
        {
            buildingItems[i].transform.position = originalPositions[i];
            buildingItems[i].transform.rotation = originalRotations[i];
            buildingItems[i].transform.localScale = originalScales[i];
        }
    }

    [ContextMenu("������ײ��")]
    public void EnableColliders() => SetCollidersEnabled(true);

    [ContextMenu("������ײ��")]
    public void DisableColliders() => SetCollidersEnabled(false);

    public void SetCollidersEnabled(bool enabled)
    {
        enableColliders = enabled;
        foreach (var item in buildingItems)
        {
            if (item.TryGetComponent<Collider>(out var collider))
            {
                collider.enabled = enabled;
            }
        }
    }

    [ContextMenu("��Ϊ������")]
    public void SetCollidersAsTriggers() => SetCollidersTrigger(true);

    [ContextMenu("ȡ��������")]
    public void SetCollidersNotTriggers() => SetCollidersTrigger(false);

    public void SetCollidersTrigger(bool isTrigger)
    {
        collidersAsTriggers = isTrigger;
        foreach (var item in buildingItems)
        {
            if (item.TryGetComponent<Collider>(out var collider))
            {
                collider.isTrigger = isTrigger;
            }
        }
    }

    [ContextMenu("��������")]
    public void EnableGravity() => SetGravity(true);

    [ContextMenu("��������")]
    public void DisableGravity() => SetGravity(false);

    public void SetGravity(bool enabled)
    {
        useGravity = enabled;
        foreach (var item in buildingItems)
        {
            if (item.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = enabled;
                rb.isKinematic = !enabled;
            }
        }
    }
}

public static class Vector3Extensions
{
    public static Vector3 WithY(this Vector3 vector, float y) => new Vector3(vector.x, y, vector.z);
}
