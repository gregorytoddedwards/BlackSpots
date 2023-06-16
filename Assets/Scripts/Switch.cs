using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public DOORSTATE _currentState;
    public GameObject _door;
    public Vector3 _closedPosition;
    public Vector3 _openPosition;
    public bool _shouldMove = false;
    public bool _isOpen;
    public float _speed = 4;
    public string _openTag = "Player";
    public bool _switchActive = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == _openTag && _shouldMove == false)
        {
            _switchActive = true;
        }
    }

    void Update()
    {
        switch (_currentState)
        {
            case DOORSTATE.Open:
            {
                _switchActive = false;
                if(_shouldMove && _door.transform.position != _openPosition)
                {
                    _door.transform.position = Vector3.MoveTowards(_door.transform.position, _openPosition, _speed * Time.deltaTime);
                }
                if(_door.transform.position == _openPosition)
                {
                    _shouldMove = false;
                    _isOpen = true;
                    _currentState = DOORSTATE.AtRest;
                }
                break;
            }

            case DOORSTATE.Close:
            {
                _switchActive = false;
                if(_shouldMove && _door.transform.position != _closedPosition)
                {
                    _door.transform.position = Vector3.MoveTowards(_door.transform.position, _closedPosition, _speed * Time.deltaTime);
                }
                if(_door.transform.position == _closedPosition)
                {
                    _isOpen = false;
                    _shouldMove = false;
                    _currentState = DOORSTATE.AtRest;
                }
                break;
            }

            case DOORSTATE.AtRest:
            {
                if (_switchActive && _isOpen == true)
                {
                    _shouldMove = true;
                    _currentState = DOORSTATE.Close;
                }
                if (_switchActive && _isOpen == false)
                {
                    _shouldMove = true;
                    _currentState = DOORSTATE.Open;
                }
                break;
            }
        }

		if (_isOpen == false)transform.GetChild(0).gameObject.SetActive(true);
		if (_isOpen == true)transform.GetChild(0).gameObject.SetActive(false);
    }
}
public enum DOORSTATE
{
    AtRest,
    Open,
    Close,
}