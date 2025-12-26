using UnityEngine;


public class MouseInteractSystem : MonoBehaviour
{
    [Header("RayCast ±¸ Å©±â")]
    [Range(0, 10)]
    [SerializeField] float interactRadius = 3f;

    [Header("Set Layer Mask")]
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask HoldLayer;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }
    }
    public void TryInteract()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var dis = mousePos - new Vector2(transform.position.x, transform.position.y);

        if (dis.magnitude > interactRadius)
            return;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, interactableLayer);
        RaycastHit2D hitHold = Physics2D.Raycast(mousePos, Vector2.zero, 0f, HoldLayer);

        if (hit.collider != null)
        {
            IInteractable currentTarget = hit.collider.GetComponent<IInteractable>();

            currentTarget.Interact();
            // CurrentState = InteractionState.Interacting;
            //CurrentTarget.OnInteractionEnded += HandleInteractionEnded;
        }

        if (hitHold.collider != null)
        {

        }
    }

    //public void HoldInteract()
    //{
    //    if (CurrentState == InteractionState.Interacting && CurrentTarget is IInteractStay stay)
    //    {
    //        stay.HoldInteract();
    //        CurrentState = InteractionState.Holding;
    //    }
    //}
    //void HandleInteractionEnded()
    //{
    //    if (CurrentTarget != null)
    //        CurrentTarget.OnInteractionEnded -= HandleInteractionEnded;
    //    EndInteract();
    //}

    //public void EndInteract()
    //{
    //    if (CurrentTarget != null) CurrentTarget.ExitInteract();
    //    CurrentState = InteractionState.None;
    //    InteractInvoke.Invoke(0);
    //}


#if UNITY_EDITOR
    [Header("Draw Gizmo")]
    [SerializeField] bool drawGizmo = true;
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Vector3 originPos = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(originPos, interactRadius);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(originPos, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f);
        }
    }
#endif
}