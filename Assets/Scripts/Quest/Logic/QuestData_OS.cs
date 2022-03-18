using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Quest", menuName = "Quest/Quest Data")]
public class QuestData_OS : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire {
        public string name;
        public int requireAmount;
        public int currentAmount;

    }

    public string questName;
    [TextArea]
    public string description;
    public bool isStarted;
    public bool isComplete;
    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();
}
