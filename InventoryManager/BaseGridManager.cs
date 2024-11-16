using RitualOfAnkaraz.Items;
using RitualOfAnkaraz.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RitualOfAnkaraz.Inventory
{
    /// <summary>
    /// Class <c>BaseGridManager</c> is the Base Inventory class to support Grid Inventories across the Codebase.
    /// The Base Implementation was taken from a third party Contributor, This is a refactored version to support multiple Inventories with custom logic.
    /// </summary>
    public abstract class BaseGridManager : MonoBehaviour
    {
        public GameObject[,] slotGrid;
        public GameObject highlightedSlot;

        [HideInInspector]
        public Vector2Int gridSize;
        protected Vector2Int totalOffset, checkSize, checkStartPos;
        protected Vector2Int otherItemPos, otherItemSize; //*3

        protected int checkState;
        protected bool isOverEdge = false;

        //NOTE Perfomance, original Creator used Update for this. Refactor this to an IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler to boost performance. (Not Worth It its fine)
        //NOTE cont. this will clean up the codebase aswell since another structure can be used in the BaseGridManager class. (Not Worth It its fine)
        protected virtual void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (highlightedSlot != null && ItemScript.selectedItem != null && !isOverEdge)
                {
                    switch (checkState)
                    {
                        case 0: //store on empty slots
                            StoreItem(ItemScript.selectedItem);
                            ColorChangeLoop(SlotColorHighlights.Blue, ItemScript.selectedItemSize, totalOffset);
                            ItemScript.ResetSelectedItem();
                            break;
                        case 1: //swap items
                            ItemScript.SetSelectedItem(SwapItem(ItemScript.selectedItem));
                            SlotSectorScript.sectorScript.PosOffset();
                            ColorChangeLoop(SlotColorHighlights.Gray, otherItemSize, otherItemPos); //*1
                            RefrechColor(true);
                            break;
                    }
                }// retrieve items
                else if (highlightedSlot != null && ItemScript.selectedItem == null && highlightedSlot.GetComponent<SlotScript>().isOccupied == true)
                {
                    ColorChangeLoop(SlotColorHighlights.Gray, highlightedSlot.GetComponent<SlotScript>().storedItemSize, highlightedSlot.GetComponent<SlotScript>().storedItemStartPos);
                    ItemScript.SetSelectedItem(RemoveItem(highlightedSlot));
                    SlotSectorScript.sectorScript.PosOffset();
                    RefrechColor(true);
                }
            }
        }

        //--------------------Utils Grid--------------------------------
        /// <summary>
        /// this Method checks weather the item is inside the boundries of the Inventory
        /// </summary>
        /// <param name="itemSize">Amount of slots the item takes up inside of the inventory. 2 dimesnional vector with x,y </param>
        private void CheckArea(Vector2Int itemSize) //*2
        {
            Vector2Int halfOffset = new();
            Vector2Int overCheck;
            halfOffset.x = (itemSize.x - (itemSize.x % 2 == 0 ? 0 : 1)) / 2;
            halfOffset.y = (itemSize.y - (itemSize.y % 2 == 0 ? 0 : 1)) / 2;
            totalOffset = highlightedSlot.GetComponent<SlotScript>().gridPos - (halfOffset + SlotSectorScript.posOffset);
            checkStartPos = totalOffset;
            checkSize = itemSize;
            overCheck = totalOffset + itemSize;
            isOverEdge = false;
            //checks if item to stores is outside grid
            if (overCheck.x > gridSize.x)
            {
                checkSize.x = gridSize.x - totalOffset.x;
                isOverEdge = true;
            }
            if (totalOffset.x < 0)
            {
                checkSize.x = itemSize.x + totalOffset.x;
                checkStartPos.x = 0;
                isOverEdge = true;
            }
            if (overCheck.y > gridSize.y)
            {
                checkSize.y = gridSize.y - totalOffset.y;
                isOverEdge = true;
            }
            if (totalOffset.y < 0)
            {
                checkSize.y = itemSize.y + totalOffset.y;
                checkStartPos.y = 0;
                isOverEdge = true;
            }
        }
        /// <summary>
        /// this Method checks weather the item can be placed at the current position inside of the Inventory
        /// </summary>
        /// <param name="itemSize">2 dimesnional vector with x,y indicating how many slots the item will take up.</param>
        /// <returns> 0 = all slots are not occupied, the item can be placed.
        /// 1 = only 1 item occupies the slots, the item can be exchanged.
        /// 2 = more than 1 item occupies the slots, the item cannot be placed.</returns>
        private int SlotCheck(Vector2Int itemSize)//*2
        {
            GameObject obj = null;
            SlotScript instanceScript;
            if (!isOverEdge)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    for (int x = 0; x < itemSize.x; x++)
                    {
                        instanceScript = slotGrid[checkStartPos.x + x, checkStartPos.y + y].GetComponent<SlotScript>();
                        if (instanceScript.isOccupied)
                        {
                            if (obj == null)
                            {
                                obj = instanceScript.storedItemObject;
                                otherItemPos = instanceScript.storedItemStartPos;
                                otherItemSize = obj.GetComponent<ItemScript>().item.size;
                            }
                            else if (obj != instanceScript.storedItemObject)
                                return 2; // if cheack Area has 1+ item occupied
                        }
                    }
                }
                if (obj == null)
                    return 0; // if checkArea is not accupied
                else
                    return 1; // if checkArea only has 1 item occupied
            }
            return 2; // check areaArea is over the grid
        }
        //NOTE ENCAPSULATION!
        //This code smells 3 public funktion doing the same thing.
        //This should be 2 private methods + 1 protected method, that should be exposed to extend the code properly.
        //This code should then be called from a public method declared in an interface or abstract method declartion
        /// <summary>
        /// Top level Function that initiates a Color Change of the Inventory. from original creator, no need for refactoring but this code segment is poorly structured!
        /// </summary>
        /// <param name="enter">I asume its an Indicator weather this code should run. performance.</param>
        public void RefrechColor(bool enter)
        {
            if (enter)
            {
                CheckArea(ItemScript.selectedItemSize);
                checkState = SlotCheck(checkSize);
                switch (checkState)
                {
                    case 0: ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos); break; //no item in area
                    case 1:
                        ColorChangeLoop(SlotColorHighlights.Yellow, otherItemSize, otherItemPos); //1 item on area and can swap
                        ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos);
                        break;
                    case 2: ColorChangeLoop(SlotColorHighlights.Red, checkSize, checkStartPos); break; //invalid position (more then 2 items in area or area is outside grid)
                }
            }
            else //on pointer exit. resets colors
            {
                isOverEdge = false;
                //checkArea(); //commented out for performance. may cause bugs if not included

                ColorChangeLoop2(checkSize, checkStartPos);
                if (checkState == 1)
                {
                    ColorChangeLoop(SlotColorHighlights.Blue2, otherItemSize, otherItemPos);
                }
            }
        }
        //This Method 0 that changes the actual color, this should be private but code structure didnt allow a bug free extension without calling this!
        public void ColorChangeLoop(Color32 color, Vector2Int size, Vector2Int startPos)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = color;
                }
            }
        }
        //This Method 1 that changes the actual color, this should be private but code structure didnt allow a bug free extension without calling this!
        public void ColorChangeLoop2(Vector2Int size, Vector2Int startPos)
        {
            GameObject slot;
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    slot = slotGrid[startPos.x + x, startPos.y + y];
                    if (slot.GetComponent<SlotScript>().isOccupied != false)
                    {
                        slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = SlotColorHighlights.Blue2;
                    }
                    else
                    {
                        slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = SlotColorHighlights.Gray;
                    }
                }
            }
        }
        /// <summary>
        /// This Methods clears all items from the Inventory.
        /// </summary>
        public void ClearAllSlots()
        {
            SlotScript instanceScript;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    instanceScript = slotGrid[x, y].GetComponent<SlotScript>();
                    slotGrid[x, y].GetComponent<Image>().color = SlotColorHighlights.Gray;
                    instanceScript.storedItemObject = null;
                    instanceScript.storedItemSize = new Vector2Int(0, 0);
                    instanceScript.storedItemStartPos = new Vector2Int(0, 0);
                    instanceScript.storedItemClass = null;
                    instanceScript.isOccupied = false;
                }
            }
            ClearDropPanel();
        }
        /// <summary>
        /// Destroys Item representation in the UI layer.
        /// </summary>
        protected abstract void ClearDropPanel();
        /// <summary>
        /// returns the attached data of the Item(itemClass) without removing or destroing it. may return null if no item is present in the slotScript.
        /// </summary>
        /// <param name="slotObject"> Inventory Slot holding items</param>
        /// <returns>attached data Object ItemClass, nullable</returns>
        public ItemClass GetItemClassInfo(GameObject slotObject)
        {
            SlotScript slotObjectScript = slotObject.GetComponent<SlotScript>();
            ItemClass retItem = slotObjectScript.storedItemClass;
            return retItem;
        }
        //NOTE ENCAPSULATION!, this should probably be private or at least protected 
        /// <summary>
        /// Checks weather an item can be stored at current hovered + adjusted position
        /// </summary>
        /// <param name="x">x position in slot grid</param>
        /// <param name="y">y position in slot grid</param>
        /// <param name="itemSize">Size of itme im slot as 2 dim vector x,y</param>
        /// <returns>true = item can be stored, false = item cannot be stored</returns>
        public bool ValidateStorageSlot(int x, int y, Vector2Int itemSize)
        {
            SlotScript instanceScriptCheckArea;

            for (int adjacentY = 0; adjacentY < itemSize.y && y + adjacentY < gridSize.y; adjacentY++)
            {
                for (int adjacentX = 0; adjacentX < itemSize.x && x + adjacentX < gridSize.x; adjacentX++)
                {
                    instanceScriptCheckArea = slotGrid[x + adjacentX, y + adjacentY].GetComponent<SlotScript>();
                    Debug.Log("x is " + x + " and y is " + y + "and occupied =" + instanceScriptCheckArea.isOccupied);
                    if (instanceScriptCheckArea.isOccupied == true || y + itemSize.y > gridSize.y || x + itemSize.x > gridSize.x) return false;
                }
            }
            return true;
        }


        //-----------------STORE AND REMOVE LOGIC------------------------
        //NOTE DRY!, this should be nested to Click
        /// <summary>
        /// Stores an item to Inventory, position is determined by clicking into Inventory while an item is dragged.
        /// </summary>
        /// <param name="item">GO with attched itemScript, that is currently being dragged</param>
        public void StoreItem(GameObject item)
        {
            SlotScript instanceScript;
            Vector2Int itemSizeL = item.GetComponent<ItemScript>().item.size;
            for (int y = 0; y < itemSizeL.y; y++)
            {
                for (int x = 0; x < itemSizeL.x; x++)
                {
                    //set each slot parameters
                    instanceScript = slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<SlotScript>();
                    instanceScript.storedItemObject = item;
                    instanceScript.storedItemClass = item.GetComponent<ItemScript>().item;
                    instanceScript.storedItemSize = itemSizeL;
                    instanceScript.storedItemStartPos = totalOffset;
                    instanceScript.isOccupied = true;
                    slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = SlotColorHighlights.Gray;
                    //set item start position on itemClass
                    instanceScript.storedItemClass.storedItemStartPos = instanceScript.storedItemStartPos;
                }
            }//set dropped parameters
            AssignItemToDropParent(item);
            ToolTipManager.Instance.ShowItemToolTip(highlightedSlot.GetComponent<SlotScript>().storedItemClass, item.transform.position);
            AddItemToInventoryList(item.GetComponent<ItemScript>().item);
        }
        /// <summary>
        /// Stores Item without button Click. This is used when loading Items or Populating Inventories
        /// </summary>
        /// <param name="gridPositionX">X value of slot in inventory</param>
        /// <param name="gridPositionY">y value of slot in inventory</param>
        /// <param name="itemGO"> GO with attached Itemscript</param>
        public void StoreItem(int gridPositionX, int gridPositionY, GameObject itemGO)
        {
            SlotScript instanceScript;
            Vector2Int startPos = new Vector2Int(gridPositionX, gridPositionY);
            Vector2Int itemSize = itemGO.GetComponent<ItemScript>().item.size;
            for (int adjy = 0; adjy < itemSize.y; adjy++)
            {
                for (int adjx = 0; adjx < itemSize.x; adjx++)
                {
                    //set each slot parameters
                    instanceScript = slotGrid[gridPositionX + adjx, gridPositionY + adjy].GetComponent<SlotScript>();
                    instanceScript.storedItemObject = itemGO;
                    instanceScript.storedItemClass = itemGO.GetComponent<ItemScript>().item;
                    instanceScript.storedItemSize = itemSize;
                    instanceScript.storedItemStartPos = startPos;
                    instanceScript.isOccupied = true;
                    slotGrid[gridPositionX + adjx, gridPositionY + adjy].GetComponent<Image>().color = SlotColorHighlights.Gray;
                    instanceScript.storedItemClass.storedItemStartPos = instanceScript.storedItemStartPos;
                }
            }
            //set dropped parameters
            AssignItemToDropParent(gridPositionX, gridPositionY, itemGO);
            ColorChangeLoop(SlotColorHighlights.Blue2, itemSize, startPos);
            itemGO.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 1);
            AddItemToInventoryList(itemGO.GetComponent<ItemScript>().item);
        }
        //NOTE DRY!, this should be nested
        //Assign Drop Parent
        /// <summary>
        /// Changes the Parent transform of the GO from drag parent to drop parent of the Inventory.Dependent on user on OnClick
        /// </summary>
        /// <param name="itemGO">GO with attached Itemscript</param>
        protected abstract void AssignItemToDropParent(GameObject itemGO);
        /// <summary>
        /// Changes the Parent transform of the GO from drag parent to drop parent of the specific Inventory. Not Dependent on OnClick
        /// </summary>
        /// <param name="gridPositionX"></param>
        /// <param name="gridPositionY"></param>
        /// <param name="itemGO"></param>
        protected abstract void AssignItemToDropParent(int gridPositionX, int gridPositionY, GameObject itemGO);
        /// <summary>
        /// Removes an item From inventory with a button click in Grid
        /// </summary>
        /// <param name="slotObject"> slot the player clicks</param>
        /// <returns>GO with attached itemscript and Itemclass of the retrieved item</returns>
        public GameObject RemoveItem(GameObject slotObject)
        {
            GameObject itemGO = GetItem(slotObject);
            RemoveItemFromInventoryList(itemGO.GetComponent<ItemScript>().item);
            return itemGO;
        }
        /// <summary>
        /// //Removes item without a btn click in Roster.Removed by ref. Currently only used for consumables that are cast.
        /// </summary>
        /// <param name="item">Data object (ItemClass) of an Item</param>
        public void RemoveItem(ItemClass item)
        {
            SlotScript instanceScript;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    instanceScript = slotGrid[x, y].GetComponent<SlotScript>();
                    if (instanceScript.storedItemClass != null && instanceScript.storedItemClass == item)
                    {
                        slotGrid[x, y].GetComponent<Image>().color = SlotColorHighlights.Gray;
                        instanceScript.storedItemObject = null;
                        instanceScript.storedItemSize = new Vector2Int(0, 0);
                        instanceScript.storedItemStartPos = new Vector2Int(0, 0);
                        instanceScript.storedItemClass = null;
                        instanceScript.isOccupied = false;
                    }
                }
            }
            RemoveItemFromInventoryList(item);
        }

        /// <summary>
        /// Swaps the current dragged item with the stored item in the Inventory Slots. Precondition items are swappable is always true and checked beforehand.
        /// </summary>
        /// <param name="item">currently dragged item</param>
        /// <returns>item stored in Inventory</returns>
        public GameObject SwapItem(GameObject item)
        {
            GameObject tempItem;
            tempItem = RemoveItem(slotGrid[otherItemPos.x, otherItemPos.y]);
            item.GetComponent<ItemScript>().border.enabled = true;
            StoreItem(item);
            return tempItem;
        }

        /// <summary>
        /// Removes Item From Grid in UI layer
        /// </summary>
        /// <param name="slotObject"></param>
        /// <returns>GO with attached itemScript and itemClass</returns>
        protected GameObject GetItem(GameObject slotObject)
        {
            SlotScript slotObjectScript = slotObject.GetComponent<SlotScript>();
            GameObject retItem = slotObjectScript.storedItemObject;
            Vector2Int tempItemPos = slotObjectScript.storedItemStartPos;
            Vector2Int itemSizeL = retItem.GetComponent<ItemScript>().item.size;

            SlotScript instanceScript;
            for (int y = 0; y < itemSizeL.y; y++)
            {
                for (int x = 0; x < itemSizeL.x; x++)
                {
                    //reset each slot parameters
                    instanceScript = slotGrid[tempItemPos.x + x, tempItemPos.y + y].GetComponent<SlotScript>();
                    instanceScript.storedItemObject = null;
                    instanceScript.storedItemSize = new Vector2Int(0, 0);
                    instanceScript.storedItemStartPos = new Vector2Int(0, 0);
                    //set item start position on itemClass
                    instanceScript.storedItemClass.storedItemStartPos = instanceScript.storedItemStartPos;
                    instanceScript.storedItemClass = null;
                    instanceScript.isOccupied = false;
                }
            }//returned item set item parameters
            retItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            retItem.GetComponent<CanvasGroup>().alpha = 0.5f;
            retItem.transform.position = Input.mousePosition;
            ToolTipManager.Instance.HideAllTooltips();
            return retItem;
        }

        /// <summary>
        /// removes the Item from Inventory List.
        /// </summary>
        /// <param name="item">dataClass of item that should be removed by ref</param>
        protected abstract void RemoveItemFromInventoryList(ItemClass item);
        /// <summary>
        /// Adds item data Object to inventory List
        /// </summary>
        /// <param name="item"></param>
        protected abstract void AddItemToInventoryList(ItemClass item);
        //NOTE this should go to store, sincee your storing GO, this eeven calls store nesteed
        /// <summary>
        /// Adds Item withou a button click involved, useed for Loading player Inventories and Populating Loot Inventories.
        /// </summary>
        /// <param name="item">GO that should be stored </param>
        /// <returns></returns>
        public bool AddItemToEmptySlot(GameObject item)
        {
            SlotScript instanceScriptNew;
            Vector2Int itemSizeNew = item.GetComponent<ItemScript>().item.size;
            {
                //check for empty slot
                for (int y = 0; y < gridSize.y; y++)
                {
                    for (int x = 0; x < gridSize.x; x++)
                    {
                        instanceScriptNew = slotGrid[x, y].GetComponent<SlotScript>();
                        if (instanceScriptNew.isOccupied == false)
                        {
                            if (ValidateStorageSlot(x, y, itemSizeNew))
                            {
                                StoreItem(x, y, item);
                                return true;
                            }
                        }

                    }
                }

            }
            return false;
        }
    }
}

namespace RitualOfAnkaraz.Items
{
    public struct SlotColorHighlights
    {
        public static Color32 Green
        { get { return new Color32(127, 223, 127, 255); } }
        public static Color32 Yellow
        { get { return new Color32(223, 223, 63, 255); } }
        public static Color32 Red
        { get { return new Color32(223, 127, 127, 255); } }
        public static Color32 Blue
        { get { return new Color32(159, 159, 223, 255); } }
        public static Color32 Blue2
        { get { return new Color32(191, 191, 223, 255); } }
        public static Color32 Gray
        { get { return new Color32(223, 223, 223, 255); } }
    }

    //NOTE this should prbably go th the actual inventory script
    public enum EncumbranceState
    {
        VeryLight,
        Light,
        Medium,
        Heavy,
        VeryHeavy,
        Encumbranced
    }
}