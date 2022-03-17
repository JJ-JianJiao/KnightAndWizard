using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string id;
    public Sprite image;
    [TextArea]
    public string text;
    public List<DialogueOption> options = new List<DialogueOption>();
}
