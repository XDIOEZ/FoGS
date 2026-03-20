using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPlane : MonoBehaviour
{
    public static ItemInfoPlane instance;
    public BuildingItem buildingItem;
    public Text buildingName;

    // ============ 新增部分 ============
    public Text descriptionText;        // 描述文本
    public GameObject descriptionPanel; // 描述面板（包含文本的整个面板）
                                        // ==================================

    public Button ChatWithAI;
    public Button CloseButton;
    public GameObject Panel;
    public GameObject ChatUI;

    private void Awake()
    {
        instance = this;
        ChatWithAI.onClick.AddListener(ChatWithAIOnClick);
        CloseButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        Hide();
        // 确保描述面板初始是关闭的
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }

    public void SetBuildingItem(BuildingItem buildingItem)
    {
        if (this.buildingItem != null)
            this.buildingItem.GetComponent<m_Outline>().enabled = false;

        this.buildingItem = buildingItem;
        this.buildingName.text = buildingItem.itemData.name;
        buildingItem.GetComponent<m_Outline>().enabled = true;

        // ============ 新增部分 ============
        // 获取物体上的 ItemDescription 组件
        ItemDescription itemDesc = buildingItem.GetComponent<ItemDescription>();
        if (itemDesc != null && descriptionText != null)
        {
            descriptionText.text = itemDesc.description;
        }
        else
        {
            descriptionText.text = "暂无描述信息";
        }
        // ==================================
    }

    void Update()
    {
        if (buildingItem != null)
            buildingItem.GetComponent<m_Outline>().enabled = true;
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
        // ============ 新增部分 ============
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);
        // ==================================
    }

    public void Hide()
    {
        if (buildingItem != null)
            buildingItem.GetComponent<m_Outline>().enabled = false;
        Panel.gameObject.SetActive(false);
        // ============ 新增部分 ============
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
        // ==================================
    }
}