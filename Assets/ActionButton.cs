using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class ActionButton : MonoBehaviour
{
	public Action OnClick;
	public Texture2D tex;
	public float AlphaThreshold = 0.1f;

	private Button b;
	private Image img;

	void Start()
	{
		img = GetComponent<Image>();
		b = GetComponent<Button>();
		img.alphaHitTestMinimumThreshold = AlphaThreshold;
	}

	public void SetAction(Action a, OrganelleActionInfo info)
	{
		OnClick = a;
		b.onClick.RemoveAllListeners();
		b.onClick.AddListener(() => OnClick());
		img.sprite = info.icon;
	}
}