using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour {

    public float width;
    public float height;
    public float yOffset;
    public GameObject barPrefab;
    public Sprite bor, red;
    GameObject empty, instBor, instRed;
    

	void Awake () {
        if(!GameObject.Find("HPBarEmpty"))
            empty = Instantiate(new GameObject("HPBarEmpty"));
        empty = GameObject.Find("HPBarEmpty");
        instBor = Instantiate(barPrefab, empty.transform);
        instBor.GetComponent<SpriteRenderer>().sprite = bor;

        instRed = Instantiate(barPrefab, empty.transform);
        instRed.GetComponent<SpriteRenderer>().sprite = red;
        instRed.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
	
	void Update () {
		if(transform.parent.gameObject.activeSelf)
        {
            instBor.SetActive(true);
            instBor.transform.localScale = new Vector3(width / instBor.GetComponent<SpriteRenderer>().size.x, height / instBor.GetComponent<SpriteRenderer>().size.y, 1);
            instBor.transform.position = transform.parent.position + new Vector3(0, yOffset, 0);

            float cur = Mathf.Max(0,transform.parent.GetComponentInChildren<Enemy>().currHealth / transform.parent.GetComponentInChildren<Enemy>().maxHealth);
            instRed.SetActive(true);
            instRed.transform.localScale = new Vector3((width / instRed.GetComponent<SpriteRenderer>().size.x - height / instRed.GetComponent<SpriteRenderer>().size.y * 0.2f) * cur, height / instRed.GetComponent<SpriteRenderer>().size.y * 0.8f, 1);
            instRed.transform.position = transform.parent.position + new Vector3((width - height * 0.2f) / -2f, yOffset, 0);
        }
        else
        {
            instBor.SetActive(false);
            instRed.SetActive(false);
        }
	}
}
