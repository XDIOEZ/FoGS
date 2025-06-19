using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Fly : MonoBehaviour
{
    public float moveSpeed = 5f;       // 水平移动速度（暂未使用）
    public float verticalSpeed = 3f;   // 上下移动速度

    public float doubleClickTime = 0.3f; // 双击判定时间间隔

    private Rigidbody rb;
    private bool isFlying = false;      // 是否处于飞行模式
    private float lastSpaceTime = -1f;  // 上一次按空格的时间

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初始默认开启重力
        rb.useGravity = true;

        // 锁定旋转，防止碰撞时产生角速度
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
                // 双击空格，切换飞行模式
                isFlying = !isFlying;

                // 动态切换刚体重力
                rb.useGravity = !isFlying;

                Debug.Log("飞行模式: " + (isFlying ? "开启" : "关闭"));
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
