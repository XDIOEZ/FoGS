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
    //����������
    void OnMouseDown()
    {
        if (!Active)
        {
            return;
        }
        //��ʾAI�Ի���
        ItemInfoPlane.instance.Show();
        ItemInfoPlane.instance.SetBuildingItem(gameObject.GetComponent<BuildingItem>());
        PlayerMouseLock.instance.UnlockCursor();
    }
}
