using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RitualOfAnkaraz.Tilemap
{
    /// <summary>
    /// <c>ForrestMapTileFactory</c> Present in the ForrestDomain.
    /// Extends <c>MapTileFactory</c>
    /// Splitting this reduces memory load, while keeping loading screens short. (PERFORMANCE)
    /// </summary>
    public class ForrestMapTileFactory : MapTileFactory
    {
        #region Prefabs
        //2x1 Tiles, Tiles are predetermined
        [SerializeField] private readonly List<GameObject> forrest2x1Tiles;
        //1x2 Tiles, Tiles are predetermined
        [SerializeField] private readonly List<GameObject> forrest1x2Tiles;
        //2x2 Tiles, Tiles are predetermined
        [SerializeField] private readonly List<GameObject> forrest2x2Tiles;
        [SerializeField] private readonly List<GameObject> forrest3x3Tiles;
        #endregion
        #region Singleton
        //Singleton Pattern
        private static ForrestMapTileFactory _instance;

        public static ForrestMapTileFactory instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("ForrestMapTileFactory").GetComponent<ForrestMapTileFactory>();
                return _instance;
            }
        }
        #endregion
        public override IMapTile CreateTile(GroupTile groupTile)
        {
            Vector3 spawnOrigin = MapTileFactory.GetSpawnOrigin(groupTile.sizeX, groupTile.sizeY, groupTile.gridAnchorPositionX, groupTile.gridAnchorPositionY);

            if (groupTile.sizeX == 1 && groupTile.sizeY == 1)
            {
                GameObject mapTile = Instantiate(GetSingleTile(groupTile),spawnOrigin,Quaternion.identity);
                ForrestMapTile forrestMapTile = mapTile.GetComponent<ForrestMapTile>();
                forrestMapTile.Init(groupTile);
                //TODO Implement in forrestMapTile , then uncomment theese 3 lines
                //forrestMapTile.SetupEnemies();
                //forrestMapTile.SetupEncounters();
                //forrestMapTile.SetupEncounters();
                return forrestMapTile;
            }
            if (groupTile.sizeX == 2 &&  groupTile.sizeY == 1)
            {
                GameObject mapTile = Instantiate(GetLargeTile(forrest2x1Tiles),spawnOrigin,Quaternion.identity);
                ForrestMapTile forrestMapTile = mapTile.GetComponent<ForrestMapTile>();
                forrestMapTile.Init(groupTile);
                //TODO Implement in forrestMapTile , then uncomment theese 3 lines
                //forrestMapTile.SetupEnemies();
                //forrestMapTile.SetupEncounters();
                //forrestMapTile.SetupEncounters();
                return forrestMapTile;
            }
            if (groupTile.sizeX == 1 && groupTile.sizeY == 2)
            {
                GameObject mapTile = Instantiate(GetLargeTile(forrest1x2Tiles), spawnOrigin, Quaternion.identity);
                ForrestMapTile forrestMapTile = mapTile.GetComponent<ForrestMapTile>();
                forrestMapTile.Init(groupTile);
                //TODO Implement in forrestMapTile , then uncomment theese 3 lines
                //forrestMapTile.SetupEnemies();
                //forrestMapTile.SetupEncounters();
                //forrestMapTile.SetupEncounters();
                return forrestMapTile;
            }
            if (groupTile.sizeX == 2 && groupTile.sizeY == 2)
            {
                GameObject mapTile = Instantiate(GetLargeTile(forrest2x2Tiles), spawnOrigin, Quaternion.identity);
                ForrestMapTile forrestMapTile = mapTile.GetComponent<ForrestMapTile>();
                forrestMapTile.Init(groupTile);
                //TODO Implement in forrestMapTile , then uncomment theese 3 lines
                //forrestMapTile.SetupEnemies();
                //forrestMapTile.SetupEncounters();
                //forrestMapTile.SetupEncounters();
                return forrestMapTile;
            }
            throw new System.NotImplementedException("Tile Size Not Implemented when creating GameObjects for map.");
        }

        public override IMapTile CreateStartTile(GroupTile groupTile)
        {
            Vector3 spawnOrigin = MapTileFactory.GetSpawnOrigin(groupTile.sizeX, groupTile.sizeY, groupTile.gridAnchorPositionX, groupTile.gridAnchorPositionY);
            GameObject mapTile = Instantiate(GetStartTile(groupTile), spawnOrigin, Quaternion.identity);
            ForrestMapTile forrestMapTile = mapTile.GetComponent<ForrestMapTile>();
            forrestMapTile.Init(groupTile);
            //TODO Implement in forrestMapTile , then uncomment theese 3 lines
            //forrestMapTile.SetupEnemies();
            //forrestMapTile.SetupEncounters();
            //forrestMapTile.SetupEncounters();
            //Setup Start Tile manually, player spawn position
            return forrestMapTile;
        }
    }
}
