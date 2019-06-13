using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveSystem
{
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