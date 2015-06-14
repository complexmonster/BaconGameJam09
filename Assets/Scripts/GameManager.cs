using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public UnityEngine.UI.Image whiteScreen, blackScreen;

    public GameObject treasure;
    public List<GameObject> allTreasure;
    public GameObject[] ledges;
    public Transform level;
    [SerializeField]
    private GameObject currentTreasure;

    public delegate void OnTick();
    public static event OnTick Tick;

    public float tick = 1f;

    public bool win = false;
    public bool gameOver = false;
    private float alpha = 0f;
    private float fadeSpeed = .2f;

	// Use this for initialization
	void Awake () {
        if (gm == null) gm = this;
	}

    void Start ()
    {
        SpawnShit();
    }
	
	// Update is called once per frame
	void Update () 
    {

        print(gameOver);
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
            }
        }

        if (currentTreasure != null)
        {
            Diver.diver.ReadyTreasure(currentTreasure);
        }

        if (win)
        {
            alpha += fadeSpeed * Time.deltaTime;
            whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, alpha);
        } 
        else if (gameOver)
        {
            alpha += fadeSpeed * Time.deltaTime;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
        }
	}

    public void SpawnShit()
    {
        for (int i = 0; i < ledges.Length; i++ )
        {
            float rnd = Random.value;

            if (rnd >= 0.35f) continue;
            else
            {
                GameObject t = (GameObject)Instantiate(treasure, new Vector3(ledges[i].transform.position.x, ledges[i].transform.position.y + 1, ledges[i].transform.position.y), Quaternion.identity);
                allTreasure.Add(t);
                t.transform.SetParent(level);
            }
        }
    }

    public void Win (float _fadeSpeed)
    {
        fadeSpeed = _fadeSpeed;
        whiteScreen.gameObject.SetActive(true);

        win = true;
    }

    public void GameOver ()
    {
        blackScreen.gameObject.SetActive(true);

        gameOver = true;

        Invoke("GoBackToMenu", 10f);
    }

    private void GoBackToMenu ()
    {
        Application.LoadLevel("Menu");
    }
}
