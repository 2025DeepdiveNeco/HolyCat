using UnityEngine;

public class BaseInteract : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GetComponent<Renderer>().material.SetFloat("_Thickness", 0.002f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GetComponent<Renderer>().material.SetFloat("_Thickness", 0f);
    }
}
