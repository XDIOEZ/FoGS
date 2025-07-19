using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    public GameObject cam;
    public Button button;

    //完善按钮的点击事件
    public void Start()
    {
        button.onClick.AddListener(SwitchCamera);
    }

    //切换摄像机的激活状态
    public void SwitchCamera()
    {
        cam.SetActive(!cam.activeSelf);
    }
}
