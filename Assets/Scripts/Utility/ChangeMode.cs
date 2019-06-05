using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMode : MonoBehaviour
{

	public GameMode mode;

	private void Start()
	{
		switch (mode)
		{
			case GameMode.Tutorial:
				GlobalValues.gameMode = GameMode.Tutorial;
				break;
			case GameMode.Training:
				GlobalValues.gameMode = GameMode.Training;
				break;
			case GameMode.Exam:
				GlobalValues.gameMode = GameMode.Exam;
				break;
			default:
				break;
		}
	}

}
