using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CanEditMultipleObjects]
[CustomEditor(typeof(SceneSaveable))]
public class SceneSaveableEditor : Editor
{

	public VisualTreeAsset uxml;
	public StyleSheet styleSheet;

	private VisualElement listHolder;

	public override VisualElement CreateInspectorGUI()
	{
		if (targets.Length > 1)
		{
			VisualElement uxmlVE = uxml.CloneTree();
			return uxmlVE;
		}
		else
		{
			SceneSaveable target = this.target as SceneSaveable;

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
	}

	private void RedrawList(SceneSaveable target, VisualElement listHolder)
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
					EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
					target.saveables[index] = obj.newValue;
				}
			});

			listHolder.Add(objectField);
		}
	}

	private void AddAllSaveables()
	{
		SceneSaveable target = this.target as SceneSaveable;

		EditorSceneManager.MarkSceneDirty(target.gameObject.scene);

		var all = target.gameObject.GetComponents<ISaveable>()
		.Select(x => x as UnityEngine.Object).ToArray();

		target.saveables = all;

		RedrawList(target, listHolder);

	}

	private void AddOne()
	{
		SceneSaveable target = this.target as SceneSaveable;

		EditorSceneManager.MarkSceneDirty(target.gameObject.scene);

		UnityEngine.Object[] newArr = new UnityEngine.Object[target.saveables.Length + 1];

		Array.Copy(target.saveables, newArr, target.saveables.Length);

		target.saveables = newArr;

		RedrawList(target, listHolder);
	}

	private void RemoveOne()
	{
		SceneSaveable target = this.target as SceneSaveable;

		if (target.saveables.Length == 0) return;

		EditorSceneManager.MarkSceneDirty(target.gameObject.scene);

		UnityEngine.Object[] newArr = new UnityEngine.Object[target.saveables.Length - 1];

		Array.Copy(target.saveables, newArr, newArr.Length);

		target.saveables = newArr;

		RedrawList(target, listHolder);
	}

}
