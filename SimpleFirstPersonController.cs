using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    private float rotationX = 0;
    private Camera playerCamera;
    private float currentSpeed;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        SetCursorState(true);
    }

    public void SetControllerEnabled(bool enabled)
    {
        this.enabled = enabled;
        SetCursorState(enabled);
    }

    public void SetCursorState(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    void Update()
    {
        if (!enabled) return;

        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);

        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void HandleMovement()
    {
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * z + transform.right * x).normalized;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }
}