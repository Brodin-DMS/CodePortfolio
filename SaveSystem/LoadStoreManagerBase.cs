using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using RitualOfAnkaraz.Stats;
/// <summary>
/// This is the base class of the Store manager and will load all general aspects of the game. Its the base class to all loaders.
/// Inherite Awake in all subclasses To Load everything but LevelData without touching a single line of code :).
/// </summary>
public class LoadStoreManagerBase : MonoBehaviour
{
    public bool isLoadedFromCharCreation;

    CharachterData[] rosterData;
    public GameObject playerGroup;

    public InvenGridManager[] gridManger;
    public GameObject eqPrefab;

    //Hotbar
    public UISkillSlotManager uISkillSlotManager;

    public GameObject playerModelPrefab;
    public GameObject playerTargetCirclePrefab;

    //charachter Models
    //TODO setup visuals based on gear after loading
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public GameObject playerThreePrefab;
    public GameObject playerFourPrefab;
    public GameObject playerFivePrefab;
    public GameObject playerSixPrefab;

    //assing in inspector
    public Material playerOneTargetCircle;
    public Material playerTwoTargetCircle;
    public Material playerThreeTargetCircle;
    public Material playerFourTargetCircle;
    public Material playerFiveTargetCircle;
    public Material playerSixTargetCircle;

