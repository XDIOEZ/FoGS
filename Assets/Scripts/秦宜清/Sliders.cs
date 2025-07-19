using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Sliders : MonoBehaviour
{
    
    
    [Range(0.2f, 1f)]
    public float disassembleAmount = 1f; // 默认值，实际值由 BuildingManager 控制
    
    [Range(0.2f, 1f)]
    public float disassembleTime = 1f; // 默认值，实际值由 BuildingManager 控制
    
    [Range(0.2f, 1f)]
    public float restoreTime = 1f; // 默认值，实际值由 BuildingManager 控制

    public BuildingManager buildingManager; // Inspector 拖拽赋值

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        OnSliderValueChanged(disassembleAmount);
        OnTimeSliderValueChanged(disassembleTime);
        OnRestoreTimeSliderValueChanged(restoreTime);
    }

    private void OnSliderValueChanged(float value)
    {
       buildingManager.disassembleAmount = value;

    }
    private void OnTimeSliderValueChanged(float value)
    {
        buildingManager.disassembleAmount = value;

    }
    private void OnRestoreTimeSliderValueChanged(float value)
    {
        buildingManager.disassembleAmount = value;
    }
}
