using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    public BuildingManager buildingManager; // ��קBuildingManager��������
    public Button disassembleButton;
    public Button reassembleButton;
    public Button reassembleButton_V;
    public Button ��ֱ;
    public Button ��ը;

    public Button UseArh;
    public Button UseDirect;
    private void Start()
    {
        // ȷ����BuildingManager����
        if (buildingManager == null)
        {
            buildingManager = FindObjectOfType<BuildingManager>();
        }

        // Ϊ��ť��ӵ���¼�
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
if (��ֱ != null)
        {
            ��ֱ.onClick.AddListener(ReassembleBuilding_V_B);
        }
if (��ը != null)
        {
            ��ը.onClick.AddListener(ReassembleBuilding_B);
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
            buildingManager.currentMode = BuildingManager.DisassembleMode.��ը;
            buildingManager.DisassembleBuilding();  
        }
    }

    public void ReassembleBuilding_V_B()
    {
        if (buildingManager != null)
        {
            buildingManager.currentMode = BuildingManager.DisassembleMode.��ֱ;
            buildingManager.DisassembleBuilding();
        }
    }

    public void UseArhMode()
    {
        if (buildingManager != null)
        {
            buildingManager.resetMode = BuildingManager.ResetMode.������;
        }
    }
  public void UseDirectMode()
    {
        if (buildingManager != null)
        {
            buildingManager.resetMode = BuildingManager.ResetMode.ֱ��;
        }
    }
}