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
    public string characterClass;

    public string sceneName;

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
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public float criticalMultiplier;
    public float criticalChance;
    public int minDamage;
    public int maxDamage;

    public int stamina;
    public int strength;
    public int agility;
    public int intellect;


    public CharacterProfile() { }

    public CharacterProfile(string type)
    {
        if (type == "Knight") {
            sceneName = "Dungeons01";
            characterClass = "Knight";
            maxHealth = 100;
            currentHealth = 100;
            baseDefence = 2;
            currentDefence = 2;
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
            sceneName = "Dungeons01";
            characterClass = "Wizard";
            maxHealth = 100;
            currentHealth = 100;
            baseDefence = 2;
            currentDefence = 2;
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

    public string GetBasicInfo() {
        string str = "\n MaxHealth: " + maxHealth.ToString();
        str += "\n currentHealth: " + currentHealth.ToString();
        str += "\n baseDefence: " + baseDefence.ToString();
        str += "\n currentDefence: " + currentDefence.ToString();
        str += "\n stamina: " + stamina.ToString();
        str += "\n strength: " + strength.ToString();
        str += "\n agility: " + agility.ToString();
        str += "\n intellect: " + intellect.ToString();
        return str;
    }

    public string GetAttackInfo()
    {
        string str = "\n currentLevel: " + currentLevel.ToString();
        str += "\n maxLevel: " + maxLevel.ToString();
        str += "\n baseExp: " + baseExp.ToString();
        str += "\n currentExp: " + currentExp.ToString();
        str += "\n levelBuff: " + levelBuff.ToString();
        str += "\n attackRange: " + attackRange.ToString();
        str += "\n skillRange: " + skillRange.ToString();
        str += "\n coolDown: " + coolDown.ToString();
        str += "\n minDamage: " + minDamage.ToString();
        str += "\n maxDamage: " + maxDamage.ToString();
        str += "\n criticalMultiplier: " + criticalMultiplier.ToString();
        str += "\n criticalChance: " + criticalChance.ToString();
        return str;
    }
}

public class SaveManager : Singleton<SaveManager>
{
    const string DATA_PATH = "SaveFile";
    const string BASE_PLAYERINFO = "PlayersInfo";

    string sceneName = "Level";
    //public  string playerFileName;
    //public  string playerCharacter;
    //public  string playerName;

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
            //SavePlayerDataPlayerPrefs();
            SavePlayerProfileXML();
            Debug.Log("saved");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //LoadPlayerData();
            
            SycPlayerData(LoadPlayerProfileXML(GameManager.Instance.playerStates.fileName));
            Debug.Log("Loaded");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneController.Instance.TransitionToMain();
        }
    }



    public void SavePlayerDataPlayerPrefs() {
        Save(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }

    public void SaveNewPlayerDataXML(CharacterProfile cp) {

        PlayersProfile playersProfile = LoadPlayersProfles();

        cp.fileName = playersProfile.GetNewPlayerName();

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

        //playerFileName = cp.fileName;
    }

    private void SaveFormatXML(CharacterProfile cp)
    {
        if (!Directory.Exists(DATA_PATH))
        {
            Directory.CreateDirectory(DATA_PATH);
        }
        string path = Path.Combine(DATA_PATH, cp.fileName);
        Stream stream = File.Open(path, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterProfile));
        serializer.Serialize(stream, cp);

        stream.Close();

        //playerFileName = cp.fileName;
    }

    public CharacterProfile LoadPlayerProfileXML(string fileName) {
        CharacterProfile cp = new CharacterProfile();
        if (!Directory.Exists(DATA_PATH))
        {
            Directory.CreateDirectory(DATA_PATH);
        }
        Stream stream = File.Open(Path.Combine(DATA_PATH, fileName), FileMode.OpenOrCreate);
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterProfile));
        cp = (CharacterProfile)serializer.Deserialize(stream);
        stream.Close();
        return cp;
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

    public PlayersProfile LoadPlayersProfles() {

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

        p =  RemoveEarlierProfile(p);

        if (!Directory.Exists(DATA_PATH))
        {
            Directory.CreateDirectory(DATA_PATH);
        }
        Stream stream = File.Open(Path.Combine(DATA_PATH, BASE_PLAYERINFO), FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(PlayersProfile));
        serializer.Serialize(stream, p);
        stream.Close();
    }


    private void SavePlayerProfileXML() {
        CharacterProfile cp = new CharacterProfile();

        //cp.fileName = playerFileName;
        cp.fileName = GameManager.Instance.playerStates.fileName;
        cp.characterClass = GameManager.Instance.playerStates.characterClass;
        cp.name = GameManager.Instance.playerStates.characterName;

        cp.currentHealth = GameManager.Instance.playerStates.characterData.currentHealth;
        cp.maxHealth = GameManager.Instance.playerStates.characterData.maxHealth ;
        cp.baseDefence = GameManager.Instance.playerStates.characterData.baseDefence;
        cp.currentDefence = GameManager.Instance.playerStates.characterData.currentDefence;
        cp.currentLevel = GameManager.Instance.playerStates.characterData.currentLevel;
        cp.maxLevel = GameManager.Instance.playerStates.characterData.maxLevel;
        cp.baseExp = GameManager.Instance.playerStates.characterData.baseExp;
        cp.currentExp = GameManager.Instance.playerStates.characterData.currentExp;
        cp.levelBuff = GameManager.Instance.playerStates.characterData.levelBuff;


        cp.attackRange = GameManager.Instance.playerStates.attackData.attackRange;
        cp.skillRange = GameManager.Instance.playerStates.attackData.skillRange;
        cp.coolDown = GameManager.Instance.playerStates.attackData.coolDown;
        cp.criticalMultiplier = GameManager.Instance.playerStates.attackData.criticalMultiplier;
        cp.criticalChance = GameManager.Instance.playerStates.attackData.criticalChance;
        cp.minDamage = GameManager.Instance.playerStates.attackData.minDamage;
        cp.maxDamage = GameManager.Instance.playerStates.attackData.maxDamage;


        SaveFormatXML(cp);
    }

    private void SycPlayerData(CharacterProfile cp)
    {
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
    }

    public bool DeleteSelectedPlayerProfile(string deleteFileName)
    {
        if (Directory.Exists(DATA_PATH)) {
            string filePath = Path.Combine(DATA_PATH, deleteFileName);
            if (File.Exists(Path.Combine(filePath))) {
                File.Delete(filePath);
            }
        }

        PlayersProfile playersProfile = LoadPlayersProfles();
        playersProfile.playerFileNameList.Remove(deleteFileName);

        SavePlayersInfo(playersProfile);

        return true;
    }


    private PlayersProfile RemoveEarlierProfile(PlayersProfile p)
    {
        if (p.Length <= 6) return p;


        for (int i = 0; i <= p.Length-6; i++)
        {
            if (DeleteSelectedPlayerProfile(p.playerFileNameList[i])) {
                p.playerFileNameList.RemoveAt(i);
            }
            
        }
        return p;

    }
}
