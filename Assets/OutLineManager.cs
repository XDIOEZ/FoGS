using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineManager : MonoBehaviour
{
    public static OutLineManager Instance { get; private set; }

    public List<m_Outline> outlines = new List<m_Outline>();

    private void Awake()
    {
        // 检查重复实例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 🔥 如果需要跨场景保留，取消注释
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        outlines.Clear();
        outlines.AddRange(GetComponentsInChildren<m_Outline>(true));

        // 初始化所有轮廓为关闭状态
        foreach (var outline in outlines)
        {
            if (outline != null)
                outline.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    

    [Header("光标设置")]
    [Tooltip("是否自动隐藏轮廓（默认false，点击其他物体才消失）")]
    public bool autoHide = false;

    [Tooltip("自动隐藏的延迟时间(秒)，autoHide为true时有效")]
    public float autoHideDelay = 3f;

    private m_Outline currentActiveOutline = null;

    
   

    /// <summary>
    /// 显示指定物体的轮廓（点击切换模式）
    /// </summary>
    public void ShowOutline(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("[OutLineManager] 目标物体为空");
            return;
        }

        // 获取目标的 m_Outline 组件
        m_Outline targetOutline = target.GetComponent<m_Outline>();

        // 如果目标本身没有，尝试在子物体中查找
        if (targetOutline == null)
        {
            targetOutline = target.GetComponentInChildren<m_Outline>();
        }

        if (targetOutline == null)
        {
            Debug.LogWarning($"[OutLineManager] {target.name} 没有 m_Outline 组件");
            return;
        }

        // 🔥 关键：如果点击的是当前已激活的物体，不做任何操作
        if (currentActiveOutline == targetOutline)
        {
            Debug.Log($"[OutLineManager] 已激活的物体，忽略: {targetOutline.gameObject.name}");
            return;
        }

        // 🔥 关键：如果已经有激活的轮廓，先隐藏它
        if (currentActiveOutline != null)
        {
            currentActiveOutline.enabled = false;
            Debug.Log($"[OutLineManager] 隐藏之前的轮廓: {currentActiveOutline.gameObject.name}");
        }

        // 激活新轮廓
        currentActiveOutline = targetOutline;
        currentActiveOutline.enabled = true;
        Debug.Log($"[OutLineManager] 显示新轮廓: {targetOutline.gameObject.name}");

        // 如果启用了自动隐藏，启动协程
        if (autoHide && autoHideDelay > 0)
        {
            StartCoroutine(HideOutlineAfterDelay());
        }
    }

    /// <summary>
    /// 强制隐藏所有轮廓
    /// </summary>
    public void HideAllOutlines()
    {
        Debug.Log("[OutLineManager] 强制隐藏所有轮廓");

        if (currentActiveOutline != null)
        {
            currentActiveOutline.enabled = false;
            currentActiveOutline = null;
        }
    }

    /// <summary>
    /// 协程：延迟隐藏轮廓
    /// </summary>
    private IEnumerator HideOutlineAfterDelay()
    {
        yield return new WaitForSeconds(autoHideDelay);

        if (currentActiveOutline != null)
        {
            Debug.Log($"[OutLineManager] 自动隐藏轮廓: {currentActiveOutline.gameObject.name}");
            currentActiveOutline.enabled = false;
            currentActiveOutline = null;
        }
    }

    /// <summary>
    /// 获取当前激活的轮廓
    /// </summary>
    public m_Outline GetCurrentActiveOutline()
    {
        return currentActiveOutline;
    }
}