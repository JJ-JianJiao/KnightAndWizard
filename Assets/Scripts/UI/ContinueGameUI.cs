using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGameUI : MonoBehaviour
{

     
    public GameObject profileUI;

    public GameObject frameBoard;

    public List<GameObject> playerProfiles;

    private void Awake()
    {

    }
    private void OnEnable()
    {
        PlayersProfile playersProfile = SaveManager.Instance.LoadPlayersProfles();

        if (playersProfile.Length > 0)
        {
            CreatePlayersProfileUIs(playersProfile);
        }
    }

    private void OnDisable()
    {
        foreach (var item in playerProfiles)
        {
            GameObject.Destroy(item);
        }
    }

    //private void Start()
    //{
    //    playersProfile = SaveManager.Instance.LoadPlayersProfles();

    //    if (playersProfile.Length > 0)
    //    {
    //        CreatePlayersProfileUIs(playersProfile);
    //    }
    //}

    private void CreatePlayersProfileUIs(PlayersProfile profiles)
    {
        //foreach (var name in profiles.playerFileNameList)
        //{
        //    CharacterProfile cp = SaveManager.Instance.LoadPlayerProfileXML(name);
        //}

        for (int i = 0; i < profiles.playerFileNameList.Count; i++)
        {
            CharacterProfile cp = SaveManager.Instance.LoadPlayerProfileXML(profiles.playerFileNameList[i]);
            var ui = Instantiate(profileUI, frameBoard.transform);
            playerProfiles.Add(ui);
            ui.GetComponent<PlayerContinueInfoUIController>().SetCharacterProfileData(cp);
            ui.GetComponent<PlayerContinueInfoUIController>().SetInfoText((i+1).ToString(), cp.name, cp.characterClass, "Level " + cp.currentLevel.ToString());
        }
    }

}
