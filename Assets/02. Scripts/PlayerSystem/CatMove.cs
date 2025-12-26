using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CatMove : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Aim")]
    public bool lookAtMouse = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Move 액션 콜백
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // 이동
        Vector2 dir = moveInput.normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        // 회전
        if (lookAtMouse)
        {
            RotateToMouse();
        }
        else
        {
            RotateToMoveDirection(dir);
        }
    }

    void RotateToMouse()
    {
        if (Camera.main == null) return;
        if (Mouse.current == null) return;

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = transform.position.z;

        Vector2 dir = (mouseWorld - transform.position);
        if (dir.sqrMagnitude <= 0.0001f) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        // 스프라이트가 위(Up)을 보고 있다면 -90 보정, 오른쪽(Right)이라면 빼지 말기

        rb.rotation = angle;
    }

    void RotateToMoveDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude <= 0.0001f) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
