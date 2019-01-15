using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Room class
/// </summary>
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
    public MapManager.SpecialRoomType specialRoomType;
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
    /// Check if room is clear and escapable.
    /// </summary>
    public bool isRoomCleared;

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
        Tilemap outerWallMap = roomInGame.transform.GetChild(4).GetComponent<Tilemap>();
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
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] + 1, 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i], 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
            }
            if (i != rightDoorLocation)
            {
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] + 1, 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i], 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
            }
        }
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
            isRoomCleared = true;
        }

        //Need to make extra works.
    }
}
