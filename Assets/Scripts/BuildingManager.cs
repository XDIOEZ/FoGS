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
    public enum DisassembleMode { 水平, 垂直, 爆炸 }

    // 新增状态枚举
    public enum BuildingState { 空闲, 拆解中, 还原中 }

    [Header("拆解设置")]
    public float disassembleAmount = 1f;
    public Vector3 extraGlobalDirection = Vector3.zero;
    public bool normalizeDirection = true;
    public DisassembleMode currentMode = DisassembleMode.水平;

    public enum ResetMode { 直接, 抛物线 }

    public ResetMode resetMode = ResetMode.直接;
    [Header("动画时长设置")]
    public float disassembleDuration = 0.1f;
    public float reassembleDuration = 0.1f;
    public float 定点还原时间间隔 = 0.1f;

    public Transform referencePoint;

    private List<BuildingItem> buildingItems = new List<BuildingItem>();

    // 当前状态
    public static BuildingState currentState = BuildingState.空闲;

    private void Start()
    {
        buildingItems.AddRange(GetComponentsInChildren<BuildingItem>());
    }

    [ContextMenu("拆解")]
    public void DisassembleBuilding()
    {
        if (currentState != BuildingState.空闲)
        {
            Debug.LogWarning("当前正在进行还原或拆解，无法再次拆解！");
            return;
        }

        currentState = BuildingState.拆解中;

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

        // 设定一个定时器，在拆解动画结束后自动切回空闲状态
        StartCoroutine(ResetStateAfterDelay(disassembleDuration));
    }

    [ContextMenu("还原")]
    public void ReassembleBuilding()
    {
        if (currentState != BuildingState.空闲)
        {
            Debug.LogWarning("当前正在进行还原或拆解，无法再次还原！");
            return;
        }

        currentState = BuildingState.还原中;

        float time = 0f;
        foreach (var item in buildingItems)
        {
            item.animationDuration = reassembleDuration;
            item.normalizeDirection = normalizeDirection;
            item.Reassemble(time += 0.02f);
        }

        StartCoroutine(ResetStateAfterDelay(reassembleDuration + time));
    }

    // 其他还原协程也需要类似处理
    [ContextMenu("定点还原")]
    public void ReassembleBuildingFixedPoint()
    {
        if (currentState != BuildingState.空闲)
        {
            Debug.LogWarning("当前正在进行还原或拆解，无法再次还原！");
            return;
        }

        currentState = BuildingState.还原中;

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
          

            if (resetMode == ResetMode.抛物线)
            {
                item.ReassembleWithArc(reassembleDuration);
            }
            else if (resetMode == ResetMode.直接)
            {
                item.Reassemble(reassembleDuration);

            }

            yield return new WaitForSeconds(定点还原时间间隔);
        }

        currentState = BuildingState.空闲;
    }

    private IEnumerator ResetStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentState = BuildingState.空闲;
    }

    private Vector3 GetDirection(Vector3 itemPos, Vector3 center)
    {
        switch (currentMode)
        {
            case DisassembleMode.水平:
                var dirH = itemPos - center;
                dirH.y = 0;
                return dirH;
            case DisassembleMode.垂直:
                float deltaY = itemPos.y - center.y;
                return new Vector3(0, deltaY, 0);
            case DisassembleMode.爆炸:
                return (itemPos - center);
            default:
                return Vector3.zero;
        }
    }


    // 初始化子对象上的组件
    [ContextMenu("初始化子对象组件")]
    public void ItemComponentInit()
    {
        // 获取所有子对象的 Transform
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            // 排除自身
            if (child == transform) continue;

            // 检测是否已有 BuildingItem 组件
            if (child.GetComponent<BuildingItem>() == null)
            {
                // 没有则添加组件
                child.gameObject.AddComponent<BuildingItem>();
            }

            if (child.GetComponent<ItemShader>() == null)
            {
                // 没有则添加组件
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
