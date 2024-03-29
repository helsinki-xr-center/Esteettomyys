﻿using System;
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
}
