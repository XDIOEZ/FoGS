using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemInfo : MonoBehaviour
{
    //����������
    void OnMouseDown()
    {
        //��ʾAI�Ի���
        ItemInfoPlane.instance.Show();
        ItemInfoPlane.instance.SetBuildingItem(gameObject.GetComponent<BuildingItem>());
        PlayerMouseLock.instance.UnlockCursor();
    }
}
