 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

/// <summary>
/// The Sublass that loads/Stores the Main Area specificly. This will Call the Base Awake Method to load all general,character,Equipment,Spellslot Data.
/// Then the specific level is Loaded/Stored.
/// </summary>
public class LoadStoreManagerKhamosMainScene : LoadStoreManagerBase
{
    //assign all enemy groups to idividual objects so they can be refferences withput FIND
    public GameObject spiderForrestGroup1;
    public GameObject goblinForrestGroup1;
    public GameObject goblinForrestGroup2;
    public GameObject goblinGroupForrest3;
    public GameObject forrestTrollGroup1;
    public GameObject treeHouseGoblinGroup1;
    public GameObject treeLogGroup1;
    public GameObject lootChest1;
    public GameObject runeOfPowerBarrier;

    /// <summary>
    /// Sets up entire Scene upon initialization.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        //LoadLevelData && SetupLevelData
        KhamosMainLevelData khamosMainLevelData = SaveSystem.LoadKhamosMainLevel(CustomGameManager.gameSaveSlot);
        SetupGameObjects(khamosMainLevelData);
    }
    /// <summary>
    /// This sets up Enemies, LootObbjects, barriers,  in the main scene.
    /// </summary>
    /// <param name="data"></param> the specific level Data of the Main area
    private void SetupGameObjects(KhamosMainLevelData data)
    {
        //Setup Enemies
        spiderForrestGroup1.SetActive(data.spiderForrestGroup1);
        goblinForrestGroup1.SetActive(data.goblinForrestGroup1);
        goblinForrestGroup2.SetActive(data.goblinForrestGroup2);
        goblinGroupForrest3.SetActive(data.goblinForrestGroup3);
        forrestTrollGroup1.SetActive(data.forrestTrollGroup1);
        treeHouseGoblinGroup1.SetActive(data.treeHouseGoblinGroup1);
        treeLogGroup1.SetActive(data.treeLogGroup);
        lootChest1.SetActive(data.lootChest1);
        runeOfPowerBarrier.SetActive(data.runeOfPowerBarrier);

        //setup Player Position
        CustomGameManager.instance.players.ForEach(player => player.GetComponent<NavMeshAgent>().enabled = false);
        CustomGameManager.instance.players[0].transform.position = new Vector3(CustomGameManager.generalGameData.player1PositionX, CustomGameManager.generalGameData.player1positionY, CustomGameManager.generalGameData.player1positionZ);
        CustomGameManager.instance.players[1].transform.position = new Vector3(CustomGameManager.generalGameData.player2PositionX, CustomGameManager.generalGameData.player2positionY, CustomGameManager.generalGameData.player2positionZ);
        CustomGameManager.instance.players[2].transform.position = new Vector3(CustomGameManager.generalGameData.player3PositionX, CustomGameManager.generalGameData.player3positionY, CustomGameManager.generalGameData.player3positionZ);
        CustomGameManager.instance.players[3].transform.position = new Vector3(CustomGameManager.generalGameData.player4PositionX, CustomGameManager.generalGameData.player4positionY, CustomGameManager.generalGameData.player4positionZ);
        CustomGameManager.instance.players[4].transform.position = new Vector3(CustomGameManager.generalGameData.player5PositionX, CustomGameManager.generalGameData.player5positionY, CustomGameManager.generalGameData.player5positionZ);
        CustomGameManager.instance.players[5].transform.position = new Vector3(CustomGameManager.generalGameData.player6PositionX, CustomGameManager.generalGameData.player6positionY, CustomGameManager.generalGameData.player6positionZ);
        CustomGameManager.instance.players.ForEach(player => player.GetComponent<NavMeshAgent>().enabled = true);
        //setupCamera
        //TODO  get camera base offset
        //GameObject.Find("CameraFocus").transform.position = CustomGameManager.instance.players[0].transform.position;
    }

}
