using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap map;
    public RoomInGame roomInGame;
    public TileBase[] fillLeftDoor;
    public TileBase[] fillRightDoor;
    public TileBase[] tile11;
    public TileBase[] tile12;
    public TileBase[] tile13;
    public TileBase[] tile14;
    public TileBase[] tile21;
    public TileBase[] tile22;
    public TileBase[] tile23;
    public TileBase[] tile24;
    public TileBase[] tile31;
    public TileBase[] tile32;
    public TileBase[] tile33;
    public TileBase[] tile34;
    public TileBase[] tile41;
    public TileBase[] tile42;
    public TileBase[] tile43;
    public TileBase[] tile44;
    public TileBase[] tile51;
    public TileBase[] tile52;
    public TileBase[] tile53;
    public TileBase[] tile54;
    public TileBase[] allTiles;

    Dictionary<string, TileBase>[,] tilesDistributed = new Dictionary<string, TileBase>[5, 4];

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 4; j++)
                tiles[i, j] = new Dictionary<string, TileBase>();
        string tileName;
        for(int i = 0; i < allTiles.Length; i++)
        {
            tileName = allTiles[i].name;
            tiles[int.Parse(tileName[0].ToString()) - 1, int.Parse(tileName[1].ToString()) - 1].Add(tileName.Substring(2), allTiles[i]);
        }
    }

    public void CheckAllTiles(Room room)
    {
        Tilemap roomTileMap = room.roomInGame.transform.GetChild(3).GetComponent<Tilemap>();
        for(int x = 0; x < 24; x++)
            for(int y = 0; y < 24; y++)
            {
                if (roomTileMap.GetTile(new Vector3Int(x, y, 0)))
                    room.tileInfo[x, y] = true;
                else
                    room.tileInfo[x, y] = false;
            }
    }

    public char CheckQuarterTile(Room room, Vector2Int originPos, Vector2Int checkPos)
    {
        int verticalTile = 0, horizontalTile = 0;
        bool[,] tileInfo = room.tileInfo;
        if ((originPos.x == 0 && (originPos.y + checkPos.y == room.doorLocations[room.leftDoorLocation]
            || originPos.y + checkPos.y == room.doorLocations[room.leftDoorLocation] + 1))
            || (originPos.x == 23 && (originPos.y + checkPos.y == room.doorLocations[room.rightDoorLocation]
            || originPos.y + checkPos.y == room.doorLocations[room.rightDoorLocation] + 1)))
            verticalTile = 3;
        else if (!IsTileInRoom(originPos.x + checkPos.x))
            horizontalTile = 2;
        else if (tileInfo[originPos.x + checkPos.x, originPos.y])
            horizontalTile = 1;
        if ((originPos.y == 0 && (originPos.x == 11 || originPos.x == 12)) || (originPos.y == 23 && (originPos.x == 11 || originPos.x == 12)))
            horizontalTile = 3;
        else if (!IsTileInRoom(originPos.y + checkPos.y))
            verticalTile = 2;
        else if (tileInfo[originPos.x, originPos.y + checkPos.y])
            verticalTile = 1;
        if((verticalTile == 2 && horizontalTile == 2) || (verticalTile == 3 && horizontalTile == 2) || (verticalTile == 2 && horizontalTile == 3))
            return 'B';
        else if (verticalTile == 2)
            return 'H';
        else if (horizontalTile == 2)
            return 'V';
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

    bool IsTileInRoom(int n)
    {
        return n >= 0 && n < 24;
    }
    
    public void ChangeTile(Room room)
    {
        int stage = MapManager.currentStage;
        int concept = room.roomConcept;
        Tilemap roomTileMap = room.roomInGame.transform.GetChild(3).GetComponent<Tilemap>();
        CheckAllTiles(room);
        for(int x = 0; x < 24; x++)
            for(int y = 0; y < 24; y++)
            {
                string tileName = CheckQuarterTile(room, new Vector2Int(x, y), new Vector2Int(-1, 1)).ToString() +
                CheckQuarterTile(room, new Vector2Int(x, y), new Vector2Int(1, 1)).ToString() +
                CheckQuarterTile(room, new Vector2Int(x, y), new Vector2Int(-1, -1)).ToString() +
                CheckQuarterTile(room, new Vector2Int(x, y), new Vector2Int(1, -1)).ToString();
                roomTileMap.SetTile(new Vector3Int(x, y, 0), tilesDistributed[stage, concept][tileName]);
            }
    }
}


