using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>An interface for awaitable UI panels. Things like login screen, mode selection and such.</summary>
 */
public interface IAwaitableUIPanel 
{
	IEnumerator WaitForFinish();
}
