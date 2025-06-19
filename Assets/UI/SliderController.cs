using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SliderController : MonoBehaviour
{
    public Slider mySlider;  // 在Inspector中拖拽Slider到该变量
    public BuildingManager buildingManager; // Inspector 拖拽赋值

    public enum ControlType
    {
        拆解范围,  // 控制 disassembleAmount
        拆解时间,   // 控制 extraGlobalDirection.x
        还原时间    // 控制 extraGlobalDirection.y
    }

    public ControlType controlType;
    private void Start()
    {
        if (mySlider == null || buildingManager == null)
        {
            Debug.LogError("Slider 或 BuildingManager 未赋值！");
            return;
        }

        // 添加监听事件
        mySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // 初始化 Slider 值
        switch (controlType)
        {
            case ControlType.拆解范围:
                mySlider.value = buildingManager.disassembleAmount;
                break;
            case ControlType.拆解时间:
                mySlider.value = buildingManager.disassembleAmount;
                break;
            case ControlType.还原时间:
                mySlider.value = buildingManager.disassembleAmount;
                break;

        }
    }
    private void OnSliderValueChanged(float value)
    {
        switch (controlType)
        {
            case ControlType.拆解范围:
                buildingManager.disassembleAmount = value;
                break;
            case ControlType.拆解时间:
                buildingManager.disassembleDuration = value;
              
                break;
            case ControlType.还原时间:
                buildingManager.reassembleDuration = value;


                break;
        }
    }
}