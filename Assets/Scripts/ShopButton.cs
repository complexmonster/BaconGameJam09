using UnityEngine;
using System.Collections;

public class ShopButton : MonoBehaviour {

    public int id = 0;
    private UnityEngine.UI.Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(() =>
        {
            ShopManager.shop.Clicked(id);
        });
	}
	
	// Update is called once per frame
	void Update () {

	}

}
