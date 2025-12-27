using System;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class BaseInteractComponent : MonoBehaviour, IInteractable, IDurability
{
    [Header("Object Setting")]
    ScoreTriggerType triggerType = ScoreTriggerType.None;
    [SerializeField] int objectID;
    [SerializeField] int damage = 10;

    [Header("Durability UI")]
    [SerializeField] Image durabilityGauge;

    [Header("Component")]
    Animator animator;

    [Header("Value")]
    int maxDurability;
    int durability;
    int breakLevels;
    int score;
    bool interactEnd;

    [Header("Damp")]
    [SerializeField] float amplitude = 0.3f;   // Èçµé¸² Å©±â
    [SerializeField] float frequency = 20f;    // Èçµé¸² ¼Óµµ
    [SerializeField] float damping = 5f;       // °¨¼è ¼Óµµ
    [SerializeField] float duration = 0.5f;    // Èçµé¸®´Â ½Ã°£

    public ScoreTriggerType TriggerType => triggerType;
    public int Durability => durability;

    public bool Interacting { get; private set; }

    public event Action OnInteractEnd;

    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>();
        SetObjectStat();

        durability = maxDurability;

        durabilityGauge.fillAmount = 1f;
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
    }

    public virtual void Interact()
    {
        if (EnterInteract())
            return;

        if (interactEnd)
            return;

        OnInteract();
    }

    protected virtual void OnInteract()
    {
        //animator.SetTrigger("interact");
        ReduceDurability(damage);
    }

    void ReduceDurability(int value)
    {
        durability -= value;

        StartCoroutine(Damping());

        if (durability <= 0)
        {
            durabilityGauge.fillAmount = 0;
            OnInteractEnd?.Invoke();
            interactEnd = true;
        }
        else
            durabilityGauge.fillAmount = (float)durability / (float)maxDurability;

        if (triggerType == ScoreTriggerType.OnHit)
            GameManager.Instance.AddScore(score);
        else if(triggerType == ScoreTriggerType.OnDestroy && durability <= 0)
            GameManager.Instance.AddScore(score);
    }

    System.Collections.IEnumerator Damping()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float damper = Mathf.Exp(-damping * t);
            float offset = Mathf.Sin(t * frequency) * amplitude * damper;

            transform.localPosition = Vector3.zero + new Vector3(offset, 0, 0);
            yield return null;
        }

        //if(durability > 0)
        //    transform.localPosition = Vector3.zero;

        ExitInteract();
    }
}
