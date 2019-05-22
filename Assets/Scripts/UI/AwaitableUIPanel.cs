using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>Abstract base class that implements <see cref="IAwaitableUIPanel"/></summary>
 */
public abstract class AwaitableUIPanel : MonoBehaviour, IAwaitableUIPanel
{
	public abstract IEnumerator WaitForFinish();
}
