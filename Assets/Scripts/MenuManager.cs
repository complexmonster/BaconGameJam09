using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {


    public GameObject credits, menu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        Application.LoadLevel("Game");
    }

    public void Shop()
    {
        Application.LoadLevel("Shop");
    }

    public void Credits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void ExitCredits ()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }


}
