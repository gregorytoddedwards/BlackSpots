using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Vector3 orderPosition;
    public Vector3 previousOrderPosition;
    private NavMeshAgent agent;
    private Vector3 basePosition;
    public string carrying;
    public string enemyTag;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		//basePosition = GameObject.FindGameObjectWithTag("BaseGreen").transform.position;
		orderPosition = GetRandomPositionNear(basePosition,3f);
		previousOrderPosition = GetRandomPositionNear(basePosition,3f);
		UnitSelections.Instance.unitList.Add(this.gameObject);
		carrying = "Empty";
	}
	
	void OnDestroy()
	{
		UnitSelections.Instance.unitList.Remove(this.gameObject);
	}
	void Update()
	{
		if (orderPosition != null)
		{
		agent.destination = orderPosition;
		}
	}
	
	private Vector3 GetRandomPositionNear(Vector3 position, float radius)
    {
    	return new Vector3(Random.Range(position.x - radius, position.x + radius), position.y, Random.Range(position.z - radius, position.z + radius));
    }
	
	void OnTriggerEnter(Collider triggerer)
	{
		string s = triggerer.tag;
		if (s.Contains("Resource"))
		{
			if (carrying == "Empty")
			{
				orderPosition  = triggerer.transform.position;
				previousOrderPosition = orderPosition;
			}
		}
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag.Contains("Resource") && carrying == "Empty")
		{
			carrying = other.collider.tag;
			Destroy(other.gameObject);
			previousOrderPosition = orderPosition;
			orderPosition = GetRandomPositionNear(basePosition,1f);
		}
		if (other.collider.tag.Contains("Base") && carrying != "Empty")
		{
			other.collider.gameObject.GetComponent<Base>().AddResource(carrying);
			carrying = "Empty";
			orderPosition = previousOrderPosition;
		}
		if (other.collider.tag == enemyTag)
		{
			UnitSelections.Instance.unitList.Remove(this.gameObject);
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
		return;
	}
}
