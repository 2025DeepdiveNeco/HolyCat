using UnityEngine;

public class DontTouchComponent : BaseInteractComponent
{
    protected override void OnInteract(Transform ts)
    {
        base.OnInteract(ts);
        GameManager.Instance.CheckGameOver();
    }
}
