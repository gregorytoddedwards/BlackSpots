using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Driver : MonoBehaviour
{
    public Vector3 orderPosition;
    private NavMeshAgent agent;
    public string enemyTag;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		orderPosition = GetRandomPositionNear(transform.position,3);
		UnitSelections.Instance.unitList.Add(this.gameObject);
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
	
	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == enemyTag)
		{
			Destroy(gameObject);
		}
		
	}
}
