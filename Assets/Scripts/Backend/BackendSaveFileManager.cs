using Newtonsoft.Json.Linq;
using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/**
* Author: Nomi Lakkala
* 
* <summary>
* A save file manager for saving into the backend service. Coupled closely with <see cref="BackendConnector"/>.
* </summary>
*/
public class BackendSaveFileManager : IAsyncSaveFileManager
{
	public static BackendSaveFileManager instance = new BackendSaveFileManager();
	public SaveData cachedSave = null;

	public Task Delete(SaveFile file)
	{
		throw new System.NotImplementedException();
	}

	public async Task<SaveFile[]> GetSaveFiles()
	{
		if (!BackendConnector.authenticated)
		{
			return Array.Empty<SaveFile>();
		}

		BackendSaveModel model = await BackendConnector.LoadSaveData();

		cachedSave = SaveData.FromStringCompressed(model.data);

		return new SaveFile[] { new SaveFile(cachedSave) };
	}

	public async Task<SaveData> Load(SaveFile file)
	{
		await Task.CompletedTask;
		return cachedSave;
	}

	public async Task<SaveFile> Save(SaveData data)
	{
		if (!BackendConnector.authenticated)
		{
			Debug.LogWarning("Can not save to server while not logged in.");
			return null;
		}

		BackendSaveModel model = new BackendSaveModel()
		{
			saveName = data.saveName,
			timeStamp = data.timestamp,
			data = data.AsStringCompressed()
		};

		_ = BackendConnector.SendSaveData(model);

		cachedSave = data;

		return new SaveFile(data);
	}

}
