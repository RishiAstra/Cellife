using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triFiller : MonoBehaviour {

	/// <summary>
	/// Triangulates the gives verticies.
	/// </summary>
	/// <param name="verticies">The verticies to fill in.
	/// </param>
	/// <param name="triIndexOffset">The offset for the tri array of indexes. 
	/// Useful when there are more verticies than just the verticie array given.
	/// </param>
	/// <returns>Array for the triangles</returns>
	/// <param name="verticies">Verticies.</param>
	/// <param name="triIndexOffset">Tri index offset.</param>
	public List<int> fillMesh(List<Vector3> verticies, int triIndexOffset){
		if (verticies.Count <= 2) {
			print ("not enough verticies" + verticies.Count);
//			return null;
		}
		List<Vector3> v = verticies;
		List<int> tri = new List<int> ();
		int cycles = 0;
		while (v.Count > 0&&cycles < v.Count*2) {
			List<int> removeThese = new List<int> ();
			int printThis = 0;
			for (int i = 0; i < v.Count; i++) {
				if (v.Count > 3) {
					int t1 = i;//counter clockwise
					int t2 = i + 1 < v.Count ? i : 0;//the one it is checking
					int t3 = i + 2 < v.Count ? i : 1;//clockwise
					float t1t3Angle = Vector2.Angle (v [t1], v [t3]);
					float t1t2Angle = Vector2.Angle (v [t1], v [t2]);
					if (Mathf.DeltaAngle(t1t3Angle, t1t2Angle) > 0) {
						tri.Add (t1 + triIndexOffset);
						tri.Add (t2 + triIndexOffset);
						tri.Add (t3 + triIndexOffset);
						v.RemoveAt (t2);
						i+=2;
					} else {
						printThis++;
					}
				} else {
					tri.Add (0 + triIndexOffset);
					tri.Add (1 + triIndexOffset);
					tri.Add (2 + triIndexOffset);
					v = new List<Vector3> ();
					break;
				}
			}
			print (printThis + "times concave found");
			
			cycles++;
		}
		if (cycles >= v.Count * 2) {
			print ("error");
		}
		print ("finished with " + tri.Count);
		return tri;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