    /// <summary>
    /// Sets up all General Game Data,CharachterData, and HotBar/Spell/onUseItems
    /// </summary>
    protected virtual void Awake()
    {
        //create Roster
        rosterData = new CharachterData[6];
        for (int i = 0; i < 6; i++)
        {
            rosterData[i] = SaveSystem.LoadCharachterData(CustomGameManager.gameSaveSlot, i);
        }
        //LoadCharachter
        Array.ForEach<CharachterData>(rosterData, data => CreatePlayer(data));
        //Load Inventory
        Array.ForEach<CharachterData>(rosterData, data => StartCoroutine(SetupInventory(data)));
        //LoadGameData
        CustomGameManager.generalGameData = SaveSystem.LoadGeneralGameData(CustomGameManager.gameSaveSlot);
        //LoadHotBarSlots
        SlotDataSeri slotDataSeri = SaveSystem.LoadSlotData(CustomGameManager.gameSaveSlot);
        slotDataSeri.DebugSeri();


        //SetupHotbar(slotDataSeri);
        uISkillSlotManager.SetAllSkills(slotDataSeri.baseSkillsData);
        //Update chakra and hp bar
        CustomGameManager.instance.players.ForEach(player => player.GetComponent<CharacterStatsBase>().UpdateChakraBar());
        CustomGameManager.instance.players.ForEach(player => player.GetComponent<CharacterStatsBase>().UpdateHealthbar());

    }
    /// <summary>
    /// Create Player instances form loaded Data
    /// </summary>
    /// <param name="data"></param> all data required to create a stat sheet and Equipment.
    public void CreatePlayer(CharachterData data)
    {
        //assignPlayerParent
        GameObject playerInstance;
        switch (data.id)
        {
            case 0:
                playerInstance = GameObject.Find("PlayerOne");
                break;
            case 1:
                playerInstance = GameObject.Find("PlayerTwo");
                break;
            case 2:
                playerInstance = GameObject.Find("PlayerThree");
                break;
            case 3:
                playerInstance = GameObject.Find("PlayerFour");
                break;
            case 4:
                playerInstance = GameObject.Find("PlayerFive");
                break;
            case 5:
                playerInstance = GameObject.Find("PlayerSix");
                break;
            default:
                Debug.LogError("could not determine player position from:" + data.id);
                playerInstance = GameObject.Find("PlayerOne");
                break;
        }
        //setup playerMotor
        playerInstance.GetComponent<PlayerMotor>().Setup(data.id);
        //setup playerController
        playerInstance.GetComponent<PlayerController>().Setup();
        //setup Inventory
        playerInstance.GetComponent<Inventory>().Setup(data.id);
        //setup CharachterAnimator --this depends on children in start call. this is not that bad aslong children get instantiated in awake. So this should work atm without invoking anything
        //setup CharacterStatbase
        playerInstance.GetComponent<CharacterStatsBase>().SetupFromCharData(data);
        //setupChildren
        //setup Modeel TODO change theese if new models for races and klasses exist -- for now the switch case is skipped and the same modell is instantiated for evryone;

        #region OBSOLETE_charachterPrefabAsignment
        //TODO && FIX ME this code is OBsolete. 
        /*
        switch (data.race)
        {
            case Race.Dwarf:
                if (data.sex == Sex.Male)
                {
                    Instantiate(maleDwarfPrefab, playerInstance.transform);
                }
                else
                {
                    Instantiate(femaleDwardPrefab, playerInstance.transform);
                }
                break;
            case Race.Elf:
                if (data.sex == Sex.Male)
                {
                    Instantiate(maleElfPrefab, playerInstance.transform);
                }
                else
                {
                    Instantiate(femaleElfPrefab, playerInstance.transform);
                }
                break;
            case Race.HalfElf:
                if (data.sex == Sex.Male)
                {
                    Instantiate(maleHalfElfPrefab, playerInstance.transform);
                }
                else
                {
                    Instantiate(femaleHalfElfPrefab, playerInstance.transform);
                }
                break;
            case Race.Halfling:
                if (data.sex == Sex.Male)
                {
                    Instantiate(maleHaflingPrefab, playerInstance.transform);
                }
                else
                {
                    Instantiate(femaleHaflingPrefab, playerInstance.transform);
                }
                break;
            case Race.HalfOrc:
                if (data.sex == Sex.Male)
                {
                    Instantiate(maleHalfOrcPrefab, playerInstance.transform);
                }
                else
                {
                    Instantiate(femaleHalfOrcPrefab, playerInstance.transform);
                }
                break;
            case Race.Human:
                if (data.sex == Sex.Male)
                {
                    if(data.initialCharClass == CharachterClass.Fighter)
                    {
                        Instantiate(maleHumanFighterPrefab, playerInstance.transform);
                    }
                    else if(data.initialCharClass == CharachterClass.Barbarian)
                    {
                        Instantiate(maleHumanBarbarianPrefab, playerInstance.transform);
                    }else if(data.initialCharClass == CharachterClass.Paladin)
                    {
                        Instantiate(maleHumanPaladinPrefab, playerInstance.transform);
                    }else if(data.initialCharClass == CharachterClass.Ranger)
                    {
                        Instantiate(maleHumanFighterPrefab, playerInstance.transform);
                    }else if(data.initialCharClass == CharachterClass.Sorcerer)
                    {
                        Instantiate(maleHumanFighterPrefab, playerInstance.transform);
                    }
                    else
                    {
                        Instantiate(maleHumanFighterPrefab, playerInstance.transform);
                    }
                }
                else
                {
                    Instantiate(femaleHumanFighterPrefab, playerInstance.transform);
                }
                break;
            default:
                Debug.LogError("could not match player class");
                break;
        }
        */
        #endregion

        //setupTargetingCircle
        GameObject playerTargetCircle = Instantiate(playerTargetCirclePrefab, playerInstance.transform);
        switch (data.id)
        {
            case 0:
                Instantiate(playerOnePrefab, playerInstance.transform);
                playerTargetCircle.GetComponent<MeshRenderer>().material = playerOneTargetCircle;
                break;
            case 1:
                Instantiate(playerTwoPrefab, playerInstance.transform);
                playerTargetCircle.GetComponent<MeshRenderer>().material = playerTwoTargetCircle;
                break;
            case 2:
                Instantiate(playerThreePrefab, playerInstance.transform);
                playerTargetCircle.GetComponent<MeshRenderer>().material = playerThreeTargetCircle;
                break;
            case 3:
                Instantiate(playerFourPrefab, playerInstance.transform);
                playerTargetCircle.GetComponent<MeshRenderer>().material = playerFourTargetCircle;
                break;
            case 4:
                Instantiate(playerFivePrefab, playerInstance.transform);
                playerTargetCircle.GetComponent<MeshRenderer>().material = playerFiveTargetCircle;
                break;
            case 5:
                Instantiate(playerSixPrefab, playerInstance.transform);
                playerTargetCircle.GetComponent<MeshRenderer>().material = playerSixTargetCircle;
                break;
            default:
                Debug.LogError("couldn't assign material to Player in slot: " + data.id);
                break;

        }

    }
    /// <summary>
    /// Load and setup inventory and equipped items. If you don't like changing manual script execution orders this has to be a coroutine, or you'll get null pointers. Since this class needs method to the inventory manager.
    /// </summary>
    /// <param name="data"></param> all data required to create a stat sheet and Equipment.
    /// <returns></returns>
    IEnumerator SetupInventory(CharachterData data)
    {
        yield return new WaitForSeconds(1f);
        //TODO CONT HERE CAll Store item to absoluteSlot
        //HAHAHAHAH BIG UFF WORST SWITCH CASE EVER
        InvenGridManager inventoryInstanceScript;
        inventoryInstanceScript = gridManger[data.id];
        //Creates an item prefab and adds it to the inventory slot
        //TODO MAYBE fix encumbrance in char data and set it to 0 while loading char sheet to cahrachter
        List<GameObject> itemGos = new List<GameObject>();
        foreach (ItemClassData itemClassData in data.inventoryItems)
        {
            GameObject newItem = Instantiate(eqPrefab);
            newItem.GetComponent<ItemScript>().SetItemObject(new ItemClass(itemClassData));
            itemGos.Add(newItem);
        }
        //store
        itemGos.ForEach(item => inventoryInstanceScript.StorItemToAbsolutSlot(item));
        //setupEquipped items
        itemGos = new List<GameObject>();
        foreach (ItemClassData itemClassData in data.eqItems)
        {
            GameObject newItem = Instantiate(eqPrefab);
            newItem.GetComponent<ItemScript>().SetItemObject(new ItemClass(itemClassData));
            itemGos.Add(newItem);
        }
        //store equipped items
        foreach (GameObject eQitemGo in itemGos)
        {
            foreach (GameObject eqSlot in inventoryInstanceScript.eqSlots)
            {
                eqSlot.GetComponent<EquipmentSlot>().SetupEqSlot(eQitemGo);
            }
        }

    }

