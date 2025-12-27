using System;
using UnityEngine;

public interface IInteractable : ITouchable
{
    bool Interacting { get; }
    void Interact(Transform ts);

    event Action OnInteractEnd;
}

public interface IHoldable : ITouchable
{
    bool Holding { get; }
    void Hold(Transform ts);
    void HoldUpdate(Transform ts);
    void Release();

    event Action OnHoldEnd;
}

public interface ITouchable
{
    bool Touchable();
    bool UnTouchable();
}

public interface ITriggerable { }


public interface IInteractInput { void TryInteract(); }
public interface IInteractStay { void HoldInteract(); }
public interface IInteractDelay { float Delay { get; } }
public interface IInteractEnable { }
public interface IInteractEnable<T> { Action<T> EndInvoke { get; } }
public interface IInteractSpawnable { }

public interface IProgress { }
public interface IProgress<T> : IProgress { event Action<T> OnProgress; }