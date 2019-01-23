using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangePlatformTile : MonoBehaviour {

    public Tilemap map;
    public Tile currentTile;
    public Tile forestRuin;
    public Tile iceTemple;
    public Tile waterWay;
    public Tile underTree;
   
	// Use this for initialization
    private void Start()
    {
         map.SwapTile(currentTile, forestRuin);
    }
    /*
      if(concept 숲속유적)
          map.SwapTile(currentTile, forestRuin);
      if concept 얼음신전
          map.SwapTile(currentTile, iceTemple);
      if concept 수로
          map.SwapTile(currentTile, waterWay);
      if concept 나무아래
          map.SwapTile(currentTile, underTree);
    */
    private void Update()
    {
        
    }


    // Update is called once per frame

}
