using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;

public class SceneController : Singleton<SceneController>, IEndGameObsever
{

    private GameObject playerPrefab;

    public GameObject playerKnightPrefab;
    public GameObject playerWizardPrefab;


    public SceneFader sceneFaderPrefab;
    GameObject player;
    NavMeshAgent playerAgent;

    bool fadeFinished;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = false;
    }
    public void TransitionToDestination(TransitionPoint transitionPoint) {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DiffrentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
            default:
                break;
        }
    }

    IEnumerator Transition(string SceneName, TransitionDestination.DestinationTag destinationTag) {
        //TODO: save data
        SaveManager.Instance.SavePlayerDataPlayerPrefs();
        SceneFader fade = Instantiate(sceneFaderPrefab);

        if (SceneManager.GetActiveScene().name != SceneName)
        {
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(SceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();

            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
        else {

            yield return StartCoroutine(fade.FadeOut(2f));

            player = GameManager.Instance.playerStates.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            //disable the agent of player. or the play will move to the original position before taking portal
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;

            yield return StartCoroutine(fade.FadeIn(2f));
            yield return null;
        }
    }

    IEnumerator LoadLevel(string scene) {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //save data
            SaveManager.Instance.SavePlayerDataPlayerPrefs();

            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
        
    }

    IEnumerator LoadLevel(string scene, CharacterProfile cp)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //save data
            //SaveManager.Instance.SavePlayerDataPlayerPrefs();
            SaveManager.Instance.SavePlayerDataXML(cp);

            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }

    }

    public void TransitionToLoadGame() {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToFirstLevel() {
        StartCoroutine(LoadLevel("Dungeons01"));
    }

    public void TransitionToFirstLevel(CharacterProfile cp)
    {
        StartCoroutine(LoadLevel("Dungeons01", cp));
    }

    public void TransitionToMain() {
        StartCoroutine(LoadMain());
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag) {

        var entrances = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag) {
                return entrances[i];
            }
        }

        return null;

    }

    IEnumerator LoadMain(float delayTime = 0) {
        yield return new WaitForSeconds(delayTime);
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if (!fadeFinished)
        {
            fadeFinished = true;
            StartCoroutine(LoadMain(5));
        }
    }

    internal void SetCurrentPlayerPrefab(string currentCharacter)
    {
        if (currentCharacter == "Knight")
            playerPrefab = playerKnightPrefab;
        else if (currentCharacter == "Wizard")
            playerPrefab = playerWizardPrefab;
    }
}
