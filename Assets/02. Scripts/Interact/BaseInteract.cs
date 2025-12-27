using UnityEngine;

public class BaseInteract : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<ITouchable>().Touchable();
            GetComponent<Renderer>().material.SetFloat("_Thickness", 0.003f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<ITouchable>().UnTouchable();
            GetComponent<Renderer>().material.SetFloat("_Thickness", 0f);
        }
    }
}
