using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class deformer : MonoBehaviour {
	public class DeformPoint{
		public List<float> weight;
		public Transform transform;
		public Vector3 startPos;
		public Vector3 startAngle;

		public DeformPoint(Transform transform, List<float> weight){
			this.transform = transform;
			this.weight = weight;
			this.startPos = transform.localPosition;
			this.startAngle = transform.localEulerAngles;
		}
	}
	public AnimationCurve weightCurve;
	public float range;
	public List<DeformPoint> points = new List<DeformPoint> ();
	public Transform deformerPointHolder;

	public MeshFilter mf;
	public Mesh mesh;
	public bool deformPointsSetRig;//should all deform point's joints have this gameobjects rig assigned as connectid rig?

	private Vector3[] origanal;
	private Rigidbody2D rig;
	private List<int> verticiesToFill = new List<int> ();
	private int[] startTri;
	private List<int> vertexIndex = new List<int> ();
    private Triangulator filler;
	//private Vector3 v{
	//	get { return new Vector3(32, 3, 2); }
	//	set { }
	//}
	//List<GameObject> g = new List<GameObject>();
    // Use this for initialization
    void Awake () {
		mf = GetComponent<MeshFilter> ();
		mesh = Instantiate(mf.sharedMesh);
		rig = GetComponent<Rigidbody2D> ();
        
		origanal = mesh.vertices;
		startTri = mesh.triangles;
		for(int i = 0;i < deformerPointHolder.childCount; i++){
			points.Add (makePoint(deformerPointHolder.GetChild(i)));
		}
		float min = 999f;
		float max = 0f;
		for(int i = 0;i < mesh.vertices.Length; i++){
			if (mesh.vertices [i].magnitude < min) {
				min = mesh.vertices [i].magnitude;
			}
			if (mesh.vertices [i].magnitude > max) {
				max = mesh.vertices [i].magnitude;
			}
		}
		for(int i = 0;i < mesh.vertices.Length; i++){
			if (mesh.vertices [i].magnitude < (min+max)/2) {
				bool can = true;
				for(int j = 0; j < verticiesToFill.Count; j++)
				{
					if(j!=i&&mesh.vertices[i] == mesh.vertices[verticiesToFill[j]])
					{
						can = false;
					}

				}
				if (can) { verticiesToFill.Add(i); }
                
//				vertexIndex.Add (i);
			}
		}
		//verticiesToFill = verticiesToFill.Distinct().ToList();
		verticiesToFill = (verticiesToFill.OrderBy(i => Mathf.Atan2(mesh.vertices[i].y, mesh.vertices[i].x))).ToList();// Vector2.Angle(Vector2.zero, mesh.vertices[i]))).ToList();//-
		verticiesToFill = verticiesToFill.Distinct().ToList();
		List<Vector3> mv = mesh.vertices.ToList();
		for(int i = 0;i < verticiesToFill.Count; i++){
			//g.Add(new GameObject(verticiesToFill[i].ToString()));
			//g[i].transform.position = mesh.vertices[verticiesToFill[i]];
            vertexIndex.Add (mv.IndexOf(mesh.vertices[verticiesToFill[i]]));
		}
        Cam.d.Add(this);
		filler = new Triangulator(new Vector2[0]);
		////print ("length" + verticiesToFill.Count + ", min" + min + ", max" + max + ", start tri" + startTri.Length);

	}

	DeformPoint makePoint(Transform trans){
		List<float> weights = new List<float> ();
		for(int i = 0;i<mesh.vertices.Length;i++){
			float d = Vector3.Distance (trans.localPosition, mesh.vertices [i]);
			if (d < range) {
				weights.Add (weightCurve.Evaluate(1+d-range)/range);
			} else {
				weights.Add (0);
			}
		}
		return new DeformPoint (trans, weights);
	}

	public void UpdateWeights(Vector3[] v)
	{
		t = Time.realtimeSinceStartup;
		Vector3 rot;
		Vector3 pos;
		for (int i = 0; i < v.Length; i++)
		{
			float totalWeight = 0;
			Vector3 total = Vector3.zero;
			for (int j = 0; j < points.Count; j++)
			{
				//first translate, then rotate.
				if (points[j].weight[i] > 0.0001f)
				{
					//					Vector3 pos = points [j].transform.localPosition-points[j].startPos;
					rot = new Vector3(0, 0, Mathf.DeltaAngle(points[j].transform.localEulerAngles.z, points[j].startAngle.z));
					pos = RotatePointAroundPivot(origanal[i], points[j].startPos, rot);
					pos += points[j].transform.localPosition - points[j].startPos;
					total += pos * points[j].weight[i];
					totalWeight += points[j].weight[i];
				}
			}
			//if (totalWeight > 1) {
			total /= totalWeight;
			//}
			v[i] = total;//(total + origanal[i]);
						 //			//print (v[i] + "|" + (origanal[i]+total));
		}
		//print("Traingle Update " + (Time.realtimeSinceStartup - t));
	}

	// Update is called once per frame
	float t;
	public void FixedUpdate () {
		//t = Time.realtimeSinceStartup;
		////print(Time.realtimeSinceStartup - t);
		//for (int i = 0; i < verticiesToFill.Count; i++)
		//{
		//	g[i].transform.position = mesh.vertices[verticiesToFill[i]];
		//}
		Vector3[] v = mesh.vertices;
		List<int> tri = startTri.ToList();
		////print("1 " + (Time.realtimeSinceStartup - t));
		//t = Time.realtimeSinceStartup;
		//Vector3 rot;
		//Vector3 pos;
		UpdateWeights(v);
		////print("2 " + (Time.realtimeSinceStartup - t));
		//t = Time.realtimeSinceStartup;
		TriangleUpdate(v, tri);
		//		t = Time.realtimeSinceStartup;
		////print("3 " + (Time.realtimeSinceStartup - t));
		//t = Time.realtimeSinceStartup;
		
		////print("Time " + (Time.realtimeSinceStartup - t));
		//t = Time.realtimeSinceStartup;
	}

	void TriangleUpdate(Vector3[] v, List<int> tri)
	{
		t = Time.realtimeSinceStartup;
		Vector2[] temp = new Vector2[verticiesToFill.Count];
		for (int i = 0; i < verticiesToFill.Count; i++)
		{
			temp[i] = mesh.vertices[verticiesToFill[i]];
		}
		filler.SetPoints(temp);
		int[] tempTri = filler.Triangulate();
		for (int i = 0; i < tempTri.Length; i++)
		{
			tempTri[i] = vertexIndex[tempTri[i]];
		}
		tri.AddRange(tempTri);
		mesh.vertices = v;
		mesh.triangles = tri.ToArray();//tri.ToArray();
		mesh.RecalculateBounds();
		////print ("temptri" + (tempTri.Length/3) + ", total tri count" + (tri.Count/3) + ", mesh tri" + (mesh.triangles.Length/3) + ", vertex" + temp.Length + ", mesh vertex" + mesh.vertexCount);
		mf.sharedMesh = mesh;
		//print("Traingle Update " + (Time.realtimeSinceStartup - t));
	}

	Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles){
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}
}
