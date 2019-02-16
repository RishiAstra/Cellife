using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squshyBall : MonoBehaviour {
//	public float forceMult;

	private Rigidbody2D rig;
	private Vector2 wantPos;
	private float angle;
	public player p;
	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();
		wantPos = new Vector2 (transform.position.x, transform.position.y);
		angle = Mathf.Atan2 (transform.localPosition.y, transform.localPosition.x) * Mathf.Rad2Deg;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
//		float dist = Vector2.Distance (wantPos, pos);
//		rig.AddForce ((wantPos-pos).normalized * dist * forceMult * Time.fixedDeltaTime);
		float pDist = Vector2.Distance(new Vector2(transform.localPosition.x, transform.localPosition.y), Vector2.zero);//distance from parent
		if (pDist > 0.01f) {
			transform.localPosition = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad) * pDist, Mathf.Sin (angle * Mathf.Deg2Rad) * pDist);
		}
	}
	void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
//			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			p.membraneClicked = true;
//			p.memClickPos = new Vector2 (pos.x, pos.y);
//			print ("click  " + pos);
		}
	}
}
