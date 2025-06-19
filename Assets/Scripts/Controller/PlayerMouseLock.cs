using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PlayerMouseLock : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public static bool IsLocked { get; private set; } = true;

    void Start()
    {
        UnlockCursor();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (IsLocked)
                UnlockCursor();
            else
                LockCursor();
        }
        /*if (IsLocked && Input.GetMouseButtonDown(0))
        {
            DetectUIClick();
        }*/
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsLocked = false;
    }

    void DetectUIClick()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2); // 中心点

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            // 如果点击到了 Button，主动触发点击事件
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log("点击了按钮：" + button.name);
                break;
            }
        }
    }
}
