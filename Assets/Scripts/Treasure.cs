using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

    public int value = 100;
    public bool hasBeenPickedUp = false;
    private GameObject bag;

	// Use this for initialization
	void Start () {

        float dist = Mathf.Abs(transform.position.y - Diver.diver.startDepth);
        value = value + Mathf.RoundToInt(value * dist/10);
        //print(value + " @ " + dist);
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(Diver.diver.gameObject.transform.position, transform.position) > 4f) GetComponent<Collider2D>().enabled = true;
	}

    public void SetLiftBag (GameObject _bag)
    {
        hasBeenPickedUp = true;
        bag = _bag;
    }

    void OnCollisionEnter2D (Collision2D _collision)
    {
        if (_collision != null)
       {
           if (_collision.gameObject.tag == "Surface")
           {
               Diver.diver.PayTheMan(value);
               Destroy(bag);
               Destroy(gameObject);
               value = 0;
           }
       }
    }
}
