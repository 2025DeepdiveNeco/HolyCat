using UnityEngine;

public class JucieWater : MonoBehaviour
{
    [SerializeField] Transform[] randomPos;

    void Awake()
    {
        int index1 = Random.Range(0, 8);
        int index2 = Random.Range(0, 8);
        if (index1 == index2) index2++;
        index2 %= 8;

        Instantiate(Resources.Load<GameObject>("Spike"), randomPos[index1].position, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Spike"), randomPos[index2].position, Quaternion.identity);
    }
}
