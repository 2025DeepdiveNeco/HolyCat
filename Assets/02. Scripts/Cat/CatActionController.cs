using UnityEngine;
using UnityEngine.InputSystem;

public class CatActionController : MonoBehaviour
{
    [Header("이동 및 점프")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("공격 콤보")]
    private int comboStep = 0;
    private float lastAttackTime;
    public float comboWindow = 0.5f; // 다음 공격을 누를 수 있는 대기 시간

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>(); // 자식 Visual에서 찾음
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleJump();
    }

    void HandleMovement()
    {
        float x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        float y = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
        Vector2 move = new Vector2(x, y).normalized;

        rb.linearVelocity = move * moveSpeed;

        // 애니메이션 파라미터 업데이트
        bool isMoving = move.magnitude > 0;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("DirX", x);
            anim.SetFloat("DirY", y);
            
            if (x != 0) sr.flipX = (x > 0);
        }
    }

    void HandleAttack()
    {
        // 일정 시간 클릭 없으면 콤보 초기화
        if (Time.time - lastAttackTime > comboWindow)
        {
            comboStep = 0;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) // 좌클 공격
        {
            lastAttackTime = Time.time;

            anim.SetInteger("Combo", comboStep);
            anim.SetTrigger("Attack");

            // 콤보 단계 증가 (0 -> 1 -> 2 -> 다시 0)
            comboStep = (comboStep + 1) % 3;

            // 공격 중에는 이동 멈춤 (선택 사항)
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            anim.SetTrigger("Jump");
            // 실제 점프 물리 로직(높이 조절 등)은 여기에 추가하거나 
            // 이전에 만든 2D 점프 코드를 합치면 됩니다.
        }
    }
}