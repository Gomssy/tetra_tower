using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pointer : MonoBehaviour {
    Room currentRoom;
    [SerializeField]
    Enemy[] enemies;
    [SerializeField]
    Enemy nearestEnemy;
    public Camera cam;
    public Vector3 p;
    public Canvas can;
    public float width, height;
    public bool pointEnabled = true;
	// Use this for initialization
	void Start () {

        width = can.GetComponent<RectTransform>().sizeDelta.x / 2;
        height = can.GetComponent<RectTransform>().sizeDelta.y / 2;
        cam = Camera.main;
        can = InventoryManager.Instance.ui.GetComponent<Canvas>();

    }
	
	// Update is called once per frame
	void Update () {
        currentRoom = MapManager.currentRoom;
        enemies= currentRoom.GetComponentsInChildren<Enemy>();
        pointEnabled = true;
        float minDistance = 99999999f;
        foreach(Enemy e in enemies)
        {
            Vector3 temp = 120f * (e.transform.parent.transform.position - cam.transform.position);
            if (Mathf.Abs(temp.x) < width && Mathf.Abs(temp.y) < height)
            {
                pointEnabled = false;
            }

                float dist= Vector3.Distance(cam.transform.position, e.transform.parent.transform.position);
            if (dist < minDistance)
            {
                nearestEnemy = e;
                minDistance = dist;

            }

        }
        if (enemies.Length > 0)
        {
            p = nearestEnemy.transform.parent.transform.position - cam.transform.position; 
               Vector3 temp= 120f*(nearestEnemy.transform.parent.transform.position - cam.transform.position);
            
            if( Mathf.Abs(temp.x) < width-20f && Mathf.Abs(temp.y) < height-20f)
            {
                pointEnabled = false;
            }

                  GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Clamp(temp.x,-(width-50f),width-50f), Mathf.Clamp(temp.y, -(height-50f), height-50f),0f);
            if (pointEnabled )
            {
                Vector3 local = nearestEnemy.transform.parent.transform.position - GameManager.Instance.player.transform.position;
                GetComponent<Image>().enabled = true;
                GetComponent<RectTransform>().localRotation= Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(local.y,local.x)-90, Vector3.forward);
            }
           
            

        }
        else
        {
            pointEnabled = false;
        }
        GetComponent<Image>().enabled = pointEnabled;
        
	}
}
