using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ��ȡ��������������������������������
        cameraTransform = Camera.main.transform;

        // ����������������ת
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // ������
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

        // ������붼Ϊ0��ֱ��ֹͣˮƽ�����ƶ�����ֹ���������
        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            // ֻ��� x �� z ������ٶȣ����� y ������ٶȣ����������в�����Ӱ�죩
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            return;
        }

        // ���������ˮƽ����
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

            // ����y�����ٶȲ��䣨����������������Ӱ�죩
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
    }
}
