using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    /*
     * variables
     * */
    /// <summary>
    /// Time press has started to collapsed.
    /// </summary>
    public float initialCollapseTime;
    /// <summary>
    /// Row the press is accounting for.
    /// </summary>
    public int row;
    /// <summary>
    /// Lowest empty row below this press.
    /// </summary>
    public int bottomRow;
    /// <summary>
    /// Created order of this press between simultaneously created presses.
    /// </summary>
    public int createdOrder;
    /// <summary>
    /// Number of presses created simultaneously with this press.
    /// </summary>
    public int simultaneouslyCreatedPressNumber;
    /// <summary>
    /// Check if this press is on left side or not.
    /// </summary>
    public bool isLeft;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals("Player"))
            GameManager.gameState = GameManager.GameState.GameOver;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
