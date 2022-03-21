using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public bool activeFriendKnight;
    public GameObject friendKnightPrefab;
    public bool activeStrangeKnight;

    public bool isDeadSlime1;
    public bool isDeadSlime2;
    public bool isDeadSlime3;

    public bool isDeadTurtle1;
    public bool isDeadTurtle2;
    public bool isDeadTurtle3;

    public bool isDeadGrunt1;
    public bool isDeadGrunt2;
    public bool isDeadGrunt3;

    public bool isDeadGolem1;

    public bool isChestPickUp;

    public bool isBigBrotherSaved;

    protected override void Awake()
    {
        base.Awake();
        InitialStates();
        DontDestroyOnLoad(this);
    }

    internal void ResetEnemies()
    {
        //activeFriendKnight = false;
        //activeStrangeKnight = false;

        isDeadSlime1 = false;
        isDeadSlime2 = false;
        isDeadSlime3 = false;

        isDeadTurtle1 = false;
        isDeadTurtle2 = false;
        isDeadTurtle3 = false;

        isDeadGrunt1 = false;
        isDeadGrunt2 = false;
        isDeadGrunt3 = false;

        isDeadGolem1 = false;
    }

    public void InitialStates() {
        activeFriendKnight = false;
        activeStrangeKnight = false;

        isDeadSlime1 = false;
        isDeadSlime2 = false;
        isDeadSlime3 = false;

        isDeadTurtle1 = false;
        isDeadTurtle2 = false;
        isDeadTurtle3 = false;

        isDeadGrunt1 = false;
        isDeadGrunt2 = false;
        isDeadGrunt3 = false;

        isDeadGolem1 = false;

        isChestPickUp = false;

        isBigBrotherSaved = false;
    }

    internal void SaveData()
    {
        SaveManager.Instance.SaveLevelStatesToXML(this, gameObject.name);
    }

    public void LoadData() {
        SaveManager.Instance.LoadLevelStatesToXML(this, gameObject.name);
    }

    internal void SyncLevelState(string sceneName)
    {
        if (sceneName == "Dungeons01")
        {
            if (isDeadSlime1) GameObject.Find("Slime1").SetActive(false);
            if (isDeadSlime2) GameObject.Find("Slime2").SetActive(false);
            if (isDeadSlime3) GameObject.Find("Slime3").SetActive(false);

            if (isDeadTurtle1) GameObject.Find("TurtleShell1").SetActive(false);
            if (isDeadTurtle2) GameObject.Find("TurtleShell2").SetActive(false);
            if (isDeadTurtle3) GameObject.Find("TurtleShell3").SetActive(false);


            if (isDeadGrunt1) GameObject.Find("Grunt1").SetActive(false);
            if (isDeadGrunt2) GameObject.Find("Grunt2").SetActive(false);
            if (isDeadGrunt3) GameObject.Find("Grunt3").SetActive(false);

            if (isDeadGolem1) GameObject.Find("Golem1").SetActive(false);
        }
        else if (sceneName == "Room")
        {
            if (isChestPickUp) GameObject.Find("ChestWithCoins").SetActive(false);
        }
        else if (sceneName == "InnTown") {
            if (activeFriendKnight) GameObject.Find("FriendKnight").SetActive(false);
            if (activeStrangeKnight) GameObject.Find("StrangeKnight").SetActive(false);
        }
    }
}
