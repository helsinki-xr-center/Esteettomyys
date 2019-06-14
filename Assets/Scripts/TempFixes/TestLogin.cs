using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Just logs in with the provided username and password. (Only for testing!)
 * </summary>
 */
public class TestLogin : MonoBehaviour
{

	public string username;
	public string password;


    // Start is called before the first frame update
    void Start()
    {
		_ = LoginManager.Login(username, password).ContinueWith(async x => Debug.Log(await x));
    }

}
