using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public enum DisassembleMode { 水平, 垂直, 爆炸 }

    [Header("拆解设置")]
    [Tooltip("控制拆解时的扩散距离")]
    public float disassembleAmount = 1f;

    [Tooltip("拆解模式：\n水平 - 水平方向扩散\n垂直 - 垂直方向堆叠\n爆炸 - 中心爆炸扩散")]
    public DisassembleMode currentMode = DisassembleMode.水平;

    [Tooltip("拆解/缩放时的参考中心点")]
    public Transform referencePoint;

    [Header("物理设置")]
    [Tooltip("是否启用重力")]
    public bool useGravity = true;

    [Tooltip("是否启用碰撞体")]
    public bool enableColliders = true;

    [Tooltip("碰撞体是否作为触发器")]
    public bool collidersAsTriggers = false;

    [Header("Disassembly Fine-Tuning Settings")]
    [Tooltip("物体之间基础偏移（乘以与参考点的距离比例）")]
    [SerializeField] private float distanceMultiplier = 1f;

    [Tooltip("拆解初始偏移量（所有物体统一）")]
    [SerializeField] private float initialOffset = 0f;

    [Tooltip("最大影响距离（超出该值的物体按最大值处理）")]
    [SerializeField] private float maxEffectiveDistance = 10f;

    [Header("Custom Disassembly Settings")]
    [Tooltip("自定义每个方向的拆解倍率 (X,Y,Z)")]
    [SerializeField] private Vector3 customDisassembleMultiplier = Vector3.one;

    [Header("Disassembly Speed Settings")]
    [Tooltip("拆解动画的速度 (单位: 量/秒)")]
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

    [ContextMenu("自定义拆解")]
    public void CustomDisassemble()
    {
        // 先还原建筑
        ReassembleBuilding();

        currentMode = DisassembleMode.水平; // 自定义其实单独逻辑，不影响
        isDisassembled = true;
        currentDisassembleAmount = 0f;
        SetGravity(useGravity);
    }

    [ContextMenu("将建筑放大")]
    public void ScaleUpBuilding()
    {
        GetComponentInParent<Transform>().localScale += Vector3.one * 0.1f;
    }


    [ContextMenu("初始化建筑")]
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

    [ContextMenu("清除所有组件")]
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
                meshCollider.isTrigger = false; // 必须先取消触发器
                meshCollider.convex = false;    // 然后取消凸面
            }
        }

        buildingItems.Clear();
        originalPositions.Clear();
        originalRotations.Clear();
        originalScales.Clear();
        isDisassembled = false;
    }


    [ContextMenu("水平拆解")]
    public void DisassembleHorizontal() => DisassembleBuilding(DisassembleMode.水平);

    [ContextMenu("垂直拆解")]
    public void DisassembleVertical() => DisassembleBuilding(DisassembleMode.垂直);

    [ContextMenu("爆炸式拆解")]
    public void DisassembleExplosion() => DisassembleBuilding(DisassembleMode.爆炸);

    public void DisassembleBuilding(DisassembleMode mode)
    {
        // 先还原建筑
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

            // 判断是否是自定义拆解
            if (currentMode == DisassembleMode.水平 && customDisassembleMultiplier != Vector3.one)
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
            case DisassembleMode.水平:
                return (buildingItems[index].transform.position - center).normalized.WithY(0);
            case DisassembleMode.垂直:
                return Vector3.up;
            case DisassembleMode.爆炸:
                return (buildingItems[index].transform.position - center).normalized;
            default:
                return Vector3.zero;
        }
    }

    [ContextMenu("还原建筑")]
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

    [ContextMenu("启用碰撞体")]
    public void EnableColliders() => SetCollidersEnabled(true);

    [ContextMenu("禁用碰撞体")]
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

    [ContextMenu("设为触发器")]
    public void SetCollidersAsTriggers() => SetCollidersTrigger(true);

    [ContextMenu("取消触发器")]
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

    [ContextMenu("启用重力")]
    public void EnableGravity() => SetGravity(true);

    [ContextMenu("禁用重力")]
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
