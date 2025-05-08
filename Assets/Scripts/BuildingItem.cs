using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItem : MonoBehaviour
{
    private Vector3 defaultPosition; // 结构块的默认位置
    private bool isDisassembled = false; // 是否处于拆解状态

    private void Start()
    {
        // 初始化时记录默认位置
        defaultPosition = transform.position;
    }

    /// <summary>
    /// 为外部调用提供接口，负责控制物块的移动方向
    /// </summary>
    /// <param name="targetPosition">目标三维坐标</param>
    public void MoveTo(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        isDisassembled = true; // 移动后标记为拆解状态
    }

    /// <summary>
    /// 如果处于拆解状态，就将物块返回到结构的默认位置
    /// </summary>
    public void TurnBack()
    {
        if (isDisassembled)
        {
            MoveTo(defaultPosition);
            isDisassembled = false; // 重置拆解状态
        }
    }
}
