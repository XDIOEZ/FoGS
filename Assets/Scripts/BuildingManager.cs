using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    public static BuildingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public enum DisassembleMode { ˮƽ, ��ֱ, ��ը }

    // ����״̬ö��
    public enum BuildingState { ����, �����, ��ԭ�� }

    [Header("�������")]
    public float disassembleAmount = 1f;
    public Vector3 extraGlobalDirection = Vector3.zero;
    public bool normalizeDirection = true;
    public DisassembleMode currentMode = DisassembleMode.ˮƽ;

    public enum ResetMode { ֱ��, ������ }

    public ResetMode resetMode = ResetMode.ֱ��;
    [Header("����ʱ������")]
    public float disassembleDuration = 0.1f;
    public float reassembleDuration = 0.1f;
    public float ���㻹ԭʱ���� = 0.1f;

    public Transform referencePoint;

    private List<BuildingItem> buildingItems = new List<BuildingItem>();

    // ��ǰ״̬
    public static BuildingState currentState = BuildingState.����;

    private void Start()
    {
        buildingItems.AddRange(GetComponentsInChildren<BuildingItem>());
    }

    [ContextMenu("���")]
    public void DisassembleBuilding()
    {
        if (currentState != BuildingState.����)
        {
            Debug.LogWarning("��ǰ���ڽ��л�ԭ���⣬�޷��ٴβ�⣡");
            return;
        }

        currentState = BuildingState.�����;

        Vector3 center = referencePoint != null
                       ? referencePoint.position
                       : transform.position;

        foreach (var item in buildingItems)
        {
            item.animationDuration = disassembleDuration;
            item.normalizeDirection = normalizeDirection;

            Vector3 dir = GetDirection(item.transform.position, center);
            Vector3 combinedDir = dir + extraGlobalDirection;

            item.Disassemble(combinedDir, disassembleAmount);
        }

        // �趨һ����ʱ�����ڲ�⶯���������Զ��лؿ���״̬
        StartCoroutine(ResetStateAfterDelay(disassembleDuration));
    }

    [ContextMenu("��ԭ")]
    public void ReassembleBuilding()
    {
        if (currentState != BuildingState.����)
        {
            Debug.LogWarning("��ǰ���ڽ��л�ԭ���⣬�޷��ٴλ�ԭ��");
            return;
        }

        currentState = BuildingState.��ԭ��;

        float time = 0f;
        foreach (var item in buildingItems)
        {
            item.animationDuration = reassembleDuration;
            item.normalizeDirection = normalizeDirection;
            item.Reassemble(time += 0.02f);
        }

        StartCoroutine(ResetStateAfterDelay(reassembleDuration + time));
    }

    // ������ԭЭ��Ҳ��Ҫ���ƴ���
    [ContextMenu("���㻹ԭ")]
    public void ReassembleBuildingFixedPoint()
    {
        if (currentState != BuildingState.����)
        {
            Debug.LogWarning("��ǰ���ڽ��л�ԭ���⣬�޷��ٴλ�ԭ��");
            return;
        }

        currentState = BuildingState.��ԭ��;

        StopAllCoroutines();

        buildingItems = buildingItems.OrderBy(item => item.transform.position.y).ToList();


        StartCoroutine(ReassembleStepByStepWithState());
    }

    private IEnumerator ReassembleStepByStepWithState()
    {
        foreach (var item in buildingItems)
        {
            item.animationDuration = reassembleDuration;
            item.normalizeDirection = normalizeDirection;
          

            if (resetMode == ResetMode.������)
            {
                item.ReassembleWithArc(reassembleDuration);
            }
            else if (resetMode == ResetMode.ֱ��)
            {
                item.Reassemble(reassembleDuration);

            }

            yield return new WaitForSeconds(���㻹ԭʱ����);
        }

        currentState = BuildingState.����;
    }

    private IEnumerator ResetStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentState = BuildingState.����;
    }

    private Vector3 GetDirection(Vector3 itemPos, Vector3 center)
    {
        switch (currentMode)
        {
            case DisassembleMode.ˮƽ:
                var dirH = itemPos - center;
                dirH.y = 0;
                return dirH;
            case DisassembleMode.��ֱ:
                float deltaY = itemPos.y - center.y;
                return new Vector3(0, deltaY, 0);
            case DisassembleMode.��ը:
                return (itemPos - center);
            default:
                return Vector3.zero;
        }
    }


    // ��ʼ���Ӷ����ϵ����
    [ContextMenu("��ʼ���Ӷ������")]
    public void ItemComponentInit()
    {
        // ��ȡ�����Ӷ���� Transform
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            // �ų�����
            if (child == transform) continue;

            // ����Ƿ����� BuildingItem ���
            if (child.GetComponent<BuildingItem>() == null)
            {
                // û����������
                child.gameObject.AddComponent<BuildingItem>();
            }

            if (child.GetComponent<ItemShader>() == null)
            {
                // û����������
                child.gameObject.AddComponent<ItemShader>();
            }

            if(child.GetComponent<m_Outline>() == null)
            {
                child.gameObject.AddComponent<m_Outline>();
            }

            if (child.GetComponent<MeshCollider>() == null)
            {
                child.gameObject.AddComponent<MeshCollider>();
            }

            if (child.GetComponent<ShowItemInfo>() == null)
            {
                child.gameObject.AddComponent<ShowItemInfo>();
            }
        }
    }

    public void ShowOne(GameObject obj)
    {
        for (int i = 0; i < buildingItems.Count; i++)
        {
            if (buildingItems[i].gameObject == obj)
            {
                buildingItems[i].gameObject.SetActive(true);
            }
        }
    }


    public void ShowAll()
    {
        for (int i = 0; i < buildingItems.Count; i++)
        {
           
            
                buildingItems[i].gameObject.SetActive(true);
            
        }
    }


    public void HideAll()
    {
        for (int i = 0; i < buildingItems.Count; i++)
        {
            buildingItems[i].gameObject.SetActive(false);
        }
    }

}
