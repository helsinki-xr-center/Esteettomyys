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
		/**
		 * <summary>
		 * Called when the GameObject is being saved. You can write your custom save data to the <see cref="SaveWriter"/>.
		 * </summary>
		 */
		void Save(SaveWriter save);

		/**
		 * <summary>
		 * Called when the GameObject is being loaded. You can read any of your custom data from the <see cref="SaveReader"/>.
		 * </summary>
		 */
		void Load(SaveReader save);
	}

}