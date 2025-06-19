using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Fly : MonoBehaviour
{
    public float moveSpeed = 5f;       // ˮƽ�ƶ��ٶȣ���δʹ�ã�
    public float verticalSpeed = 3f;   // �����ƶ��ٶ�

    public float doubleClickTime = 0.3f; // ˫���ж�ʱ����

    private Rigidbody rb;
    private bool isFlying = false;      // �Ƿ��ڷ���ģʽ
    private float lastSpaceTime = -1f;  // ��һ�ΰ��ո��ʱ��

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ��ʼĬ�Ͽ�������
        rb.useGravity = true;

        // ������ת����ֹ��ײʱ�������ٶ�
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        DetectDoubleSpace();
    }

    void DetectDoubleSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSpaceTime <= doubleClickTime)
            {
                // ˫���ո��л�����ģʽ
                isFlying = !isFlying;

                // ��̬�л���������
                rb.useGravity = !isFlying;

                Debug.Log("����ģʽ: " + (isFlying ? "����" : "�ر�"));
            }

            lastSpaceTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (!isFlying)
            return;

        Vector3 verticalMove = Vector3.zero;

        if (Input.GetKey(KeyCode.E))
        {
            verticalMove += Vector3.up * verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            verticalMove += Vector3.down * verticalSpeed;
        }

        if (verticalMove != Vector3.zero)
        {
            Vector3 newVelocity = rb.velocity;
            newVelocity.y = verticalMove.y;
            rb.velocity = newVelocity;
        }
        else
        {
            Vector3 newVelocity = rb.velocity;
            newVelocity.y = 0;
            rb.velocity = newVelocity;
        }
    }
}
