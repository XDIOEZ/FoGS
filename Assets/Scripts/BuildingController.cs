using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    public BuildingManager buildingManager; // 拖拽BuildingManager对象到这里
    public Button disassembleButton;
    public Button reassembleButton;

    private void Start()
    {
        // 确保有BuildingManager引用
        if (buildingManager == null)
        {
            buildingManager = FindObjectOfType<BuildingManager>();
        }

        // 为按钮添加点击事件
        if (disassembleButton != null)
        {
            disassembleButton.onClick.AddListener(DisassembleBuilding);
        }

        if (reassembleButton != null)
        {
            reassembleButton.onClick.AddListener(ReassembleBuilding);
        }
    }

    public void DisassembleBuilding()
    {
        if (buildingManager != null)
        {
            buildingManager.DisassembleBuilding();
        }
    }

    public void ReassembleBuilding()
    {
        if (buildingManager != null)
        {
            buildingManager.ReassembleBuilding();
        }
    }
}