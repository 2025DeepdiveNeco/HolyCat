using System;
using UnityEngine;

public class BaseHoldComponent : MonoBehaviour, IHoldable
{

    bool hoding;

    public bool Holding => hoding;

    public event Action OnHoldEnd;

    public void Hold()
    {
        OnHold();
    }

    protected virtual void OnHold()
    {
        Debug.Log("잡고 있는 중...");
    }

    [SerializeField] float openRequiredAngle = 70f;  // 몇 도 이상 돌려야 열릴지
    [SerializeField] float maxDistance = 3f;         // 문에서 너무 멀리 가면 취소

    bool isInteracting = false;
    bool isOpened = false;

    [SerializeField]  Transform player;

    Vector2 prevDir;
    float accumulatedAngle = 0f;


    void Update()
    {
        if (isOpened) return;

        if (Input.GetMouseButtonDown(0))
        {
            // 문 클릭했는지 체크 (Raycast 사용)
            if (IsClickedDoor())
                StartInteract();
        }

        if (Input.GetMouseButton(0) && isInteracting)
        {
            InteractUpdate();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopInteract();
        }
    }

    bool IsClickedDoor()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse2D = mouseWorld;

        Collider2D col = Physics2D.OverlapPoint(mouse2D);
        return col != null && col.transform == transform;
    }

    void StartInteract()
    {
        if (Vector2.Distance(player.position, transform.position) > maxDistance)
            return;

        Vector2 dir = (player.position - transform.position).normalized;

        prevDir = dir;
        accumulatedAngle = 0f;

        isInteracting = true;
    }

    void InteractUpdate()
    {
        if (Vector2.Distance(player.position, transform.position) > maxDistance)
        {
            StopInteract();
            return;
        }

        Vector2 curDir = (player.position - transform.position).normalized;

        float delta = Vector2.SignedAngle(prevDir, curDir);

        accumulatedAngle += delta;
        prevDir = curDir;

        // Debug
        Debug.Log($"누적 각도: {accumulatedAngle}");

        if (Mathf.Abs(accumulatedAngle) >= openRequiredAngle)
        {
            OpenDoor();
        }
    }

    void StopInteract()
    {
        isInteracting = false;
        accumulatedAngle = 0;
    }

    void OpenDoor()
    {
        isOpened = true;
        isInteracting = false;
        Debug.Log("문 열림!");

        // 애니메이션 호출 or 회전 등 처리
    }
}
