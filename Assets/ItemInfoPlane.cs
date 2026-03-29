using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPlane : MonoBehaviour
{
    private static ItemInfoPlane _instance;

    public static ItemInfoPlane instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemInfoPlane>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 如果有旧实例，先销毁它
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;

        ChatWithAI.onClick.AddListener(ChatWithAIOnClick);
        CloseButton.onClick.AddListener(Hide);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    public BuildingItem buildingItem;

    [Header("UI组件")]
    public Text buildingName;
    public Text descriptionText;
    public GameObject descriptionPanel;

    [Header("按钮")]
    public Button ChatWithAI;
    public Button CloseButton;

    [Header("面板")]
    public GameObject Panel;
    public GameObject ChatUI;

    

    private void Start()
    {
        Hide();
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }

    public void SetBuildingItem(BuildingItem buildingItem)
    {
        this.buildingItem = buildingItem;
        this.buildingName.text = buildingItem.itemData.name;

        // 🔥 使用 OutLineManager 显示唯一光标
        if (OutLineManager.Instance != null)
        {
            OutLineManager.Instance.ShowOutline(buildingItem.gameObject);
        }
        else
        {
            Debug.LogWarning("[ItemInfoPlane] OutLineManager.Instance 为空！");
            // 降级方案：如果没有管理器，直接启用
            var outline = buildingItem.GetComponent<m_Outline>();
            if (outline != null) outline.enabled = true;
        }

        // ============ 从字典获取描述 ============
        if (BuildingDataTable.Instance != null && descriptionText != null)
        {
            string itemName = buildingItem.itemData.name;

            // 处理带问号的名称
            if (itemName.Contains("?"))
            {
                itemName = itemName.Replace("?", "").Trim();
            }

            string description = BuildingDataTable.Instance.GetDescription(itemName);
            descriptionText.text = description;
        }
        else
        {
            if (descriptionText != null)
                descriptionText.text = "暂无详细信息";
        }
    }
  

    void ChatWithAIOnClick()
    {
        ChatUI.SetActive(true);
        Hide();
        buildingItem.AskAI();
        PlayerMouseLock.instance.UnlockCursor();
    }

    public void Show()
    {
        Panel.gameObject.SetActive(true);
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);
    }

    public void Hide()
    {
        // 🔥 关闭面板时隐藏轮廓
        if (OutLineManager.Instance != null)
        {
            OutLineManager.Instance.HideAllOutlines();
        }
        else if (buildingItem != null)
        {
            var outline = buildingItem.GetComponent<m_Outline>();
            if (outline != null) outline.enabled = false;
        }

        Panel.gameObject.SetActive(false);
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }
}