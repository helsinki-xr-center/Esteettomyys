
using System.Collections.Generic;

namespace SaveSystem
{
	public interface ISaveFileManager
	{
		SaveFile[] GetSaveFiles();
		SaveData Load(SaveFile file);
		SaveFile Save(SaveData data);
		void Delete(SaveFile file);
	}
}