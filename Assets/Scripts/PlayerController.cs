using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �÷��̾��� �����Ӽӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;  // �̵��ӵ�
    public float jumpForce = 5.0f;  // ���� ��

    // ī�޶� ���� ����
    [Header("Camera Settings")]
    public Camera firstPersonCamera;                // 1��Ī ī�޶�
    public Camera thridPersonCamera;                // 3��Ī ī�޶�

    public float radius = 5.0f;                     // 3��Ī ī�޶�� �÷��̾� ���� �Ÿ�
    public float minRadius = 1.0f;                  // ī�޶� �ּ� �Ÿ�
    public float maxRadius = 10.0f;                 // ī�޶� �ִ� �Ÿ�

    public float yMinLimit = 30;                    // ī�޶� ���� ȸ�� �ּҰ�
    public float yMaxLimit = 90;                    // ī�޶� ���� ȸ�� �ִ밢

    private float theta = 0.0f;                     // ī�޶��� ���� ȸ�� ����
    private float phi = 0.0f;                       // ī�޶��� ���� ȸ�� ����
    private float targetVerticalRoatation = 0;      // ��ǥ ���� ȸ�� ����
    private float verticalRotationSpeed = 240f;     // ���� ȸ�� �ӵ�

    public float mouseSenesitivity = 2f;            // ���콺 ����

    // ���� ������
    private bool isFirstPerson = true;  // 1��Ī ��� ���� ����
    private bool isGrounded;            // �÷��̾ ���� �ִ��� ����
    private Rigidbody rb;               // �÷��̾��� Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>();             // Rigidbody ������Ʈ�� �����´�
                                                    
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ���� ��װ� �����
        SetupCameras();
        SetActiveCamera();
    }

    void Update()
    {       
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleCameraToggle();
    }

    // Ȱ��ȭ�� ī�޶� �����ϴ� �Լ�
    void SetActiveCamera()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);  // 1��Ī ī�޶� Ȱ��ȭ ����
        thridPersonCamera.gameObject.SetActive(!isFirstPerson);  // 3��Ī ī�޶� Ȱ��ȭ ����
    }

    // ī�޶� �� ĳ���� ȸ�� ó���ϴ� �Լ�
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity;        // ���콺 �¿� �Է�
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity;        // ���콺 ���� �Է�

        // ���� ȸ�� (theta ��)
        theta += mouseX;                        // ���콺 �Է°� �߰�
        theta = Mathf.Repeat(theta, 360f);      // ���� ���� 360�� ���� �ʵ��� ����

        // ���� ȸ�� ó��
        targetVerticalRoatation -= mouseY;
        targetVerticalRoatation = Mathf.Clamp(targetVerticalRoatation, yMinLimit, yMaxLimit);   // ���� ȸ�� ����
        phi = Mathf.MoveTowards(phi, targetVerticalRoatation, verticalRotationSpeed * Time.deltaTime);

        // �÷��̾� ȸ��(ĳ���Ͱ� �������θ� ȸ��)
        transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);

        if (isFirstPerson)
        {
            firstPersonCamera.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);  // 1��Ī ī�޶� ���� ȸ��
        }
        else
        {
            // 3��Ī ī�޶� ���� ��ǥ�迡�� ��ġ �� ȸ�� ���
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * phi);
            float z = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Sin(Mathf.Deg2Rad * theta);

            thridPersonCamera.transform.position = transform.position + new Vector3(x, y, z);
            thridPersonCamera.transform.LookAt(transform);      // ī�޶� �׻� �÷��̾ �ٶ󺸵��� ����

            // ���콺 ��ũ���� ����Ͽ� ī�޶� �� ����
            radius = Mathf.Clamp(radius - Input.GetAxis("Mouse ScrollWheel") * 5, minRadius, maxRadius);
        }
    }

    // 1��Ī�� 3��Ī ī�޶� ��ȯ�ϴ� �Լ�
    void HandleCameraToggle()
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     // �������� ���� ���� ����
            isGrounded = false;                                         // ���߿� �ִ� ���·� ��ȯ
        }
    }

    // �÷��̾��� �̵��� ó���ϴ� �Լ�
    void HandleMovement()
    {
        float moverHorizontal = Input.GetAxis("Horizontal");        // �¿� �Է� (-1, 1)
        float moverVertical = Input.GetAxis("Vertical");            // �յ� �Է� (1, -1)

        if (!isFirstPerson)     // 3��Ī ��� �� ��, ī�޶� �������� �̵� ó��
        {
            Vector3 cameraForward = thridPersonCamera.transform.forward;    // ī�޶� �� ����
            cameraForward.y = 0f;           // ���� ���� ����
            cameraForward.Normalize();      // ���� ���� ����ȭ (0~1) ������ ������ ������ش�

            Vector3 cameraRight = thridPersonCamera.transform.right;        // ī�޶� ������ ����
            cameraRight.y = 0f;
            cameraRight.Normalize();

            // �̵� ���� ���
            Vector3 movement = cameraForward * moverVertical + cameraRight * moverHorizontal;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);       // ���� ��� �̵�
        }
        else
        {
            // ĳ���� �������� �̵�
            Vector3 movement = transform.right * moverHorizontal + transform.forward * moverVertical;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);       // ���� ��� �̵�
        }  
    }

    // �÷��̾ ���� ��� �ִ��� ����
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;      // �浹 ���̸� �÷��̾�� ���� �ִ�.
    }
}