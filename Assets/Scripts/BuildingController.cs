using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    public BuildingManager buildingManager; // 拖拽BuildingManager对象到这里
    public Button disassembleButton;
    public Button reassembleButton;
    public Button reassembleButton_V;
    public Button 垂直;
    public Button 爆炸;

    public Button UseArh;
    public Button UseDirect;
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

        if (reassembleButton_V != null)
        {
            reassembleButton_V.onClick.AddListener(ReassembleBuilding_V);
        }
if (垂直 != null)
        {
            垂直.onClick.AddListener(ReassembleBuilding_V_B);
        }
if (爆炸 != null)
        {
            爆炸.onClick.AddListener(ReassembleBuilding_B);
        }

        if (UseArh != null)
        {
            UseArh.onClick.AddListener(UseArhMode);
        }

        if (UseDirect != null)
        {
            UseDirect.onClick.AddListener(UseDirectMode);   
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

    public void ReassembleBuilding_V()
    {
        if (buildingManager != null)
        {
            buildingManager.ReassembleBuildingFixedPoint();
        }
    }

    public void ReassembleBuilding_B()
    {
        if (buildingManager != null)
        {
            buildingManager.currentMode = BuildingManager.DisassembleMode.爆炸;
            buildingManager.DisassembleBuilding();  
        }
    }

    public void ReassembleBuilding_V_B()
    {
        if (buildingManager != null)
        {
            buildingManager.currentMode = BuildingManager.DisassembleMode.垂直;
            buildingManager.DisassembleBuilding();
        }
    }

    public void UseArhMode()
    {
        if (buildingManager != null)
        {
            buildingManager.resetMode = BuildingManager.ResetMode.抛物线;
        }
    }
  public void UseDirectMode()
    {
        if (buildingManager != null)
        {
            buildingManager.resetMode = BuildingManager.ResetMode.直接;
        }
    }
}