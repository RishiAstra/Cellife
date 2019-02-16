using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deformPointsHolder : MonoBehaviour {
	public int amountHit;
	public int totalAmount;

	public Transform cell;
	public Transform center;
	public player p;

	public Transform[] joints;
	private float[] leftAngles;
	private float[] rightAngles;
	// Use this for initialization
	void Start () {
		joints = new Transform[transform.childCount-1];
		for (int i = 1; i < transform.childCount; i++) {
			joints [i-1] = transform.GetChild (i);
			totalAmount++;
		}
		leftAngles = new float[transform.childCount];
		rightAngles = new float[transform.childCount];
		for(int i = 0;i<joints.Length;i++) {
			int left = i>0? i - 1:joints.Length-2;
			int right = i < joints.Length-2 ? i + 1 : 0;
			AngleSlideJoint2D j = joints [i].GetComponent<AngleSlideJoint2D> ();
			if (j != null) {
				j.left = joints [left];
				j.right = joints [right];
				j.SetAngles ();
				j.centerSpring.distance = player.main.radius;
//				j.leftAngle = Vector2.Angle (joints [i].transform.position, joints [left].transform.position);
//				j.rightAngle = Vector2.Angle (joints [i].transform.position, joints [right].transform.position);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = Vector3.zero;
		for(int i = 0;i<joints.Length;i++) {
			pos += joints[i].position;
//			int left = i>0? i - 1:joints.Length-1;
//			int right = i < joints.Length-1 ? i + 1 : 0;
//
//			float rot = (
//				leftAngles[i] - Vector2.Angle (joints [i].transform.position, joints [left].transform.position) + 
//				rightAngles[i] - Vector2.Angle (joints [i].transform.position, joints [right].transform.position)
//			)/2;
//			
//			joints [i].eulerAngles = new Vector3 (0, 0, rot);
		}
		pos /= joints.Length;
		//cell.position = pos;
		center.position = pos;

	}
}
