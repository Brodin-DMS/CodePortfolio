using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RitualOfAnkaraz.Stats;
using System;

/// <summary>
/// This calss is responsible to save Game Data. Game data persists of 3 parts. 1) General Game Data, 2)Character Data, 3)Level Data
/// </summary>
public static class SaveSystem
{
    public static string generalGameData = "/generalGameData";
    public static string playerDataPath = "/playerData";
    public static string skillSlotsDataPath = "/hotBarSlotData";

    //Obsolete
    #region CharCreationSzene OBSOLETE
    [Obsolete("This method is obsolete. Call SaveCharachterData instead.", false)]
    public static void SaveCharCreationtemplate(CharCreationTemplate template)
    {
        //Obsolete
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + template.name + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        SinglePlayerCharacterCreationData data = new SinglePlayerCharacterCreationData(template);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    [Obsolete("This method is obsolete. Call SaveCharachterData instead.", true)]
    public static SinglePlayerCharacterCreationData LoadCharCreationTemplate(string name)
    {
        //Obsolete
        string path = Application.persistentDataPath + "/" + name + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SinglePlayerCharacterCreationData data = binaryFormatter.Deserialize(stream) as SinglePlayerCharacterCreationData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("file not found in: " + path);
            return null;
        }
    }
    [Obsolete("This method is obsolete. Call SaveCharachterData instead.", false)]
    public static void SaveCharTemplateByRosterSlot(CharCreationTemplate template, int saveSlot)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "/player" + template.rosterIndex + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        SinglePlayerCharacterCreationData data = new SinglePlayerCharacterCreationData(template);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    [Obsolete("This method is obsolete. Call SaveCharachterData instead.", true)]
    public static SinglePlayerCharacterCreationData LoadCharCreationTemplatebyRosterSlot(int index, int saveSlot)
    {
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "/player" + index + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SinglePlayerCharacterCreationData data = binaryFormatter.Deserialize(stream) as SinglePlayerCharacterCreationData;

            //todo create CharCreationtemplate
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("file not found in: " + path);
            return null;
        }
    }
    #endregion
    #region generalData
    //generalGameData
    /// <summary>
    /// Saves general Data of the Game that will be shared across all parties.
    /// </summary>
    /// <param name="generalGameData"></param> c# class generated from ingame data. this contains story progress, journal logs etc.
    /// <param name="saveSlot"></param> The selected save slot when the game was created.
    public static void SaveGeneralGameData(GeneralGameData generalGameData, int saveSlot)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + SaveSystem.generalGameData + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        GeneralGameData data = generalGameData;
        formatter.Serialize(stream, data);
        stream.Close();
    }
    //general Game Data from char creation initial, TODO implement save Slot from selection
    /// <summary>
    /// Saves the game initially after Charachter creation
    /// </summary>
    /// <param name="saveSlot"></param> selected save slot from the Menu Scene.
    public static void SaveGeneralGameData(int saveSlot)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + SaveSystem.generalGameData + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        GeneralGameData data = new GeneralGameData();
        formatter.Serialize(stream, data);
        stream.Close();
    }
    /// <summary>
    /// Method that will be called by Load manager to Load the current game data.
    /// </summary>
    /// <param name="saveSlot"></param> slected save slot when creating the game.
    /// <returns></returns>
    public static GeneralGameData LoadGeneralGameData(int saveSlot)
    {
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + SaveSystem.generalGameData + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GeneralGameData data = binaryFormatter.Deserialize(stream) as GeneralGameData;

            //todo create CharCreationtemplate
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("file not found in: " + path);
            return new GeneralGameData();
        }
    }
    #endregion

    #region Characters
    //Save & Load Characters
    /// <summary>
    /// this Method is used to store a character on disk. 2 addition adapter classes will Instantiate, or Strip the data of Equipment GameObjects.
    /// </summary>
    /// <param name="saveSlot"></param> the selected save slot when creating the game
    /// <param name="stats"></param> the statSheet of the Character
    /// <param name="invItems"></param> List off all items in the characters inventory
    /// <param name="eqItems"></param> List of all equipped items.
    public static void SaveCharachterData(int saveSlot, CharacterStatsBase stats, List<ItemClass> invItems, List<ItemClass> eqItems)
    {

        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + playerDataPath + stats.id + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        CharachterData data = new CharachterData(stats, invItems, eqItems);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    /// <summary>
    /// This method initially stores a Character after creation with class specific default gear.
    /// </summary>
    /// <param name="saveSlot"></param> the selected save slot when creating the game
    /// <param name="stats"></param> the statSheet of the Character, this is parsed by CharCreationTemplate class
    public static void SaveCharachterData(int saveSlot, CharachterData stats)
    {

        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + playerDataPath + stats.id + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, stats);
        stream.Close();
    }
    /// <summary>
    /// This method loads the data of a charachter based on his roster index. it wll convert it to a CharachterData instance.
    /// </summary>
    /// <param name="saveSlot"></param> the selected save slot when creating the game
    /// <param name="rosterIndex"></param> the roster slot of the character assigned at creation.(0-5)
    /// <returns></returns>
    public static CharachterData LoadCharachterData(int saveSlot, int rosterIndex)
    {
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + playerDataPath + rosterIndex + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CharachterData data = binaryFormatter.Deserialize(stream) as CharachterData;

            //todo create CharCreationtemplate
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("file not found in: " + path);
            return null;
        }
    }
    /// <summary>
    /// This saves the UI appearance and spells dragged in UI slots of Characters.
    /// </summary>
    /// <param name="saveSlot"></param> the selected save slot when creating the game
    /// <param name="slotDataSeris"></param> This was an adapter class cause saving UI elemts was such a pain in the but.
    /// It just makes it easier to understand whats going on when Load is called in the Load manager.
    public static void SaveHotbarSlots(int saveSlot,SlotDataSeri slotDataSeris)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + skillSlotsDataPath + ".dat";

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, slotDataSeris);
        stream.Close();
    }
    /// <summary>
    /// This Loads bytes and restores a instance of savedHotbarSlot Data.
    /// </summary>
    /// <param name="saveSlot"></param> the selected save slot when creating the game.
    /// <returns></returns>
    public static SlotDataSeri LoadSlotData(int saveSlot)
    {
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + skillSlotsDataPath + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SlotDataSeri data = binaryFormatter.Deserialize(stream) as SlotDataSeri;

            //todo create CharCreationtemplate
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("file not found in: " + path);

            //TODO implement inital load here

            return null;
        }
    }

    #endregion
    //TODO change this after updating Level
    #region LevelData
    /// <summary>
    /// Saves the level MainArea, Enemies, LootCHests, Destroyed barriers, finished Events ...
    /// </summary>
    /// <param name="saveSlot"></param>
    public static void SaveKhamosMainLevel(int saveSlot)
    {
        //DATA HOLDER
        //selected Game SaveSlot
        string levelName = "KhamosMain";
        //enemy groups
        bool spiderForrestGroup1 = true;
        bool goblinForrestGroup1 = true;
        bool goblinForrestGroup2 = true;
        bool goblinForrestGroup3 = true;
        bool forrestTrollGroup1 = true;
        bool treeHouseGoblinGroup1 = true;
        bool treeLogGroup = true;
        //loot Objects
        bool lootChest1 = true;
        bool runeOfPowerBarrier = true;

        //Find GameObjects
        GameObject go = new GameObject();

        go = GameObject.Find("SpiderForrestGroup1");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { spiderForrestGroup1 = false; }

        go = GameObject.Find("GoblinForrestGroup1");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { goblinForrestGroup1 = false; }

        go = GameObject.Find("GoblinForrestGroup2");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { goblinForrestGroup2 = false; }

        go = GameObject.Find("GoblinForrestGroup3");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { goblinForrestGroup3 = false; }

        go = GameObject.Find("ForrestTrollGroup1");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { forrestTrollGroup1 = false; }

        go = GameObject.Find("TreeHouseGoblinGroup1");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { treeHouseGoblinGroup1 = false; }

        go = GameObject.Find("TreeLogGroup1");
        if (go == null || go.GetComponent<EnemyGroup>().isDeafeated == true) { treeLogGroup = false; }

        go = GameObject.Find("LootChest1");
        if (go == null || go.GetComponent<LootChest>().isLooted == true) { lootChest1 = false; }

        go = GameObject.Find("RuneOfPowerBarrier");
        if( go == null || go.GetComponent<LootChest>().isLooted == true) { runeOfPowerBarrier = false; }

        //Crate DIr
        if (!Directory.Exists(Application.persistentDataPath + "/SaveSlot" + saveSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveSlot" + saveSlot);
        }
        //Write to disk
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "/Level" + levelName + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        Vector3 playerGroupPosition = GameObject.Find("PlayerOne").gameObject.transform.position;
        KhamosMainLevelData data = new KhamosMainLevelData(saveSlot, spiderForrestGroup1, goblinForrestGroup1, goblinForrestGroup2, goblinForrestGroup3, forrestTrollGroup1, treeHouseGoblinGroup1, treeLogGroup, lootChest1, runeOfPowerBarrier, playerGroupPosition);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    /// <summary>
    /// Loads the level MainArea, Enemies, LootCHests, Destroyed barriers, finished Events ...
    /// </summary>
    /// <param name="saveSlot"></param>
    /// <returns></returns>
    public static KhamosMainLevelData LoadKhamosMainLevel(int saveSlot)
    {
        string levelName = "KhamosMain";
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "/Level" + levelName + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            KhamosMainLevelData data = binaryFormatter.Deserialize(stream) as KhamosMainLevelData;

            //todo create CharCreationtemplate
            stream.Close();
            return data;
        }
        else
        {

            return new KhamosMainLevelData();
        }
    }
    /// <summary>
    /// Saves the level TreeHouse, Enemies, LootCHests, Destroyed barriers, finished Events ...
    /// </summary>
    /// <param name="saveSlot"></param>
    public static void SaveTreeHouseLevelData(int saveSlot)
    {
        string levelName = "TreeHouse";
    }
    /// <summary>
    /// loads the level TreeHouse, Enemies, LootCHests, Destroyed barriers, finished Events ...
    /// </summary>
    /// <param name="saveSlot"></param>
    /// <returns></returns>
    public static TreeHouseLevelData LoadTreeHouseLevelData(int saveSlot)
    {
        string levelName = "TreeHouse";
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "/Level" + levelName + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TreeHouseLevelData data = binaryFormatter.Deserialize(stream) as TreeHouseLevelData;

            //todo create CharCreationtemplate
            stream.Close();
            return data;
        }
        else
        {

            return new TreeHouseLevelData();
        }
    }
    /// <summary>
    /// Saves the level ForrestRuins, Enemies, LootCHests, Destroyed barriers, finished Events ...
    /// </summary>
    /// <param name="saveSlot"></param>
    public static void SaveForrestRuinsLevelData(int saveSlot)
    {
        string levelName = "ForrestRuins";
    }
    /// <summary>
    /// Loads the level ForrestRuins, Enemies, LootCHests, Destroyed barriers, finished Events ...
    /// </summary>
    /// <param name="saveSlot"></param>
    /// <returns></returns>
    public static ForrestRuinsLevelData LoadForrestRuinsLevelData(int saveSlot)
    {
        string levelName = "ForrestRuins";
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "/Level" + levelName + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ForrestRuinsLevelData data = binaryFormatter.Deserialize(stream) as ForrestRuinsLevelData;

            stream.Close();
            return data;
        }
        else
        {
            return new ForrestRuinsLevelData();
        }
    }
    #endregion


}
