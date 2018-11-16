using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoSpawner : MonoBehaviour {

    /*
     * variables
     * */
    MapManager MM;
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
        if (!MM.gameOver)
        {
            int randomPosition = Random.Range(0, MapManager.width);
            int randomTetrimino;
            if (MM.spawnBossTetrimino)
            {
                randomTetrimino = 7;
                MM.spawnBossTetrimino = false;
            }
            else
                randomTetrimino = TetriminoRandomizer();
            MM.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], MM.tetrisMapCoord + MM.tetrisMapSize * new Vector3(randomPosition, MapManager.realHeight + 1, MM.tetrisMapCoord.z), Quaternion.identity);
            MM.currentTetrimino.mapCoord = (MM.currentTetrimino.transform.position - MM.tetrisMapCoord) / MM.tetrisMapSize;
            MM.SetRoomMapCoord(MM.currentTetrimino);
            MM.MakeTetriminoRightPlace(MM.currentTetrimino);
            for(int i = 0; i < MM.currentTetrimino.rotatedPosition.Length; i++)
            {
                if (Tetrimino.rotationInformation[(int)MM.currentTetrimino.tetriminoType].horizontalLength[i] + MM.currentTetrimino.mapCoord.x > MapManager.width)
                    MM.currentTetrimino.rotatedPosition[i] = MapManager.width - Tetrimino.rotationInformation[(int)MM.currentTetrimino.tetriminoType].horizontalLength[i];
                else
                    MM.currentTetrimino.rotatedPosition[i] = (int)MM.currentTetrimino.mapCoord.x;
            }
            MakeGhost(MM.currentTetrimino, randomTetrimino);
            //MM.controlCurrentTetrimino = true;
        }
    }
    /// <summary>
    /// Make initial Tetrimino at bottom.
    /// </summary>
    public void MakeInitialTetrimino()
    {
        if (!MM.gameOver)
        {
            int randomPosition = Random.Range(0, MapManager.width);
            int randomTetrimino = TetriminoRandomizer();
            MM.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], MM.tetrisMapCoord + MM.tetrisMapSize * new Vector3(randomPosition, 0, MM.tetrisMapCoord.z), Quaternion.identity);
            MM.startRoom = MM.currentTetrimino.rooms[Random.Range(0, MM.currentTetrimino.rooms.Length)];
            MM.startRoom.specialRoomType = Room.SpecialRoomType.Start;
            MM.currentTetrimino.mapCoord = (MM.currentTetrimino.transform.position - MM.tetrisMapCoord) / MM.tetrisMapSize;
            MM.SetRoomMapCoord(MM.currentTetrimino);
            MM.MakeTetriminoRightPlace(MM.currentTetrimino);
            for (int i = 0; i < MM.currentTetrimino.rooms.Length; i++)
            {
                MM.currentTetrimino.transform.position = MM.currentTetrimino.mapCoord * MM.tetrisMapSize + MM.tetrisMapCoord;
            }
            MM.UpdateMap(MM.currentTetrimino);
            MM.CreateRoom(MM.currentTetrimino);
            MakeTetrimino();
        }
    }
    /// <summary>
    /// Make ghost for tetrimino
    /// </summary>
    /// <param name="te">Which tetrimino to make ghost</param>
    public void MakeGhost(Tetrimino te, int ghostType)
    {
        MM.currentGhost = Instantiate(ghosts[ghostType], te.transform.position, Quaternion.identity);
        MM.currentGhost.mapCoord = te.mapCoord;
        for(int i = 0; i < te.rooms.Length; i++)
        {
            MM.currentGhost.rooms[i].mapCoord = te.rooms[i].mapCoord;
        }
        MM.TetriminoMapCoordDown(MM.currentGhost);
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
        MM = GameObject.Find("MapManager").GetComponent<MapManager>();
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
