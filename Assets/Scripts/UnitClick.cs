
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;
    
    public LayerMask clickable;
    public LayerMask ground;
    
    void Start()
    {
        myCam = Camera.main;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        	RaycastHit hit;
        	Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        	
        	if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
        	{
        		if (Input.GetKey(KeyCode.LeftShift))
        		{
        			UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
        		}
        		else
        		{
        			UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
        		}
        	}
        	else
        	{
        		if (!Input.GetKey(KeyCode.LeftShift))
        		{
        			UnitSelections.Instance.DeselectAll();
        		}
        	}
        }
        if (Input.GetMouseButton(1))
        {
        	RaycastHit hit;
        	Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        	if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
        	{
        		UnitSelections.Instance.TaskAll(hit.point);
        	}
        }
    }
}
