using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject interactionPrompt; // 拖入你的UI提示面板
    public KeyCode interactionKey = KeyCode.E;

    [Header("Cursor Settings")]
    public bool hideCursorInitially = true;
    public bool showCursorAfterInteraction = true;

    private bool isPlayerInRange = false;

    void Start()
    {
        // 初始化UI状态
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // 初始化光标状态
        Cursor.visible = !hideCursorInitially;
        Cursor.lockState = hideCursorInitially ? CursorLockMode.Locked : CursorLockMode.None;
       
    }

    void Update()
    {
        // 检查玩家是否在范围内并按下了交互键
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
        // 显示鼠标光标
        Cursor.visible = showCursorAfterInteraction;
        Cursor.lockState = showCursorAfterInteraction ? CursorLockMode.None : CursorLockMode.Locked;

        // 这里可以添加其他交互逻辑
        Debug.Log("Player interacted with " + gameObject.name);

        // 可选：禁用交互提示
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}