using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    public static List<deformer> d = new List<deformer>();
    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    void OnPreRender()
    {
        foreach(deformer i in d)
        {
            //i.MeshUpdate();
        }
    }
}
