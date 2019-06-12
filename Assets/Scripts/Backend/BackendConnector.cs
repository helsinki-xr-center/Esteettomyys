using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using System;

public static class BackendConnector
{
	public const string backendUrl = "http://localhost:5000/api/";
	public static HttpClient client = new HttpClient();
	public static bool authenticated = false;
	public static string apikey = null;


	public static Task Logout()
	{
		authenticated = false;
		apikey = null;
		client.DefaultRequestHeaders.Clear();
		return Task.CompletedTask;
	}

	public static async Task<LoginResult> Login(Credentials credentials)
	{
		try
		{
			var response = await client.PostAsync(backendUrl + "login", GetContentFrom(credentials));
			if (response.IsSuccessStatusCode)
			{
				apikey = await ReadContent<string>(response.Content);
				authenticated = true;
				client.DefaultRequestHeaders.Clear();

				client.DefaultRequestHeaders.Add("x-api-key", apikey);

				return LoginResult.Success;
			}
			else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				return LoginResult.IncorrectCredentials;
			}
			else
			{
				Debug.Log(response.StatusCode + " " + response.ReasonPhrase);
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning(e);
		}
		Debug.Log("Why?");
		return LoginResult.Error;
	}

	public static async Task SendSaveData(BackendSaveModel model)
	{
		try
		{
			var response = await client.PostAsync(backendUrl + "saves", GetContentFrom(model));
			if (response.IsSuccessStatusCode)
			{
				Debug.Log("Success sending data to server.");
			}
			else
			{
				Debug.LogWarning(response.StatusCode + " " + response.ReasonPhrase);
			}
			
		}
		catch (Exception e)
		{
			Debug.LogWarning(e);
		}
	}

	public static async Task<BackendSaveModel> LoadSaveData()
	{
		try
		{
			var response = await client.GetAsync(backendUrl + "saves");
			if (response.IsSuccessStatusCode)
			{
				BackendSaveModel model = await ReadContent<BackendSaveModel>(response.Content);
				return model;
			}
			else
			{
				Debug.LogWarning(response.StatusCode + " " + response.ReasonPhrase);
			}

		}
		catch (Exception e)
		{
			Debug.LogWarning(e);
		}

		return null;
	}



	private static StringContent GetContentFrom(object o)
	{
		return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
	}

	private static async Task<T> ReadContent<T>(HttpContent content)
	{
		return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
	}
}
