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
        //SaveManager.Instance.SavePlayerDataPlayerPrefs();

        GameObject generatePlayer;

        SaveManager.Instance.SavePlayerProfileXML();

        InventoryManager.Instance.SaveData();

        LevelManager.Instance.SaveData();


        SceneFader fade = Instantiate(sceneFaderPrefab);

        if (SceneManager.GetActiveScene().name != SceneName)
        {
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(SceneName);
            yield return generatePlayer = Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

            //SaveManager.Instance.LoadPlayerData();

            SaveManager.Instance.SycPlayerData(SaveManager.Instance.LoadPlayerProfileXML(GameManager.Instance.fileName));
            InventoryManager.Instance.LoadData();
            LevelManager.Instance.LoadData();

            SaveManager.Instance?.LoadTasks(QuestManager.Instance.tasks, "QuestsInfo");


            LevelManager.Instance.SyncLevelState(SceneName);
            if (LevelManager.Instance.activeFriendKnight)
            {
                var friendKnight = Instantiate(LevelManager.Instance.friendKnightPrefab, generatePlayer.transform.position +
                    new Vector3((UnityEngine.Random.value < 0.5f ? -1f : 1f) * 3, (UnityEngine.Random.value < 0.5f ? -1f : 1f) * 3,
                    (UnityEngine.Random.value < 0.5f ? -1f : 1f) * 3), generatePlayer.transform.rotation);
                friendKnight.GetComponent<FriendKnight>().SetFollowTarget(generatePlayer);

            }

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
    /// <summary>
    /// This mothod is for PlayerPrefs Save model.
    /// </summary>
    /// <param name="scene"> scene name </param>
    /// <returns></returns>
    IEnumerator LoadLevel(string scene) {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //save data
            SaveManager.Instance.SavePlayerDataPlayerPrefs();

            InventoryManager.Instance.SaveData();

            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
        
    }

    /// <summary>
    /// This SaveLevel is for the Multiple PlayerPrefabs and save as XML format
    /// </summary>
    /// <param name="scene"> scene name </param>
    /// <param name="cp"> CharacterProfile, which have the current player info </param>
    /// <returns></returns>
    IEnumerator SaveLevel(string scene, CharacterProfile cp)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //save data
            //SaveManager.Instance.SavePlayerDataPlayerPrefs();
            SaveManager.Instance.SaveNewPlayerDataXML(cp);

            InventoryManager.Instance.SaveData();

            LevelManager.Instance.SaveData();

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
            //yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            if (cp.positionX != -1 && cp.positionY != -1 && cp.positionZ != -1 && cp.rotationX != -1 && cp.rotationY != -1 && cp.rotationZ != -1)
            {
                yield return player = Instantiate(playerPrefab, new Vector3(cp.positionX,cp.positionY,cp.positionZ), Quaternion.Euler(cp.rotationX,cp.rotationY,cp.rotationZ));
            }
            else {
                yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);
            }


            GameManager.Instance.playerStates.fileName = cp.fileName;
            GameManager.Instance.playerStates.characterClass = cp.characterClass;
            GameManager.Instance.playerStates.characterName = cp.name;


            GameManager.Instance.playerStates.characterData.currentHealth = cp.currentHealth;
            GameManager.Instance.playerStates.characterData.maxHealth = cp.maxHealth;
            GameManager.Instance.playerStates.characterData.baseDefence = cp.baseDefence;
            GameManager.Instance.playerStates.characterData.currentDefence = cp.currentDefence;
            GameManager.Instance.playerStates.characterData.currentLevel = cp.currentLevel;
            GameManager.Instance.playerStates.characterData.maxLevel = cp.maxLevel;
            GameManager.Instance.playerStates.characterData.baseExp = cp.baseExp;
            GameManager.Instance.playerStates.characterData.currentExp = cp.currentExp;
            GameManager.Instance.playerStates.characterData.levelBuff = cp.levelBuff;


            GameManager.Instance.playerStates.attackData.attackRange = cp.attackRange;
            GameManager.Instance.playerStates.attackData.skillRange = cp.skillRange;
            GameManager.Instance.playerStates.attackData.coolDown = cp.coolDown;
            GameManager.Instance.playerStates.attackData.criticalMultiplier = cp.criticalMultiplier;
            GameManager.Instance.playerStates.attackData.criticalChance = cp.criticalChance;
            GameManager.Instance.playerStates.attackData.minDamage = cp.minDamage;
            GameManager.Instance.playerStates.attackData.maxDamage = cp.maxDamage;
            //SaveManager.Instance.playerFileName = cp.fileName;
            InventoryManager.Instance.LoadData();
            LevelManager.Instance.LoadData();

            SaveManager.Instance?.LoadTasks(QuestManager.Instance.tasks, "QuestsInfo");


            if (LevelManager.Instance.activeFriendKnight) {
                var friendKnight = Instantiate(LevelManager.Instance.friendKnightPrefab, player.transform.position + 
                    new Vector3((UnityEngine.Random.value < 0.5f ? -1f : 1f) * 3, (UnityEngine.Random.value < 0.5f ? -1f : 1f) * 3, 
                    (UnityEngine.Random.value < 0.5f ? -1f : 1f) * 3), player.transform.rotation);
                friendKnight.GetComponent<FriendKnight>().SetFollowTarget(player);

            }
            LevelManager.Instance.SyncLevelState(SceneManager.GetActiveScene().name);
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }

    }

    public void TransitionToLoadGame() {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }


    public void TransitionToLoadGame(CharacterProfile characterProfile)
    {
        //StartCoroutine(SaveLevel(characterProfile.sceneName, characterProfile));
        //TODO: sceneName need to be saved
        //StartCoroutine(LoadLevel("Dungeons01", characterProfile));
        StartCoroutine(LoadLevel(characterProfile.sceneName, characterProfile));
    }

    public void TransitionToFirstLevel() {
        //StartCoroutine(LoadLevel("Dungeons01"));
        StartCoroutine(LoadLevel("InnTown"));
    }

    public void TransitionToFirstLevel(CharacterProfile cp)
    {
        //StartCoroutine(SaveLevel("Dungeons01", cp));
        StartCoroutine(SaveLevel(cp.sceneName, cp));
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
