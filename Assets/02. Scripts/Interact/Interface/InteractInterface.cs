using System;

public interface IInteractable
{
    bool Interacting { get; }
    void Interact();

    event Action OnInteractEnd;
}

public interface IHoldable
{
    bool Holding { get; }
    void Hold();

    event Action OnHoldEnd;
}

public interface IInteractInput { void TryInteract(); }
public interface IInteractStay { void HoldInteract(); }
public interface IInteractDelay { float Delay { get; } }
public interface IInteractEnable { }
public interface IInteractEnable<T> { Action<T> EndInvoke { get; } }
public interface IInteractSpawnable { }

public interface IProgress { }
public interface IProgress<T> : IProgress { event Action<T> OnProgress; }