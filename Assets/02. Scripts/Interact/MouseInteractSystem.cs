using GameInteract;
using UnityEngine;


public class MouseInteractSystem : MonoBehaviour
{
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

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("Interact"));

        if (hit.collider != null)
        {
            IInteractable currentTarget = hit.collider.GetComponent<IInteractable>();

            currentTarget.Interact();
            // CurrentState = InteractionState.Interacting;
            //CurrentTarget.OnInteractionEnded += HandleInteractionEnded;
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
}