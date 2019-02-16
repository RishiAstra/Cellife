using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mySpring : MonoBehaviour {
	public Rigidbody2D connectedBody;
	public float targetDist;
	public float force;
	private Rigidbody2D me;
	private Transform t;
	// Use this for initialization
	void Start () {
		me = GetComponent<Rigidbody2D> ();
		t = connectedBody.transform;
		targetDist = Vector2.Distance(transform.position, t.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float dist = Vector2.Distance (transform.position, t.position);
		float a = Vector2.Angle (transform.position, t.position);
		Vector2 f = Vector2.zero;
		if (dist > targetDist) {
			float mult = dist - targetDist;
			f = new Vector2 (Mathf.Cos(a) * force * mult, Mathf.Sin(a) * force * mult);
		} else {
			float mult = targetDist-dist;
			f = new Vector2 (Mathf.Cos(a) * force * mult, Mathf.Sin(a) * force * mult);
		}
		me.AddForce (f);
		connectedBody.AddForce (-f);
	}
}
