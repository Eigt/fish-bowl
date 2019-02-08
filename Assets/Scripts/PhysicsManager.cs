using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour {

	public float gravityScale;
	public GameObject gameManager;

	protected Rigidbody2D rb;
	private bool hit;

	void Awake() {
		rb = GetComponent<Rigidbody2D> ();
		gameManager = GameObject.FindGameObjectWithTag ("GameController");
		if (this.tag == "Bug") {
			gravityScale = 0;
		}
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
		foreach (GameObject box in boxes) {
			Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), box.GetComponent<BoxCollider2D>());
		}
	}

	void FixedUpdate () {

		if (this.transform.position.y < -5f || this.transform.position.y > 8f) {
			Destroy (this.gameObject);
		} else if (this.transform.position.x < -7f || this.transform.position.x > 7f) {
			Destroy (this.gameObject);
		}

		if (this.transform.position.y >= 1.5f) {
			Vector2 wind = gameManager.GetComponent<GameManager> ().GetWind ();;
			rb.AddForce (wind, ForceMode2D.Force);
		}

		rb.AddForce (Physics2D.gravity*gravityScale, ForceMode2D.Force);

		GameObject[] bugs = GameObject.FindGameObjectsWithTag ("Bug");
		foreach (GameObject bug in bugs) {
			Physics2D.IgnoreCollision (this.GetComponent<Collider2D>(), bug.GetComponent<BoxCollider2D>());
		}

		GameObject[] water = GameObject.FindGameObjectsWithTag ("Water");
		foreach (GameObject segment in water) {
			if (IntersectsWater (this.transform.position, segment.transform.position)) {
				Destroy (this.gameObject);
			}
		}

		GameObject[] boxes = GameObject.FindGameObjectsWithTag ("Box");
		foreach (GameObject box in boxes) {
			if (this.tag == "Bug") {
				if (BoxIntersectsBox (this.gameObject, box)) {
					// collision response
				}
			} else if (this.tag == "Circle") {

			}
		}
		GameObject[] drops = GameObject.FindGameObjectsWithTag ("Circle");
		if (this.tag == "Circle") {
			foreach (GameObject drop in drops) {
				Physics2D.IgnoreCollision (this.GetComponent<CircleCollider2D>(), drop.GetComponent<CircleCollider2D>());
			}
		}
		if (this.tag == "Bug" && hit == false) {
			foreach (GameObject drop in drops) {
				if (CircleIntersectsBox (this.gameObject, drop)) {
					Destroy (drop.gameObject);
					hit = true;
					gravityScale = 0.5f;
				}
			}
		}

	}

	bool BoxIntersectsBox (GameObject box1, GameObject box2) {
		BoxCollider2D collider1 = box1.GetComponent<BoxCollider2D> ();
		Transform box1Transform = collider1.transform;
		Vector3 position1 = box1Transform.TransformPoint (0, 0, 0);
		Vector2 size1 = new Vector2 (collider1.size.x * box1Transform.localScale.x * 0.5f, collider1.size.y * box1Transform.localScale.y * 0.5f);

		Vector3 tlPoint1 = new Vector2 (-size1.x, size1.y);
		Vector3 trPoint1 = new Vector2 (size1.x, size1.y);
		Vector3 brPoint1 = new Vector2 (size1.x, -size1.y);
		Vector3 blPoint1 = new Vector2 (-size1.x, -size1.y);

		tlPoint1 = RotateOnPivot (tlPoint1, Vector3.zero, box1Transform.eulerAngles);
		trPoint1 = RotateOnPivot (trPoint1, Vector3.zero, box1Transform.eulerAngles);
		brPoint1 = RotateOnPivot (brPoint1, Vector3.zero, box1Transform.eulerAngles);
		blPoint1 = RotateOnPivot (blPoint1, Vector3.zero, box1Transform.eulerAngles);

		tlPoint1 += position1;
		trPoint1 += position1;
		brPoint1 += position1;
		blPoint1 += position1;

		BoxCollider2D collider2 = box2.GetComponent<BoxCollider2D> ();
		Transform box2Transform = collider2.transform;
		Vector3 position2 = box2Transform.TransformPoint (0, 0, 0);
		Vector2 size2 = new Vector2 (collider2.size.x * box2Transform.localScale.x * 0.5f, collider2.size.y * box2Transform.localScale.y * 0.5f);

		Vector3 tlPoint2 = new Vector2 (-size2.x, size2.y);
		Vector3 trPoint2 = new Vector2 (size2.x, size2.y);
		Vector3 brPoint2 = new Vector2 (size2.x, -size2.y);
		Vector3 blPoint2 = new Vector2 (-size2.x, -size2.y);

		tlPoint2 = RotateOnPivot (tlPoint2, Vector3.zero, box2Transform.eulerAngles);
		trPoint2 = RotateOnPivot (trPoint2, Vector3.zero, box2Transform.eulerAngles);
		brPoint2 = RotateOnPivot (brPoint2, Vector3.zero, box2Transform.eulerAngles);
		blPoint2 = RotateOnPivot (blPoint2, Vector3.zero, box2Transform.eulerAngles);

		tlPoint2 += position2;
		trPoint2 += position2;
		brPoint2 += position2;
		blPoint2 += position2;

		if (PointInRectangle (position1, tlPoint2, trPoint2, brPoint2, blPoint2)) {
			return true;
		} else if (PointInRectangle (position2, tlPoint1, trPoint1, brPoint1, blPoint1)) {
			return true;
		}
		if (LinesIntersect (tlPoint1, trPoint1, trPoint2, brPoint2)) {
			return true;
		} else if (LinesIntersect (tlPoint1, trPoint1, brPoint2, blPoint2)) {
			return true;
		} else if (LinesIntersect (tlPoint1, trPoint1, blPoint2, tlPoint2)) {
			return true;
		} else if (LinesIntersect (trPoint1, brPoint1, brPoint2, blPoint2)) {
			return true;
		} else if (LinesIntersect (trPoint1, brPoint1, blPoint2, tlPoint2)) {
			return true;
		} else if (LinesIntersect (trPoint1, brPoint1, tlPoint2, trPoint2)) {
			return true;
		} else if (LinesIntersect (brPoint1, blPoint1, blPoint2, tlPoint2)) {
			return true;
		} else if (LinesIntersect (brPoint1, blPoint1, tlPoint2, trPoint2)) {
			return true;
		} else if (LinesIntersect (brPoint1, blPoint1, trPoint2, brPoint2)) {
			return true;
		} else if (LinesIntersect (blPoint1, tlPoint1, tlPoint2, trPoint2)) {
			return true;
		} else if (LinesIntersect (blPoint1, tlPoint1, trPoint2, brPoint2)) {
			return true;
		} else if (LinesIntersect (blPoint1, tlPoint1, brPoint2, blPoint2)) {
			return true;
		} else {
			return false;
		}
	}

	bool CircleIntersectsBox (GameObject box, GameObject circle) {
		BoxCollider2D colliderB = box.GetComponent<BoxCollider2D> ();
		Transform boxTransform = colliderB.transform;
		Vector3 positionB = boxTransform.TransformPoint (0, 0, 0);
		Vector2 size = new Vector2 (colliderB.size.x * boxTransform.localScale.x * 0.5f, colliderB.size.y * boxTransform.localScale.y * 0.5f);

		Vector3 tlPoint = new Vector2 (-size.x, size.y);
		Vector3 trPoint = new Vector2 (size.x, size.y);
		Vector3 brPoint = new Vector2 (size.x, -size.y);
		Vector3 blPoint = new Vector2 (-size.x, -size.y);

		tlPoint = RotateOnPivot (tlPoint, Vector3.zero, boxTransform.eulerAngles);
		trPoint = RotateOnPivot (trPoint, Vector3.zero, boxTransform.eulerAngles);
		brPoint = RotateOnPivot (brPoint, Vector3.zero, boxTransform.eulerAngles);
		blPoint = RotateOnPivot (blPoint, Vector3.zero, boxTransform.eulerAngles);

		tlPoint += positionB;
		trPoint += positionB;
		brPoint += positionB;
		blPoint += positionB;

		// now we have the positions of the four corners of the box collider

		CircleCollider2D colliderC = circle.GetComponent<CircleCollider2D> ();
		Transform circleTransform = colliderC.transform;
		Vector3 originC = circleTransform.TransformPoint (0, 0, 0);
		float radius = colliderC.radius;

		// now we have the origin and radius of the circle collider

		if (PointInRectangle (originC, tlPoint, trPoint, brPoint, blPoint)) {
			return true;
		} else if (CircleIntersectsLine (originC, radius, tlPoint, trPoint)) {
			return true;
		} else if (CircleIntersectsLine (originC, radius, trPoint, brPoint)) {
			return true;
		} else if (CircleIntersectsLine (originC, radius, brPoint, blPoint)) {
			return true;
		} else if (CircleIntersectsLine (originC, radius, blPoint, tlPoint)) {
			return true;
		} else {
			return false;
		}

	}

	bool IntersectsWater (Vector3 objPos, Vector3 waterPos) {
		if (objPos.x > waterPos.x+0.2f || objPos.x < waterPos.x-0.2f) {
			return false;
		} else if (objPos.y > waterPos.y+0.2f) {
			return false;
		} else {
			return true;
		}
	}

	bool PointInRectangle (Vector3 point, Vector3 tlPoint, Vector3 trPoint, Vector3 brPoint, Vector3 blPoint) {
		return RightOfLine (tlPoint, trPoint, point)
			&& RightOfLine (trPoint, brPoint, point)
			&& RightOfLine (brPoint, blPoint, point)
			&& RightOfLine (blPoint, tlPoint, point);
	}

	bool RightOfLine (Vector3 p0, Vector3 p1, Vector3 p2) {
		float result = (p1.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (p1.y - p0.y);
		if (result > 0) {
			return false;
		} else {
			return true;
		}
	}

	bool CircleIntersectsLine (Vector3 origin, float r, Vector3 p1, Vector3 p2) {
		float dist = (p2.x - p1.x)*(p1.y - origin.y) - (p1.x - origin.x)*(p2.y - p1.y);
		dist = dist * dist;
		dist = dist / (Mathf.Pow ((p2.x - p1.x), 2) + Mathf.Pow ((p2.y - p1.y), 2));

		if (dist <= Mathf.Pow(r, 2)) {
			return true;
		} else {
			return false;
		}
	}

	bool LinesIntersect (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
		float x = ((p1.x*p2.y-p1.y*p2.x)*(p3.x-p4.x)-(p1.x-p2.x)*(p3.x*p4.y-p3.y*p4.x))/(p1.x-p2.x)*(p3.y-p4.y)-(p1.y-p2.y)*(p3.x-p4.x);
		float y = ((p1.x*p2.y-p1.y*p2.x)*(p3.y-p4.y)-(p1.y-p2.y)*(p3.x*p4.y-p3.y*p4.x))/(p1.x-p2.x)*(p3.y-p4.y)-(p1.y-p2.y)*(p3.x-p4.x);

		if (p1.x >= p2.x) {
			if (!(p2.x <= x && x <= p1.x)) {
				return false;
			} else if (!(p1.x <= x && x <= p2.x)) {
				return false;
			}
		}
		if (p1.y >= p2.y) {
			if (!(p2.y <= y && y <= p1.y)) {
				return false;
			} else if (!(p1.y <= y && y <= p2.y)) {
				return false;
			}
		}
		if (p3.x >= p4.x) {
			if (!(p4.x <= x && x <= p3.x)) {
				return false;
			} else if (!(p3.x <= x && x <= p4.x)) {
				return false;
			}
		}
		if (p3.y >= p4.y) {
			if (!(p4.y <= y && y <= p3.y)) {
				return false;
			} else if (!(p3.y <= y && y <= p4.y)) {
				return false;
			}
		}
		return true;
	}

	Vector3 RotateOnPivot (Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler (angles) * dir;
		point = dir + pivot;
		return point;
	}
}