    /// <summary>
    /// Restores a saved HotbarSlot after loading
    /// </summary>
    /// <param name="slotDataSeri"></param> A saved hotbarSlot, with all data needed to link it to spells/OnUseitems
    public void SetupHotbar(SlotDataSeri slotDataSeri)
    {

        //Try calling a setSLots Method and then store all results.

        Debug.Log("HOTBAR___READ starting setting up hotbar");
        Debug.Log("HOTBAR___READ allListCount " + slotDataSeri.baseSkillsData.Count); ;

        //Iterates the six lists
        for (int playerIndex = 0; playerIndex < 6; playerIndex++)
        {
            List<BaseSkillData> baseSkillDataList = slotDataSeri.baseSkillsData[playerIndex];
            Debug.Log("HOTBAR___READ SlotDataLists.Count:" + baseSkillDataList.Count);


            List<SlotData> slotDataList = new List<SlotData>();
            int skillIndex = 0;
            foreach (BaseSkillData baseSkillData in baseSkillDataList)
            {
                if (baseSkillData != null)
                {

                    //Sprite sprite =(Resources.Load<Sprite>(baseSkillData.iconPath));
                    //uISkillSlotManager.hotBarSlotManager.slots[skillIndex]

                    slotDataList.Add(new SlotData(uISkillSlotManager.hotBarSlotManager.slots[skillIndex].image.sprite, new BaseSkill(baseSkillData),null,null));
                    if (slotDataList.Count > 0)
                    {
                        Debug.Log("SETUP_HOT_BAR " + "baseskill is: " + baseSkillData.spellName + "is at index= "+ skillIndex );
                    }
                    
                }
                else
                {
                    slotDataList.Add(new SlotData());
                }
                skillIndex++;
            }
            //uISkillSlotManager.hotBarSlotManager.UpdateSlots(slotDataList);
            Debug.Log("HOTBAR___ Len of slotDataList = " + slotDataList.Count);
            //SET SLOTS FIRST
            //uISkillSlotManager.StoreSkillbar(playerIndex, slotDataList);
            uISkillSlotManager.StoreSkillbar(playerIndex, slotDataList);
            //Update Slots

        }
    }

}
