
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaveSystem
{
	public interface ISaveFileManager
	{
		SaveFile[] GetSaveFiles();
		SaveData Load(SaveFile file);
		SaveFile Save(SaveData data);
		void Delete(SaveFile file);
	}

	public interface IAsyncSaveFileManager
	{
		Task<SaveFile[]> GetSaveFiles();
		Task<SaveData> Load(SaveFile file);
		Task<SaveFile> Save(SaveData data);
		Task Delete(SaveFile file);
	}
}