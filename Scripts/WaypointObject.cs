using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointObject : MonoBehaviour
{
    public float objectSpeed = 2.0f;
    public float slopRadius = 1.0f;
    public GameObject[] objectWaypoints;
    int waypointIndex = 0;
    //private Vector3 previous;
    //private Vector3 velocity;

    void Update()
    {
        //velocity = ((transform.position - previous)) / Time.deltaTime;

        //previous = transform.position;

        if (Vector3.Distance(objectWaypoints[waypointIndex].transform.position, transform.position) < slopRadius)
        {
            waypointIndex++;
            if (waypointIndex >= objectWaypoints.Length)
            {
                waypointIndex = 0;
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, objectWaypoints[waypointIndex].transform.position, Time.deltaTime * objectSpeed);
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.attachedRigidbody)
    //        if (other.attachedRigidbody == null) return;
    //    //Debug.Log(other.tag);
    //    //other.attachedRigidbody.velocity = new Vector3(velocity.x, velocity.y, 0.0f);
    //    if (other.tag == "Player") other.attachedRigidbody.velocity += new Vector3(0f,velocity.y * f, 0f);
    //    else return;
    //}
}