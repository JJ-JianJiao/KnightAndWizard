using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml;
using System.Xml.Serialization;




public class PlayersProfile {

    public int newPlayerNumbers;

    public List<string> playerFileNameList;

    public int Length
    {
        get { return playerFileNameList.Count; }
    }

    public PlayersProfile() {
        newPlayerNumbers = 0;
        playerFileNameList = new List<string>();
    }

    public string GetNewPlayerName() {
        return "Player" + newPlayerNumbers.ToString();
    }

    public void AddedNewPlayerName() {
        playerFileNameList.Add("Player" + newPlayerNumbers.ToString());
        newPlayerNumbers++; 
    }

}

public class CharacterProfile
{
    public string fileName;
    public string name;
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
    internal float attackRange;
    internal float skillRange;
    internal float coolDown;
    internal float criticalMultiplier;
    internal float criticalChance;
    internal int minDamage;
    internal int maxDamage;

    public int stamina;
    public int strength;
    public int agility;
    public int intellect;


    public CharacterProfile() { }

    public CharacterProfile(string type)
    {
        if (type == "Knight") {
            maxHealth = 100;
            currentHealth = 100;
            baseDefence = 2;
            baseDefence = 2;
            currentLevel = 1;
            maxLevel = 30;
            baseExp = 50;
            currentExp = 0;
            levelBuff = 0.1f;

            attackRange = 2.2f;
            skillRange = 0;
            coolDown = 0.7f;
            minDamage = 4;
            maxDamage = 6;
            criticalMultiplier = 2;
            criticalChance = 0.2f;

            stamina = 0;
            strength = 0;
            agility = 0;
            intellect = 0;
        }
        else if (type == "Wizard")
        {
            maxHealth = 100;
            currentHealth = 100;
            baseDefence = 2;
            baseDefence = 2;
            currentLevel = 1;
            maxLevel = 30;
            baseExp = 50;
            currentExp = 0;
            levelBuff = 0.1f;

            attackRange = 2.2f;
            skillRange = 0;
            coolDown = 0.7f;
            minDamage = 4;
            maxDamage = 6;
            criticalMultiplier = 2;
            criticalChance = 0.2f;

            stamina = 0;
            strength = 0;
            agility = 0;
            intellect = 0;
        }
    }
}

public class SaveManager : Singleton<SaveManager>
{
    const string DATA_PATH = "SaveFile";
    const string BASE_PLAYERINFO = "PlayersInfo";

    string sceneName = "Level";

    public string SceneName {
        get { return PlayerPrefs.GetString(sceneName); } 
    }


    protected override void Awake()
    {
        base.Awake();

        //if (!Directory.Exists(DATA_PATH))
        //{
        //    Directory.CreateDirectory(DATA_PATH);
        //}

        DontDestroyOnLoad(this);
    
    }

    private void Update()
    {   //TODO: this hot key is just for testing. these should be in pause menu
        if (Input.GetKeyDown(KeyCode.F1)) {
            SavePlayerDataPlayerPrefs();
            //SavePlayerDataXML();
            Debug.Log("saved");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadPlayerData();
            Debug.Log("Loaded");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneController.Instance.TransitionToMain();
        }
    }

    public void SavePlayerDataPlayerPrefs() {
        Save(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }

    public void SavePlayerDataXML(CharacterProfile cp) {

        PlayersProfile playersProfile = LoadPlayersInfo();

        cp.fileName = playersProfile.GetNewPlayerName();

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


        SaveFormatXML(cp, playersProfile);
    }

    private void SaveFormatXML(CharacterProfile cp, PlayersProfile playersProfile)
    {
        if (!Directory.Exists(DATA_PATH))
        {
            Directory.CreateDirectory(DATA_PATH);
        }
        string path = Path.Combine(DATA_PATH, cp.fileName);
        Stream stream = File.Open(path, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterProfile));
        serializer.Serialize(stream, cp);
        playersProfile.AddedNewPlayerName();
        SavePlayersInfo(playersProfile);
        stream.Close();
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }

    public void Save(Object data, string key) {
        var jasonData = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jasonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key) {
        if (PlayerPrefs.HasKey(key)) {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

    public PlayersProfile LoadPlayersInfo() {

        PlayersProfile playersProfile = new PlayersProfile();

        if (!Directory.Exists(DATA_PATH))
        {
            Directory.CreateDirectory(DATA_PATH);
        }
        Stream stream = File.Open(Path.Combine(DATA_PATH, BASE_PLAYERINFO), FileMode.OpenOrCreate);
        if (stream.Length == 0)
        {
            XmlSerializer serializerInput = new XmlSerializer(typeof(PlayersProfile));
            serializerInput.Serialize(stream, playersProfile);
        }
        else{
            XmlSerializer serializer = new XmlSerializer(typeof(PlayersProfile));
            playersProfile = (PlayersProfile)serializer.Deserialize(stream);
        }
        stream.Close();
        return playersProfile;

    }

    public void SavePlayersInfo(PlayersProfile p) {
        if (!Directory.Exists(DATA_PATH))
        {
            Directory.CreateDirectory(DATA_PATH);
        }
        Stream stream = File.Open(Path.Combine(DATA_PATH, BASE_PLAYERINFO), FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(PlayersProfile));
        serializer.Serialize(stream, p);
        stream.Close();
    }
}
