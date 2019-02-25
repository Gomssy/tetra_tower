using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    LifeStoneManager lifeStoneManager;
    public static int tx, ty;
    public static float X = 0.7f, Y = 1.6f;
    public int ttx;
    public int tty;
    public Text displayText;
    
    public IEnumerator DisplayText(string _text)
    {
        displayText.text = _text;
        for (int i = 255; i >= 0; i -= 10)
        {
            yield return null;
            displayText.transform.position = transform.position + new Vector3(0, 2, 0);
            Debug.Log(i);
            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, (float)i /255);
        }
        displayText.text = "";
    }

    // Use this for initialization
    void Start () {
        displayText = Instantiate(displayText, GameManager.Instance.textCanvas.transform);
        ttx = (int)(transform.position.x / 24f);
        tty = (int)(transform.position.y - 0.8f / 24f);
        lifeStoneManager = LifeStoneManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        tx = (int)(transform.position.x / 24f);
        ty = (int)((transform.position.y - 0.8f) / 24f);
        if ((ttx != tx || tty != ty) && MapManager.isRoomFalling != true)
        {
            MapManager.tempRoom = MapManager.mapGrid[tx, ty];
            if (tx < ttx)
            {
                MapManager.currentRoom.CloseDoor("Left", true);
            }
            else if (tx > ttx)
            {
                MapManager.currentRoom.CloseDoor("Right", true);
            }
            else if (ty < tty)
            {
                MapManager.currentRoom.CloseDoor("Down", true);
            }
            else if (ty > tty)
            {
                transform.position += new Vector3(0, 0.5f, 0);
                MapManager.currentRoom.CloseDoor("Up", true);
            }
        }
        ttx = tx;
        tty = ty;
        if (lifeStoneManager.CountType() == 0)
            GameManager.gameState = GameState.GameOver;
	}
}
