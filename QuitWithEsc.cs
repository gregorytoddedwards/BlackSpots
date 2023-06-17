using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitWithEsc : MonoBehaviour
{
 	[SerializeField]
	KeyCode keyEscape;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keyEscape))
		Application.Quit();
    }
}
