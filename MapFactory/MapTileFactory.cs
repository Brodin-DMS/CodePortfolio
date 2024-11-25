using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RitualOfAnkaraz.Tilemap
{
    /// <summary>
    /// <c>MapTileFactory</c> is the BaseImplementation Used to extend all other Tile producing Factories that are domain specific.
    /// </summary>
    public abstract class MapTileFactory : MonoBehaviour
    {
        const float TILE_STEP_SIZE = 60f;
        //Holds a refference to prefabs of Map Tiles
        #region Prefabs
        //Single Tiles
        [SerializeField] protected List<GameObject> singleTiles4040E0000;
        [SerializeField] protected List<GameObject> singleTiles4040E0001;
        [SerializeField] protected List<GameObject> singleTiles4040E0010;
        [SerializeField] protected List<GameObject> singleTiles4040E0011;
        [SerializeField] protected List<GameObject> singleTiles4040E0100;
        [SerializeField] protected List<GameObject> singleTiles4040E0101;
        [SerializeField] protected List<GameObject> singleTiles4040E0110;
        [SerializeField] protected List<GameObject> singleTiles4040E0111;
        [SerializeField] protected List<GameObject> singleTiles4040E1000;
        [SerializeField] protected List<GameObject> singleTiles4040E1001;
        [SerializeField] protected List<GameObject> singleTiles4040E1010;
        [SerializeField] protected List<GameObject> singleTiles4040E1011;
        [SerializeField] protected List<GameObject> singleTiles4040E1100;
        [SerializeField] protected List<GameObject> singleTiles4040E1101;
        [SerializeField] protected List<GameObject> singleTiles4040E1110;
        [SerializeField] protected List<GameObject> singleTiles4040E1111;
        //Single Start Tiles
        [SerializeField] protected List<GameObject> singleStartTiles4040E0000;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0001;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0010;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0011;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0100;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0101;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0110;
        [SerializeField] protected List<GameObject> singleStartTiles4040E0111;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1000;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1001;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1010;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1011;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1100;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1101;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1110;
        [SerializeField] protected List<GameObject> singleStartTiles4040E1111;

        //TODO probably have to implement more start tiles with each exit, or dynamicly add a start position.
        #endregion
        /// <summary>
        /// Creates a Tile based on its dataContainer.
        /// </summary>
        /// <param name="groupTile">is the data container attached to every existing Dungeon Tile, on a logical level.</param>
        /// <returns><c>IMapTile</c></returns>
        public abstract IMapTile CreateTile(GroupTile groupTile);
        /// <summary>
        /// Creates a Tile based on its dataContainer. adds specifc Objects, and funktionality to the players spawn positions.
        /// </summary>
        /// <param name="groupTile">is the data container attached to every existing Dungeon Tile, on a logical level.</param>
        /// <returns><c>IMapTile</c></returns>
        public abstract IMapTile CreateStartTile(GroupTile groupTile);
        /// <summary>
        /// Helper funktion to Determin a 60x60 tile with valid tile connectors.
        /// </summary>
        /// <param name="groupTile">is the data container attached to every existing Dungeon Tile, on a logical level.</param>
        /// <returns>prefab of an uninatialised tile</returns>
        protected virtual GameObject GetSingleTile(GroupTile groupTile)
        {
            SingleTile tile = groupTile.singleTiles[0];
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 0) { return singleTiles4040E0000[Random.Range(0, singleTiles4040E0000.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 1) { return singleTiles4040E0001[Random.Range(0, singleTiles4040E0001.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 0) { return singleTiles4040E0010[Random.Range(0, singleTiles4040E0010.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 1) { return singleTiles4040E0011[Random.Range(0, singleTiles4040E0011.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 0) { return singleTiles4040E0100[Random.Range(0, singleTiles4040E0100.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 1) { return singleTiles4040E0101[Random.Range(0, singleTiles4040E0101.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 0) { return singleTiles4040E0110[Random.Range(0, singleTiles4040E0110.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 1) { return singleTiles4040E0111[Random.Range(0, singleTiles4040E0111.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 0) { return singleTiles4040E1000[Random.Range(0, singleTiles4040E1000.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 1) { return singleTiles4040E1001[Random.Range(0, singleTiles4040E1001.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 0) { return singleTiles4040E1010[Random.Range(0, singleTiles4040E1010.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 1) { return singleTiles4040E1011[Random.Range(0, singleTiles4040E1011.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 0) { return singleTiles4040E1100[Random.Range(0, singleTiles4040E1100.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 1) { return singleTiles4040E1101[Random.Range(0, singleTiles4040E1101.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 0) { return singleTiles4040E1110[Random.Range(0, singleTiles4040E1110.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 1) { return singleTiles4040E1111[Random.Range(0, singleTiles4040E1111.Count)]; }
            throw new System.NotImplementedException("Tile not implemented");
        }
        /// <summary>
        /// Helper funktion to Determin a 60x60 tile with valid tile connectors, and player spawn positions.
        /// </summary>
        /// <param name="groupTile"></param>
        /// <returns>prefab of an uninatialised tile, including player spawn interaction</returns>
        protected virtual GameObject GetStartTile(GroupTile groupTile)
        {
            SingleTile tile = groupTile.singleTiles[0];
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 0) { return singleStartTiles4040E0000[Random.Range(0, singleStartTiles4040E0000.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 1) { return singleStartTiles4040E0001[Random.Range(0, singleStartTiles4040E0001.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 0) { return singleStartTiles4040E0010[Random.Range(0, singleStartTiles4040E0010.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 1) { return singleStartTiles4040E0011[Random.Range(0, singleStartTiles4040E0011.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 0) { return singleStartTiles4040E0100[Random.Range(0, singleStartTiles4040E0100.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 1) { return singleStartTiles4040E0101[Random.Range(0, singleStartTiles4040E0101.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 0) { return singleStartTiles4040E0110[Random.Range(0, singleStartTiles4040E0110.Count)]; }
            if (tile.northExit == 0 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 1) { return singleStartTiles4040E0111[Random.Range(0, singleStartTiles4040E0111.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 0) { return singleStartTiles4040E1000[Random.Range(0, singleStartTiles4040E1000.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 0 && tile.westExit == 1) { return singleStartTiles4040E1001[Random.Range(0, singleStartTiles4040E1001.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 0) { return singleStartTiles4040E1010[Random.Range(0, singleStartTiles4040E1010.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 0 && tile.southExit == 1 && tile.westExit == 1) { return singleStartTiles4040E1011[Random.Range(0, singleStartTiles4040E1011.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 0) { return singleStartTiles4040E1100[Random.Range(0, singleStartTiles4040E1100.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 0 && tile.westExit == 1) { return singleStartTiles4040E1101[Random.Range(0, singleStartTiles4040E1101.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 0) { return singleStartTiles4040E1110[Random.Range(0, singleStartTiles4040E1110.Count)]; }
            if (tile.northExit == 1 && tile.eastExit == 1 && tile.southExit == 1 && tile.westExit == 1) { return singleStartTiles4040E1111[Random.Range(0, singleStartTiles4040E1111.Count)]; }
            throw new System.NotImplementedException("Tile not implemented");
        }
        //FIXME, list order can't be asured, use hashmap instead
        //NOTE unities editor might asure order cause of its underlying serialised structure?
        /// <summary>
        /// Helper funktion to determin tiles bigger than 60x60. Theese tiles don't have Mutable Exits.
        /// </summary>
        /// <param name="prefabs">predetermined large Map Tile</param>
        /// <returns></returns>
        public static GameObject GetLargeTile(List<GameObject> prefabs)
        {
            return prefabs[Random.Range(0,prefabs.Count)];
        }
        //TODO Refactor static Solutions to MapUtils
        public static Vector3 GetSpawnOrigin(int sizeX, int sizeY, int gridPositionX, int gridPositionY)
        {
            float xCord = gridPositionX * TILE_STEP_SIZE - (sizeX / 2f);
            float zCord = gridPositionY * TILE_STEP_SIZE - (sizeY / 2f);
            Vector3 spawnOrigin = new Vector3(xCord, 0f, zCord);
            return spawnOrigin;
        }
    }
}
