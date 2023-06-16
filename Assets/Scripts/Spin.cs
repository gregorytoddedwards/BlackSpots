using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 _spinSpeed;
    public GameObject _objectToFollow;

    void Update()
    {
        //transform.position = _objectToFollow.transform.position;
        transform.Rotate(_spinSpeed * Time.deltaTime, Space.World);
    }
}
