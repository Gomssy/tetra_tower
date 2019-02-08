using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    /// <summary>
    /// Array of all tiles.
    /// </summary>
    public TileBase[] allTiles;
    /// <summary>
    /// Dictionary for distributing all tiles.
    /// Each dimensions for stage and concept.
    /// </summary>
    Dictionary<string, TileBase>[,] tilesDistributed = new Dictionary<string, TileBase>[5, 4];

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 4; j++)
                tilesDistributed[i, j] = new Dictionary<string, TileBase>();
        string tileName;
        for(int i = 0; i < allTiles.Length; i++)
        {
            tileName = allTiles[i].name;
            tilesDistributed[int.Parse(tileName.Substring(0, 1)) - 1, int.Parse(tileName.Substring(1, 1)) - 1].Add(tileName.Substring(2), allTiles[i]);
        }
    }
    /// <summary>
    /// Check room's tilemap's all position that if tile exists there or not.
    /// </summary>
    /// <param name="roomInGame">Room you want to check.</param>
    public void CheckAllTiles(RoomInGame roomInGame)
    {
        Tilemap roomTileMap = roomInGame.transform.Find("wall").GetComponent<Tilemap>();
        for (int x = 0; x < 24; x++)
            for(int y = 0; y < 24; y++)
            {
                if (roomTileMap.GetTile(new Vector3Int(x, y, 0)))
                    roomInGame.tileInfo[x, y] = true;
                else
                    roomInGame.tileInfo[x, y] = false;
            }
    }
    /// <summary>
    /// Check tile and set name for specified direction.
    /// </summary>
    /// <param name="roomInGame">Room you want to check.</param>
    /// <param name="originPos">Position of tile map you want to check.</param>
    /// <param name="checkPos">Direction you want to check.</param>
    /// <returns></returns>
    public char CheckQuarterTile(RoomInGame roomInGame, Vector2Int originPos, Vector2Int checkPos)
    {
        int verticalTile = 0, horizontalTile = 0;
        bool[,] tileInfo = roomInGame.tileInfo;
        if (!IsTileInRoom(originPos.x + checkPos.x))
            horizontalTile = 2;
        else if (tileInfo[originPos.x + checkPos.x, originPos.y])
            horizontalTile = 1;
        if (!IsTileInRoom(originPos.y + checkPos.y))
            verticalTile = 2;
        else if (tileInfo[originPos.x, originPos.y + checkPos.y])
            verticalTile = 1;
        if ((verticalTile == 2 && horizontalTile == 2) || (verticalTile == 0 && horizontalTile == 2) || (verticalTile == 2 && horizontalTile == 0))
            return '3';
        else if (verticalTile == 2)
            return '1';
        else if (horizontalTile == 2)
            return '2';
        else if (verticalTile == 1 && horizontalTile == 1)
        {
            if (tileInfo[originPos.x + checkPos.x, originPos.y + checkPos.y])
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
        //int concept = room.roomConcept;
        int concept = room.roomConcept % 2;
        RoomInGame roomInGame = room.roomInGame;
        Tilemap roomTileMap = roomInGame.transform.Find("wall").GetComponent<Tilemap>();
        CheckAllTiles(roomInGame);
        for(int y = 0; y < 24; y++)
        {
            /*if (y % 3 == 0)
                yield return null;*/
            for(int x = 0; x < 24; x++)
            {
                if (roomInGame.tileInfo[x, y])
                {
                    string tileName = CheckQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(-1, 1)).ToString() +
                    CheckQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(1, 1)).ToString() +
                    CheckQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(-1, -1)).ToString() +
                    CheckQuarterTile(roomInGame, new Vector2Int(x, y), new Vector2Int(1, -1)).ToString();
                    //Debug.Log(tileName);
                    if(tilesDistributed[stage, concept].ContainsKey(tileName))
                        roomTileMap.SetTile(new Vector3Int(x, y, 0), tilesDistributed[stage, concept][tileName]);
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
}


