using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.Rendering;

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
        FindInteractable();

        if (Input.GetMouseButtonDown(0))
            TryInteract();

        if (Input.GetMouseButton(0) && currentHold != null)
            currentHold.HoldUpdate();

        if (Input.GetMouseButtonUp(0))
        {
            if (currentHold != null)
            {
                currentHold.Release();
                currentHold = null;
            }
        }
    }

    void FindInteractable()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            interactRadius,
            buffer,
            interactableLayer
        );

        for(int i = 0; i < hitCount; i++)
        {
            Debug.Log(buffer[i].name);
            buffer[i].GetComponent<Renderer>().material.SetFloat("_Thickness", 0.002f);
        }
    }

    void TryInteract()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var dis = mousePos - (Vector2)transform.position;
        if (dis.magnitude > interactRadius)
            return;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, interactableLayer);
        if (hit.collider != null)
        {
            IInteractable currentTarget = hit.collider.GetComponent<IInteractable>();
            currentTarget?.Interact();
        }

        RaycastHit2D hitHold = Physics2D.Raycast(mousePos, Vector2.zero, 0f, HoldLayer);
        if (hitHold.collider != null)
        {
            currentHold = hitHold.collider.GetComponent<IHoldable>();
            currentHold.Hold();
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