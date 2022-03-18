using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask {
        public QuestData_OS questData;
        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isStarted = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isStarted = value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    public bool HaveQuest(QuestData_OS data) {
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);
        else
            return false;
    }

    public QuestTask GetTask(QuestData_OS data) {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
