using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    public BuildingManager buildingManager; // ��קBuildingManager��������
    public Button disassembleButton;
    public Button reassembleButton;

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