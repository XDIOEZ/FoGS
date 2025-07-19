using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraMouseLock : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public static CameraMouseLock instance;
    public EventSystem eventSystem;
    public static bool IsLocked { get; private set; } = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UnlockCursor(); // 默认开始时解锁鼠标
    }

    private void Update()
    {
         if (Input.GetMouseButtonDown(1))
        {
            ToggleCursorLock();
        }
        // 强制确保鼠标不被锁定
        if (Cursor.lockState != CursorLockMode.None)
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButtonDown(1))
            ToggleCursor();
    }

    private void ToggleCursor()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 切换鼠标锁定状态（隐藏/显示）
    /// </summary>
    public void ToggleCursorLock()
    {
        if (IsLocked)
            UnlockCursor();
        else
            LockCursor();
    }

    /// <summary>
    /// 锁定鼠标（仅隐藏，不锁定位置）
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // 不锁定在屏幕中心
        Cursor.visible = false; // 隐藏鼠标
        IsLocked = true;
    }

    /// <summary>
    /// 解锁鼠标（显示并完全释放）
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // 确保完全释放
        Cursor.visible = true; // 显示鼠标
        IsLocked = false;
    }

    /// <summary>
    /// 检测UI点击（从屏幕中心发射射线）
    /// </summary>
    void DetectUIClick()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition; // 使用当前鼠标位置

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log("UI按钮被点击: " + button.name);
                break;
            }
        }
    }
}