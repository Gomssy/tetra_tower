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
    /// <summary>
    /// Save probability of which concept will be made next.
    /// </summary>
    int[] randomConcept = { 1, 1, 1, 1 };

    /*
     * functions
     * */
    /// <summary>
    /// Make new Tetrimino at top.
    /// </summary>
    public void MakeTetrimino()
    {
        if (GameManager.gameState != GameState.GameOver)
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
            MapManager.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], 
                MapManager.tetrisMapCoord + MapManager.tetrisMapSize * new Vector3(randomPosition, MapManager.realHeight + 1, MapManager.tetrisMapCoord.z), Quaternion.identity);
            MapManager.currentTetrimino.mapCoord = (MapManager.currentTetrimino.transform.position - MapManager.tetrisMapCoord) / MapManager.tetrisMapSize;
            mapManager.SetRoomMapCoord(MapManager.currentTetrimino);
            mapManager.MakeTetriminoRightPlace(MapManager.currentTetrimino);
            int tetriminoConcept = ConceptRandomizer();
            for (int i = 0; i < MapManager.currentTetrimino.rotatedPosition.Length; i++)
            {
                if (Tetrimino.rotationInformation[(int)MapManager.currentTetrimino.tetriminoType].horizontalLength[i] + MapManager.currentTetrimino.mapCoord.x > MapManager.width)
                    MapManager.currentTetrimino.rotatedPosition[i] = MapManager.width - Tetrimino.rotationInformation[(int)MapManager.currentTetrimino.tetriminoType].horizontalLength[i];
                else
                    MapManager.currentTetrimino.rotatedPosition[i] = (int)MapManager.currentTetrimino.mapCoord.x;
            }
            for (int i = 0; i < MapManager.currentTetrimino.rooms.Length; i++)
            {
                Room room = MapManager.currentTetrimino.rooms[i];
                MapManager.currentTetrimino.transform.position = MapManager.currentTetrimino.mapCoord * MapManager.tetrisMapSize + MapManager.tetrisMapCoord;
                room.stage = MapManager.currentStage;
                room.roomConcept = tetriminoConcept;
                mapManager.SetRoomSprite(room, i);
            }
            MakeGhost(MapManager.currentTetrimino, randomTetrimino);
            MapManager.isTetriminoFalling = false;
            while (mapManager.roomsWaiting.Count != 0 && MapManager.currentTetrimino.notNormalRoomCount < 4)
            {
                mapManager.UpgradeRoom(mapManager.roomsWaiting.Dequeue());
            }
            mapManager.tetriminoCreatedTime = Time.time;
            StartCoroutine(mapManager.CountTetriminoWaitingTime());
        }
    }
    /// <summary>
    /// Make initial Tetrimino at bottom.
    /// </summary>
    public void MakeInitialTetrimino()
    {
        if (GameManager.gameState != GameState.GameOver)
        {
            int randomPosition = Random.Range(0, MapManager.width);
            int randomTetrimino = TetriminoRandomizer();
            MapManager.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], MapManager.tetrisMapCoord + MapManager.tetrisMapSize * new Vector3(randomPosition, 0, MapManager.tetrisMapCoord.z), Quaternion.identity);
            MapManager.currentRoom = MapManager.currentTetrimino.rooms[0];
            MapManager.currentRoom.specialRoomType = RoomType.Start;
            MapManager.currentTetrimino.mapCoord = (MapManager.currentTetrimino.transform.position - MapManager.tetrisMapCoord) / MapManager.tetrisMapSize;
            mapManager.SetRoomMapCoord(MapManager.currentTetrimino);
            mapManager.MakeTetriminoRightPlace(MapManager.currentTetrimino);
            int tetriminoConcept = ConceptRandomizer();
            for (int i = 0; i < MapManager.currentTetrimino.rooms.Length; i++)
            {
                Room room = MapManager.currentTetrimino.rooms[i];
                MapManager.currentTetrimino.transform.position = MapManager.currentTetrimino.mapCoord * MapManager.tetrisMapSize + MapManager.tetrisMapCoord;
                room.stage = MapManager.currentStage;
                room.roomConcept = tetriminoConcept;
                mapManager.SetRoomSprite(room, i);
            }
            mapManager.UpdateMap(MapManager.currentTetrimino);
            mapManager.CreateRoom(MapManager.currentTetrimino);
            MapManager.currentRoom.fog.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            MapManager.currentRoom.GetComponent<SpriteRenderer>().sprite = mapManager.roomsSpritesDistributed[MapManager.currentStage][(int)RoomSpriteType.Current];
            MapManager.currentRoom.ClearRoom();
            MapManager.tempRoom = MapManager.currentRoom;
            MakeTetrimino();
        }
    }
    /// <summary>
    /// Make ghost for tetrimino
    /// </summary>
    /// <param name="te">Which tetrimino to make ghost</param>
    public void MakeGhost(Tetrimino te, int ghostType)
    {
        MapManager.currentGhost = Instantiate(ghosts[ghostType], te.transform.position, Quaternion.identity);
        MapManager.currentGhost.mapCoord = te.mapCoord;
        for(int i = 0; i < te.rooms.Length; i++)
        {
            MapManager.currentGhost.rooms[i].mapCoord = te.rooms[i].mapCoord;
            MapManager.currentGhost.rooms[i].GetComponent<SpriteRenderer>().sprite = MapManager.currentTetrimino.rooms[i].GetComponent<SpriteRenderer>().sprite;
            MapManager.currentGhost.rooms[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
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
                {
                    randomTetrimino[i]++;
                }
                randomTetrimino[count] = 0;
                return count;
            }
        }
        return count;
    }
    /// <summary>
    /// Logic for random concept.
    /// </summary>
    /// <returns>Concept that would be set next.</returns>
    int ConceptRandomizer()
    {
        int sum = 0, count;
        foreach (int child in randomConcept)
            sum += child;
        int randomizer = Random.Range(0, sum);
        for (count = 0; count < randomConcept.Length; count++)
        {
            randomizer -= randomConcept[count];
            if (randomizer <= 0)
            {
                for (int i = 0; i < randomConcept.Length; i++)
                {
                    randomConcept[i]++;
                }
                randomConcept[count] = 0;
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
