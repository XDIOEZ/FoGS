using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMouseLock : MonoBehaviour
{
    public EventSystem eventSystem;
    public static bool IsLocked { get; private set; } = true;

    void Start()
    {
        LockCursor();
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
}
