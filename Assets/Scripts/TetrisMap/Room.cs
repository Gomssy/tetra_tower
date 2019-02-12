using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    /*
     *  variables
     */
    /// <summary>
    /// Room's Location on tetris map.
    /// Not related to real location.
    /// </summary>
    public Vector3 mapCoord;
    /// <summary>
    /// Stage per rooms.
    /// </summary>
    public int stage;
    /// <summary>
    /// Room concept per rooms.
    /// Range from 0 to 3.
    /// </summary>
    public int roomConcept;
    /// <summary>
    /// Item room type per rooms.
    /// 0 for normal room, 1~ for item rooms.
    /// </summary>
    public int itemRoomType;
    /// <summary>
    /// Special room types.
    /// </summary>
    public RoomType specialRoomType;
    /// <summary>
    /// Location of the left door.
    /// </summary>
    public int leftDoorLocation;
    /// <summary>
    /// Location of the right door.
    /// </summary>
    public int rightDoorLocation;
    /// <summary>
    /// Height of side doors.
    /// </summary>
    public int[] doorLocations = { 1, 9, 17 };
    /// <summary>
    /// Fog of the room.
    /// </summary>
    public GameObject fog;
    /// <summary>
    /// Left door of the room.
    /// </summary>
    public GameObject leftTetrisDoor;
    /// <summary>
    /// Right door of the room.
    /// </summary>
    public GameObject rightTetrisDoor;
    /// <summary>
    /// Up door of the room.
    /// </summary>
    public GameObject inGameDoorUp;
    /// <summary>
    /// Down door of the room.
    /// </summary>
    public GameObject inGameDoorDown;
    /// <summary>
    /// Left door of the room.
    /// </summary>
    public GameObject inGameDoorLeft;
    /// <summary>
    /// Right door of the room.
    /// </summary>
    public GameObject inGameDoorRight;
    /// <summary>
    /// Room in InGame of the room.
    /// </summary>
    public RoomInGame roomInGame;
    /// <summary>
    /// Portal of the room.
    /// </summary>
    public GameObject portal;
    /// <summary>
    /// Portal surface of the room.
    /// </summary>
    public GameObject portalSurface;
    /// <summary>
    /// Check if room is clear and escapable.
    /// </summary>
    public bool isRoomCleared;
    /// <summary>
    /// Check if room is destroyed.
    /// </summary>
    public bool isRoomDestroyed = false;
    /// <summary>
    /// Check if portal is active or not.
    /// </summary>
    public bool isPortal = false;

    /*
     *  functions
     */
    /// <summary>
    /// Select which doors would be opened.
    /// </summary>
    public void SetDoors()
    {
        if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null)
            rightDoorLocation = MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].leftDoorLocation;
        else
            rightDoorLocation = Random.Range(0, 3);
        if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null)
            leftDoorLocation = MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].rightDoorLocation;
        else
            leftDoorLocation = Random.Range(0, 3);
    }
    /// <summary>
    /// Create doors.
    /// </summary>
    public void CreateDoors(GameObject _leftTetrisDoor, GameObject _rightTetrisDoor, GameObject _inGameDoorUp, GameObject _inGameDoorDown, GameObject _inGameDoorLeft, GameObject _inGameDoorRight)
    {
        float standardSize = MapManager.tetrisMapSize / 24;
        Tilemap wallTileMap = roomInGame.transform.Find("wall").GetComponent<Tilemap>();
        leftTetrisDoor = Instantiate(_leftTetrisDoor, transform.position + new Vector3(standardSize, doorLocations[leftDoorLocation], 0), Quaternion.identity, transform);
        rightTetrisDoor = Instantiate(_rightTetrisDoor, transform.position + new Vector3(standardSize * 23, doorLocations[rightDoorLocation], 0), Quaternion.identity, transform);

        inGameDoorUp = Instantiate(_inGameDoorUp, transform.position + new Vector3(standardSize * 11, standardSize * 23, 2), Quaternion.identity, transform);
        inGameDoorDown = Instantiate(_inGameDoorDown, transform.position + new Vector3(standardSize * 11, 0, 2), Quaternion.identity, transform);
        inGameDoorLeft = Instantiate(_inGameDoorLeft, transform.position + new Vector3(0, doorLocations[leftDoorLocation], 2), Quaternion.identity, transform);
        inGameDoorRight = Instantiate(_inGameDoorRight, transform.position + new Vector3(standardSize * 23f, doorLocations[rightDoorLocation], 2), Quaternion.identity, transform);
        for (int i = 0; i < 3; i++)
        {
            if (i != leftDoorLocation)
            {
                wallTileMap.SetTile(new Vector3Int(0, doorLocations[i] + 1, 0), wallTileMap.GetTile(new Vector3Int(0, 0, 0)));
                wallTileMap.SetTile(new Vector3Int(0, doorLocations[i], 0), wallTileMap.GetTile(new Vector3Int(0, 0, 0)));
            }
            if (i != rightDoorLocation)
            {
                wallTileMap.SetTile(new Vector3Int(23, doorLocations[i] + 1, 0), wallTileMap.GetTile(new Vector3Int(0, 0, 0)));
                wallTileMap.SetTile(new Vector3Int(23, doorLocations[i], 0), wallTileMap.GetTile(new Vector3Int(0, 0, 0)));
            }
        }
    }
    /// <summary>
    /// Create portal in cleared room.
    /// </summary>
    public void CreatePortal()
    {
        MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        portal = roomInGame.transform.Find("portalspot").gameObject;
        if (specialRoomType != RoomType.Normal)
            isPortal = true;
        else
        {
            int portalDistance = 0;
            for(int i = 1; i <= 3; i++)
            {
                for(int j = 0; j < i; j++)
                {
                    if(mapCoord.x + j < MapManager.width && mapCoord.y + (i - j) <= MapManager.realHeight &&
                        MapManager.mapGrid[(int)mapCoord.x + j, (int)mapCoord.y + (i - j)] != null &&
                        MapManager.mapGrid[(int)mapCoord.x + j, (int)mapCoord.y + (i - j)].isPortal == true)
                    {
                        portalDistance = i;
                        break;
                    }
                    if (mapCoord.x + (i - j) < MapManager.width && mapCoord.y - j >= 0 &&
                        MapManager.mapGrid[(int)mapCoord.x + (i - j), (int)mapCoord.y - j] != null &&
                        MapManager.mapGrid[(int)mapCoord.x + (i - j), (int)mapCoord.y - j].isPortal == true)
                    {
                        portalDistance = i;
                        break;
                    }
                    if (mapCoord.x - j >= 0 && mapCoord.y - (i - j) >= 0 &&
                        MapManager.mapGrid[(int)mapCoord.x - j, (int)mapCoord.y - (i - j)] != null &&
                        MapManager.mapGrid[(int)mapCoord.x - j, (int)mapCoord.y - (i - j)].isPortal == true)
                    {
                        portalDistance = i;
                        break;
                    }
                    if (mapCoord.x - (i - j) >= 0 && mapCoord.y + j <= MapManager.realHeight &&
                        MapManager.mapGrid[(int)mapCoord.x - (i - j), (int)mapCoord.y + j] != null &&
                        MapManager.mapGrid[(int)mapCoord.x - (i - j), (int)mapCoord.y + j].isPortal == true)
                    {
                        portalDistance = i;
                        break;
                    }
                }
                if (portalDistance != 0)
                    break;
            }
            switch (portalDistance)
            {
                case 1:
                    break;
                case 2:
                    if (Random.Range(0, 10) % 10 == 0)
                    {
                        isPortal = true;
                    }
                    break;
                case 3:
                    if (Random.Range(0, 4) % 10 == 0)
                    {
                        isPortal = true;
                    }
                    break;
                case 4:
                    if (Random.Range(0, 2) % 10 == 0)
                    {
                        isPortal = true;
                    }
                    break;
            }
        }
        if(isPortal)
            portal = Instantiate(mapManager.portal, portal.transform.position, Quaternion.identity, roomInGame.transform);
    }
    /// <summary>
    /// Open selected door of this room.
    /// </summary>
    /// <param name="direction">Direction of door to open.</param>
    public void OpenDoor(string direction)
    {
        Animator animatorThisRoom = null;
        Animator animatorNextRoom = null;
        GameObject door = null;
        switch (direction)
        {
            case "Up":
                if (mapCoord.y < MapManager.realHeight && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1] != null && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].isRoomDestroyed != true)
                {
                    door = inGameDoorUp;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown.GetComponent<Animator>();
                }
                break;
            case "Down":
                if (mapCoord.y > 0 && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1] != null && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].isRoomDestroyed != true)
                {
                    door = inGameDoorDown;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp.GetComponent<Animator>();
                }
                break;
            case "Left":
                if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null)
                {
                    door = inGameDoorLeft;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight.GetComponent<Animator>();
                }
                break;
            case "Right":
                if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null)
                {
                    door = inGameDoorRight;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft.GetComponent<Animator>();
                }
                break;
        }
        if (animatorThisRoom != null)
        {
            animatorThisRoom.SetBool("doorOpen", true);
            animatorThisRoom.SetBool("doorClose", false);
        }
        if (animatorNextRoom != null)
        {
            animatorNextRoom.SetBool("doorOpen", true);
            animatorNextRoom.SetBool("doorClose", false);
        }
    }
    /// <summary>
    /// Close selected door of this room.
    /// </summary>
    /// <param name="direction">Direction of door to open.</param>
    public void CloseDoor(string direction, bool isPlayerPass)
    {
        Animator animatorThisRoom = null;
        Animator animatorNextRoom = null;
        GameObject door = null;
        switch (direction)
        {
            case "Up":
                if (mapCoord.y < MapManager.realHeight && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1] != null)
                {
                    door = inGameDoorUp;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown.GetComponent<Animator>();
                }
                break;
            case "Down":
                if (mapCoord.y > 0 && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1] != null)
                {
                    door = inGameDoorDown;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp.GetComponent<Animator>();
                }
                break;
            case "Left":
                if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null)
                {
                    door = inGameDoorLeft;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight.GetComponent<Animator>();
                }
                break;
            case "Right":
                if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null)
                {
                    door = inGameDoorRight;
                    animatorThisRoom = door.GetComponent<Animator>();
                    animatorNextRoom = MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft.GetComponent<Animator>();
                }
                break;
        }
        if(isPlayerPass == true)
        {
            if(door != null)
            {
                switch (direction)
                {
                    case "Up":
                        door.GetComponent<Door>().enteredPosition = 0;
                        break;
                    case "Down":
                        door.GetComponent<Door>().enteredPosition = 2;
                        break;
                    case "Left":
                        door.GetComponent<Door>().enteredPosition = 3;
                        break;
                    case "Right":
                        door.GetComponent<Door>().enteredPosition = 1;
                        break;
                }
                door.GetComponent<Door>().animatorThisRoom = animatorThisRoom;
                door.GetComponent<Door>().animatorNextRoom = animatorNextRoom;
            }
        }
        else
        {
            if (animatorThisRoom != null)
            {
                animatorThisRoom.SetBool("isPlayerTouchEnded", true);
                animatorThisRoom.SetBool("doorOpen", false);
                animatorThisRoom.SetBool("doorClose", true);
            }
            if (animatorNextRoom != null)
            {
                animatorNextRoom.SetBool("isPlayerTouchEnded", true);
                animatorNextRoom.SetBool("doorOpen", false);
                animatorNextRoom.SetBool("doorClose", true);
            }
        }
    }
    /// <summary>
    /// Clear the cleared room.
    /// Open all the doors and change fog to cleared fog.
    /// </summary>
    public void ClearRoom()
    {
        if(isRoomCleared != true)
        {
            OpenDoor("Up");
            OpenDoor("Down");
            OpenDoor("Left");
            OpenDoor("Right");
            Vector3 fogPosition = fog.transform.position;
            Destroy(fog);
            fog = Instantiate(GameObject.Find("MapManager").GetComponent<MapManager>().clearedFog, fogPosition, Quaternion.identity, transform);
            fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            if(specialRoomType != RoomType.Start)
                CreatePortal();
            if (isPortal == true)
            {
                for (int x = 0; x < MapManager.width; x++)
                    MapManager.portalDistributedHorizontal[x].Clear();
                for (int y = 0; y <= MapManager.realHeight; y++)
                    MapManager.portalDistributedVertical[y].Clear();
                for (int x = 0; x < MapManager.width; x++)
                    for (int y = 0; y <= MapManager.realHeight; y++)
                        if (MapManager.mapGrid[x, y] != null && MapManager.mapGrid[x, y].isPortal == true)
                        {
                            MapManager.portalGrid[x, y] = true;
                            MapManager.portalDistributedHorizontal[x].Add(y);
                            MapManager.portalDistributedVertical[y].Add(x);
                        }
                portalSurface = Instantiate(GameObject.Find("MapManager").GetComponent<MapManager>().portalSurface, transform.position + new Vector3(12, 12, 0), Quaternion.identity, transform);
            }
            isRoomCleared = true;
            if (specialRoomType == RoomType.Boss)
                MapManager.currentStage += 1;
        }
        //Need to make extra works.
    }
}
