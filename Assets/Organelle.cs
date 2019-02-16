using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct OrganelleActionInfo
{
	public Sprite icon;
}
public delegate void Action();
public enum OraganelleType { Nucleus};
public class Organelle : MonoBehaviour {
	public static GameObject moveGameObject;

	public float ATPConsumation;
	public string Name;
	public Sprite img;
	public List<OrganelleActionInfo> ActionInfos = new List<OrganelleActionInfo>();
	public List<Action> Actions = new List<Action>();
	public bool[] AvailableActions;
	public float moveForce;//TODO moving almost working, fix
	public float moveDist;//the distance to apply max force to move;distances smaller than this will have less force
	public float moveThreshold;
	//public GameObject moveGameObject;

	
	public Vector2 moveTarget;
	public bool AutoMove;
	public bool ChoosingMoveLocation;
	public bool moving;
	public float radius;

	private Rigidbody2D rig;
	private float angle;
	private float mult;
	private bool canMove;
	public bool active;
	// Use this for initialization
	void Start()
	{
		rig = GetComponent<Rigidbody2D>();
		//Actions.Add(Move);
		for(int i = 0; i < AvailableActions.Length; i++)
		{
			if (AvailableActions[i])
			{
				//print(i);
				//print(ActionInfos);
				//Debug.LogAssertion(player.main.ActionInfos==null);
				ActionInfos.Add(player.main.ActionInfos[i]);
				Actions.Add(player.main.Actions[i]);
			}
		}
		if (moveGameObject == null) {
			moveGameObject = GameObject.Find("MoveTarget");
			moveGameObject.SetActive(false);
		}
	}

	

	// Update is called once per frame
	void Update() {
		if (player.main.selected != this){
			ChoosingMoveLocation = false;
			moveGameObject.SetActive(false);
		}else if (ChoosingMoveLocation)
		{
			moveGameObject.SetActive(true);
		}
		float dist = Vector2.Distance(transform.localPosition, moveTarget);
		if (dist > moveThreshold && (moving || AutoMove))
		{
			mult = moveForce;
			if (dist < moveDist)
			{
				mult *= dist / moveDist;
			}
			if (player.main.DecreaseNutrient("ATP", ATPConsumation * mult * Time.deltaTime)) {
				//float off = transform.localPosition - moveTarget;
				angle = Mathf.Atan2(moveTarget.y - transform.localPosition.y, moveTarget.x - transform.localPosition.x) * Mathf.Rad2Deg;// Vector2.Angle(transform.localPosition, moveTarget);
				rig.AddForce(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * mult, Mathf.Sin(angle * Mathf.Deg2Rad) * mult));
			}
		}
		else
		{
			moving = false;
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Actions[0]();
		}
		if (ChoosingMoveLocation)
		{
						
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			moveGameObject.transform.position = pos;
			pos -= (Vector2)transform.parent.position;
			if (Input.GetMouseButtonUp(0))
			{
				if (canMove)
				{
					
					if (pos.magnitude > ((player.main.radius + player.main.membraneThickness) - radius))
					{
						pos *= (((player.main.radius + player.main.membraneThickness) - radius) / pos.magnitude);
					}
					Collider2D[] hit = Physics2D.OverlapCircleAll(pos, radius);
					print(hit.Length);
					if (hit.Length == 0 || (hit.Length == 1 && hit[0] == GetComponent<Collider2D>()))
					{
						moving = true;
						moveTarget = pos;
						moveGameObject.SetActive(false);
						ChoosingMoveLocation = false;
						canMove = false;
						player.main.selLastFrame = true;
					}
				}
				else
				{
					canMove = true;
				}
				
			}
			
		}
		//if (moving||AutoMove)
		//{
		//	if (Input.GetMouseButtonUp(0))
		//	{
				
		//	}
		//}
		
	}
	public void Move()
	{
		ChoosingMoveLocation = true;
		moveGameObject.SetActive(true);
	}
	public void ToggleActive()
	{
		active = !active;
		Animator anim = GetComponent<Animator>();
		if(anim != null)
		{
			anim.SetBool("Active", active);
		}
	}
	private void OnDrawGizmos()
	{
		//Gizmos.color = Color.gray;
		Gizmos.DrawSphere(moveTarget, 0.1f);
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * mult, Mathf.Sin(angle * Mathf.Deg2Rad) * mult));
	}

	public void OnMouseOver()
	{
		if (!Input.GetMouseButtonUp(0)) return;
		GameObject g = player.main.ActionPanel;
		player.main.selLastFrame = true;
		if (player.main.selected == this)
		{
			player.main.selected = null;
			g.SetActive(false);
			return;
		}
		else
		{
			g.SetActive(true);
			
			player.main.selected = this;
			g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Name;
			g.transform.GetChild(1).GetComponent<Image>().sprite = img;
		}
		
		Transform t = GameObject.Find("Action Button Holder").transform;
		for (int i = 0;i<t.childCount;i++)
		{
			if(i < ActionInfos.Count)
			{
				t.GetChild(i).gameObject.SetActive(true);
				t.GetChild(i).GetComponent<ActionButton>().SetAction(Actions[i], ActionInfos[i]);
			}
			else
			{
				t.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
