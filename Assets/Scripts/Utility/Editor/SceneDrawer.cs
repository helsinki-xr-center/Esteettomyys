using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneAttribute))]
public class SceneDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

		if (property.propertyType == SerializedPropertyType.String) {
			var sceneObject = GetSceneObject(property.stringValue);
			var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
			if (scene == null) {
				property.stringValue = "";
			} else if (scene.name != property.stringValue) {
				var sceneObj = GetSceneObject(scene.name);
				if (sceneObj == null) {
					Debug.Log("The scene " + scene.name + " is not in build settings. Adding it now.");
					AddSceneToEditorBuildSettings(scene as SceneAsset);
					property.stringValue = scene.name;
				} else {
					property.stringValue = scene.name;
				}
			}
		} else
			EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
	}
	protected SceneAsset GetSceneObject (string sceneObjectName) {
		if (string.IsNullOrEmpty(sceneObjectName)) {
			return null;
		}

		foreach (var editorScene in EditorBuildSettings.scenes) {
			if (editorScene.path.IndexOf(sceneObjectName) != -1) {
				return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
			}
		}

		return null;
	}

	protected void AddSceneToEditorBuildSettings(SceneAsset scene)
	{
		string scenePath = AssetDatabase.GetAssetPath(scene);
		if (!string.IsNullOrEmpty(scenePath))
		{
			EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(scenePath, true));
		}
		else
		{
			Debug.LogWarning($"Scene {scene} does not exist in assets.");
		}
	}
}