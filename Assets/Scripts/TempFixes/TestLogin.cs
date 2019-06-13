using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
