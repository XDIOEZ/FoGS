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
    public Color uiRayColor = Color.cyan; // UIר��������ɫ
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
    private bool isMovementLocked = false; // �������ƶ�����״̬
    private bool mouseMod = false;
    public GameObject mousePoint;
    [Header("Flight Mode")]
    public bool isInFlightMode = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // ��ʼ��������Ⱦ��
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.enabled = false;

        // ȷ����EventSystem
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject es = new GameObject("EventSystem");
            eventSystem = es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        // ��ʼ�����״̬
        LockPlayerMovement(false);
    }

    void Update()
    {
        // ���E�����������ƶ�
        if (Input.GetKeyDown(KeyCode.E))
        {
            LockPlayerMovement(true);
            mousePoint.SetActive(false);
        }

        // ���ESC�����½����ƶ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockPlayerMovement(false);
            mousePoint.SetActive(true);
        }

        // ֻ�ڿ����ƶ�ʱ�����ƶ����ӽ�
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

    // ����/��������ƶ����ӽ�
    void LockPlayerMovement(bool shouldLock)
    {
        isMovementLocked = shouldLock;

        if (shouldLock)
        {
            // ����״̬����ʾ��꣬�������
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // ����״̬��������꣬�������
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
        // 1. ����Ƿ��ڷ��п���״̬
        bool isInFlightControl = false;
        PlayerStateManager flightManager = GetComponent<PlayerStateManager>();

        if (flightManager != null && flightManager.isFlightControlsActive)
        {
            isInFlightControl = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl);
        }

        // 2. ���ڷǷ��п���ʱ��������
        if (Input.GetButtonDown("Fire1") && !isInFlightControl)
        {
            rayOrigin = playerCamera.transform.position;
            Vector3 rayDirection = playerCamera.transform.forward;

            // UI����߼�������ԭ����
            bool hitUI = false;
            string hitObjectName = "δ�����κ�����";
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
                        Debug.Log("��ť�����: " + hitUIObject.name);
                    }
                    endPoint = rayOrigin + rayDirection * 2f;
                }
            }

            // 3D�����⣨����ԭ����
            RaycastHit hit;
            if (!hitUI && Physics.Raycast(rayOrigin, rayDirection, out hit, rayRange))
            {
                hitObjectName = "3D����: " + hit.collider.gameObject.name;
                endPoint = hit.point;
            }

            // ��ʾ���ߺ��ı�������ԭ����
            DrawRay(rayOrigin, endPoint, hitUI);
            UpdateHitText(hitObjectName, hitUI);
        }
    }

    // ���෽�����ֲ���...
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