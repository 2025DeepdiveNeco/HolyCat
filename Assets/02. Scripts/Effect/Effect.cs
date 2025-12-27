using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
    Action OnAction;
    
    public void Init(Define.CupType type)
    {
        OnAction = type == Define.CupType.MugCup ? MugCup : Jucie;
        OnAction?.Invoke();
    }

    void MugCup()
    {

    }

    void Jucie()
    {

    }

    public void DestroyEffect() => Destroy(gameObject);
}
