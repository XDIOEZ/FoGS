using UnityEngine;
using System.Collections;

public class SimpleOrbitCamera : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("玩家需要观察的目标物体")]
    public Transform target;

    [Tooltip("鼠标水平移动的灵敏度 (左右旋转)")]
    public float horizontalSensitivity = 2.0f;

    [Tooltip("鼠标垂直移动的灵敏度 (上下旋转)")]
    public float verticalSensitivity = 2.0f;

    [Header("角度限制 (防止翻转)")]
    [Tooltip("向上看的最大角度 (例如 80 度)")]
    public float maxVerticalAngle = 80f;
    [Tooltip("向下看的最小角度 (例如 -80 度)")]
    public float minVerticalAngle = -80f;

    [Header("距离设置 🔥 新增")]
    [Tooltip("摄像机距离目标的最小距离")]
    public float minDistance = 2f;
    [Tooltip("摄像机距离目标的最大距离")]
    public float maxDistance = 20f;
    [Tooltip("鼠标滚轮的灵敏度，数值越大缩放越快")]
    public float scrollSensitivity = 2f;

    [Header("复位设置")]
    [Tooltip("双击检测的时间间隔(秒)，建议 0.2~0.4")]
    public float doubleClickInterval = 0.3f;
    [Tooltip("复位动画的持续时间(秒)")]
    public float resetDuration = 1.0f;

    // 🔥 新增：记录默认状态 + 双击计时 + 复位锁 + 当前距离
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private float defaultDistance; // 🔥 记录默认距离
    private float lastRightClickTime = 0f;
    private bool isResetting = false;
    private float currentDistance; // 🔥 当前实际距离

    void Start()
    {
        // 🔥 记录游戏开始时的摄像机状态，作为"默认位置"
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        // 🔥 计算初始距离（摄像机到目标的距离）
        if (target != null)
        {
            defaultDistance = Vector3.Distance(transform.position, target.position);
            currentDistance = defaultDistance;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (isResetting) return; // 🔥 复位过程中禁用鼠标操作，避免冲突

        // --- 🔥 新增：鼠标滚轮控制距离 ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 滚轮向上(正值) = 拉近，滚轮向下(负值) = 拉远
            currentDistance -= scroll * scrollSensitivity;
            // 限制距离范围
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }

        // --- 1. 左右旋转 (鼠标右键) ---
        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X") * horizontalSensitivity;
            transform.RotateAround(target.position, Vector3.up, h);
        }

        // --- 2. 上下旋转 (当前也是右键，如需左键请改成 GetMouseButton(0)) ---
        if (Input.GetMouseButton(1))
        {
            float v = -Input.GetAxis("Mouse Y") * verticalSensitivity;

            // 角度限制逻辑
            float currentX = transform.eulerAngles.x;
            if (currentX > 180f) currentX -= 360f;

            bool isClamped = false;
            if (currentX < minVerticalAngle && v < 0) isClamped = true;
            if (currentX > maxVerticalAngle && v > 0) isClamped = true;

            if (!isClamped)
            {
                transform.RotateAround(target.position, transform.right, v);
            }
        }

        // --- 🔥 新增：应用距离变化 ---
        // 让摄像机保持在"目标方向 × 当前距离"的位置
        Vector3 direction = (transform.position - target.position).normalized;
        transform.position = target.position + direction * currentDistance;

        // --- 3. 双击右键检测 ---
        if (Input.GetMouseButtonDown(1))
        {
            float currentTime = Time.time;

            if (currentTime - lastRightClickTime <= doubleClickInterval)
            {
                // ✅ 双击成功！执行平滑复位（包含距离）
                StartCoroutine(ResetToDefaultCoroutine());
                lastRightClickTime = 0f;
            }
            else
            {
                lastRightClickTime = currentTime;
            }
        }

        // 4. 确保摄像机始终看着目标
        transform.LookAt(target);
    }

    // 🔥 平滑复位协程（已更新：包含距离复位）
    private IEnumerator ResetToDefaultCoroutine()
    {
        isResetting = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float startDistance = currentDistance; // 🔥 记录当前距离
        float elapsed = 0f;

        while (elapsed < resetDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / resetDuration);

            // 平滑插值：位置 + 旋转 + 距离
            transform.position = Vector3.Lerp(startPos, defaultPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, defaultRotation, t);
            currentDistance = Mathf.Lerp(startDistance, defaultDistance, t); // 🔥 距离也平滑过渡

            // 复位过程中保持看着目标
            if (target != null)
                transform.LookAt(target);

            yield return null;
        }

        // 确保最终状态 100% 准确
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
        currentDistance = defaultDistance; // 🔥 重置距离

        if (target != null)
            transform.LookAt(target);

        isResetting = false;
        Debug.Log("🎥 摄像机已复位到默认位置（含距离）");
    }

    // 可视化连线，方便调试
    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);

            // 🔥 可选：显示当前距离
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(target.position, 0.1f);
        }
    }
}