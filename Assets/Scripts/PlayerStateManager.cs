using UnityEngine;

public enum PlayerState { Grounded, Flying }

public class PlayerStateManager : MonoBehaviour
{

    [Header("Raycast Settings")]
    public LayerMask focusDetectionMask; // 在Inspector中设置需要检测的层级
    [Header("References")]
    public FirstPersonRayShooter shooterScript;
    public CharacterController characterController;
    public Camera playerCamera;
    public GameObject focusCameraPrefab; // 聚焦摄像机预制体
   

    [Header("Flight Settings")]
    public float flySpeed = 15f;
    public float flyBoostMultiplier = 2f;
    public float flyLookSpeed = 150f;
    public float verticalFlySpeed = 8f;
    public float initialFlightHeight = 5f;

    [Header("Focus Settings")]
    public float focusDistance = 3f;
    public float focusRotationSpeed = 100f;
    public KeyCode exitFocusKey = KeyCode.Escape;

    private PlayerState currentState = PlayerState.Grounded;
    private float originalMoveSpeed;
    private float originalMouseSensitivity;
    private float originalJumpForce;
    public bool isFlightControlsActive = false;
    private Vector3 flightMovement;

    // 聚焦相关变量
    private bool isFocusing = false;
    private Transform focusTarget;
    private Camera focusCamera;
    private Vector3 focusOffset;
    private float focusXRotation = 0f;
    private float focusYRotation = 0f;

    void Start()
    {
        originalMoveSpeed = shooterScript.moveSpeed;
        originalMouseSensitivity = shooterScript.mouseSensitivity;
        originalJumpForce = shooterScript.jumpForce;
    }

    public void ToggleFlightMode()
    {
        switch (currentState)
        {
            case PlayerState.Grounded:
                EnterFlightMode();
                break;
            case PlayerState.Flying:
                ExitFlightMode();
                break;
        }
    }

    private void EnterFlightMode()
    {
        shooterScript.isInFlightMode = true;
        currentState = PlayerState.Flying;

        shooterScript.moveSpeed = flySpeed;
        shooterScript.mouseSensitivity = flyLookSpeed;
        shooterScript.jumpForce = 0f;

        flightMovement.y = 0f;

        characterController.enabled = false;
        transform.position += Vector3.up * initialFlightHeight;
        characterController.enabled = true;

        isFlightControlsActive = true;
        Debug.Log("飞行模式激活 | 空格上升/Ctrl下降 | 完全手动控制");
    }

    private void ExitFlightMode()
    {
        shooterScript.isInFlightMode = false;
        currentState = PlayerState.Grounded;
        shooterScript.moveSpeed = originalMoveSpeed;
        shooterScript.mouseSensitivity = originalMouseSensitivity;
        shooterScript.jumpForce = originalJumpForce;
        isFlightControlsActive = false;

        // 退出时确保关闭聚焦模式
        if (isFocusing)
        {
            ExitFocusMode();
        }
    }

    void Update()
    {
        if (isFocusing)
        {
            HandleFocusMode();
        }
        else if (isFlightControlsActive)
        {
            HandleFlightMovement();

            // 飞行模式下检测射线点击
            if (Input.GetButtonDown("Fire1"))
            {
                TryFocusOnObject();
            }
        }
    }

    private void TryFocusOnObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shooterScript.rayRange))
        {
            StartFocusMode(hit.transform);
        }
    }

    private void StartFocusMode(Transform target)
    {
        isFocusing = true;
        focusTarget = target;

       

        // 创建聚焦摄像机
        if (focusCameraPrefab != null)
        {
            GameObject focusCamObj = Instantiate(focusCameraPrefab);
            focusCamera = focusCamObj.GetComponent<Camera>();
        }
        else
        {
            GameObject focusCamObj = new GameObject("FocusCamera");
            focusCamera = focusCamObj.AddComponent<Camera>();
        }

        // 禁用玩家摄像机
        playerCamera.enabled = false;

        // 计算初始位置和旋转
        focusOffset = (playerCamera.transform.position - target.position).normalized * focusDistance;
        focusCamera.transform.position = target.position + focusOffset;
        focusCamera.transform.LookAt(target.position);

        // 初始化旋转角度
        Vector3 angles = focusCamera.transform.eulerAngles;
        focusXRotation = angles.x;
        focusYRotation = angles.y;

        // 禁用飞行控制
        isFlightControlsActive = false;

        // 锁定并隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleFocusMode()
    {
        if (focusCamera == null || focusTarget == null)
        {
            ExitFocusMode();
            return;
        }

        // 鼠标右键旋转控制
        if (Input.GetMouseButton(1)) // 右键按住旋转
        {
            float mouseX = Input.GetAxis("Mouse X") * focusRotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * focusRotationSpeed * Time.deltaTime;

            focusYRotation += mouseX;
            focusXRotation -= mouseY;
            focusXRotation = Mathf.Clamp(focusXRotation, -89f, 89f);

            // 计算新的偏移方向
            Quaternion rotation = Quaternion.Euler(focusXRotation, focusYRotation, 0);
            focusOffset = rotation * Vector3.back * focusDistance;
        }

        // 更新摄像机位置和旋转
        focusCamera.transform.position = focusTarget.position + focusOffset;
        focusCamera.transform.LookAt(focusTarget.position);

        // 退出聚焦模式
        if (Input.GetKeyDown(exitFocusKey))
        {
            ExitFocusMode();
        }
    }

    private void ExitFocusMode()
    {
        if (focusCamera != null)
        {
            Destroy(focusCamera.gameObject);
        }

      
        isFocusing = false;
        focusTarget = null;

        // 恢复玩家摄像机
        playerCamera.enabled = true;

        // 恢复飞行控制
        if (currentState == PlayerState.Flying)
        {
            isFlightControlsActive = true;
        }

        // 恢复鼠标状态
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleFlightMovement()
    {
        Vector3 move = (transform.forward * Input.GetAxis("Vertical") +
                       transform.right * Input.GetAxis("Horizontal")).normalized;

        float verticalInput = 0f;
        bool isUsingFlightKeys = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl);

        if (isUsingFlightKeys)
        {
            if (Input.GetKey(KeyCode.Space)) verticalInput = 1f;
            if (Input.GetKey(KeyCode.LeftControl)) verticalInput = -1f;
        }

        Vector3 finalMove = new Vector3(
            move.x * flySpeed,
            verticalInput * verticalFlySpeed,
            move.z * flySpeed
        );

        if (Input.GetKey(KeyCode.LeftShift))
        {
            finalMove.x *= flyBoostMultiplier;
            finalMove.z *= flyBoostMultiplier;
        }

        characterController.Move(finalMove * Time.deltaTime);
    }

    public void OnFlightButtonPressed()
    {
        ToggleFlightMode();
    }
}