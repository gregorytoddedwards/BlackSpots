using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    private Vector3 aimPosition;
    public GameObject bullet;
    private bool canFire = true;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float fireRate;
    [SerializeField] private string enemyTag;
    
    void OnTriggerStay(Collider triggerer)
	{
		string s = triggerer.tag;
		if (s.Contains(enemyTag))
		{
			aimPosition = triggerer.GetComponent<Collider>().gameObject.transform.position;
			if(canFire)StartCoroutine(FireBulletPlease());
		}
	}
	
	IEnumerator FireBulletPlease()
    {
        canFire = false;
        transform.LookAt(aimPosition);
        GameObject g = Instantiate(bullet, transform.position, Quaternion.identity);
        g.GetComponent<Collider>().isTrigger = true;
        g.transform.LookAt(aimPosition);
        g.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * bulletVelocity, ForceMode.Impulse);
        yield return new WaitForSeconds(0.05f);
        g.GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
