using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject bug;

	private Vector2 windSpeed;
	private float windUpdate = 0;
	private float bugSpawn = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > windUpdate) {
			if (Random.value > 0.5f) {
				windUpdate += 1f;
			} else {
				windUpdate += 2f;
			}
			windSpeed = new Vector2 (Random.Range (1.4f, 7f), 0f);
		}
		if (Time.time > bugSpawn) {
			if (Random.value > 0.5f) {
				bugSpawn += 3.5f;
			} else {
				bugSpawn += 5f;
			}
			Instantiate (bug);
		}
	}

	public Vector2 GetWind () {
		return windSpeed;
	}
}
