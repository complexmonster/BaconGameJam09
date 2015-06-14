using UnityEngine;
using System.Collections;

public class Diver : MonoBehaviour {

    public static Diver diver;

    public float startDepth;
    public float nitrogen = 0;
    public float energy = 100;
    public float maxEnergy = 100;
    public float energyConsumption = 10f;
    public float energyBoost = 10f;
    public float energyGain = 1f;
    public float swimPower = 200f;
    public int liftingBags = 1;
    public int cashMoney = 0;

    private float currentDepth;

    private float timeStart = 0f;
    private float timeDiveTotal = 0f;

    private bool readyForTreasure = false;

    public UnityEngine.UI.Text depthText, cashText;
    public UnityEngine.UI.Image weightBar, nitrogenBar;
    public GameObject bagPrefab, interactUI;

    private float degree, angle;

    void Awake ()
    {
        if (diver == null) diver = this;
    }

	// Use this for initialization
	void Start () 
    {
        startDepth = transform.position.y;
        timeStart = Time.time;

        GameManager.Tick += UpdateNitrogenLevels;
        GameManager.Tick += UpdateEnergyLevels;
	}

    public void ReadyTreasure (GameObject _treasure)
    {
        if (liftingBags >= 1)
        {
            //interactUI.SetActive(true);

            if (readyForTreasure && !_treasure.GetComponent<Treasure>().hasBeenPickedUp) ApplyTreasureBag(_treasure);
        }
    }

    void ApplyTreasureBag (GameObject _treasure)
    {
        GameObject tempBag = (GameObject) Instantiate(bagPrefab, new Vector3(_treasure.transform.position.x, _treasure.transform.position.y+1, 0), Quaternion.identity);
        tempBag.GetComponent<DistanceJoint2D>().connectedBody = _treasure.GetComponent<Rigidbody2D>();

        _treasure.GetComponent<Treasure>().SetLiftBag(tempBag);
        _treasure.GetComponent<BoxCollider2D>().enabled = false;

        liftingBags -= 1;
    }

    public void PayTheMan (int _money)
    {
        cashMoney += _money;
        cashText.gameObject.GetComponent<Animator>().Play("Cash");
    }

    void UpdateNitrogenLevels()
    {
        if (nitrogen > 100f) { print("DEAD AT:" + Time.time); GameManager.gm.GameObver(); }

        if (Mathf.Abs(currentDepth) > 1f)
        {
            nitrogen += Mathf.Abs(currentDepth) / 10;
        }
    }

    void UpdateEnergyLevels ()
    {
        if (energy + energyGain < maxEnergy) energy += energyGain;
    }
	
	// Update is called once per frame
	void Update () 
    {
        // UPDATE VALUES
        currentDepth = transform.position.y - startDepth;
        timeDiveTotal = Time.time - timeStart;

        UpdateUI();
	}

    void UpdateUI ()
    {
        depthText.text = Mathf.Abs(Mathf.RoundToInt(currentDepth)).ToString();
        weightBar.gameObject.GetComponent<StatusBar>().UpdateBar(energy, 100);
        nitrogenBar.gameObject.GetComponent<StatusBar>().UpdateBar(nitrogen, 100);
        cashText.text = "$"+cashMoney.ToString();
    }

    void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>().velocity.y < 0f) angle = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            degree += 50f;
            angle = 180f;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Time.deltaTime * swimPower), ForceMode2D.Force);

            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-10 * Time.deltaTime, 0), ForceMode2D.Impulse);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * Time.deltaTime, 0), ForceMode2D.Impulse);
            }

        }
        else { 
            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-10 * Time.deltaTime, 0), ForceMode2D.Impulse);
                angle = -15f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * Time.deltaTime, 0), ForceMode2D.Impulse);
                angle = 15f;
            }

            if (Input.GetKey(KeyCode.S) && energy >= energyConsumption)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -energyBoost * Time.deltaTime), ForceMode2D.Impulse);
                energy -= energyConsumption * Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            readyForTreasure = true;
            interactUI.SetActive(false);
        }
        else readyForTreasure = false;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10);
    }

    // DEBUGGING
    void OnGUI ()
    {
        GUILayout.Label("TIME: " + Mathf.RoundToInt(timeDiveTotal).ToString() + "secs");
        GUILayout.Label("TOXICITY: " + nitrogen.ToString());
        GUILayout.Label("VELOCITY: " + GetComponent<Rigidbody2D>().velocity.y.ToString());

        if (GUILayout.Button ("Energy!"))
        {
            energy += 10f;
        }

        if (GUILayout.Button("NITROGEN!"))
        {
            nitrogen += 10f;
        }
    }
}
