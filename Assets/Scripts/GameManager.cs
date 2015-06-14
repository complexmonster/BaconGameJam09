using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public List<GameObject> allTreasure;
    [SerializeField]
    private GameObject currentTreasure;

    public delegate void OnTick();
    public static event OnTick Tick;

    public float tick = 1f;

	// Use this for initialization
	void Awake () {
        if (gm == null) gm = this;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Tick != null)
        {
            if (Time.time % tick <= Time.deltaTime) Tick();
        }

        foreach (GameObject t in allTreasure)
        {
            if (t)
            {
                if (Vector2.Distance(Diver.diver.transform.position, t.transform.position) < 0.75f)
                {
                    currentTreasure = t;
                }
                // currentTreasure = null;
            }
        }

        if (currentTreasure != null)
        {
            Diver.diver.ReadyTreasure(currentTreasure);
        }
	}

    public void GameObver ()
    {
        print("GameOver");
    }
}
