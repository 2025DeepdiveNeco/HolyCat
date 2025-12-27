using UnityEngine;

public class DoorHoldComponent : BaseHoldComponent
{
    [SerializeField] float openRequiredAngle = 60f;
    [SerializeField] float maxDistance = 3f;

    Vector2 startDir;
    float accumulatedAngle = 0f;

    bool opened = false;

    protected override void OnHoldStart(Transform ts)
    {
        base.OnHoldStart(ts);

        startDir = (ts.position - transform.position).normalized;
        accumulatedAngle = 0f;
    }

    protected override void OnHolding(Transform ts)
    {
        if (opened) return;

        if (Vector2.Distance(ts.position, transform.position) > maxDistance)
        {
            Release();
            return;
        }

        Vector2 curDir = (ts.position - transform.position).normalized;

        float delta = Vector2.SignedAngle(startDir, curDir);

        accumulatedAngle += delta;
        startDir = curDir;

        transform.rotation = Quaternion.Euler(0, 0, accumulatedAngle);


        if (Mathf.Abs(accumulatedAngle) >= openRequiredAngle)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        opened = true;
        Release();
    }

    protected override void OnHoldReleased()
    {
        base.OnHoldReleased();
        accumulatedAngle = 0;
    }
}
