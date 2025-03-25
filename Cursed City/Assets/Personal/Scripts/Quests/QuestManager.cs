using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using TMPro;

public class QuestManager : Singleton<QuestManager>
{
    public int index = 0;
    public List<Quest> quests = new List<Quest>();
    [ReadOnly] public Quest currentQuest;

    [HorizontalLine]
    public TMP_Text questTitle;
    public TMP_Text progressText;
    public TMP_Text description;

    void Start() 
    {
        
    }

    public void StartQuest(string questName)
    {
        foreach (var q in quests)
        {
            if(q.questName == questName && !q.started)
            {
                questTitle.text = q.questName;
                q.StartQuest();
                //index += 1;
                //currentQuest = q;
            }
        }
    }

    void Update() 
    {
        if(index >= quests.Count) index = quests.Count - 1;
        currentQuest = quests[index];
        if (currentQuest.started) UpdateProgressText();
        if(Input.GetKeyDown(KeyCode.X))
        {
           StartQuest(currentQuest.questName);
           description.text = currentQuest.questDescription;
        }
        if (currentQuest.finished && Input.GetKeyDown(KeyCode.X))
        {
            index += 1;
            if(currentQuest.questType == Quest.QuestType.INT || currentQuest.questType == Quest.QuestType.FLOAT)
            {
                Currency.Instance.Earn(1000);
            }
            else{
                Currency.Instance.Earn(2000);
            }
        }
        
        
    }

    void UpdateProgressText()
    {
        if(currentQuest == null) return;

        if(currentQuest.questType == Quest.QuestType.INT)
        {
            progressText.text = currentQuest.intProgress + "/" + currentQuest.maxIntProgress;
        }
        else if(currentQuest.questType == Quest.QuestType.FLOAT)
        {
            progressText.text = currentQuest.floatProgress + "/" + currentQuest.maxFloatProgress;
        }
        else
        {
            progressText.text = currentQuest.finished? " FINISHED" : "NOT FINISHED";
        }
    }
}
