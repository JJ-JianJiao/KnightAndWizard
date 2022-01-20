using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ExitSelectCharacter : MonoBehaviour
{
    Button exitToMain;

    public PlayableDirector exitSelectCharacterDirector;
    public SelectCharacterUI knightSelectCharacter;
    public SelectCharacterUI wizardSelectCharacter;

    private void Awake()
    {
        exitToMain = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        exitToMain.onClick.AddListener(ExitSelectCharacterUIToMain);
    }

    private void ExitSelectCharacterUIToMain()
    {
        if (knightSelectCharacter.gameObject.activeInHierarchy)
        {
            knightSelectCharacter.CloseAllBoard();
        }
        if (wizardSelectCharacter.gameObject.activeInHierarchy)
        {
            wizardSelectCharacter.CloseAllBoard();
        }
        exitSelectCharacterDirector.Play();
    }

}
