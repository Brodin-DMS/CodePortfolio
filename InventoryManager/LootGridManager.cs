using RitualOfAnkaraz.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RitualOfAnkaraz.Inventory
{
    /// <summary>
    /// This Class <c>LootGridManager</c> manages Grid logic for all Loot Panels.
    /// </summary>
    public class LootGridManager : BaseGridManager
    {
        [SerializeField] private Transform dropParent;
        public List<ItemClass> itemClasses;

        private void Awake()
        {
            itemClasses = new List<ItemClass>();
        }
        protected override void ClearDropPanel()
        {
            List<ItemScript> itemScripts = new List<ItemScript>(dropParent.GetComponentsInChildren<ItemScript>().ToList());
            foreach (ItemScript obj in itemScripts)
            {
                Destroy(obj.gameObject);
            }
        }

        protected override void AssignItemToDropParent(GameObject itemGO)
        {
            itemGO.transform.SetParent(dropParent);
            itemGO.GetComponent<RectTransform>().pivot = Vector2.zero;
            itemGO.transform.position = slotGrid[totalOffset.x, totalOffset.y].transform.position;
            itemGO.GetComponent<CanvasGroup>().alpha = 1f;
        }
        protected override void AssignItemToDropParent(int gridPositionX, int gridPositionY, GameObject itemGO)
        {
            itemGO.transform.SetParent(dropParent);
            itemGO.GetComponent<RectTransform>().pivot = Vector2.zero;
            itemGO.transform.position = slotGrid[gridPositionX, gridPositionY].transform.position;
            itemGO.GetComponent<CanvasGroup>().alpha = 1f;
        }

        protected override void RemoveItemFromInventoryList(ItemClass item)
        {
            bool itemIsRemoved = false;
            itemIsRemoved = itemClasses.Remove(item);
        }

        protected override void AddItemToInventoryList(ItemClass item)
        {
            itemClasses.Add(item);
        }
    }
}