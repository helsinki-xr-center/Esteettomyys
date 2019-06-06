using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Usable static methods
/// </summary>
public static class ExtensionMethods
{

	/// <summary>
	/// Resets Color Back to the Original color
	/// </summary>
	/// <param name="obj">Object that color needs to be reset</param>
	/// <param name="original">Original color of the object</param>
	public static void MaterialResetColorChange(GameObject obj, Color original)
	{

		MeshRenderer[] meshRenderers = obj.GetComponents<MeshRenderer>();

		foreach (MeshRenderer mr in meshRenderers)
		{
			mr.material.color = original;
		}
	}

	/// <summary>
	/// Changes MeshRenderers Material Color;
	/// </summary>
	/// <param name="obj">Object that needs to have mesh material color Changed</param>
	/// <param name="newColor">Color to change to</param>
	public static void MaterialColorChange(GameObject obj, Color newColor)
	{

		MeshRenderer[] meshRenderers = obj.GetComponents<MeshRenderer>();

		foreach (MeshRenderer mr in meshRenderers)
		{
			mr.material.color = newColor;
		}
	}

	public static Transform[] GetChildrenRecursive(this Transform transform)
	{
		List<Transform> transforms = new List<Transform>();
		Queue<Transform> toTraverse = new Queue<Transform>();

		toTraverse.Enqueue(transform);

		while(toTraverse.Count > 0)
		{
			Transform current = toTraverse.Dequeue();
			foreach(Transform child in current)
			{
				toTraverse.Enqueue(child);
				transforms.Add(child);
			}
		}
		return transforms.ToArray();
	}

	public static IEnumerable<Transform> EnumerateChildrenRecursive(this Transform transform)
	{
		foreach(Transform child in transform)
		{
			yield return child;
			foreach (var cc in child.EnumerateChildrenRecursive()) yield return cc;
		}
	}


	public static T MinBy<T>(this IEnumerable<T> enumerable, Func<T, float> predicate)
	{
		T closest = default;
		float closestDistance = float.MaxValue;

		foreach (var item in enumerable)
		{
			float newDist = predicate.Invoke(item);
			if(newDist < closestDistance)
			{
				closest = item;
				closestDistance = newDist;
			}
		}

		return closest;
	}

	public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
	{
		int i = -1;
		foreach(var t in enumerable)
		{
			i++;
			if(predicate.Invoke(t))
			{
				return i;
			}
		}
		return i;
	}

	public static T[] GrowBy<T>(this T[] original, int num)
	{
		if(num == 0)
		{
			return original;
		}
		if(num < 0)
		{
			throw new ArgumentException($"{nameof(num)} cannot be negative!");
		}
		if(original == null)
		{
			throw new ArgumentException($"{nameof(original)} cannot be null!");
		}

		T[] newArr = new T[original.Length + num];

		Array.Copy(original, newArr, original.Length);

		return newArr;
	}

	public static T[] Append<T>(this T[] original, T value)
	{
		var newArr = original.GrowBy(1);
		newArr[newArr.Length - 1] = value;
		return newArr;
	}
}
