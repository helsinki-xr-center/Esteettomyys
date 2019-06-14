using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveSystem
{
	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Represents saves without the actual save data.
	 * </summary>
	 */
	public class SaveFile
	{
		public string saveName;
		public DateTime timestamp;

		public SaveFile() { }

		public SaveFile(SaveData data)
		{
			saveName = data.saveName;
			timestamp = data.timestamp;
		}
	}
}