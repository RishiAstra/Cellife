using UnityEngine;
using System.Collections.Generic;

public class Triangulator1
{
	private List<Vector2> points = new List<Vector2>();

	public Triangulator1 (Vector2[] points) {
		this.points = new List<Vector2>(points);
	}

	public int[] Triangulate() {
		List<int> indices = new List<int>();
        List<Vector2> vert = points;
        List<int> vertIndex = new List<int>();
        for(int j = 0; j < vert.Count; j++)
        {
            vertIndex.Add(j);
        }
		int n = points.Count;
		if (n < 3)
			return indices.ToArray();

        int maxLoops = 25;
        Vector2 v1 = vert[0];// point 1
        Vector2 v2 = vert[1];// point between points 1 and 2
        Vector2 v3 = vert[2];// point 2
        while (vert.Count > 3 && maxLoops > 0)
        {
            for (int i = 0; i < vert.Count; i++)
            {
                if (vert.Count == 3) break;
                int i1 = i + 1;
                if (i1 >= vert.Count) i1 -= vert.Count;
                int i2 = i + 2;
                if (i2 >= vert.Count) i2 -= vert.Count;
                v1 = vert[i];// point 1
                v2 = vert[i1];// point between points 1 and 2
                v3 = vert[i2];// point 2
                Vector2 avg = (v1+v2+v3) / 3;//average
                if (IsPointInPolygon4(vert.ToArray(), avg))//if v1, v2, v3 is convex relative to the polygon
                {
                    //Debug.Log(i1 + ", " + i2 + ", " + i3);
                    //Debug.Log("good");

                    //check order of triangle verticies
                    if (((v2.x - v1.x) * (v3.y - v1.y) - (v2.y - v1.y) * (v3.x - v1.x)) > 0)
                    {
                        indices.Add(vertIndex[i]);//add a triangle
                        indices.Add(vertIndex[i1]);
                        indices.Add(vertIndex[i2]);
                    }
                    else
                    {
                        indices.Add(vertIndex[i1]);//add a triangle
                        indices.Add(vertIndex[i]);
                        indices.Add(vertIndex[i2]);
                    }
                    vert.RemoveAt(i1);
                    vertIndex.RemoveAt(i1);
                }
                //else
                //{
                //    Debug.Log("bad");
                //}
            }
            maxLoops--;
        }
        v1 = vert[0];// point 1
        v2 = vert[1];// point between points 1 and 2
        v3 = vert[2];// point 2
        if (vert.Count == 3) {
            if (((v2.x - v1.x) * (v3.y - v1.y) - (v2.y - v1.y) * (v3.x - v1.x)) > 0)
            {
                indices.Add(vertIndex[0]);//add a triangle
                indices.Add(vertIndex[1]);
                indices.Add(vertIndex[2]);
            }
            else
            {
                indices.Add(vertIndex[1]);//add a triangle
                indices.Add(vertIndex[0]);
                indices.Add(vertIndex[2]);
            }
        }
        if (maxLoops <= 0)
        {
            Debug.LogError("Infinite loop");
        }
		indices.Reverse();
		return indices.ToArray();
	}

	private float Area () {
		int n = points.Count;
		float A = 0.0f;
		for (int p = n - 1, q = 0; q < n; p = q++) {
			Vector2 pval = points[p];
			Vector2 qval = points[q];
			A += pval.x * qval.y - qval.x * pval.y;
		}
		return (A * 0.5f);
	}

	private bool Snip (int u, int v, int w, int n, int[] V) {
		int p;
		Vector2 A = points[V[u]];
		Vector2 B = points[V[v]];
		Vector2 C = points[V[w]];
		if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
			return false;
		for (p = 0; p < n; p++) {
			if ((p == u) || (p == v) || (p == w))
				continue;
			Vector2 P = points[V[p]];
			if (InsideTriangle(A, B, C, P))
				return false;
		}
		return true;
	}

	//public bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
	//{
	//    int polygonLength = polygon.Length, i = 0;
	//    bool inside = false;
	//    // x, y for tested point.
	//    float pointX = point.x, pointY = point.y;
	//    // start / end point for the current polygon segment.
	//    float startX, startY, endX, endY;
	//    Vector2 endPoint = polygon[polygonLength - 1];
	//    endX = endPoint.x;
	//    endY = endPoint.y;
	//    while (i < polygonLength)
	//    {
	//        startX = endX; startY = endY;
	//        endPoint = polygon[i++];
	//        endX = endPoint.x; endY = endPoint.y;
	//        //
	//        inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
	//                  && /* if so, test if it is under the segment */
	//                  ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
	//    }
	//    return inside;
	//}

	public static bool IsPointInPolygon4(Vector2[] polygon, Vector2 testPoint)
	{
		bool result = false;
		int j = polygon.Length - 1;
		for (int i = 0; i < polygon.Length; i++)
		{
			if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y)
			{
				if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x)
				{
					result = !result;
				}
			}
			j = i;
		}
		return result;
	}

	private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
		float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
		float cCROSSap, bCROSScp, aCROSSbp;

		ax = C.x - B.x; ay = C.y - B.y;
		bx = A.x - C.x; by = A.y - C.y;
		cx = B.x - A.x; cy = B.y - A.y;
		apx = P.x - A.x; apy = P.y - A.y;
		bpx = P.x - B.x; bpy = P.y - B.y;
		cpx = P.x - C.x; cpy = P.y - C.y;

		aCROSSbp = ax * bpy - ay * bpx;
		cCROSSap = cx * apy - cy * apx;
		bCROSScp = bx * cpy - by * cpx;

		return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
	}
}