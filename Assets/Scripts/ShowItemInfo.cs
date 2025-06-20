using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemInfo : MonoBehaviour
{
    public bool Active = true;

    void Start()
    {
        Active = true;
    }
    //鼠标左键按下
    void OnMouseDown()
    {
        if (!Active)
        {
            return;
        }
        //显示AI对话框
        ItemInfoPlane.instance.Show();
        ItemInfoPlane.instance.SetBuildingItem(gameObject.GetComponent<BuildingItem>());
        PlayerMouseLock.instance.UnlockCursor();
    }
}
