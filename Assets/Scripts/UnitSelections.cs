using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();
    
    private static UnitSelections _instance;
    public static UnitSelections Instance { get { return _instance;}}
    
    void Awake()
    {
    	if (_instance != null && _instance != this)
    	{
    		Destroy(this.gameObject);
    	}
    	else
    	{
    		_instance = this;
    	}
    }
	public void ClickSelect(GameObject unitToAdd)
	{
		DeselectAll();
		unitsSelected.Add(unitToAdd);
		unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
	}
	
	public void ShiftClickSelect(GameObject unitToAdd)
	{
		if(!unitsSelected.Contains(unitToAdd))
		{
			unitsSelected.Add(unitToAdd);
			unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			unitsSelected.Remove(unitToAdd);
			unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
		}
	}
	
	public void DragSelect(GameObject unitToAdd)
	{
	
	}
	
	public void DeselectAll()
	{
		//Debug.Log("DeselectingAll");
		foreach (var unit in unitsSelected)
		{
			unit.transform.GetChild(0).gameObject.SetActive(false);
		}
		unitsSelected.Clear();
	}
	
	public void Deselect(GameObject unitToDeselect)
	{
		//Debug.Log("Deselect");
		unitToDeselect.transform.GetChild(0).gameObject.SetActive(false);
		unitsSelected.Remove(unitToDeselect);
	}
	
	public void TaskAll(Vector3 taskPosition)
	{
		for (int i = 0; i < unitsSelected.Count; i++)
		{
			if (unitsSelected[i].GetComponent<Unit>() != null) unitsSelected[i].GetComponent<Unit>().orderPosition = taskPosition;
			if (unitsSelected[i].GetComponent<Driver>() != null) unitsSelected[i].GetComponent<Driver>().orderPosition = taskPosition;
			unitsSelected[i].transform.GetChild(0).gameObject.SetActive(false);
			unitsSelected.Remove(unitsSelected[i]);
		}
	}
}
