#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorStuff : EditorWindow {
	public enum Axis {
		x,
		y, 
		z
	}

	public int amount;
	public float rad;
	public Axis axis;
	public GameObject cloneThis;
	public bool alsoAddspring;

	[MenuItem("Window/Editor Stuff")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		EditorStuff window = (EditorStuff)EditorWindow.GetWindow(typeof(EditorStuff));
		window.Show();
	}

	void OnGUI()
	{
		amount = EditorGUILayout.IntField ("Pie Slices", amount);
		rad = EditorGUILayout.FloatField ("Pie Radius", rad);
		axis = (Axis)EditorGUILayout.EnumPopup ("Axis", axis);
		cloneThis = (GameObject)EditorGUILayout.ObjectField ("Clone This", cloneThis, typeof(GameObject), true);
		alsoAddspring = EditorGUILayout.Toggle ("Also Add Spring One to Next", alsoAddspring);
		if (GUILayout.Button ("Make GameObjects in radial formation")&&Selection.activeGameObject != null) {
			for (int i = 0; i < amount; i++) {
				GameObject g = (GameObject)PrefabUtility.InstantiatePrefab (cloneThis);
				g.name = "OBJ a:" + Mathf.RoundToInt(360f/amount*i);
				g.transform.SetParent (Selection.activeGameObject.transform);
				g.transform.localPosition = angleToVector3 (360/amount*i, axis) * rad;
			}
			if (alsoAddspring) {
				Transform g = Selection.activeGameObject.transform;
				for (int i = 0; i < g.childCount; i++) {
					int next = i + 1;
					if (i >= g.childCount - 1) {
						next = 0;
					}
					SpringJoint2D sp = g.GetChild(i).gameObject.AddComponent <SpringJoint2D>();
					sp.connectedBody = g.GetChild(next).GetComponent<Rigidbody2D> ();
				}
			}
		}
		if (GUILayout.Button ("Add Spring Joint one to next")&& Selection.activeGameObject!= null && Selection.activeGameObject.transform.childCount> 1) {
			Transform g = Selection.activeGameObject.transform;
			for (int i = 0; i < g.childCount; i++) {
				int next = i + 1;
				if (i >= g.childCount - 1) {
					next = 0;
				}
				SpringJoint2D sp = g.GetChild(i).gameObject.AddComponent <SpringJoint2D>();
				sp.connectedBody = g.GetChild(next).GetComponent<Rigidbody2D> ();
			}
		}
	}

	Vector3 angleToVector3(float a, Axis axis){
		Vector3 temp = new Vector3(0, 0, 0);
		if (axis == Axis.x) {
			temp.y = Mathf.Cos (a * Mathf.Deg2Rad);
			temp.z = Mathf.Sin (a * Mathf.Deg2Rad);
		}
		if (axis == Axis.y) {
			temp.z = Mathf.Cos (a * Mathf.Deg2Rad);
			temp.x = Mathf.Sin (a * Mathf.Deg2Rad);
		}
		if (axis == Axis.z) {
			temp.x = Mathf.Cos (a * Mathf.Deg2Rad);
			temp.y = Mathf.Sin (a * Mathf.Deg2Rad);
		}
		return temp;
	}
}
#endif