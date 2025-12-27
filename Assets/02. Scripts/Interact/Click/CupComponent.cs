using UnityEngine;

public class CupComponent : BaseInteractComponent
{
    [SerializeField] float pushDistance = 2f;
    [SerializeField] float pushSpeed = 5f;
    [SerializeField] GameObject gauge;

    Transform player;
    int count = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        OnInteractEnd += StartPush;
    }

    private void OnDisable()
    {
        OnInteractEnd -= StartPush;
    }

    protected override void OnInteract(Transform ts)
    {
        base.OnInteract(ts);
        count++;
        player = ts;
        animator.SetInteger("broken", count);
    }

    void StartPush()
    {
        gauge.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Push());
    }

    System.Collections.IEnumerator Push()
    {
        Vector2 start = transform.position;

        Vector2 dir = ((Vector2)transform.position - (Vector2)player.transform.position).normalized;
        Vector2 end = start + dir * pushDistance;

        float t = 0f;
        while (t < 3f)
        {
            t += Time.deltaTime * pushSpeed;
            transform.position = Vector2.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;

        GameObjectDestroy();
        OnEffect();
    }
}
