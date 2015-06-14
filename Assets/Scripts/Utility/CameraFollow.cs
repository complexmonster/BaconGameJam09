using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float y = target.position.y;

        y = Mathf.Clamp(y,-310.53f, 320);

        transform.position = new Vector3(transform.position.x, y, transform.position.z);

	}
}
