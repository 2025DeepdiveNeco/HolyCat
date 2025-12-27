using UnityEngine;
using UnityEngine.UI;

public class DoorHoldComponent : BaseHoldComponent
{
    [SerializeField] float openRequiredAngle = 60f;
    [SerializeField] float maxDistance = 3f;

    [Header("Hold Gauge")]
    [SerializeField] Image gauge;
    [SerializeField] float duration = 5f;
    float elapsed;

    bool opened = false;

    protected override void OnHoldStart(Transform ts)
    {
        base.OnHoldStart(ts);

        gauge.fillAmount = 0f;
        elapsed = 0f;
    }

    protected override void OnHolding(Transform ts)
    {
        if (opened) return;

        if (Vector2.Distance(ts.position, transform.position) > maxDistance)
        {
            Release();
            return;
        }

        elapsed += Time.deltaTime;
        gauge.fillAmount = elapsed / duration;

        if (gauge.fillAmount >= 1)
        {
            OpenDoor();
            gauge.gameObject.SetActive(false);
        }
    }

    void OpenDoor()
    {
        opened = true;
        Release();

        // TODO : 타일맵 애니메이션? 작동
    }

    protected override void OnHoldReleased()
    {
        base.OnHoldReleased();
        gauge.fillAmount = 0f;
    }
}
