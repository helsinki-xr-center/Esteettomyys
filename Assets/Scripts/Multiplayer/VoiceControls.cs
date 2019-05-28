using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Recorder))]
public class VoiceControls : MonoBehaviour
{
	public Texture voiceEnabled;
	public Texture voiceDisabled;
	public bool showDebugGUI = true;

	Recorder recorder;


	// Start is called before the first frame update
	void Start()
	{
		recorder = GetComponent<Recorder>();
		recorder.TransmitEnabled = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			recorder.IsRecording = !recorder.IsRecording;
		}
	}


	private void OnGUI()
	{
		if (!showDebugGUI)
		{
			return;
		}
		if (recorder.IsRecording)
		{
			GUI.DrawTexture(new Rect(50, 50, 50, 50), voiceEnabled);
		}
		else
		{
			GUI.DrawTexture(new Rect(50, 50, 50, 50), voiceDisabled);
		}

	}
}
