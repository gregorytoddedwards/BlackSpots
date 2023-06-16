using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTag : MonoBehaviour
{
    public GameObject particle;
    public int numberOfParticles;
	public string destroyTag = "BulletPlayer";
    
	private Vector3 GetRandomPositionNear(Vector3 position, float radius)
    {
    	return new Vector3(Random.Range(position.x - radius, position.x + radius), position.y, Random.Range(position.z - radius, position.z + radius));
    }

	void OnTriggerStay(Collider other)
    {
        if (other.tag == destroyTag)
        {
            for (int x = 0; x < numberOfParticles; x = x + 1)
            {
                Instantiate(particle, GetRandomPositionNear(transform.position,1f), Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}