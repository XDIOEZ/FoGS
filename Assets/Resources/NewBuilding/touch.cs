using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject target; // Ŀ������
    public float distanceThreshold = 5f; // ������ֵN��
    public float skillRange = 1f; // �����ͷŷ�Χ1��

    void Update()
    {
        // ����Player��Target֮��ľ���
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // �������С��N��
        if (distance < distanceThreshold)
        {
            // �ͷż���
            ReleaseSkill();
        }
    }

    void ReleaseSkill()
    {
        // ��ȡPlayer�ĵ�ǰλ�úͷ���
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;
        Vector3 playerRight = transform.right;
        Vector3 playerLeft = -transform.right;

        // ���㼼���ͷ�λ��
        Vector3 skillPositionFront = playerPosition + playerForward * skillRange;
        Vector3 skillPositionRight = playerPosition + playerRight * skillRange;
        Vector3 skillPositionLeft = playerPosition + playerLeft * skillRange;

        // ������ʵ�ּ����ͷŵ��߼�������������Ч�򴥷��¼�
        Debug.Log("�ͷż�������ǰ��: " + skillPositionFront);
        Debug.Log("�ͷż������ҷ�: " + skillPositionRight);
        Debug.Log("�ͷż�������: " + skillPositionLeft);

        // ʾ�����ڼ����ͷ�λ������һ������
        CreateSkillEffect(skillPositionFront);
        CreateSkillEffect(skillPositionRight);
        CreateSkillEffect(skillPositionLeft);
    }

    void CreateSkillEffect(Vector3 position)
    {
        // ����һ��������Ϊ����Ч��
        GameObject skillEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        skillEffect.transform.position = position;
        skillEffect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // ��С�����С
        Destroy(skillEffect, 1f); // 1�����������
    }
}