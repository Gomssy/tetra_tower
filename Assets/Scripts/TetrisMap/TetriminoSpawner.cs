using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoSpawner : MonoBehaviour {

    /*
     * variables
     * */
    MapManager mapManager;
    /// <summary>
    /// All tetriminoes.
    /// </summary>
    public Tetrimino[] tetriminoes;
    /// <summary>
    /// All ghosts.
    /// </summary>
    public Tetrimino[] ghosts;
    /// <summary>
    /// Save probability of which tetrimino will be made next.
    /// </summary>
    int[] randomTetrimino = { 1, 1, 1, 1, 1, 1, 1 };

    /*
     * functions
     * */
    /// <summary>
    /// Make new Tetrimino at top.
    /// </summary>
    public void MakeTetrimino()
    {
        if (!mapManager.gameOver)
        {
            int randomPosition = Random.Range(0, MapManager.width);
            int randomTetrimino;
            if (mapManager.spawnBossTetrimino)
            {
                randomTetrimino = 7;
                mapManager.spawnBossTetrimino = false;
            }
            else
                randomTetrimino = TetriminoRandomizer();
            mapManager.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], 
                mapManager.tetrisMapCoord + mapManager.tetrisMapSize * new Vector3(randomPosition, MapManager.realHeight + 1, mapManager.tetrisMapCoord.z), Quaternion.identity);
            mapManager.currentTetrimino.mapCoord = (mapManager.currentTetrimino.transform.position - mapManager.tetrisMapCoord) / mapManager.tetrisMapSize;
            mapManager.SetRoomMapCoord(mapManager.currentTetrimino);
            mapManager.MakeTetriminoRightPlace(mapManager.currentTetrimino);
            for(int i = 0; i < mapManager.currentTetrimino.rotatedPosition.Length; i++)
            {
                if (Tetrimino.rotationInformation[(int)mapManager.currentTetrimino.tetriminoType].horizontalLength[i] + mapManager.currentTetrimino.mapCoord.x > MapManager.width)
                    mapManager.currentTetrimino.rotatedPosition[i] = MapManager.width - Tetrimino.rotationInformation[(int)mapManager.currentTetrimino.tetriminoType].horizontalLength[i];
                else
                    mapManager.currentTetrimino.rotatedPosition[i] = (int)mapManager.currentTetrimino.mapCoord.x;
            }
            MakeGhost(mapManager.currentTetrimino, randomTetrimino);
            //MM.controlCurrentTetrimino = true;
        }
    }
    /// <summary>
    /// Make initial Tetrimino at bottom.
    /// </summary>
    public void MakeInitialTetrimino()
    {
        if (!mapManager.gameOver)
        {
            int randomPosition = Random.Range(0, MapManager.width);
            int randomTetrimino = TetriminoRandomizer();
            mapManager.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], mapManager.tetrisMapCoord + mapManager.tetrisMapSize * new Vector3(randomPosition, 0, mapManager.tetrisMapCoord.z), Quaternion.identity);
            mapManager.startRoom = mapManager.currentTetrimino.rooms[Random.Range(0, mapManager.currentTetrimino.rooms.Length)];
            mapManager.startRoom.specialRoomType = Room.SpecialRoomType.Start;
            mapManager.currentTetrimino.mapCoord = (mapManager.currentTetrimino.transform.position - mapManager.tetrisMapCoord) / mapManager.tetrisMapSize;
            mapManager.SetRoomMapCoord(mapManager.currentTetrimino);
            mapManager.MakeTetriminoRightPlace(mapManager.currentTetrimino);
            for (int i = 0; i < mapManager.currentTetrimino.rooms.Length; i++)
            {
                mapManager.currentTetrimino.transform.position = mapManager.currentTetrimino.mapCoord * mapManager.tetrisMapSize + mapManager.tetrisMapCoord;
            }
            mapManager.UpdateMap(mapManager.currentTetrimino);
            mapManager.CreateRoom(mapManager.currentTetrimino);
            MakeTetrimino();
        }
    }
    /// <summary>
    /// Make ghost for tetrimino
    /// </summary>
    /// <param name="te">Which tetrimino to make ghost</param>
    public void MakeGhost(Tetrimino te, int ghostType)
    {
        mapManager.currentGhost = Instantiate(ghosts[ghostType], te.transform.position, Quaternion.identity);
        mapManager.currentGhost.mapCoord = te.mapCoord;
        for(int i = 0; i < te.rooms.Length; i++)
        {
            mapManager.currentGhost.rooms[i].mapCoord = te.rooms[i].mapCoord;
        }
    }

    /// <summary>
    /// Logic for random tetrimino.
    /// </summary>
    /// <returns>Tetrimino that would be made next.</returns>
    int TetriminoRandomizer()
    {
        int sum = 0, count;
        foreach (int child in randomTetrimino)
            sum += child;
        int randomizer = Random.Range(0, sum);
        for(count = 0; count < randomTetrimino.Length; count++)
        {
            randomizer -= randomTetrimino[count];
            if(randomizer <= 0)
            {
                for (int i = 0; i < randomTetrimino.Length; i++)
                    randomTetrimino[i]++;
                randomTetrimino[count] = 0;
                return count;
            }
        }
        return count;
    }

    /*
     * Test
     * */
    private void Awake()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
