using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject particlePrefab;
    public GameObject spawnPosition;

    GameObject[] particles;


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
    
	void Start () {
        particles = new GameObject[30];
        for(int i=0; i<30; i++)
        {
            particles[i] = Instantiate(particlePrefab);
            particles[i].GetComponent<MenuParticle>().Init(spawnPosition.transform.position);
        }
	}
	
	void Update ()
	{
	}
}
