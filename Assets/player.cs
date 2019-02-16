using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct Nutrient {
	public string name;
	public float amount;
	public float pAmount;
	public float maxAmount;
	public Color color;

	public void setAmount(float newAmount)
	{
		pAmount = amount;
		amount = newAmount;
	}

	//public Nutrient(string n, float a, Color c)
	//{
	//	name = n;
	//	amount = a;
	//	color = c;
	//}
}

public class player : MonoBehaviour {
	public static player main;

	public List<Nutrient> nutrients;
	public List<OrganelleActionInfo> ActionInfos;
	public List<Action> Actions = new List<Action>();
	public Organelle selected;
	public GameObject ActionPanel;
	public GameObject NutrientPanel;

	public bool hasWorldTargetPos;
	public Vector2 worldTargetPos;
	public float radius;
	public float membraneThickness;

	public deformPointsHolder ph;
	public GameObject arrow;
	public deformer membrane;
	public float minMoveDist;
	public float membraneSize = 1;
	public Rigidbody2D rig;
	public Vector2 childForce;
	public Transform deformPointsHolder;

	private Vector2 startClick;
	private Vector2 endClick;
	public bool membraneClicked;
	public Vector2 memClickPos;
	private bool lastMemClick;
	public bool selLastFrame;
	public bool mouseClickLastFrame;
	private SpriteRenderer arrowSprite;
	private AngleSlideJoint2D[] joints;

