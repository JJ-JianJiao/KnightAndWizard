using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;




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

    public float positionX;
    public float positionY;
    public float positionZ;

    public float rotationX;
    public float rotationY;
    public float rotationZ;

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

            positionX = -1f;
            positionY = -1f;
            positionZ = -1f;

            rotationX = -1f;
            rotationY = -1f;
            rotationZ = -1f;

            sceneName = "InnTown";
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

            positionX = -1f;
            positionY = -1f;
            positionZ = -1f;

            rotationX = -1f;
            rotationY = -1f;
            rotationZ = -1f;

            sceneName = "InnTown";
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
        //if (Input.GetKeyDown(KeyCode.F1)) {
        //    //SavePlayerDataPlayerPrefs();
        //    SavePlayerProfileXML();
        //    if (InventoryManager.Instance != null) {
        //        InventoryManager.Instance.SaveData();
        //    }
        //    Debug.Log("saved");
        //}
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    //LoadPlayerData();
            
        //    SycPlayerData(LoadPlayerProfileXML(GameManager.Instance.fileName));

        //    if (InventoryManager.Instance != null)
        //    {
        //        InventoryManager.Instance.LoadData();
        //    }


        //    Debug.Log("Loaded");
        //}
        //if (Input.GetKeyDown(KeyCode.Escape)) {
        //    SceneController.Instance.TransitionToMain();
        //}
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


        GameManager.Instance.fileName = cp.fileName;
        GameManager.Instance.characterClass = cp.characterClass;
        GameManager.Instance.characterName = cp.name;

        SaveFormatXML(cp, playersProfile);
    }

    private void SaveFormatXML(CharacterProfile cp, PlayersProfile playersProfile)
    {
        //Base on the fileName, create a folder for the current player

        string currentDataPath = DATA_PATH + "\\" + cp.fileName;

        //if (!Directory.Exists(DATA_PATH))
        if (!Directory.Exists(currentDataPath))
        {
            Directory.CreateDirectory(currentDataPath);
        }
        string path = Path.Combine(currentDataPath, cp.fileName);
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
        string currentPath = DATA_PATH + "\\" + cp.fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        string path = Path.Combine(currentPath, cp.fileName);
        Stream stream = File.Open(path, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterProfile));
        serializer.Serialize(stream, cp);

        stream.Close();

        //playerFileName = cp.fileName;
    }

    public CharacterProfile LoadPlayerProfileXML(string fileName) {
        CharacterProfile cp = new CharacterProfile();

        string currentPath = DATA_PATH + "\\" + fileName;
        
        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        Stream stream = File.Open(Path.Combine(currentPath, fileName), FileMode.OpenOrCreate);
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


    public void SavePlayerProfileXML() {
        CharacterProfile cp = new CharacterProfile();

        //cp.fileName = playerFileName;
        cp.sceneName = SceneManager.GetActiveScene().name;
        cp.fileName = GameManager.Instance.playerStates.fileName;
        cp.characterClass = GameManager.Instance.playerStates.characterClass;
        cp.name = GameManager.Instance.playerStates.characterName;

        cp.positionX = GameManager.Instance.playerStates.transform.position.x;
        cp.positionY = GameManager.Instance.playerStates.transform.position.y;
        cp.positionZ = GameManager.Instance.playerStates.transform.position.z;

        cp.rotationX = GameManager.Instance.playerStates.transform.eulerAngles.x;
        cp.rotationY = GameManager.Instance.playerStates.transform.eulerAngles.y;
        cp.rotationZ = GameManager.Instance.playerStates.transform.eulerAngles.z;

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

    public void SycPlayerData(CharacterProfile cp)
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
        //if (Directory.Exists(DATA_PATH)) {
        //    string filePath = Path.Combine(DATA_PATH, deleteFileName);
        //    if (File.Exists(Path.Combine(filePath))) {
        //        File.Delete(filePath);
        //    }
        //}

        string currentPath = DATA_PATH + "\\" + deleteFileName;


        if (Directory.Exists(currentPath)) {
            DirectoryInfo di = new DirectoryInfo(currentPath);
            di.Delete(true);
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

    public void SavePlayerInventoryFileToXML(Object data, string name) {

        var jsonData = JsonUtility.ToJson(data,true);
        //Debug.Log(jsonData);

        string fileName = GameManager.Instance.playerStates.fileName;

        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }

        string _name = "error";

        if (name.Contains("My Bag"))
        {
            _name = fileName + "MyBag";
        }
        else if (name.Contains("Action Bar"))
        {
            _name = fileName + "ActionBar";
        }
        else if (name.Contains("Equipment"))
        {
            _name = fileName + "Equipment";
        }
        else if (name.Contains("ChestContainer01"))
        {
            _name = fileName + "ChestContainer01";
        }
        else if (name.Contains("ChestContainer02"))
        {
            _name = fileName + "ChestContainer02";
        }
        else if (name.Contains("ChestContainer03"))
        {
            _name = fileName + "ChestContainer03";
        }


        string fullPath = Path.Combine(currentPath, _name);

        FileInfo file = new FileInfo(fullPath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(jsonData);
        sw.Close();

        //TODO: need do some test and make it clear between Close and Dispose
        sw.Dispose();

    }


    public void LoadPlayerInventoryFileToXML(Object data, string name)
    {
        string fileName = GameManager.Instance.playerStates.fileName;

        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }

        string _name = "error";

        if (name.Contains("My Bag"))
        {
            _name = fileName + "MyBag";
        }
        else if (name.Contains("Action Bar"))
        {
            _name = fileName + "ActionBar";
        }
        else if (name.Contains("Equipment"))
        {
            _name = fileName + "Equipment";
        }
        else if (name.Contains("ChestContainer01"))
        {
            _name = fileName + "ChestContainer01";
        }
        else if (name.Contains("ChestContainer02"))
        {
            _name = fileName + "ChestContainer02";
        }
        else if (name.Contains("ChestContainer03"))
        {
            _name = fileName + "ChestContainer03";
        }

        string fullPath = Path.Combine(currentPath, _name);

        if (!File.Exists(fullPath))
        {
            Debug.Log("Can not find: " + fullPath);
            return;
        }

        StreamReader sr = new StreamReader(fullPath);
        if (sr == null) {
            return;
        }

        string json = sr.ReadToEnd();

        if (json.Length > 0) {
            JsonUtility.FromJsonOverwrite(json, data);
        }
        sr.Close();
        sr.Dispose();
    }

    public void SaveLevelStatesToXML(LevelManager levelManager, string name)
    {
        GameLevelStates states;
        states.activeFriendKnight = levelManager.activeFriendKnight;
        states.activeStrangeKnight = levelManager.activeStrangeKnight;
        states.isDeadSlime1 = levelManager.isDeadSlime1;
        states.isDeadSlime2 = levelManager.isDeadSlime2;
        states.isDeadSlime3 = levelManager.isDeadSlime3;
        states.isDeadTurtle1 = levelManager.isDeadTurtle1;
        states.isDeadTurtle2 = levelManager.isDeadTurtle2;
        states.isDeadTurtle3 = levelManager.isDeadTurtle3;
        states.isDeadGrunt1 = levelManager.isDeadGrunt1;
        states.isDeadGrunt2 = levelManager.isDeadGrunt2;
        states.isDeadGrunt3 = levelManager.isDeadGrunt3;
        states.isDeadGolem1 = levelManager.isDeadGolem1;
        states.isChestPickUp = levelManager.isChestPickUp;

        string fileName = GameManager.Instance.playerStates.fileName;
        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        string path = Path.Combine(currentPath, name);
        Stream stream = File.Open(path, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(GameLevelStates));
        serializer.Serialize(stream, states);
        stream.Close();
    }

    public void LoadLevelStatesToXML(LevelManager levelManager, string name)
    {
        GameLevelStates states;
        string fileName = GameManager.Instance.playerStates.fileName;
        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        string path = Path.Combine(currentPath, name);
        Stream stream = File.Open(path, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(GameLevelStates));
        states = (GameLevelStates)serializer.Deserialize(stream);
        stream.Close();
        levelManager.activeFriendKnight = states.activeFriendKnight;
        levelManager.activeStrangeKnight = states.activeStrangeKnight;
        levelManager.isDeadSlime1 = states.isDeadSlime1;
        levelManager.isDeadSlime2 = states.isDeadSlime2;
        levelManager.isDeadSlime3 = states.isDeadSlime3;
        levelManager.isDeadTurtle1 = states.isDeadTurtle1;
        levelManager.isDeadTurtle2 = states.isDeadTurtle2;
        levelManager.isDeadTurtle3 = states.isDeadTurtle3;
        levelManager.isDeadGrunt1 = states.isDeadGrunt1;
        levelManager.isDeadGrunt2 = states.isDeadGrunt2;
        levelManager.isDeadGrunt3 = states.isDeadGrunt3;
        levelManager.isDeadGolem1 = states.isDeadGolem1;
        levelManager.isChestPickUp = states.isChestPickUp;

    }

    //Abandon
    public void SaveTasks(Object currentTasks, string name)
    {
        var jsonData = JsonUtility.ToJson(currentTasks, true);
        string fileName = GameManager.Instance.playerStates.fileName;

        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        string fullPath = Path.Combine(currentPath, name);


        FileInfo file = new FileInfo(fullPath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(jsonData);
        sw.Close();

        //TODO: need do some test and make it clear between Close and Dispose
        sw.Dispose();
    }

    public void SaveTasks(List<QuestManager.QuestTask> tasks, string name)
    {
        var jsonData = "";
        for (int i = 0; i < tasks.Count; i++)
        {
            jsonData += JsonUtility.ToJson(tasks[i].questData, true);
            jsonData += "^EndQuest^";
        }

        //var jsonData = JsonUtility.ToJson(currentTasks, true);
        string fileName = GameManager.Instance.playerStates.fileName;

        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        string fullPath = Path.Combine(currentPath, name);


        FileInfo file = new FileInfo(fullPath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(jsonData);
        sw.Close();

        //TODO: need do some test and make it clear between Close and Dispose
        sw.Dispose();
    }


    public void LoadTasks(List<QuestManager.QuestTask> tasks, string name) {


        string fileName = GameManager.Instance.playerStates.fileName;

        string currentPath = DATA_PATH + "\\" + fileName;

        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        string fullPath = Path.Combine(currentPath, name);

        if (!File.Exists(fullPath))
        {
            Debug.Log("Can not find: " + fullPath);
            return;
        }

        StreamReader sr = new StreamReader(fullPath);
        if (sr == null)
        {
            return;
        }

        string json = sr.ReadToEnd();

        sr.Close();
        sr.Dispose();
        string sliptStr = "^EndQuest^";

        //string test = "aaajsbbbjscccjs";

        //string[] testList = Regex.Split(test, "js", RegexOptions.None);

        if (json.Length > 0)
        {
            //string[] jsonList = Regex.Split(json, sliptStr, RegexOptions.IgnoreCase);

            string[] jsonList2 = json.Split(new string[] { "^EndQuest^" }, System.StringSplitOptions.None);

            for (int i = 0; i < jsonList2.Length-1; i++)
            {
                var tempTask = ScriptableObject.CreateInstance<QuestData_OS>();

                JsonUtility.FromJsonOverwrite(jsonList2[i], tempTask);
                tasks.Add(new QuestManager.QuestTask(tempTask));
            }
            
        }
    }

    public struct GameLevelStates
    {
        public bool activeFriendKnight;
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
    }
}


