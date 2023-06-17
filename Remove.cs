using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : MonoBehaviour
{
    public float _life = 5f;
    void Start()
    {
        StartCoroutine(CleanPlease(_life));
    }
    IEnumerator CleanPlease(float _lifeSpan)
    {
        yield return new WaitForSeconds(Random.Range( 0.65f  * _lifeSpan, _lifeSpan));
        Destroy(gameObject);
    }
}
