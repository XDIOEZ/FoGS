using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FirstPersonRayShooter : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float jumpForce = 5f;

    [Header("Raycast Settings")]
    public float rayRange = 100f;
    public Color rayColor = Color.red;
    public Color uiRayColor = Color.cyan; // UI专用射线颜色
    public float rayDisplayTime = 0.5f;

    [Header("UI Settings")]
    public Text hitObjectText;
    public float textDisplayTime = 2f;
    public GraphicRaycaster canvasRaycaster;

    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalRotation = 0f;
    private float verticalVelocity = 0f;
    private LineRenderer lineRenderer;
    private float rayTimer;
    private float textTimer;
    private Vector3 rayOrigin;
    private EventSystem eventSystem;
    private bool isMovementLocked = false; // 新增：移动锁定状态
    private bool mouseMod = false;
    public GameObject mousePoint;
    [Header("Flight Mode")]
    public bool isInFlightMode = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // 初始化射线渲染器
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.enabled = false;

        // 确保有EventSystem
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject es = new GameObject("EventSystem");
            eventSystem = es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        // 初始化鼠标状态
        LockPlayerMovement(false);
    }

    void Update()
    {
        // 检测E键按下锁定移动
        if (Input.GetKeyDown(KeyCode.E))
        {
            LockPlayerMovement(true);
            mousePoint.SetActive(false);
        }

        // 检测ESC键按下解锁移动
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockPlayerMovement(false);
            mousePoint.SetActive(true);
        }

        // 只在可以移动时处理移动和视角
        if (!isMovementLocked)
        {
            HandleMovement();
            HandleRotation();
            HandleJump();
        }

        HandleRayShooting();
        UpdateRayTimer();
        UpdateTextTimer();
    }

    // 锁定/解锁玩家移动和视角
    void LockPlayerMovement(bool shouldLock)
    {
        isMovementLocked = shouldLock;

        if (shouldLock)
        {
            // 锁定状态：显示鼠标，解锁光标
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 解锁状态：隐藏鼠标，锁定光标
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void HandleRotation()
    {
        if (isMovementLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleRayShooting()
    {
        // 1. 检查是否处于飞行控制状态
        bool isInFlightControl = false;
        PlayerStateManager flightManager = GetComponent<PlayerStateManager>();

        if (flightManager != null && flightManager.isFlightControlsActive)
        {
            isInFlightControl = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl);
        }

        // 2. 仅在非飞行控制时处理射线
        if (Input.GetButtonDown("Fire1") && !isInFlightControl)
        {
            rayOrigin = playerCamera.transform.position;
            Vector3 rayDirection = playerCamera.transform.forward;

            // UI检测逻辑（保持原样）
            bool hitUI = false;
            string hitObjectName = "未命中任何物体";
            Vector3 endPoint = rayOrigin + rayDirection * rayRange;

            if (canvasRaycaster != null)
            {
                PointerEventData pointerData = new PointerEventData(eventSystem)
                {
                    position = new Vector2(Screen.width / 2, Screen.height / 2)
                };

                List<RaycastResult> results = new List<RaycastResult>();
                canvasRaycaster.Raycast(pointerData, results);

                if (results.Count > 0)
                {
                    hitUI = true;
                    GameObject hitUIObject = results[0].gameObject;
                    hitObjectName = "UI: " + hitUIObject.name;

                    Button button = hitUIObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke();
                        Debug.Log("按钮被点击: " + hitUIObject.name);
                    }
                    endPoint = rayOrigin + rayDirection * 2f;
                }
            }

            // 3D物体检测（保持原样）
            RaycastHit hit;
            if (!hitUI && Physics.Raycast(rayOrigin, rayDirection, out hit, rayRange))
            {
                hitObjectName = "3D物体: " + hit.collider.gameObject.name;
                endPoint = hit.point;
            }

            // 显示射线和文本（保持原样）
            DrawRay(rayOrigin, endPoint, hitUI);
            UpdateHitText(hitObjectName, hitUI);
        }
    }

    // 其余方法保持不变...
    void DrawRay(Vector3 start, Vector3 end, bool isUI = false)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startColor = isUI ? uiRayColor : rayColor;
        lineRenderer.endColor = isUI ? uiRayColor : rayColor;
        lineRenderer.enabled = true;
        rayTimer = rayDisplayTime;
    }

    void UpdateHitText(string name, bool isUI)
    {
        if (hitObjectText != null)
        {
            hitObjectText.text = isUI
                ? $"<color=#00FF00>{name}</color>"
                : $"<color=#FFA500>{name}</color>";
            textTimer = textDisplayTime;
        }
    }

    void UpdateRayTimer()
    {
        if (rayTimer > 0)
        {
            rayTimer -= Time.deltaTime;
            if (rayTimer <= 0)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    void UpdateTextTimer()
    {
        if (textTimer > 0)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0 && hitObjectText != null)
            {
                hitObjectText.text = "";
            }
        }
    }

    void HandleMovement()
    {
        if (isMovementLocked) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection = moveDirection.normalized * moveSpeed;


        if (!isInFlightMode)
        {
            if (characterController.isGrounded)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
            moveDirection.y = verticalVelocity;
        }
        else
        {
            moveDirection.y = 0;
        }

        characterController.Move(moveDirection * Time.deltaTime);
       
    }

    void HandleJump()
    {
        if (isMovementLocked) return;

        if (!isInFlightMode && characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = jumpForce;
        }
    }
}