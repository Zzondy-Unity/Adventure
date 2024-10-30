using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Cursor = UnityEngine.Cursor;
using System;


public class PlayerController : MonoBehaviour
{

    [Header("ToggleView")]
    [SerializeField] private CinemachineFreeLook TPCamera;
    [SerializeField] private CinemachineVirtualCamera FPCamera;
    [SerializeField] private GameObject Crosshair;
    private Camera mainCamera;
    public bool isTPView = true;

    private Animator animator;
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private LayerMask Ground;
    private Vector3 inputVector;
    private float moveSpeed;
    private float jumpPower;
    private float runSpeed;
    private float minRunSpeed = 1;
    private float runningStaminaDecay;
    public bool isRunning;

    [Header("Mouse")]
    [SerializeField] private LayerMask playerLayerMask;
    private Vector2 mouseDelta;
    private float mouseSensitivity;
    private float minVerticalAngle = -80;
    private float maxVerticalAngle = 80;
    private float verticalRotation;

    public event Action Jump;
    public event Action ToggleInventory;

    private bool isOpenInventory = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        Crosshair.SetActive(false);
        FPCamera.enabled = false;
        TPCamera.enabled = true;
        isTPView = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (!isOpenInventory)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if(!isOpenInventory)
        CameraLook();
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.one;
        if (isTPView)
        {
            moveDirection = Camera.main.transform.forward * inputVector.y + Camera.main.transform.right * inputVector.x;
        }
        else
        {
            moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        }

        moveDirection.Normalize();

        if (isRunning && CharacterManager.Instance.player.condition.UseStamina(runningStaminaDecay * Time.deltaTime))
        {
            moveDirection *= runSpeed;
        }
        else
        {
            moveDirection *= minRunSpeed;
        }
        Vector3 horizontalVector = moveDirection * moveSpeed;
        rb.velocity = new Vector3(horizontalVector.x, rb.velocity.y, horizontalVector.z);

        animator.SetFloat("dirX", inputVector.x * moveSpeed);
        animator.SetFloat("dirZ", inputVector.y * moveSpeed);
        animator.SetFloat("Speed", (moveDirection * moveSpeed).magnitude);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveSpeed = CharacterManager.Instance.player.playerData.speed;

        if (context.phase == InputActionPhase.Performed)
        {
            inputVector = context.ReadValue<Vector2>().normalized;
            animator.SetBool("Moving", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            inputVector = Vector3.zero;
            animator.SetBool("Moving", false);
        }
    }

    public void OnToggleView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isTPView)
            {
                FPCamera.enabled = true;
                TPCamera.enabled = false;
                isTPView = false;
            }
            else
            {
                FPCamera.enabled = false;
                TPCamera.enabled = true;
                isTPView = true;
            }

            Crosshair.SetActive(!Crosshair.activeSelf);
            inputVector = Vector3.zero;
            animator.SetBool("Moving", false);

        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseSensitivity = CharacterManager.Instance.player.playerData.mouseSensitivity;
        mouseDelta = context.ReadValue<Vector2>();
    }

    //시네머신으로 대체
    private void CameraLook()
    {
        verticalRotation -= mouseDelta.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        if (isTPView)
        {
            // 3인칭: 카메라가 회전
            TPCamera.m_XAxis.Value += mouseDelta.x * mouseSensitivity;
            TPCamera.m_YAxis.Value = verticalRotation * 0.005f; // 수직 회전
            Vector3 cameraRotation = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);

        }
        else
        {
            // 1인칭: 캐릭터가 회전
            transform.eulerAngles += new Vector3(0, mouseDelta.x * mouseSensitivity, 0);
            FPCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
        }
    }

    private bool IsGround()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(transform.position + Vector3.forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + -Vector3.forward* 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + Vector3.right * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + -Vector3.forward * 0.2f + Vector3.up * 0.01f, Vector3.down)
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.1f, Ground))
            {
                return true;
            }
        }
        return false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpPower = CharacterManager.Instance.player.playerData.jumpPower;
        float jumpStamina = CharacterManager.Instance.player.playerData.jumpStamina;

        if (context.phase == InputActionPhase.Started && IsGround())
        {
            if (CharacterManager.Instance.player.condition.UseStamina(jumpStamina / 2))
            {
                Jump?.Invoke();
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            }
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        runningStaminaDecay = CharacterManager.Instance.player.playerData.runningStaminaDecay;
        if (context.phase == InputActionPhase.Performed)
        {
            runSpeed = CharacterManager.Instance.player.playerData.runSpeed;
            isRunning = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            runSpeed = minRunSpeed;
            isRunning = false;
        }
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ToggleInventory?.Invoke();
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.Confined : CursorLockMode.Locked;
            isOpenInventory = !isOpenInventory;
        }
    }
}
