using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanle : MonoBehaviour
{
    public static ShowPanle instance;
    public BuildingItem buildingItem;
    public Text buildingName;
    public Text buildingDescription;
    public Button CloseButton;
    public GameObject Panel;


    private void Awake()
    {
        instance = this;
       
        CloseButton.onClick.AddListener(Hide);
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        
    }
    public void SetBuildingItem(BuildingItem buildingItem)
    {
        this.buildingItem = buildingItem;
        this.buildingName.text = buildingItem.itemData.name;
        this.buildingDescription.text = buildingItem.itemData.description;

    }
    void Update()
    {
        if (buildingItem != null)
            buildingItem.GetComponent<m_Outline>().enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                BuildingItem item = hit.collider.GetComponent<BuildingItem>();
                if (item != null)
                {
                    SetBuildingItem(item);
                    Show();
                }
            }
        }
    }
    public void Show()
    {
        Panel.gameObject.SetActive(true);
    }

    public void Hide()
    {

       
            Panel.gameObject.SetActive(false);
       
    }
}

