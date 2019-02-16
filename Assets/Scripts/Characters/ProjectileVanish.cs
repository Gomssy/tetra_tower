using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVanish : MonoBehaviour {

    public float vanishDistance;
    Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }
    void Update () {
        if (Vector3.Distance(startPosition, transform.position) > vanishDistance)
            Destroy(gameObject);
	}
}
