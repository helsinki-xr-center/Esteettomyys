using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Marks a script as Saveable. 
	 * Saveable scripts can be saved if attached to a 
	 * <see cref="SceneSaveable"/> or <see cref="SpawnedSaveable"/>
	 * </summary>
	 */
	public interface ISaveable
	{
		void Save(SaveWriter save);
		void Load(SaveReader save);
	}

}