using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    private int health = 100;
    private int population = 0;
    private int blueResource = 40;
    private int blackResource = 40;
    private int redResource = 40;
    private int yellowResource = 40;
    public TextMeshProUGUI baseStats;
    public GameObject unit;
    public GameObject turret;
    public GameObject wall;
    public GameObject deathMachine;
    private float t;
    
    void Start()
    {
    	t = 0;
    }
    
    public void AddResource(string resourceType)
    {
    	//Debug.Log(resourceType);
    	if (resourceType == "ResourceBlue") blueResource++;
    	if (resourceType == "ResourceBlack") blackResource++;
    	if (resourceType == "ResourceYellow") yellowResource++;
    	if (resourceType == "ResourceRed") redResource++;
    	
    }
    
    public void SpawnUnit()
    {
    	if (t > 1f && blueResource >= 2 && blackResource >= 2 && yellowResource >= 2)
    	{
    		blueResource -= 2;
    		blackResource -= 2;
    		yellowResource -= 2;
    		t = 0f;
    		population++;
    		Instantiate(unit,GetRandomPositionNear(3), Quaternion.identity);
    	}
    }
    
    public void SpawnTurret()
    {
    	if (t > 1f && redResource >= 2 && blackResource >= 2 && yellowResource >= 2)
    	{
    		redResource -= 2;
    		blackResource -= 2;
    		yellowResource -= 2;
    		t = 0f;
    		population++;
    		Instantiate(turret,GetRandomPositionNear(3), Quaternion.identity);
    	}
    }
    
    public void SpawnWall()
    {
    	if (t > 1f && redResource >= 2 && blackResource >= 2 && yellowResource >= 2)
    	{
    		redResource -= 4;
    		blackResource -= 4;
    		blueResource -= 4;
    		t = 0f;
    		population++;
    		Instantiate(wall,GetRandomPositionNear(3), Quaternion.identity);
    	}
    }
    
    public void SpawnDeathMachine()
    {
    	if (t > 1f && redResource >= 2 && blackResource >= 2 && yellowResource >= 2)
    	{
    		redResource -= 8;
    		blackResource -= 10;
    		yellowResource -= 8;
    		t = 0f;
    		population++;
    		Instantiate(deathMachine,GetRandomPositionNear(3), Quaternion.identity);
    	}
    }
    
    private Vector3 GetRandomPositionNear(int radius)
    {
    	return new Vector3(Random.Range(transform.position.x - radius, transform.position.x + radius), transform.position.y, Random.Range(transform.position.z - radius, transform.position.z + radius));
    }
    
    void Update()
    {
    	t += Time.deltaTime;
    	baseStats.text = "Health: " + health.ToString() + "\nPopulation: " + population.ToString() + " \nRed: " + redResource.ToString() + "\nBlue: " + blueResource.ToString() + "\nYellow: " + yellowResource.ToString() + "\nBlack: " + blackResource.ToString();
    }
}
