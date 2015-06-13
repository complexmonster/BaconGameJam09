using UnityEngine;
using System.Collections;

public class Bubbles : MonoBehaviour {

    public GameObject bubblePrefab;

    private float rndDelay = 1;
    private float spawnTime;
    public float minDelay, maxDelay;
    //public int bubblesPerSpawn;
    public float xOffset;
    public bool spawnInWorldSpace = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - spawnTime > rndDelay)
        {
            SpawnBubble();
            spawnTime = Time.time;
            rndDelay = Random.Range(minDelay, maxDelay);
        }
	}

    void SpawnBubble()
    {
        GameObject tempBubble = (GameObject)Instantiate(bubblePrefab, new Vector2(transform.position.x + Random.Range(-xOffset, xOffset), transform.position.y), Quaternion.identity);

        if (!spawnInWorldSpace) tempBubble.transform.SetParent(transform);
    }
}
