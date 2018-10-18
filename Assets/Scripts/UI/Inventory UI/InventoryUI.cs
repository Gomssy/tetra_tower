using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    Canvas canvas;
    bool isActive;

    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
        isActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;
            canvas.enabled = isActive;
            if (isActive)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            
        }
    }

}
