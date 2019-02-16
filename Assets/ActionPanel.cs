using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CloseActionPanel()
	{
		player.main.selLastFrame = true;
		player.main.selected = null;
		gameObject.SetActive(false);
	}
}
