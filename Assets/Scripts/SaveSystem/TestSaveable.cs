using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestSaveable : MonoBehaviour, ISaveable
{
	public Color saved = Color.blue;

	private Material material;


	public void Start()
	{
		Renderer rend = GetComponent<Renderer>();
		rend.material = new Material(rend.material);
		material = rend.material;
		material.color = saved;
	}

	private void OnMouseDown()
	{
		saved = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
		material.color = saved;
	}

	public void Load(SaveReader save)
	{
		saved = save.Read<Color>(nameof(saved));
		material.color = saved;
	}

	public void Save(SaveWriter save)
	{
		save.Write(nameof(saved), saved);
	}
}