using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SliderController : MonoBehaviour
{
    public Slider mySlider;  // ��Inspector����קSlider���ñ���
    public BuildingManager buildingManager; // Inspector ��ק��ֵ

    public enum ControlType
    {
        ��ⷶΧ,  // ���� disassembleAmount
        ���ʱ��,   // ���� extraGlobalDirection.x
        ��ԭʱ��    // ���� extraGlobalDirection.y
    }

    public ControlType controlType;
    private void Start()
    {
        if (mySlider == null || buildingManager == null)
        {
            Debug.LogError("Slider �� BuildingManager δ��ֵ��");
            return;
        }

        // ��Ӽ����¼�
        mySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // ��ʼ�� Slider ֵ
        switch (controlType)
        {
            case ControlType.��ⷶΧ:
                mySlider.value = buildingManager.disassembleAmount;
                break;
            case ControlType.���ʱ��:
                mySlider.value = buildingManager.disassembleAmount;
                break;
            case ControlType.��ԭʱ��:
                mySlider.value = buildingManager.disassembleAmount;
                break;

        }
    }
    private void OnSliderValueChanged(float value)
    {
        switch (controlType)
        {
            case ControlType.��ⷶΧ:
                buildingManager.disassembleAmount = value;
                break;
            case ControlType.���ʱ��:
                buildingManager.disassembleDuration = value;
              
                break;
            case ControlType.��ԭʱ��:
                buildingManager.reassembleDuration = value;


                break;
        }
    }
}