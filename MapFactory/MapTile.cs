using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RitualOfAnkaraz.Tilemap
{
    /// <summary>
    /// This is attached to gameobjects, it holds Interests and gets Instantiated after the Tilemapgeneration on a logical Level is finished.
    /// </summary>
    public class MapTile : MonoBehaviour, IMapTile
    {
        [SerializeReference] public GroupTile groupTile;
        public List<EnemyEncounterTileInterest> enemyLocations;
        public List<LootInterest> lootLocations;
        public List<EncounterInterest> encounterLocations;

        public virtual void Init(GroupTile groupTile)
        {
            this.groupTile = groupTile;
            this.enemyLocations = GetComponents<EnemyEncounterTileInterest>().ToList();
            this.lootLocations = GetComponents<LootInterest>().ToList();
            this.encounterLocations = GetComponents<EncounterInterest>().ToList();
        }

        public virtual void SetupEnemies()
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetupLoot()
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetupEncounters()
        {
            throw new System.NotImplementedException();
        }
    }

}