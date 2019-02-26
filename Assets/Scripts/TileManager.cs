using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {
    /// <summary>
    /// Array of all wall tiles.
    /// </summary>
    public TileBase[] allWallTiles;
    /// <summary>
    /// Array of all platform tiles.
    /// </summary>
    public TileBase[] allPlatformTiles;
    /// <summary>
    /// Array of all rope tiles.
    /// </summary>
    public TileBase[] allRopeTiles;
    /// <summary>
    /// Array of all spike tiles.
    /// </summary>
    public TileBase[] allSpikeTiles;
    /// <summary>
    /// Dictionary for distributing all wall tiles.
    /// Each dimensions for stage and concept.
    /// </summary>
    Dictionary<string, TileBase>[,] wallTilesDistributed = new Dictionary<string, TileBase>[5, 4];
    /// <summary>
    /// Dictionary for distributing all platform tiles.
    /// Each dimensions for stage and concept.
    /// </summary>
    Dictionary<string, TileBase>[,] platformTilesDistributed = new Dictionary<string, TileBase>[5, 4];
    /// <summary>
    /// Dictionary for distributing all spike tiles.
    /// Each dimensions for stage and concept.
    /// </summary>
    Dictionary<string, TileBase>[,] spikeTilesDistributed = new Dictionary<string, TileBase>[5, 4];
    /// <summary>
    /// Dictionary for distributing all rope tiles.
    /// Each dimensions for stage and concept.
    /// </summary>
    Dictionary<string, TileBase>[,] ropeTilesDistributed = new Dictionary<string, TileBase>[5, 4];

    /// <summary>
    /// Check room's tilemap's all position that if tile exists there or not.
    /// </summary>
    /// <param name="roomInGame">Room you want to check.</param>
    public void CheckAllTiles(RoomInGame roomInGame)
    {
        Tilemap wallTileMap = roomInGame.transform.Find("wall").GetComponent<Tilemap>();
        Tilemap platformTileMap = roomInGame.transform.Find("platform").GetComponent<Tilemap>();
        Tilemap ropeTileMap = roomInGame.transform.Find("rope").GetComponent<Tilemap>();
        Tilemap spikeTileMap = roomInGame.transform.Find("spike").GetComponent<Tilemap>();
        for (int x = 0; x < 24; x++)
            for(int y = 0; y < 24; y++)
            {
                if (wallTileMap.GetTile(new Vector3Int(x, y, 0)))
                    roomInGame.wallTileInfo[x, y] = true;
                if (platformTileMap.GetTile(new Vector3Int(x, y, 0)))
                    roomInGame.platformTileInfo[x, y] = true;
                if (ropeTileMap.GetTile(new Vector3Int(x, y, 0)))
                    roomInGame.ropeTileInfo[x, y] = true;
                if (spikeTileMap.GetTile(new Vector3Int(x, y, 0)))
                    roomInGame.spikeTileInfo[x, y] = true;
            }
    }
    /// <summary>
    /// Check tile and set name for specified direction.
    /// </summary>
    /// <param name="roomInGame">Room you want to check.</param>
    /// <param name="originPos">Position of tile map you want to check.</param>
    /// <param name="checkPos">Direction you want to check.</param>
    /// <returns></returns>
    public char CheckWallQuarterTile(RoomInGame roomInGame, Vector2Int originPos, Vector2Int checkPos)
    {
        int verticalTile = 0, horizontalTile = 0;
        if (!IsTileInRoom(originPos.x + checkPos.x))
            horizontalTile = 2;
        else if (roomInGame.wallTileInfo[originPos.x + checkPos.x, originPos.y])
            horizontalTile = 1;
        if (!IsTileInRoom(originPos.y + checkPos.y))
            verticalTile = 2;
        else if (roomInGame.wallTileInfo[originPos.x, originPos.y + checkPos.y])
            verticalTile = 1;
        if ((verticalTile == 2 && horizontalTile == 2) || (verticalTile == 0 && horizontalTile == 2) || (verticalTile == 2 && horizontalTile == 0))
            return '3';
        else if (verticalTile == 2)
            return '1';
        else if (horizontalTile == 2)
            return '2';
        else if (verticalTile == 1 && horizontalTile == 1)
        {
            if (roomInGame.wallTileInfo[originPos.x + checkPos.x, originPos.y + checkPos.y])
                return 'o';
            else
                return 's';
        }
        else if (verticalTile == 1)
            return 'v';
        else if (horizontalTile == 1)
            return 'h';
        else
            return 'b';
    }
    public char CheckPlatformTile(RoomInGame roomInGame, Vector2Int originPos)
    {
        bool left = false, right = false;
        if(roomInGame.platformTileInfo[originPos.x + 1, originPos.y] || roomInGame.wallTileInfo[originPos.x + 1, originPos.y])
            right = true;
        if (roomInGame.platformTileInfo[originPos.x - 1, originPos.y] || roomInGame.wallTileInfo[originPos.x - 1, originPos.y])
            left = true;
        if (left && right)
            return 'c';
        else if (left)
            return 'r';
        else if(right)
            return 'l';
        else
            return 'o';
    }
    public char CheckRopeTile(RoomInGame roomInGame, Vector2Int originPos)
    {
        bool up = false, down = false;
        if (roomInGame.ropeTileInfo[originPos.x, originPos.y + 1])
            up = true;
        if (roomInGame.ropeTileInfo[originPos.x, originPos.y - 1])
            down = true;
        if (up && down)
            return 'c';
        else if (up)
            return 'd';
        else if (down)
            return 'u';
        else
            return 'o';
    }
    public char CheckSpikeTile(RoomInGame roomInGame, Vector2Int originPos)
    {
        Tilemap spikeTileMap = roomInGame.transform.Find("spike").GetComponent<Tilemap>();
        if (spikeTileMap.GetTile(new Vector3Int(originPos.x, originPos.y, 0)).name.Equals("spikeu"))
            return 'u';
        else if (spikeTileMap.GetTile(new Vector3Int(originPos.x, originPos.y, 0)).name.Equals("spiked"))
            return 'd';
        else if (spikeTileMap.GetTile(new Vector3Int(originPos.x, originPos.y, 0)).name.Equals("spikel"))
            return 'l';
        else
            return 'r';
    }
    /// <summary>
    /// Check if it is out of the room or not.
    /// </summary>
    /// <param name="n">Position you want to check.</param>
    /// <returns></returns>
    bool IsTileInRoom(int n)
    {
        return n >= 0 && n < 24;
    }
    /// <summary>
    /// Set all tiles of the room.
    /// Cut for every 72 tiles.
    /// </summary>
    /// <param name="roomInGame">Room you want to set.</param>
    /// <returns></returns>
    public IEnumerator SetAllTiles(Room room)
    {
        int stage = MapManager.currentStage;
        int concept = room.roomConcept;
        if (concept == 3)
            concept = 0;
        RoomInGame roomInGame = room.roomInGame;
        Tilemap wallTileMap = roomInGame.transform.Find("wall").GetComponent<Tilemap>();
        Tilemap platformTileMap = roomInGame.transform.Find("platform").GetComponent<Tilemap>();
        Tilemap ropeTileMap = roomInGame.transform.Find("rope").GetComponent<Tilemap>();
        Tilemap spikeTileMap = roomInGame.transform.Find("spike").GetComponent<Tilemap>();
        CheckAllTiles(roomInGame);
        for(int y = 0; y < 24; y++)
        {
            for(int x = 0; x < 24; x++)
            {
                if (roomInGame.wallTileInfo[x, y])
                {
                    string tileName = CheckWallQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(-1, 1)).ToString() +
                    CheckWallQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(1, 1)).ToString() +
                    CheckWallQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(-1, -1)).ToString() +
                    CheckWallQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(1, -1)).ToString();
                    if (wallTilesDistributed[stage, concept].ContainsKey(tileName))
                        wallTileMap.SetTile(new Vector3Int(x, y, 0), wallTilesDistributed[stage, concept][tileName]);
                    else
                        Debug.Log(stage + concept + tileName + " tile is missing!");
                }
                if (roomInGame.platformTileInfo[x, y] && y != 0 && y != 23)
                {
                    string tileName = CheckPlatformTile(roomInGame, new Vector2Int(x, y)).ToString();
                    if (platformTilesDistributed[stage, concept].ContainsKey(tileName))
                        platformTileMap.SetTile(new Vector3Int(x, y, 0), platformTilesDistributed[stage, concept][tileName]);
                    else
                        Debug.Log(stage + concept + tileName + " tile is missing!");
                }
                if (roomInGame.ropeTileInfo[x, y])
                {
                    string tileName = CheckRopeTile(roomInGame, new Vector2Int(x, y)).ToString();
                    if (ropeTilesDistributed[stage, concept].ContainsKey(tileName))
                        ropeTileMap.SetTile(new Vector3Int(x, y, 0), ropeTilesDistributed[stage, concept][tileName]);
                    else
                        Debug.Log(stage + concept + tileName + " tile is missing!");
                }
                if (roomInGame.spikeTileInfo[x, y])
                {
                    string tileName = CheckSpikeTile(roomInGame, new Vector2Int(x, y)).ToString();
                    if (spikeTilesDistributed[stage, concept].ContainsKey(tileName))
                        spikeTileMap.SetTile(new Vector3Int(x, y, 0), spikeTilesDistributed[stage, concept][tileName]);
                    else
                        Debug.Log(stage + concept + tileName + " tile is missing!");
                }
            }
        }
        yield return null;
    }
    /// <summary>
    /// Set tiles for all rooms in tetrimino.
    /// </summary>
    /// <param name="tetrimino">Tetrimino you want to set.</param>
    /// <returns></returns>
    public IEnumerator SetTetriminoTiles(Tetrimino tetrimino)
    {
        for(int i = 0; i < tetrimino.rooms.Length; i++)
        {
            yield return null;
            StartCoroutine(SetAllTiles(tetrimino.rooms[i]));
        }
    }

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 4; j++)
            {
                wallTilesDistributed[i, j] = new Dictionary<string, TileBase>();
                platformTilesDistributed[i, j] = new Dictionary<string, TileBase>();
                ropeTilesDistributed[i, j] = new Dictionary<string, TileBase>();
                spikeTilesDistributed[i, j] = new Dictionary<string, TileBase>();
            }
        string tileName;
        for (int i = 0; i < allWallTiles.Length; i++)
        {
            tileName = allWallTiles[i].name;
            wallTilesDistributed[int.Parse(tileName.Substring(0, 1)) - 1, int.Parse(tileName.Substring(1, 1)) - 1].Add(tileName.Substring(2), allWallTiles[i]);
        }
        for (int i = 0; i < allPlatformTiles.Length; i++)
        {
            tileName = allPlatformTiles[i].name;
            platformTilesDistributed[int.Parse(tileName.Substring(0, 1)) - 1, int.Parse(tileName.Substring(1, 1)) - 1].Add(tileName.Substring(10), allPlatformTiles[i]);
        }
        for (int i = 0; i < allRopeTiles.Length; i++)
        {
            tileName = allRopeTiles[i].name;
            ropeTilesDistributed[int.Parse(tileName.Substring(0, 1)) - 1, int.Parse(tileName.Substring(1, 1)) - 1].Add(tileName.Substring(6), allRopeTiles[i]);
        }
        for (int i = 0; i < allSpikeTiles.Length; i++)
        {
            tileName = allSpikeTiles[i].name;
            spikeTilesDistributed[int.Parse(tileName.Substring(0, 1)) - 1, int.Parse(tileName.Substring(1, 1)) - 1].Add(tileName.Substring(7), allSpikeTiles[i]);
        }
    }
}
