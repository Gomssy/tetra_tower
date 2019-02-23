using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

	IEnumerator RandomColor()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			Camera.main.backgroundColor = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));

		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		Camera.main.backgroundColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		GameObject.Find("Text").GetComponent<Text>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		GameObject.Find("Title").GetComponent<Text>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		GameObject.Find("Button").GetComponent<Button>().image.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}
}
