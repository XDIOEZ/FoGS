using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public enum DisassembleMode { ˮƽ, ��ֱ, ��ը }

    [Header("�������")]
    [Tooltip("ȫ�ֲ�����ϵ��")]
    public float disassembleAmount = 1f;

    [Tooltip("ȫ�ֶ��ⷽ��ƫ�ƣ���������ṹ��ĸ����ƶ�����")]
    public Vector3 extraGlobalDirection = Vector3.zero;

    [Tooltip("�Ƿ�Բ�ⷽ����й�һ������")]
    public bool normalizeDirection = true;

    [Tooltip("���ģʽ")]
    public DisassembleMode currentMode = DisassembleMode.ˮƽ;

    [Tooltip("���/��ԭʱ�Ķ���ʱ�����Ḳ�Ǹ� BuildingItem ��Ĭ��ֵ��")]
    public float animationDuration = 0.5f;

    [Tooltip("�ο����ĵ㣨Ϊ�����Ա���������Ϊ׼��")]
    public Transform referencePoint;

    private List<BuildingItem> buildingItems = new List<BuildingItem>();

    private void Start()
    {
        // ��ȡ���������
        buildingItems.AddRange(GetComponentsInChildren<BuildingItem>());
    }

    /// <summary>
    /// һ���Դ������
    /// </summary>
    public void DisassembleBuilding()
    {
        Vector3 center = referencePoint != null
                       ? referencePoint.position
                       : transform.position;

        foreach (var item in buildingItems)
        {
            // ͬ������ʱ���͹�һ������
            item.animationDuration = animationDuration;
            item.normalizeDirection = normalizeDirection;

            // ���������������
            Vector3 dir = GetDirection(item.transform.position, center);

            // �ϳ�ȫ�ֶ���ƫ��
            Vector3 combinedDir = dir + extraGlobalDirection;

            // ���ò��
            item.Disassemble(combinedDir, disassembleAmount);
        }
    }

    /// <summary>
    /// һ���Դ�����ԭ
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
    /// ���ݲ��ģʽ�����ⷽ��
    /// ˮƽ: XY����ɢ, ��ֱ: ��Y������, ��ը: ȫ��ɢ��
    /// </summary>
    private Vector3 GetDirection(Vector3 itemPos, Vector3 center)
    {
        switch (currentMode)
        {
            case DisassembleMode.ˮƽ:
                var dirH = itemPos - center;
                dirH.y = 0;
                return dirH;
            case DisassembleMode.��ֱ:
                // ��ֱ���죺���������ĵ�Y��ֵ���췽��
                float deltaY = itemPos.y - center.y;
                return new Vector3(0, deltaY, 0);
            case DisassembleMode.��ը:
                return (itemPos - center);
            default:
                return Vector3.zero;
        }
    }

    // --- Inspector �Ҽ����� ---
    [ContextMenu("���")]
    private void TestDisassemble() => DisassembleBuilding();

    [ContextMenu("��ԭ")]
    private void TestReassemble() => ReassembleBuilding();
}
