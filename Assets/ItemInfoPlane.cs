using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPlane : MonoBehaviour
{
    public static ItemInfoPlane instance;
    public BuildingItem buildingItem;
    public Text buildingName;
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
    }
    public void SetBuildingItem(BuildingItem buildingItem)
    {
        this.buildingItem = buildingItem;
        this.buildingName.text = buildingItem.itemData.name;
        buildingItem.GetComponent<m_Outline>().enabled = true;
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
    }

    public void Hide()
    {
        if(buildingItem!= null)
        buildingItem.GetComponent<m_Outline>().enabled = false;
        Panel.gameObject.SetActive(false);
    }
}
