using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

    public float speed = 0.1f;
    private float startY;
    public int deleteAtDistance = 10;

	// Use this for initialization
	void Start () {
        startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        speed += .1f;
        float rndMultiplier = Random.Range(4f, 5f);
        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
        transform.localScale = new Vector2(transform.localScale.x + Time.deltaTime / rndMultiplier, transform.localScale.y + Time.deltaTime / rndMultiplier);

        if (transform.position.y - startY > deleteAtDistance) Destroy(gameObject);
	}
}
