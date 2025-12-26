using System;
using TMPro;
using UnityEngine;
using static Define;

public class BaseInteractComponent : MonoBehaviour, IInteractable, IDurability
{
    [SerializeField] EnityType enityType = EnityType.None;
    [SerializeField] ScoreTriggerType triggerType = ScoreTriggerType.None;
    [SerializeField] int objectID;
    [SerializeField] int damage = 10;

    [Header("Test")]
    [SerializeField] TextMeshProUGUI maxD;
    [SerializeField] TextMeshProUGUI curD;

    [Header("Component")]
    Animator animator;

    [Header("Value")]
    int maxDurability;
    int durability;
    int breakLevels;
    int score;

    public EnityType Type => enityType;
    public ScoreTriggerType TriggerType => triggerType;
    public int Durability => durability;

    public bool Interacting { get; private set; }

    public event Action OnInteractEnd;

    void Awake()
    {
        animator = GetComponent<Animator>();
        SetObjectStat();

        durability = maxDurability;

        maxD.text = $"maxD = {maxDurability.ToString()}";
        curD.text = $"curD = {durability.ToString()}";
    }

    void Update()
    {
        curD.text = $"curD = {durability.ToString()}";
    }

    void SetObjectStat()
    {
        var objectData = DataTableManager.Instance.GetObjectData(objectID);

        maxDurability = objectData.MaxDurability;
        triggerType = (ScoreTriggerType)objectData.ScoreTriggerType;
        score = objectData.ScoreValue;
        breakLevels = objectData.BreakLevels;
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
        ReduceDurability(damage);
        GameManager.Instance.AddScore(score);
    }

    protected virtual void EndInteract()
    {
        OnInteractEnd?.Invoke();
    }

    public void Animation_InteractEnd()
    {
        ExitInteract();
    }

    void ReduceDurability(int value)
    {
        durability -= value;
    }
}
