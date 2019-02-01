using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwapTile : MonoBehaviour {

    public TileBase[] tileFR;
    public TileBase[] tileIT;
    public TileBase[] tileWW;
    public TileBase[] tileUT;
    public Tilemap map;
    public RoomInGame roomInGame;
    public TileBase[] fillLeftDoor;
    public TileBase[] fillRightDoor;
        //기본 테마를 숲속 유적으로 하고 if(얼음신전)
    public void Init()
    {
        for(int i=0;i<tileFR.Length;i++)
          map.SwapTile(tileFR[i], tileIT[i]);
    }
 

	

    /*
        같은 방식으로
        if(수로)
        for(int i=0;i<tileFR.Length;i++)
            map.SwapTile(tileFR[i], tileWW[i]);

        if(나무아래)
        for(int i=0;i<tileFR.Length;i++)
            map.SwapTile(tileFR[i], tileUT[i]);
    */

    public void FillEmptyDoor(int leftDoorLocation, int rightDoorLocation)
    {
        Tilemap outerWallMap = roomInGame.transform.GetChild(4).GetComponent<Tilemap>();
        int[] doorLocations = { 1, 9 ,17};
        for (int i = 0; i < 3; i++)
        {
            //경우를 따지자
            /*
             *  왼쪽의 경우
             *      문 위 타일
             *      1. 오른쪽에 타일이 없음 -> [| |] 모양 넣기 0
             *      2. 오른쪽에만 타일이 있음 -> [| .] 모양 넣기 2
             *      3. 오른쪽이랑 오른쪽 대각선 위에 타일이 있음 -> [| ] 모양 넣기 1
             *      
             *      문 아래 타일
             *      1. 오른쪽 타일 없음 -> 문 위랑 동일
             *      2. 오른족에만 타일 -> [| '] 모양 넣기 3
             *      3. 오른족이랑 오른쪽 대각선 아래에 타일 -> [| ] 모양
             *      
             *  오른쪽의 경우
             *      문 위 타일
             *      1. 왼쪽에 X -> [| |]
             *      2. 왼쪽에만 -> [. |]
             *      3. 왼쪽이랑 왼쪽 대각선 아래 -> [ |]
             *      
             *      문 아래
             *      1. 왼쪽에 X -> [| |]
             *      2. 왼쪽에만 -> [' |]
             *      3. 왼쪽이랑 왼쪽 대각선 위 -> [ |]
             */
            if (i != leftDoorLocation)
            {
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] + 1, 0), fillLeftDoor[0]);
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i], 0), fillLeftDoor[0]);
               
                if(outerWallMap.HasTile(new Vector3Int(1, doorLocations[i] + 2, 0)) && outerWallMap.HasTile(new Vector3Int(1, doorLocations[i] + 3, 0)))
                    outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] + 2, 0), fillLeftDoor[1]);
                else if (outerWallMap.HasTile(new Vector3Int(1, doorLocations[i] + 2, 0)))
                    outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] + 2, 0), fillLeftDoor[2]);
                else
                    outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] + 2, 0), fillLeftDoor[0]);
                
                if(outerWallMap.HasTile(new Vector3Int(1, doorLocations[i] -1, 0)) && outerWallMap.HasTile(new Vector3Int(1, doorLocations[i] - 2, 0)))
                    outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] - 1, 0), fillLeftDoor[1]);
                else if(outerWallMap.HasTile(new Vector3Int(1, doorLocations[i] - 1, 0)))
                    outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] - 1, 0), fillLeftDoor[3]);
                else
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] - 1, 0), fillLeftDoor[0]);
            }
            if (i != rightDoorLocation)
            {
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] + 1, 0), fillRightDoor[0]);
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i], 0), fillRightDoor[0]);

                if (outerWallMap.HasTile(new Vector3Int(22, doorLocations[i] + 2, 0)) && outerWallMap.HasTile(new Vector3Int(22, doorLocations[i] + 3, 0)))
                    outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] + 2, 0), fillRightDoor[1]);
                else if(outerWallMap.HasTile(new Vector3Int(22, doorLocations[i] + 2, 0)))
                    outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] + 2, 0), fillRightDoor[2]);
                else
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] + 2, 0), fillRightDoor[0]);

                if (outerWallMap.HasTile(new Vector3Int(22, doorLocations[i] - 1, 0)) && outerWallMap.HasTile(new Vector3Int(22, doorLocations[i] - 2, 0)))
                    outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] - 1, 0), fillRightDoor[1]);
                else if (outerWallMap.HasTile(new Vector3Int(22, doorLocations[i] - 1, 0)))
                    outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] - 1, 0), fillRightDoor[3]);
                else
                    outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] - 1, 0), fillRightDoor[0]);
            }
        }
    }
}


