using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("Custom/Angle Slide Joint 2D", 0)]
public class AngleSlideJoint2D : MonoBehaviour
{
	public deformPointsHolder h;
	//public static int amountHit;//the amount of things in the right position (reached destination)
	//	public Rigidbody2D connectedBody;
	public float angle;//target angle
	public float dist;//target distance
	public float force;//force to get target distance
	public float forceDist;//dist that max force will be used
	public float moveDist;//dist from move target that max force will be used
	public float moveForce;//max force to use to get world target pos
	public float allMoveDist;
	public Transform center;
	public Transform right;//clockwise from this
	public Transform left;//

	public float rightAngle;
	public float leftAngle;
	public float rightAngleOff;
	public float leftAngleOff;
	//private bool hasWorldTargetPos;
	public Vector2 worldTargetPos;
	private Rigidbody2D attachedBody;
	public player p;
	public Vector2 t;
	public Vector2 distOff;

	public bool moving;
	public float moveAngle;
	public float mm;
	public float a;
	public float mult;
	public SpringJoint2D centerSpring;

	private bool hit;
	private Vector2 distOffWorld;
	// Use this for initialization
	void Start()
	{
		angle = Mathf.Atan2(-GetLocalPosition().y, -GetLocalPosition().x) * Mathf.Rad2Deg;//Vector2.Angle (Vector2.zero, getLocalPosition());
		dist = p.membraneSize;
		//GetComponent<SpringJoint2D>().distance = Vector2.Distance(transform.position, right.position);
		attachedBody = GetComponent<Rigidbody2D>();
	}

	public void SetAngles()
	{
		leftAngle = Mathf.DeltaAngle(Vector2.Angle(transform.position, left.transform.position), 0);
		rightAngle = Mathf.DeltaAngle(Vector2.Angle(transform.position, right.transform.position), 0);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Move();
		//t = -new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * dist, Mathf.Sin(angle * Mathf.Deg2Rad) * dist);
		//Vector2 t1 = h.p.worldTargetPos;// new Vector2 (0, 0);
		//float mult = force;
		////distOff = getLocalPosition()-t;
		//distOff = (t1 - t) - (Vector2)transform.position;
		//float a = Mathf.Atan2 (distOff.y, distOff.x);// * Mathf.Rad2Deg;
		//if (distOff.magnitude < forceDist) {
		//	mult *= distOff.magnitude / forceDist;
		//}
		////if (!h.p.hasWorldTargetPos)
		////{
		//	attachedBody.AddForce(new Vector2(Mathf.Cos(a) * mult, Mathf.Sin(a) * mult));
		////}
		//mult = moveForce;//go to world pos
		//distOff = (t1-t)-(Vector2)transform.position;
		////hit target
		//if (distOff.magnitude < 0.05f && !hit) 
		//{
		//	hit = true;
		//	h.amountHit++;
		//}
		//if(distOff.magnitude >= 0.05f && hit)
		//{
		//	hit = false;
		//	h.amountHit--;
		//}

		//a = Mathf.Atan2 (distOff.y, distOff.x);// * Mathf.Rad2Deg;
		//if (distOff.magnitude < moveDist) {
		//	mult *= distOff.magnitude / moveDist;
		//}
		//moving = (Mathf.Abs(Mathf.DeltaAngle(angle, a)) < 30 || h.amountHit > 2);//h.p.hasWorldTargetPos && (Mathf.Abs(Mathf.DeltaAngle(angle, a)) < 30||amountHit > 2)
		//if (moving)
		//{
		//	attachedBody.AddForce(new Vector2(Mathf.Cos(a) * mult, Mathf.Sin(a) * mult));
		//}

		float rightA = Mathf.DeltaAngle(Vector2.Angle(transform.position, right.transform.position), 0);
		float leftA = Mathf.DeltaAngle(Vector2.Angle(transform.position, left.transform.position), 0);
		float rot = (
			leftAngle - leftA +
			rightAngle - rightA
		) / 2;
		leftAngleOff = leftAngle - leftA;
		rightAngleOff = rightAngle - rightA;
		transform.eulerAngles = new Vector3(0, 0, rot);


		float l = Vector2.Angle(transform.position, left.position);
		float r = Vector2.Angle(transform.position, right.position);
		float m = Vector2.Angle(left.position, right.position);
	}

	void Move()
	{
		

		moving = h.p.hasWorldTargetPos &&( Mathf.Abs(Mathf.DeltaAngle(a, angle)) < 30 || h.amountHit > 2 || distOffWorld.magnitude < allMoveDist);
		//bool worldTarget = (moving);

		t = -new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * dist, Mathf.Sin(angle * Mathf.Deg2Rad) * dist);
		Vector2 t1 = h.p.hasWorldTargetPos ? h.p.worldTargetPos : (Vector2)h.cell.position;// new Vector2 (0, 0);

		//print(worldTarget);

		mult = moving ? moveForce : force;
		distOff = (t1 - t) - (Vector2)transform.position;
		distOffWorld = distOff;
		a = Mathf.Atan2(distOff.y, distOff.x) * Mathf.Rad2Deg;

		if (h.p.hasWorldTargetPos)
		{
			
			

			//h.p.hasWorldTargetPos && (Mathf.Abs(Mathf.DeltaAngle(angle, a)) < 30||amountHit > 2)
			//if (h.p.hasWorldTargetPos)
			//{
			if (moving)
			{//h.amountHit < h.totalAmount)

				mult = moveForce;
				if (distOff.magnitude < 0.05f && !hit)
				{
					hit = true;
					h.amountHit++;
				}
				if (distOff.magnitude >= 0.05f && hit)
				{
					hit = false;
					h.amountHit--;
				}
			}
			else
			{
				//mult = 0;
				t1 = (Vector2)h.center.position;// new Vector2 (0, 0);
				mult = force;
				distOff = (t1 - t) - (Vector2)transform.position;
			}


			//}
		}


		//if (h.amountHit == h.totalAmount || !moving)
		//{
		//	distOff = GetLocalPosition() - t;
		//}


		
		if (distOff.magnitude < moveDist)
		{
			mult *= distOff.magnitude / moveDist;
		}
		attachedBody.AddForce(new Vector2(Mathf.Cos(a * Mathf.Deg2Rad) * mult, Mathf.Sin(a * Mathf.Deg2Rad) * mult));

	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButton(0))
		{
			p.membraneClicked = true;
		}
	}

	Vector2 GetLocalPosition()
	{
		return (Vector2)(center.transform.position - transform.position);
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.gray;
	//	Gizmos.DrawSphere(transform.position + (Vector3)distOff, 0.1f);
	//	Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(a * Mathf.Deg2Rad) * mult, Mathf.Sin(a * Mathf.Deg2Rad) * mult, 0));
	//	//Gizmos.color = Color.black;
	//	//Gizmos.DrawSphere(h.center.position + (Vector3)t, 0.1f);
	//}
}
