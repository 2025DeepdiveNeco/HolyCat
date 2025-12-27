using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
    Action OnAction;
    Transform pos;

    public void Init(Define.CupType type, Transform ts)
    {
        pos = ts;
        OnAction = type == Define.CupType.MugCup ? MugCup : Jucie;
        OnAction?.Invoke();
    }

    void MugCup()
    {
        Instantiate(Resources.Load<GameObject>("Spike"), pos.position, Quaternion.identity);
    }

    void Jucie()
    {
        Instantiate(Resources.Load<GameObject>("Effect/JucieWater"), pos.position, Quaternion.identity);
    }

    public void DestroyEffect() => Destroy(gameObject);
}
