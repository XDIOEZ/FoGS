using UnityEngine;
using UnityEngine.UI;

public class LeftMouseDown : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject descriptionPanel;  // 描述信息面板
    public Text descriptionText;        // 描述文本组件
    public float maxInteractionDistance = 10f;  // 最大交互距离

    [Header("Layer Settings")]
    public LayerMask interactableLayer;  // 可交互物品层级

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxInteractionDistance, interactableLayer))
            {
                print("Hit: " + hit.collider.name);
                BuildingItem buildingItem = hit.collider.GetComponent<BuildingItem>();
                if (buildingItem != null)
                {
                    print("111");
                    // 调用原始ItemInfoPlane显示物品名称
                    ItemInfoPlane.instance.SetBuildingItem(buildingItem);
                    ItemInfoPlane.instance.Show();

                    // 显示自定义描述面板
                    ShowItemDescription(buildingItem.itemData.description);
                }
            }
        }
    }

    // 显示物品描述
    private void ShowItemDescription(string description)
    {
        if (descriptionText != null && descriptionPanel != null)
        {
            descriptionText.text = description;
            descriptionPanel.SetActive(true);
        }
    }

    // 关闭描述面板（可绑定到UI按钮）
    public void CloseDescriptionPanel()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }
}