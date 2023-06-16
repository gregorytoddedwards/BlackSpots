using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithOffset : MonoBehaviour
{
	private GameObject followObject;
	public string followTag = "Player";
	public float offset = -10;
    void Start()
    {
        followObject = GameObject.FindWithTag(followTag);
    }
    void LateUpdate()
    {
        if (followObject != null)transform.position = Vector3.Lerp(new Vector3(followObject.transform.position.x, followObject.transform.position.y + offset, followObject.transform.position.z), transform.position, 2 * Time.deltaTime);
    }
}
