using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPhysicsManager : PhysicsManager {

	public float startForce;

	void Start () {
		rb.AddRelativeForce (new Vector2 (0f, -startForce), ForceMode2D.Impulse);
	}

}
