using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Effects: MonoBehaviour {

//	public float intensity;
	public Material material;

	// Creates a private material used to the effect
	void Awake ()
	{
		
	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
//		material.SetFloat("_bwBlend", intensity);
//		Graphics.Blit (source, destination, material);
		Graphics.Blit (source, destination, material);
	}
}