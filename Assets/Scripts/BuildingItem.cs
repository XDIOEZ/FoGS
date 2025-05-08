using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingItem : MonoBehaviour
{
    private Vector3 defaultPosition; // �ṹ���Ĭ��λ��
    private bool isDisassembled = false; // �Ƿ��ڲ��״̬

    private void Start()
    {
        // ��ʼ��ʱ��¼Ĭ��λ��
        defaultPosition = transform.position;
    }

    /// <summary>
    /// Ϊ�ⲿ�����ṩ�ӿڣ�������������ƶ�����
    /// </summary>
    /// <param name="targetPosition">Ŀ����ά����</param>
    public void MoveTo(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        isDisassembled = true; // �ƶ�����Ϊ���״̬
    }

    /// <summary>
    /// ������ڲ��״̬���ͽ���鷵�ص��ṹ��Ĭ��λ��
    /// </summary>
    public void TurnBack()
    {
        if (isDisassembled)
        {
            MoveTo(defaultPosition);
            isDisassembled = false; // ���ò��״̬
        }
    }
}
