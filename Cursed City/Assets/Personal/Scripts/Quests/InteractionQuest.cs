using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class InteractionQuest : Quest
{
    public int startScore = 0;
    public int finishedScore = 5;

    public override void StartQuest()
    {
        base.StartQuest();
    }

    public void QuestFinish()
    {
        base.FinishQuest();
    }
    public void addScore(int amount){
        if(startScore < finishedScore){
            startScore += amount;
            base.intProgress += amount;
            if(startScore >= finishedScore){
                QuestFinish();
            }
        }
    }
}
