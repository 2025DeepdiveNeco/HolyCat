using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("시각적 요소 (자식 오브젝트)")]
    public Transform visualTransform;
    public SpriteRenderer playerVisualSr;
    private Vector3 visualOriginalPos;

    [Header("점프 및 높이 설정")]
    public float jumpForce = 6f;
    public float gravity = -16f;
    public int defaultSortingOrder = 2;
    private float verticalVelocity;
    private float currentHeight;
    private bool isGrounded = true;
    private bool isUnderObject = false; // 머리 위 장애물 체크

    [Header("공격 콤보 설정")]
    public float comboWindow = 0.5f;
    private int comboStep = 0;
    private float lastAttackTime;

    [Header("Water Step 설정")]
    [SerializeField] float waterStepOffSetDistance = 1.5f; // 이펙트 생성 간격 (수정됨)
    bool isWaterStep;  // 이펙트 생성 중인지
    bool onWaterStep;  // 물 위에 있는지
    Vector2 startWaterStep; // 이펙트 기준 위치

    public void OnWaterStep() => onWaterStep = true;
    public void OffWaterStep() => onWaterStep = false; // 물에서 나갔을 때 호출용

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        if (visualTransform != null)
        {
            visualOriginalPos = visualTransform.localPosition;
            if (playerVisualSr == null)
                playerVisualSr = visualTransform.GetComponent<SpriteRenderer>();
        }

        startWaterStep = transform.position;
    }

    void Update()
    {
        HandleInput();
        ApplyJumpPhysics();
        UpdateVisuals();

        // Water Step 로직 처리
        if (onWaterStep && isGrounded)
        {
            WaterStep();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        // 1. 이동 처리
        float x = (keyboard.dKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed ? 1 : 0);
        float y = (keyboard.wKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed ? 1 : 0);
        moveInput = new Vector2(x, y).normalized;

        bool isMoving = moveInput.magnitude > 0;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("DirX", x);
            anim.SetFloat("DirY", y);
            if (x != 0) playerVisualSr.flipX = (x > 0);
        }

        // 2. 점프 처리 (머리 위 체크)
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (isGrounded && !isUnderObject)
            {
                StartJump();
            }
            else if (isUnderObject)
            {
                Debug.Log("머리 박음! 점프 불가");
            }
        }

        // 3. 공격 처리
        if (mouse.leftButton.wasPressedThisFrame)
        {
            HandleAttack();
        }
    }

    private void StartJump()
    {
        isGrounded = false;
        verticalVelocity = jumpForce;
        anim.SetTrigger("Jump");
    }

    private void ApplyJumpPhysics()
    {
        if (isGrounded) return;

        verticalVelocity += gravity * Time.deltaTime;
        currentHeight += verticalVelocity * Time.deltaTime;

        if (currentHeight <= 0)
        {
            currentHeight = 0;
            verticalVelocity = 0;
            isGrounded = true;
        }

        if (visualTransform != null)
        {
            visualTransform.localPosition = new Vector3(
                visualOriginalPos.x,
                visualOriginalPos.y + currentHeight,
                visualOriginalPos.z
            );
        }
    }

    private void HandleAttack()
    {
        if (Time.time - lastAttackTime > comboWindow)
        {
            comboStep = 0;
        }

        lastAttackTime = Time.time;
        anim.SetInteger("Combo", comboStep);
        anim.SetTrigger("Attack");

        comboStep = (comboStep + 1) % 3;
        rb.linearVelocity = Vector2.zero;
    }

    // --- Water Step 로직 ---
    void WaterStep()
    {
        float distance = Vector2.Distance(startWaterStep, transform.position);

        if (distance >= waterStepOffSetDistance)
        {
            startWaterStep = transform.position;
            // Resources/Effect/ObjectEffect 경로에 프리팹이 있어야 합니다.
            GameObject effect = Resources.Load<GameObject>("Effect/ObjectEffect");
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Water Step 이펙트를 찾을 수 없습니다: Effect/ObjectEffect");
            }
        }
    }

    private void UpdateVisuals()
    {
        // 공중 상태일 때 정렬 순서 조정
        if (!isGrounded)
        {
            if (playerVisualSr.sortingOrder <= defaultSortingOrder)
            {
                playerVisualSr.sortingOrder = defaultSortingOrder + 1;
            }
        }
    }

    // --- 외부 호출용 인터페이스 (FadeObstacle 등) ---

    public bool IsJumping() => !isGrounded;

    public void SetIsUnderObject(bool value) => isUnderObject = value;

    public float GetCurrentHeight() => currentHeight;
}