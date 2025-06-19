using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 获取主摄像机（虚拟摄像机会控制主摄像机）
        cameraTransform = Camera.main.transform;

        // 禁用重力，锁定旋转
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // 输入检测
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 如果输入都为0，直接停止水平方向移动，防止物理缓冲惯性
        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            // 只清除 x 和 z 方向的速度，保留 y 方向的速度（重力、飞行不会受影响）
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            return;
        }

        // 计算摄像机水平方向
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        if (moveDir.sqrMagnitude > 0f)
        {
            moveDir.Normalize();
            Vector3 velocity = moveDir * moveSpeed;

            // 保持y方向速度不变（可能受重力或其他影响）
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
    }
}
