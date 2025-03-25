using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class KillQuest : Quest
{
    [ReadOnly] public int startingKills;
    [ReadOnly] public int killGoal;

    public override void StartQuest()
    {
        base.StartQuest();
        if(questType != QuestType.INT) print("KILL QUEST USES INT TYPE TO WORK");

        intProgress = 0;
        startingKills = KillCount.Instance.killCount;
        killGoal = startingKills + maxIntProgress;
    }

    public override void UpdateQuest()
    {
        base.UpdateQuest();

        if(finished || QuestManager.Instance.quests[QuestManager.Instance.index] != this) return;
        
        intProgress = KillCount.Instance.killCount - startingKills;
        if(intProgress >= maxIntProgress) FinishQuest();
    }
}
