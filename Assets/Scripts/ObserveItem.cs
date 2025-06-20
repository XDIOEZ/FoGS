using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObserveItem : MonoBehaviour
{
    [Header("速度参数")]
    [Tooltip("旋转速度：越大旋转越快")]
    public float rotateSpeed = 200f;
    [Tooltip("平移（拉进/推远）速度：越大移动越快")]
    public float zoomSpeed = 2f;
    [Tooltip("缩放速度：越大缩放越快")]
    public float scaleSpeed = 0.5f;

    // 用于记录是否正在拖拽
    private bool isDragging = false;

    void OnMouseDown()
    {
        // 鼠标按下时开始拖拽
        isDragging = true;
    }

    void OnMouseUp()
    {
        // 鼠标抬起时结束拖拽
        isDragging = false;
    }

    void OnMouseDrag()
    {
        // 仅当确实处于拖拽中时处理旋转
        if (!isDragging) return;

        // 获取鼠标在本帧的移动量
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ObserveItem: 未找到主摄像机 (Camera.main)，无法执行旋转和平移操作。");
            return;
        }

        // 围绕摄像机的 up 方向旋转水平角度（左右移动鼠标）
        transform.Rotate(cam.transform.up, -dx * rotateSpeed * Time.deltaTime, Space.World);
        // 围绕摄像机的 right 方向旋转垂直角度（上下移动鼠标）
        transform.Rotate(cam.transform.right, dy * rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnMouseOver()
    {
        // 当鼠标在物体上时检测滚轮
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 1e-5f) return;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ObserveItem: 未找到主摄像机 (Camera.main)，无法执行平移/缩放操作。");
            return;
        }

        // 如果按住 Ctrl，则执行缩放
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // 按照当前缩放比例进行等比缩放
            // 这里 newScale = oldScale * (1 + scroll * scaleSpeed)
            float factor = 1f + scroll * scaleSpeed;
            if (factor <= 0f)
            {
                // 防止缩放倒转或归零，忽略此次过大的滚动
                return;
            }
            Vector3 newScale = transform.localScale * factor;
            // 可选：对最小/最大缩放进行限制，比如最小 0.1，最大 10。根据需求自行调整：
            float minScale = 0.1f;
            float maxScale = 10f;
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
            transform.localScale = newScale;
        }
        else
        {
            // 未按 Ctrl，则沿摄像机前向/后向平移
            // scroll > 0: 往摄像机前进方向移动（拉近物体）；scroll < 0: 远离摄像机
            Vector3 dir = cam.transform.forward;
            transform.position += dir * scroll * zoomSpeed;
        }
    }
}
