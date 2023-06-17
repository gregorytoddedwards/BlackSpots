using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithKeys : MonoBehaviour
{
	public float speed;
	private Rigidbody rigid;

	void Start()
	{
		rigid = GetComponent<Rigidbody>();
	}
	void FixedUpdate()
	{
	if (Input.GetKey(KeyCode.W)) rigid.AddForce(new Vector3(0f,0f,speed));
	if (Input.GetKey(KeyCode.A)) rigid.AddForce(new Vector3(-speed,0f,0f));
	if (Input.GetKey(KeyCode.S)) rigid.AddForce(new Vector3(0f,0f,-speed));
	if (Input.GetKey(KeyCode.D)) rigid.AddForce(new Vector3(speed,0f,0f));
	}
}
