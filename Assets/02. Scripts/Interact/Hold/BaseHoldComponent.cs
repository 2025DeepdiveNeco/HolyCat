using System;
using UnityEngine;

public class BaseHoldComponent : BaseInteract, IHoldable
{

    bool holding;
    bool canTouch;

    public bool Holding => holding;

    public bool Touchable() => canTouch = true;

    public bool UnTouchable() => canTouch = false;

    public event Action OnHoldEnd;

    public void Hold(Transform ts)
    {
        if (!canTouch)
            return;

        holding = true;
        OnHoldStart(ts);
    }

    public void HoldUpdate(Transform ts)
    {
        if (!holding)
            return;

        OnHolding(ts);
    }

    public void Release()
    {
        if (!holding)
            return;

        holding = false;
        OnHoldEnd?.Invoke();
        OnHoldReleased();
    }

    protected virtual void OnHoldStart(Transform ts)
    {
        Debug.Log("Hold 시작");
    }

    protected virtual void OnHolding(Transform ts)
    {

    }

    protected virtual void OnHoldReleased()
    {
        Debug.Log("Hold 종료");
    }
}
