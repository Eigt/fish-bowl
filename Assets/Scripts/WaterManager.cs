using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour {

	public GameObject segment;
	public float amplitude;

	// Use this for initialization
	void Start () {
		Vector3 pos1 = new Vector3 (-4.5f, -2f, -2f);
		Vector3 pos2 = new Vector3 (4.5f, -2f, -2f);
		Instantiate (segment, pos1, Quaternion.identity);
		Instantiate (segment, pos2, Quaternion.identity);
		LineGenerate (pos1, pos2, amplitude);
	}
	
	// LineGenerate is a recursive midpoint-bisection
	void LineGenerate (Vector2 left, Vector2 right, float amp) {

		float xm = left.x + (right.x - left.x) / 2f;
		float ym;
		float dif = Mathf.Abs (right.y - left.y);
		if (right.y >= left.y) {
			ym = left.y + dif / 2f;
		} else {
			ym = right.y + dif / 2f;
		}
		Vector3 mid;

		if (right.x - left.x <= 0.1f) {
			mid = new Vector3 (xm, ym, -2f);
			Instantiate (segment, mid, Quaternion.identity);
			return;
		}
			
		if (Random.value >= 0.5f) {
			ym = ym + Random.value * amp;
		} else {
			ym = ym - Random.value * amp;
		}
		mid = new Vector3 (xm, ym, -2f);
		Instantiate (segment, mid, Quaternion.identity);

		LineGenerate (left, mid, dif/2f);
		LineGenerate (mid, right, dif/2f);

	}
}
