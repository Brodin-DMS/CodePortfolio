using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RitualOfAnkaraz.Stats;
using System;

namespace RitualOfAnkaraz.Items
{
    public class LootGenerator : MonoBehaviour
    {
        private static int baseUpgradeAmount = 2;
        //CombatLoot
        public static List<ItemClass> GenerateLoot(List<CharacterStatsBase> enemies)
        {
            List<ItemClass> itemList = new List<ItemClass>();

            foreach (CharacterStatsBase enemy in enemies)
            {
                //50% chance to drop an item
                if (UnityEngine.Random.Range(0, 101) <= 50)
                {
                    itemList.Add(generateItem(enemy.level));
                }
            }
            //bad luck protection
            if (itemList.Count == 0)
            {
                //drop atleast 1 item after combat
                itemList.Add(generateItem(enemies[0].level));
            }
            return itemList;
        }
        //chest LOOT
        public static List<ItemClass> GenerateLoot(int lootAmount, int lootLevel)
        {
            List<ItemClass> itemList = new List<ItemClass>();
            for (int i = 0; i < lootAmount; i++)
            {
                itemList.Add(generateItem(lootLevel));
            }
            return itemList;
        }
        //generates items randomly, based on enemy level
        public static ItemClass generateItem(int level)
        {
            int quality = 0;
            int rarity = 0;
            ItemClass newItem;
            //Array.ForEach(CustomGameManager.Instance.CharacterStats, stat => { quality += stat.lootLuck; rarity += stat.lootLuck; });

            for (int i = 0; i < level + baseUpgradeAmount; i++)
            {
                if (UnityEngine.Random.Range(1, 101) < 20)
                {
                    quality++;
                }
                if (UnityEngine.Random.Range(1, 101) < 20 && rarity < 5)
                {
                    rarity++;
                }

            }
            int randomLootTypeNumber = UnityEngine.Random.Range(1, 101);

            int lootType = 25; //something goes wgron return healing potion

            // 66% chance to drop equip FIXME TODO
            if (randomLootTypeNumber <= 100)
            {
                //generate Eq
                lootType = UnityEngine.Random.Range(0, 33);
                newItem = ItemFactory.CreateEquipment(rarity, quality, level);

            }
            else
            {
                //generate Consumable
                lootType = UnityEngine.Random.Range(33, 56);
                newItem = ItemFactory.CreateConsumable(level);


            }
            return newItem;

        }
        //Generates Specific Equipment By Type, maybe not needed.. if i know item type i can call constructor
        public static Equipment GenerateEquipment(ItemType itemType,  int rarity, int quality, int level)
        {
            switch (itemType)
            {
                //Weapons
                case ItemType.Axe:
                    return new Axe(rarity, quality, level);
                case ItemType.Dagger:
                    return new Dagger(rarity, quality, level);
                case ItemType.Shortsword:
                    return new ShortSword(rarity, quality, level);
                case ItemType.Sword:
                    return new Sword(rarity, quality, level);
                case ItemType.Mace:
                    return new Mace(rarity, quality, level);
                case ItemType.Wand:
                    return new OneHandedWand(rarity, quality, level);
                case ItemType.LargeArcaneRod:
                    return new TwoHandedSourceStaff(rarity, quality, level);
                case ItemType.LargeSpiritualRod:
                    return new TwoHandedSpiritStaff(rarity, quality, level);
                case ItemType.RecurveBow:
                    return new RecurveBow(rarity, quality, level);
                case ItemType.LongBow:
                    return new LongBow(rarity, quality, level);
                case ItemType.Crossbow:
                    return new Crossbow(rarity, quality, level);
                case ItemType.Quarterstaff:
                    return new Quarterstaff(rarity, quality, level);
                case ItemType.GreatAxe:
                    return new GreatAxe(rarity, quality, level);
                case ItemType.GreatSword:
                    return new GreatSword(rarity,quality, level);
                case ItemType.GreatWarHammer:
                    return new TwoHandedMace(rarity, quality, level);
                case ItemType.FistWeapon:
                    return  new FistWeapon(rarity, quality, level);
                case ItemType.LargeHeavyShield:
                    return new LargeHeavyShield(rarity, quality, level);
                case ItemType.SmallHeavyShield:
                    return new SmallHeavyShield(rarity, quality, level);
                case ItemType.SmallLightShield:
                    return new SmallLightShield(rarity, quality, level);
                case ItemType.Quiver:
                    return new Quiver(rarity, quality, level);
                case ItemType.Codex:
                    return new Codex(rarity,quality,level);
                case ItemType.ClothHelmet:
                    return new ClothHelemt(rarity,quality,level);
                case ItemType.LeatherHelmet:
                    return new LeatherHelmet(rarity,quality,level);
                case ItemType.PlateHelmet:
                    return new PlateHelmet(rarity, quality, level);
                case ItemType.ClothChest:
                    return new ClothChest(rarity,quality,level);
                case ItemType.LeaterChest:
                    return new LeatherChest(rarity,quality,level);
                case ItemType.PlateChest:
                    return new PlateChest(rarity,quality,level);
                case ItemType.ClothBelt:
                    return new ClothBelt(rarity,quality,level);
                case ItemType.LeatherBelt:
                    return new LeatherBelt(rarity,quality,level);
                case ItemType.PlateBelt:
                    return new PlateBelt(rarity,quality,level);
                case ItemType.Gloves:
                    return new Gloves(rarity,quality,level);
                case ItemType.ClothBoots:
                    return new ClothBoots(rarity,quality,level);
                case ItemType.LeatherBoots:
                    return new LeatherBoots(rarity,quality,level);
                case ItemType.PlateBoots:
                    return new PlateBoots(rarity,quality,level);
                case ItemType.Ring:
                    return new Ring(rarity,quality,level);
                case ItemType.Amulet:
                    return new Neck(rarity,quality,level);
                default: throw new System.NotImplementedException("Item Type nt implemented in LootGenerator");
            }
        }
        public static Consumable GenerateConsumable(ItemType itemType)
        {
            throw new System.NotImplementedException("Consumable are not yet implemented");
        }
    }
}