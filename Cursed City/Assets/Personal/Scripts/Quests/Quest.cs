using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : MonoBehaviour
{
    public enum QuestType { INT, FLOAT, NONE };

    public string questName;
    public string questDescription;
    public QuestType questType;
    
    [HorizontalLine][ShowIf("IsIntType")] public int maxIntProgress;
    [HorizontalLine][ShowIf("IsIntType")] [ReadOnly] public int intProgress;

    [HorizontalLine][ShowIf("IsFloatType")] public float maxFloatProgress;
    [HorizontalLine][ShowIf("IsFloatType")] [ReadOnly] public float floatProgress;

    [HorizontalLine]
    [ReadOnly] public bool started;
    [ReadOnly] public bool finished;
    public UnityEvent OnFinish;

    public virtual void StartQuest() { started = true; }
    public virtual void FinishQuest()
    {
        finished = true;
        OnFinish?.Invoke();
    }
    public virtual void UpdateQuest() { }

    void Update()
    {
        UpdateQuest();
    }

    public bool IsIntType() => questType == QuestType.INT;
    public bool IsFloatType() => questType == QuestType.FLOAT;
}
