using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new tasks",menuName ="Quest/Tasks")]
public class QuestTasks_OS : ScriptableObject
{
    public List<QuestData_OS> quests = new List<QuestData_OS>();
}
