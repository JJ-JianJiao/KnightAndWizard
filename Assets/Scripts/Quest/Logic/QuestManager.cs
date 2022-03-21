using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask {
        public QuestData_OS questData;
        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }

        public QuestTask(QuestData_OS data) {
            questData = data;
        }

        public QuestTask() { }
    }

    internal void FinishQuest(string name)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].questData.questName.Contains(name)) {
                tasks[i].questData.isComplete = true;
                for (int j = 0; j < tasks[i].questData.questRequires.Count; j++)
                {
                    tasks[i].questData.questRequires[j].currentAmount = tasks[i].questData.questRequires[j].requireAmount;
                    LevelManager.Instance.isBigBrotherSaved = true;
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    public void SaveQuestSystem() {


        //var questTasks = ScriptableObject.CreateInstance<QuestTasks_OS>();
        //for (int i = 0; i < tasks.Count; i++)
        //{
        //    questTasks.quests.Add(tasks[i].questData);
        //}
        //SaveManager.Instance.SaveTasks(questTasks, "Quests");

        SaveManager.Instance.SaveTasks(tasks, "QuestsInfo");
    }


    public bool HaveQuest(QuestData_OS data) {
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);
        else
            return false;
    }

    public QuestTask GetTask(QuestData_OS data) {
        return tasks.Find(q => q.questData.questName == data.questName);
    }

    //enemy die or pick up something
    public void UpdateQuestProgress(string requireName, int amount) {
        foreach (var task in tasks)
        {
            if (task.IsFinished)
                continue;
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if (matchTask != null) {
                matchTask.currentAmount += amount;
            }
            task.questData.CheckQuestProgress();
        }
    }

}
