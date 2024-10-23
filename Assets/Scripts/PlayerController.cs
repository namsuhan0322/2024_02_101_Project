using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �÷��̾��� �����Ӽӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;      // �̵��ӵ�
    public float jumpForce = 5.0f;      // ���� ��
    public float rotationSpeed = 10f;   // ȸ�� �ӵ�

    // ī�޶� ���� ����
    [Header("Camera Settings")]
    public Camera firstPersonCamera;                // 1��Ī ī�޶�
    public Camera thridPersonCamera;                // 3��Ī ī�޶�

    public float cameraDistance = 5.0f;             // ī�޶� �Ÿ�
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;

    private float CurrentX = 0.0f;                  // ���� ȸ�� ����
    private float CurrentY = 45.0f;                 // ���� ȸ�� ����
        
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    public float mouseSensitivity = 200f;           // ���콺 ����

    public float radius = 5.0f;                     // 3��Ī ī�޶�� �÷��̾� ���� �Ÿ�
    public float minRadius = 1.0f;                  // ī�޶� �ּ� �Ÿ�
    public float maxRadius = 10.0f;                 // ī�޶� �ִ� �Ÿ�

    public float yMinLimit = 30;                    // ī�޶� ���� ȸ�� �ּҰ�
    public float yMaxLimit = 90;                    // ī�޶� ���� ȸ�� �ִ밢

    private float theta = 0.0f;                     // ī�޶��� ���� ȸ�� ����
    private float phi = 0.0f;                       // ī�޶��� ���� ȸ�� ����
    private float targetVerticalRoatation = 0;      // ��ǥ ���� ȸ�� ����
    private float verticalRotationSpeed = 240f;     // ���� ȸ�� �ӵ�

    // ���� ������
    public bool isFirstPerson = true;   // 1��Ī ��� ���� ����
    //private bool isGrounded;            // �÷��̾ ���� �ִ��� ����
    private Rigidbody rb;               // �÷��̾��� Rigidbody

    public float fallingThreshold = -0.1f;          // �������°����� ������ ���� �ӵ��Ӱ谪

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                 // ��� ������ �ִ� ��� ����
    public const int groundCheckPoints = 5;         // ���� üũ ����Ʈ ��

    void Start()
    {
        rb = GetComponent<Rigidbody>();             // Rigidbody ������Ʈ�� �����´�
                                                    
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ���� ��װ� �����
        SetupCameras();
        SetActiveCamera();
    }

    void Update()
    {        
        HandleRotation();
        HandleCameraToggle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Ȱ��ȭ�� ī�޶� �����ϴ� �Լ�
    void SetActiveCamera()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);  // 1��Ī ī�޶� Ȱ��ȭ ����
        thridPersonCamera.gameObject.SetActive(!isFirstPerson);  // 3��Ī ī�޶� Ȱ��ȭ ����
    }

    // ī�޶� �� ĳ���� ȸ�� ó���ϴ� �Լ�
    public void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (isFirstPerson)
        {
            // 1��Ī ī�޶� ������ ����
            transform.rotation = Quaternion.Euler(0.0f, CurrentX, 0.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(CurrentY, 0.0f, 0.0f);
        }
        else
        {
            // 3��ġ ī�޶� ���� ����
            CurrentX += mouseX;
            CurrentY -= mouseY;

            // ���� ȸ�� ����
            CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            // ī�޶� ��ġ �� ȸ�� ���
            Vector3 dir = new Vector3(0, 0, -cameraDistance);
            Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);
            thridPersonCamera.transform.position = transform.position + rotation * dir;
            thridPersonCamera.transform.LookAt(transform.position);

            // �� ó��
            cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
        }
    }

    // 1��Ī�� 3��Ī ī�޶� ��ȯ�ϴ� �Լ�
    public void HandleCameraToggle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;     // ī�޶� ��� ��ȯ
            SetActiveCamera();
        }
    }

    // ī�޶� �ʱ� ��ġ �� ȸ���� �����ϴ� �Լ�
    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0f, 0.6f, 0f);      // 1��Ī ī�޶� ��ġ
        firstPersonCamera.transform.localRotation = Quaternion.identity;            // 1��Ī ī�޶� ȸ�� �ʱ�ȭ
    }

    // �÷��̾� ������ ó���ϴ� �Լ�
    void HandleJump()
    {
        // ���� ��ư�� ������ ���� ���� ��
        if (isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     // �������� ���� ���� ����
        }
    }

    // �÷��̾��� �̵��� ó���ϴ� �Լ�
    public void HandleMovement()
    {
        float moverHorizontal = Input.GetAxis("Horizontal");        // �¿� �Է� (-1, 1)
        float moverVertical = Input.GetAxis("Vertical");            // �յ� �Է� (1, -1)

        Vector3 movement;

        if (!isFirstPerson)     // 3��Ī ��� �� ��, ī�޶� �������� �̵� ó��
        {
            Vector3 cameraForward = thridPersonCamera.transform.forward;    // ī�޶� �� ����
            cameraForward.y = 0f;           // ���� ���� ����
            cameraForward.Normalize();      // ���� ���� ����ȭ (0~1) ������ ������ ������ش�

            Vector3 cameraRight = thridPersonCamera.transform.right;        // ī�޶� ������ ����
            cameraRight.y = 0f;
            cameraRight.Normalize();

            // �̵� ���� ���
            movement = cameraForward * moverVertical + cameraRight * moverHorizontal;
        }
        else
        {
            // ĳ���� �������� �̵�
            movement = transform.right * moverHorizontal + transform.forward * moverVertical;
        }  

        // �̵� �������� ĳ���� ȸ��
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    public bool isFalling()     // �������� ���� Ȯ��
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public bool isGrounded()    // �� üũ Ȯ��
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public float GetVerticalVelocitiy()     // �÷��̾��� Y�� �ӵ� Ȯ��
    {
        return rb.velocity.y;
    }
}