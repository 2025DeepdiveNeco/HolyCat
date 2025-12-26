using GameInteract;
using System;
using UnityEditor;
using UnityEngine;
using static Define;

public class BaseInteractComponent : MonoBehaviour, IInteractable, IDurability
{
    [SerializeField] EnityType enityType = EnityType.None;
    [SerializeField] ScoreTriggerType triggerType = ScoreTriggerType.None;

    [Header("Component")]
    Animator animator;

    [Header("Value")]
    float maxDurability;
    float durability;
    int breakLevels;

    public EnityType Type => enityType;
    public ScoreTriggerType TriggerType => triggerType;
    public float Durability => durability;

    public bool Interacting { get; private set; }

    public event Action OnInteractEnd;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void SetObjectStat()
    {

    }

    protected virtual bool EnterInteract()
    {
        if (!Interacting)
        {
            Interacting = true;
            return false;
        }
        else
            return true;
    }

    protected virtual void ExitInteract()
    {
        Interacting = false;
        EndInteract();
    }

    public virtual void Interact()
    {
        if (EnterInteract())
            return;

        OnInteract();
    }

    protected virtual void OnInteract()
    {
        animator.SetTrigger("interact");
        ReduceDurability(1.0f);
    }

    protected virtual void EndInteract()
    {
        OnInteractEnd?.Invoke();
    }

    public void Animation_InteractEnd()
    {
        ExitInteract();
    }

    public void ReduceDurability(float value)
    {
        durability -= value;
    }
}
