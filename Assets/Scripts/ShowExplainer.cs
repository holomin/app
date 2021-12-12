using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowExplainer : MonoBehaviour
{
	[SerializeField] private Animation Explainer;

	public void Animate()
	{
		Explainer.Play();
	}
}
