using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
	public interface ISaveable
	{
		void Save(SaveWriter save);
		void Load(SaveReader save);
	}

}