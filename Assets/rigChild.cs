using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigChild : MonoBehaviour {
	public float threshold;
	private Rigidbody2D parent;
	private Vector3 lastPos;
	// Use this for initialization
	void Start () {
		parent = transform.parent.GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//transform.Translate (parent.velocity * Time.fixedDeltaTime);
		if (Vector3.Distance (lastPos, transform.localPosition) < threshold) {
			transform.localPosition = lastPos;
		}
		lastPos = transform.localPosition;
	}
}
