using UnityEngine;
using DG.Tweening;

public class BuildingItem : MonoBehaviour
{
    [Header("物品信息")]
    public ItemData itemData;
    [Header("微调参数")]
    [Tooltip("影响当前块拆解距离的独立系数")]
    public float disassembleMultiplier = 1f;

    [Tooltip("额外的方向偏移，用于定制本块拆解方向")]
    public Vector3 extraDisassembleDirection = Vector3.zero;

    [Header("全局设置")]
    [Tooltip("是否对拆解方向进行归一化处理（由 Manager 控制）")]
    public bool normalizeDirection = true;

    [Header("动画参数")]
    [Tooltip("拆解/还原动画时长")]
    public float animationDuration = 0.5f;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private Vector3 defaultScale;
    private bool isDisassembled = false;

    private void Awake()
    {
        // 记录初始状态
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;
        Debug.Log("000");
    }

    [ContextMenu("询问AI")]
    public void AskAI()
    {
       
        // 通过 Tag 获取标记为 "AISample" 的 GameObject
        GameObject aiSample = GameObject.FindGameObjectWithTag("AIChat");
      
        if (aiSample != null)
        {
            Debug.Log("找到 AISample： " + aiSample.name);
            // 你可以在这里对这个 GameObject 做进一步操作，例如调用它的某个方法
            aiSample.GetComponent<PreInput>().Show();
            aiSample.GetComponent<PreInput>().inputString = itemData.name+ " 是什么?";
            aiSample.GetComponent<PreInput>().SyncInput();
            aiSample.GetComponent<PreInput>().SendMessage();
        }
        else
        {
            Debug.LogWarning("未找到 Tag 为 AISample 的对象");
        }
    }


public void Start()
    {
        if (itemData.name == "")
            itemData.name = gameObject.name;
    }

    /// <summary>
    /// 执行拆解动画
    /// </summary>
    public void Disassemble(Vector3 globalDirection, float globalAmount)
    {
        if (isDisassembled) return;

        // 合并偏移方向
        Vector3 combinedDir = globalDirection + extraDisassembleDirection;
        if (normalizeDirection)
            combinedDir = combinedDir.normalized;

        // 目标位置 = 初始位置 + 方向向量 * 全局系数 * 本块系数
        Vector3 targetPos = defaultPosition + combinedDir * (globalAmount * disassembleMultiplier);

        // DOTween 平滑移动/旋转/缩放
        transform.DOMove(targetPos, animationDuration).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(defaultRotation, animationDuration).SetEase(Ease.InOutSine);
        transform.DOScale(defaultScale, animationDuration).SetEase(Ease.InOutSine);

        isDisassembled = true;
    }

    /// <summary>
    /// 用动画把块送回初始状态
    /// </summary>
    public void Reassemble()
    {
        if (!isDisassembled) return;

        transform.DOMove(defaultPosition, animationDuration).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(defaultRotation, animationDuration).SetEase(Ease.InOutSine);
        transform.DOScale(defaultScale, animationDuration)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(() => isDisassembled = false);
    }
}
[System.Serializable]
public class ItemData
{
    // 物品名称
    public string name;
    // 物品描述
    [TextArea]
    public string description;
}