	public void IncreaseNutrient(string name, float amount)
	{
		for (int i = 0; i < nutrients.Count; i++)
		{
			if (nutrients[i].name == name)
			{
				Nutrient n = nutrients[i];
				n.amount += amount;
				nutrients[i] = n;
				return;
			}
		}

	}
	public bool DecreaseNutrient(string name, float amount)
	{
		int index = -1;
		for (int i = 0; i < nutrients.Count; i++)
		{
			if (nutrients[i].name == name)
			{
				index = i;
				break;
			}
		}
		if (index == -1)
		{
			Debug.LogError("Error nutrient not existant");
			return false;
		}
		if (nutrients[index].amount >= amount)
		{
			Nutrient n = nutrients[index];
			n.amount -= amount;
			nutrients[index] = n;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Move()
	{
		selected.Move();
		selLastFrame = true;
	}
	public void ToggleActive()
	{
		selected.ToggleActive();
		selLastFrame = true;
	}

	private void StartNutrients()
	{
		NutrientPanel = GameObject.Find("Nutrient Panel");
		for (int i = 0; i < NutrientPanel.transform.childCount; i++)
		{
			Transform temp = NutrientPanel.transform.GetChild(i);
			if (i < nutrients.Count)
			{
				temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = nutrients[i].name;
				temp.GetChild(2).GetComponent<Image>().color = nutrients[i].color;
				RectTransform rt = temp.GetChild(2).GetComponent<RectTransform>();
				rt.sizeDelta = new Vector2(80 * Mathf.Clamp01(nutrients[i].amount / nutrients[i].maxAmount), rt.sizeDelta.y);
				temp.GetChild(3).GetComponent<TextMeshProUGUI>().text = NumToString(nutrients[i].amount);
			}
			else
			{
				temp.gameObject.SetActive(false);
			}
		}
	}

	private void UpdateNutrients()
	{
		for (int i = 0; i < nutrients.Count; i++)
		{
			Transform temp = NutrientPanel.transform.GetChild(i);
			RectTransform rt = temp.GetChild(2).GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(80 * Mathf.Clamp01(nutrients[i].amount / nutrients[i].maxAmount), rt.sizeDelta.y);
			temp.GetChild(3).GetComponent<TextMeshProUGUI>().text = NumToString(nutrients[i].amount);
		}
	}

	// Use this for initialization
	void Start () {
		StartNutrients();

		ActionPanel = GameObject.Find("Action Panel");
		ActionPanel.SetActive(false);
		Actions.Add(Move);
		Actions.Add(ToggleActive);

		main = this;
		arrowSprite = arrow.GetComponent<SpriteRenderer> ();
//		membrane.p = this;
		rig = GetComponent<Rigidbody2D>();
		joints = deformPointsHolder.GetComponentsInChildren<AngleSlideJoint2D> ();
		foreach(AngleSlideJoint2D j in joints)
		{
			j.dist = radius;
		}

	}

	void FixedUpdate(){
//		if (childForce.magnitude < 0.001f) {
//			return;
//		}
//		Vector2 v = childForce * Time.fixedDeltaTime / rig.mass;
//		transform.Translate(v.x, v.y, 0);
//		childForce = Vector2.zero;
	}

	public void SetSelLastFrame()
	{
		selLastFrame = true;
	}

	// Update is called once per frame
	void Update () {
		UpdateNutrients();
		float d = Vector2.Distance (startClick, endClick);
		bool showArrow = d > minMoveDist && membraneClicked;
		float a = Mathf.Atan2(endClick.y-startClick.y, endClick.x - startClick.x) * Mathf.Rad2Deg;
		if (hasWorldTargetPos && Vector2.Distance(ph.cell.position, worldTargetPos) < 0.05f)
		{
			hasWorldTargetPos = false;
		}
		if (showArrow) {
			if(Input.GetMouseButtonUp(0)){
				print ("Moving cell");
				//Vector2 pos = new Vector2 (endClick.x, endClick.y);
				worldTargetPos = endClick;
				hasWorldTargetPos = true;
				GameObject g = new GameObject ("test");
				g.transform.position = new Vector3 (endClick.x, endClick.y, 0);
				foreach(Transform t in ph.joints)
				{
					AngleSlideJoint2D aa = t.GetComponent<AngleSlideJoint2D>();
					if (aa == null) continue;
					Vector2 tt = -new Vector2(Mathf.Cos(aa.angle * Mathf.Deg2Rad) * aa.dist, Mathf.Sin(aa.angle * Mathf.Deg2Rad) * aa.dist);
					Vector2 t1 = worldTargetPos;// new Vector2 (0, 0);
					Vector2 distOff = (t1 - tt) - (Vector2)aa.center.position;// aa.transform.position;
					
					//aa.moveAngle = Mathf.Abs(Mathf.DeltaAngle(aa.angle, Mathf.Atan2(distOff.y, distOff.x) * Mathf.Rad2Deg));
				}
			}
			//make arrow point right direction

			//			arrow.transform.rotation = Quaternion.Euler (0, 0, a);
			arrow.transform.position = new Vector3 (endClick.x, endClick.y, 0);
			arrow.transform.rotation = Quaternion.Euler (0, 0, a);
			arrowSprite.size = new Vector2 (d, arrowSprite.size.y);

		}
		if (membraneClicked) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			memClickPos = new Vector2 (pos.x, pos.y);
			if (!lastMemClick) {
				startClick = memClickPos;
			} else {
				endClick = memClickPos;
			}
		} else if (lastMemClick) {
//			movePos (endClick-startClick);
		}
		if (Input.GetMouseButtonUp (0)) {
			membraneClicked = false;
//			movePos (endClick);
		}


		arrow.SetActive (showArrow);
		lastMemClick = membraneClicked;
		
	}

	private void LateUpdate()
	{
		mouseClickLastFrame = Input.GetMouseButtonUp(0);

		if (mouseClickLastFrame && !selLastFrame)
		{
			selected = null;
			ActionPanel.SetActive(false);
		}
		selLastFrame = false;
	}

	//	void movePos(Vector2 pos){
	//		foreach (deformer.deformPoint d in membrane.points) {
	//			d.transform.GetComponent<AngleSlideJoint2D> ().move(pos);
	//		}
	//	}

	public string NumToString(float i)
	{
		if(i < 100000)
		{
			return i.ToString();
		}else if(i < 100000000)
		{
			return (((int)(i / 1000)).ToString() + "K");
		}
		else
		{
			return (((int)(i / 1000000)).ToString() + "M");
		}
	}
}
