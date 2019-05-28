using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Controls voice chat with keyboard input. Attached to the same GameObject as Recorder.
 * </summary>
 */
[RequireComponent(typeof(Recorder))]
public class VoiceControls : MonoBehaviour
{
	public Texture voiceEnabled;
	public Texture voiceDisabled;
	public bool showDebugGUI = true;

	Recorder recorder;

	void Start()
	{
		recorder = GetComponent<Recorder>();

		SetSettings(GlobalValues.settings);
	}

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
			GUI.Label(new Rect(10, 10, 200, 30), "Press M to Mute");
			GUI.DrawTexture(new Rect(50, 50, 50, 50), voiceEnabled);
		}
		else
		{
			GUI.Label(new Rect(10, 10, 200, 30), "Press M to Unmute");
			GUI.DrawTexture(new Rect(50, 50, 50, 50), voiceDisabled);
		}

	}

	private void OnEnable()
	{
		Settings.OnSettingsChanged += SetSettings;
	}

	private void OnDisable()
	{
		Settings.OnSettingsChanged -= SetSettings;
	}

	/**
	 * <summary>
	 * Event callback for <see cref="Settings.OnSettingsChanged"/>
	 * </summary>
	 */
	private void SetSettings(Settings settings)
	{
		if (settings.voiceChatEnabled)
		{
			recorder.TransmitEnabled = true;
			recorder.VoiceDetection = true;
		}
	}
}
