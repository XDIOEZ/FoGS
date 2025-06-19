using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;

    [Header("虚拟摄像机")]
    public CinemachineVirtualCamera virtualCamera;

    private CharacterController characterController;
    private Transform cameraTransform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // 获取主摄像机（虚拟摄像机会控制主摄像机）
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A、D 键
        float vertical = Input.GetAxis("Vertical");     // W、S 键

        // 获取摄像机方向，只取水平分量
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // 计算移动方向
        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        // 不处理旋转（你已明确不要转向）
        // 如果后续需要，直接注释取消

        // 执行移动
        characterController.SimpleMove(moveDir.normalized * moveSpeed);
    }
}
