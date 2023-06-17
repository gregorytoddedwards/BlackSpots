using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScroll : MonoBehaviour
{
    public int _minimumZoom = 3;
    public int _maximumZoom = 8;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0 && 
        (Input.mouseScrollDelta.y + _cam.orthographicSize > _minimumZoom))
        {
            {
                _cam.orthographicSize --;
            }
        }
        if (Input.mouseScrollDelta.y < 0 && 
        (Input.mouseScrollDelta.y + _cam.orthographicSize < _maximumZoom))
        {
            {
                _cam.orthographicSize ++;
            }
        }
    }
}
