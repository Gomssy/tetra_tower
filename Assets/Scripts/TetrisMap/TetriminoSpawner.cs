using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoSpawner : MonoBehaviour {

    /*
     * variables
     * */
     /// <summary>
     /// All tetriminoes.
     /// </summary>
    public Tetrimino[] tetriminoes;

    /// <summary>
    /// Save probability of which tetrimino will be made next.
    /// </summary>
    int[] randomTetrimino = { 1, 1, 1, 1, 1, 1, 1 };

    /*
     * functions
     * */
    /// <summary>
    /// Make new Tetrimino on top.
    /// </summary>
    public void MakeTetrimino()
    {
        var MM = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
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
            //MM.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], MM.tetrisMapCoord + MM.tetrisMapSize * new Vector3(randomPosition, MapManager.realHeight + 1, MM.tetrisMapCoord.z), Quaternion.identity);
            MM.currentTetrimino = Instantiate(tetriminoes[randomTetrimino], MM.tetrisMapCoord + MM.tetrisMapSize * new Vector3(0, 0, 0), Quaternion.identity);
            MM.MakeTetriminoRightPlace(MM.currentTetrimino);
            MM.currentTetrimino.mapCoord = (MM.currentTetrimino.transform.position - MM.tetrisMapCoord) / MM.tetrisMapSize;
            MM.SetRoomMapCoord(MM.currentTetrimino);
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

    // Use this for initialization
    void Start () {
        MakeTetrimino();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
