using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using RitualOfAnkaraz.Items;
using RitualOfAnkaraz.UI;
using Unity.VisualScripting;
using System;

namespace RitualOfAnkaraz.Inventory
{
    /// <summary>
    /// This class <c>InvenGridManger</c> manages the Grid logic for Player Inventories.
    /// </summary>
    public class InvenGridManager : BaseGridManager
    {
        [SerializeField] private Transform[] dropParents;
        public List<ItemClass>[] charItems; //Pass this in editor
        public EquipmentSlot[] eqSlots; //Pass this in editor

        //--------------------Support For Multiple Players---------------
        public Inventory[] inventories;
        public InventorySlotRuntimeData[][,] characterSlotGridData;
        private const int AMOUNT_INVENTORIES = 4;
        private const int GRID_SIZE_X = 14;
        private const int GRID_SIZE_Y = 16;
        //---------------------------------------------------------------

        public static int cloth, leather, iron, wood, sourceEssence, goodlyEssence;
        public TextMeshProUGUI encDisplay;
        public TextMeshProUGUI encStateDisplay;

        private void Awake()
        {
            //Pass this in editor, probably OBSOLETE
            charItems = new List<ItemClass>[4];
            for (int i = 0; i < charItems.Length; i++)
            {
                charItems[i] = new List<ItemClass>();
            }

            //Init slot grids
            characterSlotGridData = new InventorySlotRuntimeData[AMOUNT_INVENTORIES][,];
            for (int i = 0; i < AMOUNT_INVENTORIES; i++)
            {
                characterSlotGridData[i] = new InventorySlotRuntimeData[GRID_SIZE_X, GRID_SIZE_Y];

                //Init slot Script Data with nothing for all chrachters, Change this when loading from real Serializable Data on Scene Load
                for (int j = 0; j < GRID_SIZE_X; j++)
                {
                    for (int k = 0; k < GRID_SIZE_Y; k++)
                    {
                        characterSlotGridData[i][j, k] = new InventorySlotRuntimeData();
                    }
                }
            }

        }
        private void Start()
        {
            eqSlots = EquipmentManager.Instance.eqSlots;
        }

        protected sealed override void Update()
        {
            base.Update();
            
            if (Input.GetMouseButton(1))
            {
                //TODO handle Potion use out of combat, maybe allow salves from inventory aswell salves aswell.
                throw new NotImplementedException("Implement Rigth Click in Inventory to use Consumables Out Of Combat without hotkeying them.");
            }
        }
        protected override void AddItemToInventoryList(ItemClass item)
        {
            charItems[CustomGameManager.SelectedCharacter].Add(item);
            inventories[CustomGameManager.SelectedCharacter].AddItem(item);
        }
        protected override void AssignItemToDropParent(GameObject itemGO)
        {
            itemGO.transform.SetParent(dropParents[CustomGameManager.SelectedCharacter]);
            itemGO.GetComponent<RectTransform>().pivot = Vector2.zero;
            itemGO.transform.position = slotGrid[totalOffset.x, totalOffset.y].transform.position;
            itemGO.GetComponent<CanvasGroup>().alpha = 1f;
        }
        protected override void AssignItemToDropParent(int gridPositionX, int gridPositionY, GameObject itemGO)
        {
            itemGO.transform.SetParent(dropParents[CustomGameManager.SelectedCharacter]);
            itemGO.GetComponent<RectTransform>().pivot = Vector2.zero;
            itemGO.transform.position = slotGrid[gridPositionX, gridPositionY].transform.position;
            itemGO.GetComponent<CanvasGroup>().alpha = 1f;
        }
        protected override void ClearDropPanel()
        {
            List<ItemScript> itemScripts = dropParents[CustomGameManager.SelectedCharacter].GetComponentsInChildren<ItemScript>().ToList();
            foreach (ItemScript obj in itemScripts)
            {
                Destroy(obj.gameObject);
            }
        }
        protected override void RemoveItemFromInventoryList(ItemClass item)
        {
            charItems[CustomGameManager.SelectedCharacter].Remove(item);
            inventories[CustomGameManager.SelectedCharacter].RemoveItem(item);
        }
        public void UpdateEncumbrance()
        {
            throw new NotImplementedException("Encumbrance change is not implemented for swapping gear yet");
        }
        /// <summary>
        /// Calculates and returns the encumbrancestate to determin Movement penealty in Treshholds.
        /// </summary>
        /// <param name="curEnc">characters current wheigt determined by items and equipment</param>
        /// <param name="maxEnc">characters maximum carry limit based on BaseValue and Strength</param>
        /// <returns>enum that represents the Intervall</returns>
        public static EncumbranceState GetEncumbranceState(int curEnc, int maxEnc)
        {
            float res = curEnc / (float)maxEnc;
            if (res < 0.1f) return EncumbranceState.VeryLight;
            if (res < 0.25f) return EncumbranceState.Light;
            if (res < .5f) return EncumbranceState.Medium;
            if (res < .75f) return EncumbranceState.Heavy;
            if (res < 1f) return EncumbranceState.VeryHeavy;
            return EncumbranceState.Encumbranced;
        }


        //--------------------Load Store Grid multiple Players----------
        /// <summary>
        /// This Method is used to Populate the Grid View, based on the players Inventory data without Monobehaviour.
        /// </summary>
        /// <param name="characterIndex">0-3, determines wich </param>
        private void LoadSlotGrid(int characterIndex)
        {
            for(int i = 0; i < GRID_SIZE_X; i++)
            {
                for (int j = 0; j < GRID_SIZE_Y; j++)
                {
                    slotGrid[i, j].GetComponent<SlotScript>().Setup(characterSlotGridData[characterIndex][i, j]);
                }
            }
            //NOTE, the next two lines should be part of view
            Vector2Int maxGrid = new Vector2Int(GRID_SIZE_X,GRID_SIZE_Y);
            ColorChangeLoop2(maxGrid,Vector2Int.zero);
        }
        /// <summary>
        /// This Method is used to store Inventory Data of the Player in a data Object without Monobehaviour.
        /// </summary>
        /// <param name="oldCharIndex"></param>
        private void StoreSlotGrid(int oldCharIndex)
        {
            for (int i = 0; i < GRID_SIZE_X; i++)
            {
                for (int j = 0; j < GRID_SIZE_Y; j++)
                {
                    characterSlotGridData[oldCharIndex][i, j] = slotGrid[i,j].GetComponent<SlotScript>().CreateRuntimeData();
                }
            }
        }
        /// <summary>
        /// This Method Swaps Inventory Data when a different Character is selected.
        /// </summary>
        /// <param name="newCharIndex">0-3, old charachter Index</param>
        /// <param name="oldCharIndex">0-3, new character Index</param>
        public void UpdateInventoryData(int newCharIndex, int oldCharIndex)
        {
            StoreSlotGrid(oldCharIndex);
            LoadSlotGrid(newCharIndex);
        }
        //Note this should be seperate in View
        /// <summary>
        /// Disables All Items Object Pools represented as GameObjcts, then enables the current Selected Object Pool.
        /// </summary>
        public void UpdateView()
        {
            Array.ForEach(dropParents, dropParent => dropParent.gameObject.SetActive(false));
            dropParents[CustomGameManager.SelectedCharacter].gameObject.SetActive(true);
            Vector2Int maxGrid = new Vector2Int(GRID_SIZE_X, GRID_SIZE_Y);
            ColorChangeLoop2(maxGrid, Vector2Int.zero);
        }
    }
}