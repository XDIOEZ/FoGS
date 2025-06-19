using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject interactionPrompt; // �������UI��ʾ���
    public KeyCode interactionKey = KeyCode.E;

    [Header("Cursor Settings")]
    public bool hideCursorInitially = true;
    public bool showCursorAfterInteraction = true;

    private bool isPlayerInRange = false;

    void Start()
    {
        // ��ʼ��UI״̬
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // ��ʼ�����״̬
        Cursor.visible = !hideCursorInitially;
        Cursor.lockState = hideCursorInitially ? CursorLockMode.Locked : CursorLockMode.None;
       
    }

    void Update()
    {
        // �������Ƿ��ڷ�Χ�ڲ������˽�����
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            OnInteract();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }

    void OnInteract()
    {
        // ��ʾ�����
        Cursor.visible = showCursorAfterInteraction;
        Cursor.lockState = showCursorAfterInteraction ? CursorLockMode.None : CursorLockMode.Locked;

        // �������������������߼�
        Debug.Log("Player interacted with " + gameObject.name);

        // ��ѡ�����ý�����ʾ
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}