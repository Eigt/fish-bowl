using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float xMin, xMax;
	public float speed;
	public float rotationSpeed;
	public float fireSpeed;
	public GameObject bigDrop, medDrop, smallDrop;
	public Transform bSpawn, mSpawn, sSpawn;

	private Transform core;
	private float nextFire = 0;

	// Use this for initialization
	void Start () {
		core = this.gameObject.transform.GetChild (0);
	}

	// Update is called once per frame
	void Update () {
		Vector3 movement = Vector3.zero;
		Vector3 spin = Vector3.zero;
		if (Input.GetKey("a")) {
			movement = Vector3.left;
		} else if (Input.GetKey("d")) {
			movement = Vector3.right;
		}
		if (Input.GetKey("w")) {
			spin = new Vector3 (0f, 0f, 1f);
		} else if (Input.GetKey("s")) {
			spin = new Vector3 (0f, 0f, -1f);
		}
		if (Input.GetKeyDown ("space") && Time.time > nextFire) {
			nextFire += fireSpeed;
			Instantiate (bigDrop, bSpawn.position, bSpawn.rotation);
			Instantiate (medDrop, mSpawn.position, mSpawn.rotation);
			Instantiate (smallDrop, sSpawn.position, sSpawn.rotation);
		}

		transform.Translate (movement*speed*Time.deltaTime);
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, xMin, xMax), transform.position.y, transform.position.z);

		core.Rotate (spin*rotationSpeed*Time.deltaTime);
		core.localEulerAngles = new Vector3 (core.transform.localEulerAngles.x, core.transform.localEulerAngles.y, Mathf.Clamp (core.transform.localEulerAngles.z, 120f, 240f));
	}

	void FixedUpdate () {

	}

}
