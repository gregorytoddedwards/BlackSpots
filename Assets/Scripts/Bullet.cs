using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject _bulletParticle;
    public int _numberOfParticles = 3;
	public string _opposingTeamTag;
    
	private Vector3 GetRandomPositionNear(Vector3 position, float radius)
    {
    	return new Vector3(Random.Range(position.x - radius, position.x + radius), position.y, Random.Range(position.z - radius, position.z + radius));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == _opposingTeamTag)
        {
            for (int x = 0; x < _numberOfParticles; x = x + 1)
            {
                GameObject g = Instantiate(_bulletParticle, GetRandomPositionNear(transform.position,.5f), Quaternion.identity);
                g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1,1),Random.Range(-1,1),Random.Range(-1,1)) * Random.Range(1,2), ForceMode.Impulse);
            }
        }
        if (collision.collider.tag == "Wall")
        {
            Destroy(gameObject);
            for (int x = 0; x < _numberOfParticles; x = x + 1)
            {
                GameObject g = Instantiate(_bulletParticle, GetRandomPositionNear(transform.position,.5f), Quaternion.identity);
                g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1,1),Random.Range(-1,1),Random.Range(-1,1)) * Random.Range(1,2), ForceMode.Impulse);
            }
        }
    }
}
