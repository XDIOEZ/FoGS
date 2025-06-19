using UnityEngine;

public enum PlayerState { Grounded, Flying }

public class PlayerStateManager : MonoBehaviour
{

    [Header("Raycast Settings")]
    public LayerMask focusDetectionMask; // ��Inspector��������Ҫ���Ĳ㼶
    [Header("References")]
    public FirstPersonRayShooter shooterScript;
    public CharacterController characterController;
    public Camera playerCamera;
    public GameObject focusCameraPrefab; // �۽������Ԥ����
   

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

    // �۽���ر���
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
        Debug.Log("����ģʽ���� | �ո�����/Ctrl�½� | ��ȫ�ֶ�����");
    }

    private void ExitFlightMode()
    {
        shooterScript.isInFlightMode = false;
        currentState = PlayerState.Grounded;
        shooterScript.moveSpeed = originalMoveSpeed;
        shooterScript.mouseSensitivity = originalMouseSensitivity;
        shooterScript.jumpForce = originalJumpForce;
        isFlightControlsActive = false;

        // �˳�ʱȷ���رվ۽�ģʽ
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

            // ����ģʽ�¼�����ߵ��
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

       

        // �����۽������
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

        // ������������
        playerCamera.enabled = false;

        // �����ʼλ�ú���ת
        focusOffset = (playerCamera.transform.position - target.position).normalized * focusDistance;
        focusCamera.transform.position = target.position + focusOffset;
        focusCamera.transform.LookAt(target.position);

        // ��ʼ����ת�Ƕ�
        Vector3 angles = focusCamera.transform.eulerAngles;
        focusXRotation = angles.x;
        focusYRotation = angles.y;

        // ���÷��п���
        isFlightControlsActive = false;

        // �������������
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

        // ����Ҽ���ת����
        if (Input.GetMouseButton(1)) // �Ҽ���ס��ת
        {
            float mouseX = Input.GetAxis("Mouse X") * focusRotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * focusRotationSpeed * Time.deltaTime;

            focusYRotation += mouseX;
            focusXRotation -= mouseY;
            focusXRotation = Mathf.Clamp(focusXRotation, -89f, 89f);

            // �����µ�ƫ�Ʒ���
            Quaternion rotation = Quaternion.Euler(focusXRotation, focusYRotation, 0);
            focusOffset = rotation * Vector3.back * focusDistance;
        }

        // ���������λ�ú���ת
        focusCamera.transform.position = focusTarget.position + focusOffset;
        focusCamera.transform.LookAt(focusTarget.position);

        // �˳��۽�ģʽ
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

        // �ָ���������
        playerCamera.enabled = true;

        // �ָ����п���
        if (currentState == PlayerState.Flying)
        {
            isFlightControlsActive = true;
        }

        // �ָ����״̬
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