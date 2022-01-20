using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerContinueInfoUIController : MonoBehaviour
{
    public Button startContinueBtn;
    public Button deletePlayerProfileBtn;

    public TMP_Text boardIndex;
    public TMP_Text characterName;
    public TMP_Text characterClass;
    public TMP_Text characterLevel;

    CharacterProfile characterProfile;

    private void Awake()
    {
        startContinueBtn.onClick.AddListener(ContinueGame);
        deletePlayerProfileBtn.onClick.AddListener(DeleteProfile);
    }

    private void DeleteProfile()
    {
        Debug.Log(boardIndex.text + "Show a delete alert window");
    }

    private void ContinueGame()
    {
        Debug.Log(boardIndex.text + "Continue the game with the selected profile");
        DebugCharacterProfile(characterProfile);

        transform.root.GetComponent<CanvasGroup>().alpha = 0;
        transform.root.GetComponent<CanvasGroup>().interactable = false;
        SceneController.Instance.SetCurrentPlayerPrefab(characterProfile.characterClass);
        SceneController.Instance.TransitionToLoadGame(characterProfile);
    }

    public void SetInfoText(string index, string cName, string cClass, string cLevel) {
        boardIndex.text = index;
        characterName.text = cName;
        characterClass.text = cClass;
        characterLevel.text = cLevel;
    }

    public void SetCharacterProfileData(CharacterProfile cp) {
        characterProfile = cp;
    }

    private void DebugCharacterProfile(CharacterProfile cp)
    {

        Debug.Log(cp.GetBasicInfo());
        Debug.Log(cp.GetAttackInfo());

    }
}
