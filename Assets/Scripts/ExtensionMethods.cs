using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Usable static methods
/// </summary>
public static class ExtensionMethods
{

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


	public static T MinBy<T, Q>(this IEnumerable<T> enumerable, Func<T, Q> predicate) where Q : IComparable<Q>
	{
		T closest = default;
		Q leastDist = default;
		bool first = true;
		foreach (var item in enumerable)
		{
			if (first)
			{
				leastDist = predicate.Invoke(item);
				closest = item;
				first = false;
			}
			else
			{
				Q newDist = predicate.Invoke(item);
				if (leastDist.CompareTo(newDist) > 0)
				{
					closest = item;
					leastDist = newDist;
				}
			}

		}

		return closest;
	}

	public static T MaxBy<T, Q>(this IEnumerable<T> enumerable, Func<T, Q> predicate) where Q : IComparable<Q>
	{
		T furthest = default;
		Q mostDist = default;
		bool first = true;
		foreach (var item in enumerable)
		{
			if (first)
			{
				mostDist = predicate.Invoke(item);
				furthest = item;
				first = false;
			}
			else
			{
				Q newDist = predicate.Invoke(item);
				if (newDist.CompareTo(mostDist) > 0)
				{
					furthest = item;
					mostDist = newDist;
				}
			}

		}

		return furthest;
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

	/**
	 * <summary>
	 * Draws bounds Editor window with lines.
	 * </summary>
	 */
	public static void DrawDebug(this Bounds b, float delay = 0)
	{
		// bottom
		var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
		var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
		var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
		var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

		Debug.DrawLine(p1, p2, Color.blue, delay);
		Debug.DrawLine(p2, p3, Color.red, delay);
		Debug.DrawLine(p3, p4, Color.yellow, delay);
		Debug.DrawLine(p4, p1, Color.magenta, delay);

		// top
		var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
		var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
		var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
		var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

		Debug.DrawLine(p5, p6, Color.blue, delay);
		Debug.DrawLine(p6, p7, Color.red, delay);
		Debug.DrawLine(p7, p8, Color.yellow, delay);
		Debug.DrawLine(p8, p5, Color.magenta, delay);

		// sides
		Debug.DrawLine(p1, p5, Color.white, delay);
		Debug.DrawLine(p2, p6, Color.gray, delay);
		Debug.DrawLine(p3, p7, Color.green, delay);
		Debug.DrawLine(p4, p8, Color.cyan, delay);
	}
}
