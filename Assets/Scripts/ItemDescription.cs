using UnityEngine;

public class ItemDescription : MonoBehaviour
{
    [TextArea(3, 10)]
    public string description = "这是一个物体的描述信息...";

    // 可选：添加标题
    public string title = "物品名称";
}