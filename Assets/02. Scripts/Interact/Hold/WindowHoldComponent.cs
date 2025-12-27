using Unity.VisualScripting;
using UnityEngine;

public class WindowHoldComponent : BaseHoldComponent
{
    [SerializeField] float requiredMove = 1.5f;
    [SerializeField] float slideDistance = 1.5f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] bool vertical;
    [SerializeField] int right = 0;

    Vector3 originPos;
    Vector2 startPlayerPos;
    Vector2 allowDir;

    bool opened = false;

    private void Start()
    {
        originPos = transform.position;
        allowDir = right == 1 ? transform.right : transform.right * -1;
    }

    protected override void OnHoldStart(Transform ts)
    {
        base.OnHoldStart(ts);

        if (Vector2.Distance(ts.position, transform.position) > maxDistance)
            return;

        startPlayerPos = ts.position;
    }

    protected override void OnHolding(Transform ts)
    {
        if (opened)
            return;

        if (Vector2.Distance(ts.position, transform.position) > maxDistance)
        {
            Release();
            return;
        }

        Vector2 move = (Vector2)ts.position - startPlayerPos;

        float dotDir = Vector3.Dot(move.normalized, allowDir.normalized);

        if (dotDir < 0)
        {
            Release();
            return;
        }

        if (move.sqrMagnitude < 0.001f)
            return;

        Vector2 dir = move.normalized;
        float distance = move.magnitude;

        if (!vertical) dir.y = 0;
        else dir.x = 0;

        float t = Mathf.Clamp01(distance / requiredMove);

        Vector3 targetPos = originPos + (Vector3)(dir * slideDistance * t);
        transform.position = targetPos;

        if (t >= 1f)
            OpenWindow();
    }

    void OpenWindow()
    {
        opened = true;
        Release();
    }
}