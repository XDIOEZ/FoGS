using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 5f;

    [Header("���������")]
    public CinemachineVirtualCamera virtualCamera;

    private CharacterController characterController;
    private Transform cameraTransform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // ��ȡ��������������������������������
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A��D ��
        float vertical = Input.GetAxis("Vertical");     // W��S ��

        // ��ȡ���������ֻȡˮƽ����
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // �����ƶ�����
        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        // ��������ת��������ȷ��Ҫת��
        // ���������Ҫ��ֱ��ע��ȡ��

        // ִ���ƶ�
        characterController.SimpleMove(moveDir.normalized * moveSpeed);
    }
}
