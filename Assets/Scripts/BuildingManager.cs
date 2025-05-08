using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public enum DisassembleMode { 水平, 垂直, 爆炸 }

    [Header("拆解设置")]
    [Tooltip("全局拆解距离系数")]
    public float disassembleAmount = 1f;

    [Tooltip("全局额外方向偏移，用于整体结构块的附加移动方向")]
    public Vector3 extraGlobalDirection = Vector3.zero;

    [Tooltip("是否对拆解方向进行归一化处理")]
    public bool normalizeDirection = true;

    [Tooltip("拆解模式")]
    public DisassembleMode currentMode = DisassembleMode.水平;

    [Tooltip("拆解/还原时的动画时长（会覆盖各 BuildingItem 的默认值）")]
    public float animationDuration = 0.5f;

    [Tooltip("参考中心点（为空则以本物体中心为准）")]
    public Transform referencePoint;

    private List<BuildingItem> buildingItems = new List<BuildingItem>();

    private void Start()
    {
        // 拉取所有子物块
        buildingItems.AddRange(GetComponentsInChildren<BuildingItem>());
    }

    /// <summary>
    /// 一次性触发拆解
    /// </summary>
    public void DisassembleBuilding()
    {
        Vector3 center = referencePoint != null
                       ? referencePoint.position
                       : transform.position;

        foreach (var item in buildingItems)
        {
            // 同步动画时长和归一化设置
            item.animationDuration = animationDuration;
            item.normalizeDirection = normalizeDirection;

            // 计算基础方向向量
            Vector3 dir = GetDirection(item.transform.position, center);

            // 合成全局额外偏移
            Vector3 combinedDir = dir + extraGlobalDirection;

            // 调用拆解
            item.Disassemble(combinedDir, disassembleAmount);
        }
    }

    /// <summary>
    /// 一次性触发还原
    /// </summary>
    public void ReassembleBuilding()
    {
        foreach (var item in buildingItems)
        {
            item.animationDuration = animationDuration;
            item.normalizeDirection = normalizeDirection;
            item.Reassemble();
        }
    }

    /// <summary>
    /// 根据拆解模式计算拆解方向
    /// 水平: XY面疏散, 垂直: 按Y轴拉伸, 爆炸: 全向散开
    /// </summary>
    private Vector3 GetDirection(Vector3 itemPos, Vector3 center)
    {
        switch (currentMode)
        {
            case DisassembleMode.水平:
                var dirH = itemPos - center;
                dirH.y = 0;
                return dirH;
            case DisassembleMode.垂直:
                // 垂直拉伸：根据与中心的Y差值拉伸方向
                float deltaY = itemPos.y - center.y;
                return new Vector3(0, deltaY, 0);
            case DisassembleMode.爆炸:
                return (itemPos - center);
            default:
                return Vector3.zero;
        }
    }

    // --- Inspector 右键测试 ---
    [ContextMenu("拆解")]
    private void TestDisassemble() => DisassembleBuilding();

    [ContextMenu("还原")]
    private void TestReassemble() => ReassembleBuilding();
}
