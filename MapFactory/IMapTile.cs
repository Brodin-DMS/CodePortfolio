using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RitualOfAnkaraz.Tilemap
{
    /// <summary>
    /// Structures Functionality needed to create a Dungeon Tiles when generating random Maps.
    /// </summary>
    public interface IMapTile
    {
        /// <summary>
        /// initializes Enemy, Loot, and Encounter Interests with attached Monobehaviours that extend <c>BaseTileInterest</c>s attached to a map tile
        /// </summary>
        /// <param name="groupTile"> is the data container attached to every existing dungeon tile, on a logical level.</param>
        public void Init(GroupTile groupTile);
        /// <summary>
        /// Sets up possible enemy encampmants and moving patrols with a probability.
        /// </summary>
        public void SetupEnemies();
        /// <summary>
        /// Sets up possible Loot locations with a probability
        /// </summary>
        public void SetupLoot();
        /// <summary>
        /// Sets up possible game events locations with a probability
        /// </summary>
        public void SetupEncounters();

    }

}