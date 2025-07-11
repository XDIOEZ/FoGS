using UnityEngine;
using UnityEngine.UI;

public class NewBuildingControl : MonoBehaviour
{
    public BuildingManager buildingManager;
    public Button horizontalDisassembleBtn;
    public Button explosionDisassembleBtn;
    public Button reassembleOneByOneBtn;
    public Button reassembleSimultaneouslyBtn;

    private void Awake()
    {
        BuildingManager.currentState = BuildingManager.BuildingState.空闲;
    }
    private void Start()
    {
        if (buildingManager == null)
        {
            buildingManager = FindObjectOfType<BuildingManager>();
        }

        horizontalDisassembleBtn.onClick.AddListener(HorizontalDisassemble);
        explosionDisassembleBtn.onClick.AddListener(ExplosionDisassemble);
        reassembleOneByOneBtn.onClick.AddListener(ReassembleOneByOne);
        reassembleSimultaneouslyBtn.onClick.AddListener(ReassembleSimultaneously);
    }

    private void Update()
    {
        if (BuildingManager.currentState != BuildingManager.BuildingState.空闲)
        {
            //将所有按钮禁用
            horizontalDisassembleBtn.interactable = false;
            explosionDisassembleBtn.interactable = false;
            reassembleOneByOneBtn.interactable = false;
            reassembleSimultaneouslyBtn.interactable = false;

        }
        else
        {
            //将所有按钮激活
            horizontalDisassembleBtn.interactable = true;
            explosionDisassembleBtn.interactable = true;
            reassembleOneByOneBtn.interactable = true;
            reassembleSimultaneouslyBtn.interactable = true;

        }

    }

    // 水平拆解（对应第一个代码块的DisassembleBuilding）
    public void HorizontalDisassemble()
    {
        if (buildingManager != null)
        {
            buildingManager.DisassembleBuilding();
            print("水平拆解已触发");
        }
    }

    // 爆炸拆解（对应第一个代码块的ReassembleBuilding_B）
    public void ExplosionDisassemble()
    {
        if (buildingManager != null)
        {
            buildingManager.currentMode = BuildingManager.DisassembleMode.爆炸;
            buildingManager.DisassembleBuilding();
            print("爆炸拆解已触发");
        }
    }

    // 逐个还原（对应第一个代码块的ReassembleBuilding_V_B）
    public void ReassembleOneByOne()
    {
        if (buildingManager != null)
        {
            if (buildingManager != null)
            {
                buildingManager.resetMode = BuildingManager.ResetMode.抛物线;
                
                    buildingManager.ReassembleBuildingFixedPoint();
                print("逐个还原已触发");

            }
            //buildingManager.currentMode = BuildingManager.DisassembleMode.水平; // 确保模式是水平
            //buildingManager.DisassembleBuilding(); // 注意：这里实际是拆解操作
        }
    }

    // 同步还原（对应第一个代码块的ReassembleBuilding）
    public void ReassembleSimultaneously()
    {
        if (buildingManager != null)
        {
            buildingManager.ReassembleBuilding();
            print("同步还原已触发");
        }
    }
}