using UnityEngine;

public class JucieWater : MonoBehaviour
{
    [SerializeField] Transform[] randomPos;

    void Awake()
    {
        int index1 = Random.Range(0, 9);
        int index2 = Random.Range(0, 9);
        if (index1 == index2) index2++;

        Instantiate(Resources.Load<GameObject>("Spike"), randomPos[index1].position, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Spike"), randomPos[index2].position, Quaternion.identity);
    }
}
