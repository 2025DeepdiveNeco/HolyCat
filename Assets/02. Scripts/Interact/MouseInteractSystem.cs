using UnityEngine;

public class MouseInteractSystem : MonoBehaviour
{
    [Header("RayCast 구 크기")]
    [Range(0, 10)]
    [SerializeField] float interactRadius = 3f;

    [Header("Set Layer Mask")]
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask HoldLayer;

    Collider[] buffer;
    IHoldable currentHold;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryInteract();

        if (Input.GetMouseButton(0) && currentHold != null)
            currentHold.HoldUpdate(transform);

        if (Input.GetMouseButtonUp(0))
        {
            if (currentHold != null)
            {
                currentHold.Release();
                currentHold = null;
            }
        }
    }

    void TryInteract()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //var dis = mousePos - (Vector2)transform.position;
        //if (dis.magnitude > interactRadius)
        //    return;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, interactableLayer);
        if (hit.collider != null)
        {
            IInteractable currentTarget = hit.collider.GetComponent<IInteractable>();
            currentTarget?.Interact(transform);
        }

        RaycastHit2D hitHold = Physics2D.Raycast(mousePos, Vector2.zero, 0f, HoldLayer);
        if (hitHold.collider != null)
        {
            currentHold = hitHold.collider.GetComponent<IHoldable>();
            currentHold.Hold(transform);
        }
    }

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