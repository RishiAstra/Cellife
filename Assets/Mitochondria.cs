using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitochondria : MonoBehaviour {
	public int minATPPerGlucose, maxATPPerGlucose;

	public void ConvertGluecoseToATP(int amount)
	{
		if (player.main.DecreaseNutrient("G", amount))
		{
			player.main.IncreaseNutrient("ATP", amount * Random.Range(minATPPerGlucose, maxATPPerGlucose));

		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
