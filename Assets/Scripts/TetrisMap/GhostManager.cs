using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour {

    MapManager MM;
    public bool IsRightGhost(Tetrimino te)
    {
        for (int i = 0; i < te.rooms.Length; i++)
        {
            if (te.rooms[i].mapCoord.y < 0)
                return false;
            else if (MapManager.mapGrid[(int)te.rooms[i].mapCoord.x, (int)te.rooms[i].mapCoord.y] != null)
                return false;
        }
        return true;
    }
    public void GhostDown(Tetrimino ghost, Tetrimino te)
    {
        while (IsRightGhost(ghost))
        {
            MM.MoveTetriminoMapCoord(ghost, new Vector3(0, -1, 0));
        }
        MM.MoveTetriminoMapCoord(ghost, new Vector3(0, 1, 0));
    }

    private void Awake()
    {
        MM = GameObject.Find("MapManager").GetComponent<MapManager>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MM.currentGhost.mapCoord = MM.currentTetrimino.mapCoord;
        for (int i = 0; i < MM.currentGhost.rooms.Length; i++)
        {
            MM.currentGhost.rooms[i].mapCoord = MM.currentTetrimino.rooms[i].mapCoord;
            MM.currentGhost.rooms[i].transform.position = (MM.currentGhost.rooms[i].mapCoord - MM.currentGhost.mapCoord) * MM.tetrisMapSize + MM.currentGhost.transform.position;
        }
        GhostDown(MM.currentGhost, MM.currentTetrimino);
	}
}
