using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(SpawnedSaveable))]
public class SpawnedSaveableEditor : Editor
{
	public VisualTreeAsset uxml;
	public StyleSheet styleSheet;

	private VisualElement listHolder;
	private PrefabStage prefabStage;

	public override VisualElement CreateInspectorGUI()
	{
		prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
		SpawnedSaveable target = this.target as SpawnedSaveable;

		CheckResourcePath(target);

		if (prefabStage == null)
		{
			VisualElement ve = new VisualElement();
			Label label = new Label("Editing only available in prefab mode.");
			ve.Add(label);
			return ve;
		}

		if (target.saveables == null)
		{
			target.saveables = new UnityEngine.Object[0];
		}

		VisualElement uxmlVE = uxml.CloneTree();

		uxmlVE.Add(new Label("Saveable targets")
		{
			style =
			{
				unityFontStyleAndWeight = FontStyle.Bold
			}
		});

		listHolder = new VisualElement();
		RedrawList(target, listHolder);

		VisualElement buttonHolder = new VisualElement()
		{
			style =
			{
				flexWrap = Wrap.Wrap,
				flexDirection = FlexDirection.RowReverse
			}
		};

		Button addAll = new Button(AddAllSaveables)
		{
			style =
			{
				width = 100,
				marginLeft = 5
			},
			text = "Add all"
		};

		Button minus = new Button(RemoveOne)
		{
			style =
			{
				width = 20,
				marginLeft = 5
			},
			text = "-"
		};

		Button plus = new Button(AddOne)
		{
			style =
			{
				width = 20,
				marginLeft = 5

			},
			text = "+"
		};

		buttonHolder.Add(addAll);
		buttonHolder.Add(minus);
		buttonHolder.Add(plus);


		uxmlVE.Add(listHolder);
		uxmlVE.Add(buttonHolder);

		return uxmlVE;
	}

	private void RedrawList(SpawnedSaveable target, VisualElement listHolder)
	{
		listHolder.Clear();
		for (int i = 0; i < target.saveables.Length; i++)
		{
			var objectField = new ObjectField(i.ToString());
			objectField.allowSceneObjects = true;
			objectField.objectType = typeof(ISaveable);
			objectField.value = (target.saveables[i] as UnityEngine.Object);

			var index = i;
			objectField.RegisterValueChangedCallback(
			(obj) =>
			{
				if (obj.newValue is ISaveable)
				{
					EditorSceneManager.MarkSceneDirty(prefabStage.scene);
					target.saveables[index] = obj.newValue;
				}
			});

			listHolder.Add(objectField);
		}
	}

	private void AddAllSaveables()
	{
		SpawnedSaveable target = this.target as SpawnedSaveable;

		EditorSceneManager.MarkSceneDirty(prefabStage.scene);

		var all = target.gameObject.GetComponents<ISaveable>()
		.Select(x => x as UnityEngine.Object).ToArray();

		target.saveables = all;

		RedrawList(target, listHolder);

	}

	private void AddOne()
	{
		SpawnedSaveable target = this.target as SpawnedSaveable;

		EditorSceneManager.MarkSceneDirty(prefabStage.scene);

		UnityEngine.Object[] newArr = new UnityEngine.Object[target.saveables.Length + 1];

		Array.Copy(target.saveables, newArr, target.saveables.Length);

		target.saveables = newArr;

		RedrawList(target, listHolder);
	}

	private void RemoveOne()
	{
		SpawnedSaveable target = this.target as SpawnedSaveable;

		if (target.saveables.Length == 0) return;

		EditorSceneManager.MarkSceneDirty(prefabStage.scene);

		UnityEngine.Object[] newArr = new UnityEngine.Object[target.saveables.Length - 1];

		Array.Copy(target.saveables, newArr, newArr.Length);

		target.saveables = newArr;

		RedrawList(target, listHolder);
	}

	private void CheckResourcePath(SpawnedSaveable target)
	{
		string path = AssetDatabase.GetAssetPath(target);

		if (prefabStage != null)
		{
			path = prefabStage.prefabAssetPath;
		}

		if (!string.IsNullOrEmpty(path))
		{
			int pathSplit = path.LastIndexOf("Resources/");

			if (pathSplit == -1)
			{
				Debug.LogWarning($"Saveable {path} not in resources folder! Please move the prefab to Resources.", target);
				return;
			}

			string rest = path.Substring(pathSplit + "Resources/".Length);
			string withoutEnd = rest.Replace(".prefab", "");

			if (target.resourcePath == withoutEnd)
			{
				return;
			}

			target.resourcePath = withoutEnd;

			if (prefabStage != null)
			{
				EditorSceneManager.MarkSceneDirty(prefabStage.scene);
			}
			else
			{
				EditorUtility.SetDirty(target);
			}
		}
	}
}
