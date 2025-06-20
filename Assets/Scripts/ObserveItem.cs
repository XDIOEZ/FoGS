using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObserveItem : MonoBehaviour
{
    [Header("�ٶȲ���")]
    [Tooltip("��ת�ٶȣ�Խ����תԽ��")]
    public float rotateSpeed = 200f;
    [Tooltip("ƽ�ƣ�����/��Զ���ٶȣ�Խ���ƶ�Խ��")]
    public float zoomSpeed = 2f;
    [Tooltip("�����ٶȣ�Խ������Խ��")]
    public float scaleSpeed = 0.5f;

    // ���ڼ�¼�Ƿ�������ק
    private bool isDragging = false;

    void OnMouseDown()
    {
        // ��갴��ʱ��ʼ��ק
        isDragging = true;
    }

    void OnMouseUp()
    {
        // ���̧��ʱ������ק
        isDragging = false;
    }

    void OnMouseDrag()
    {
        // ����ȷʵ������ק��ʱ������ת
        if (!isDragging) return;

        // ��ȡ����ڱ�֡���ƶ���
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ObserveItem: δ�ҵ�������� (Camera.main)���޷�ִ����ת��ƽ�Ʋ�����");
            return;
        }

        // Χ��������� up ������תˮƽ�Ƕȣ������ƶ���꣩
        transform.Rotate(cam.transform.up, -dx * rotateSpeed * Time.deltaTime, Space.World);
        // Χ��������� right ������ת��ֱ�Ƕȣ������ƶ���꣩
        transform.Rotate(cam.transform.right, dy * rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnMouseOver()
    {
        // �������������ʱ������
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 1e-5f) return;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ObserveItem: δ�ҵ�������� (Camera.main)���޷�ִ��ƽ��/���Ų�����");
            return;
        }

        // �����ס Ctrl����ִ������
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // ���յ�ǰ���ű������еȱ�����
            // ���� newScale = oldScale * (1 + scroll * scaleSpeed)
            float factor = 1f + scroll * scaleSpeed;
            if (factor <= 0f)
            {
                // ��ֹ���ŵ�ת����㣬���Դ˴ι���Ĺ���
                return;
            }
            Vector3 newScale = transform.localScale * factor;
            // ��ѡ������С/������Ž������ƣ�������С 0.1����� 10�������������е�����
            float minScale = 0.1f;
            float maxScale = 10f;
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
            transform.localScale = newScale;
        }
        else
        {
            // δ�� Ctrl�����������ǰ��/����ƽ��
            // scroll > 0: �������ǰ�������ƶ����������壩��scroll < 0: Զ�������
            Vector3 dir = cam.transform.forward;
            transform.position += dir * scroll * zoomSpeed;
        }
    }
}
