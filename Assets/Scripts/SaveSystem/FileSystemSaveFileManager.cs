using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace SaveSystem
{
	public class FileSystemSaveFileManager : ISaveFileManager
	{
		public static FileSystemSaveFileManager Default { get; set; } = new FileSystemSaveFileManager(Application.persistentDataPath + "/Saves/", true);

		private const string extension = ".sav";
		private string saveFilePath;
		private bool compress;

		public FileSystemSaveFileManager(string filePath, bool compress = true)
		{
			saveFilePath = filePath;
			this.compress = compress;
		}
		public void Delete(SaveFile file)
		{
			string path = GetPath(file.saveName);
			File.Delete(path);
		}

		public SaveFile[] GetSaveFiles()
		{
			CheckDirectory();

			var files = Directory.EnumerateFiles(saveFilePath, "*.sav", SearchOption.TopDirectoryOnly);

			foreach (var s in files) Debug.Log(s);

			return files.Select(x => new SaveFile() { saveName = NameFromPath(x), timestamp = File.GetCreationTime(x) }).ToArray();
		}

		public SaveData Load(SaveFile file)
		{
			string path = GetPath(file.saveName);

			using(FileStream stream = File.OpenRead(path))
			{
				if(compress)
				{
					return SaveData.FromStreamCompressed(stream);
				}
				else
				{
					return SaveData.FromStream(stream);
				}
			}
		}

		public SaveFile Save(SaveData data)
		{
			string path = GetPath(data.saveName);

			CheckDirectory();

			using (FileStream stream = File.Create(path))
			{
				if (compress)
				{
					data.WriteToStreamCompressed(stream);
				}
				else
				{
					data.WriteToStream(stream);
				}
			}

			File.SetCreationTime(path, data.timestamp);
			Debug.Log("Saved! " + path);

			return new SaveFile() { saveName = data.saveName, timestamp = data.timestamp };
		}

		private string GetPath(string saveName)
		{
			saveName = saveName.Replace(' ', '_');
			foreach (var c in Path.GetInvalidFileNameChars())
			{
				saveName = saveName.Replace(c, '-');
			}
			return saveFilePath + saveName + extension;
		}

		private string NameFromPath(string path)
		{
			return path.Replace(saveFilePath, "").Replace(extension, "");
		}

		private void CheckDirectory()
		{
			if (!Directory.Exists(saveFilePath))
			{
				Directory.CreateDirectory(saveFilePath);

			}
		}
	}

}