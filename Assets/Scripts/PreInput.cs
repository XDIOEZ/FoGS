using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreInput : MonoBehaviour
{

    public static PreInput instance;
    public InputField inputText;
    public String inputString;
    public ChatSample chatSample;

    public void Awake()
    {
        instance = this;
    }

    [ContextMenu("显示")]
    public void Show()
    {
        //调整缩放为1
        transform.localScale = Vector3.one;
    }

    [ContextMenu("隐藏")]
    public void Hide()
    {
        //调整缩放为0
        transform.localScale = Vector3.zero;
    }
    [ContextMenu("同步文字")]
    public void SyncInput()
    {
        inputText.text = inputString;
    }
    [ContextMenu("发送消息")]
    public void SendMessage()
    {
        chatSample.SendData();
    }

}
