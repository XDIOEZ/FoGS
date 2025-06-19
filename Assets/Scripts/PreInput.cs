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

    [ContextMenu("��ʾ")]
    public void Show()
    {
        //��������Ϊ1
        transform.localScale = Vector3.one;
    }

    [ContextMenu("����")]
    public void Hide()
    {
        //��������Ϊ0
        transform.localScale = Vector3.zero;
    }
    [ContextMenu("ͬ������")]
    public void SyncInput()
    {
        inputText.text = inputString;
    }
    [ContextMenu("������Ϣ")]
    public void SendMessage()
    {
        chatSample.SendData();
    }

}
