using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestSaveable : MonoBehaviour, ISaveable
{
	public int saved = 2;

	public void Load(SaveReader save)
	{
		saved = save.Read<int>(nameof(saved));
	}

	public void Save(SaveWriter save)
	{
		save.Write(nameof(saved), saved);
	}
}