using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialogue", menuName ="Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.id)) {
                dialogueIndex.Add(piece.id, piece);
            }
        }
    }
#else
    private void Awake()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.id))
            {
                dialogueIndex.Add(piece.id, piece);
            }
        }
    }
#endif

    public QuestData_OS GetQuest() {
        QuestData_OS tempQuest = null;
        foreach (var piece in dialoguePieces)
        {
            if (piece.quest != null) {
                tempQuest = piece.quest;
                break;
            }
        }
        return tempQuest;
    }
}
