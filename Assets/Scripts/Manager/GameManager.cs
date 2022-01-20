using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStates;

    //the freelook type of cinemachine
    private CinemachineFreeLook followCamera;

    List<IEndGameObsever> endGameObsevers = new List<IEndGameObsever>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RigisterPlayer(CharacterStats player) {
        playerStates = player;

        //bind the follow camera to the player when player is enable
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null) {
            followCamera.Follow = playerStates.transform.GetChild(2);
            followCamera.LookAt = playerStates.transform.GetChild(2);
        }
    }

    public void AddObserver(IEndGameObsever observer) {
        endGameObsevers.Add(observer);
    }

    public void RemoveObserver(IEndGameObsever obsever) {
        endGameObsevers.Remove(obsever);
    }

    public void NotifyObservers() {
        foreach (var observer in endGameObsevers)
        {
            observer.EndNotify();
        }
    }

    //find the enter positon of current active scene
    public Transform GetEntrance() {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER) {
                return item.transform;
            }
        }
        return null;
    }
}
