using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Diver : MonoBehaviour {

    public static Diver diver;
    private SaveFile save = new SaveFile();

    public float startDepth;
    public float nitrogen = 0;
    public float nitrogenProduction = 1.0f;
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
    //private float timeDiveTotal = 0f;

    private bool readyForTreasure = false;

    public UnityEngine.UI.Text depthText, cashText;
    public UnityEngine.UI.Image weightBar, nitrogenBar;
    public GameObject bagPrefab, interactUI, bagsUI, bagIcon;

    private float degree, angle;

    void Awake ()
    {
        if (diver == null) diver = this;

        LoadUpgrades();

        startDepth = transform.position.y;
    }

	// Use this for initialization
	void Start () 
    {
        timeStart = Time.time;

        GameManager.Tick += UpdateNitrogenLevels;
        GameManager.Tick += UpdateEnergyLevels;
	}

    public void ReadyTreasure (GameObject _treasure)
    {
        if (liftingBags >= 1)
        {
            if (readyForTreasure && !_treasure.GetComponent<Treasure>().hasBeenPickedUp) ApplyTreasureBag(_treasure);
        }
    }

    void ApplyTreasureBag (GameObject _treasure)
    {
        GameObject tempBag = (GameObject) Instantiate(bagPrefab, new Vector3(_treasure.transform.position.x, _treasure.transform.position.y+1, 0), Quaternion.identity);
        tempBag.GetComponent<DistanceJoint2D>().connectedBody = _treasure.GetComponent<Rigidbody2D>();

        _treasure.GetComponent<Treasure>().SetLiftBag(tempBag);
        _treasure.GetComponent<BoxCollider2D>().enabled = false;
        if (bagsUI.transform.GetChild(0).gameObject) Destroy ( bagsUI.transform.GetChild(0).gameObject );

        liftingBags -= 1;
    }

    public void PayTheMan (int _money)
    {
        cashMoney += _money;
        cashText.gameObject.GetComponent<Animator>().Play("Cash");
    }

    void LoadUpgrades ()
    {
        if (File.Exists(Application.persistentDataPath + "/save.diver"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.diver", FileMode.Open);
            save = (SaveFile)bf.Deserialize(file);

            file.Close();

            cashMoney = save.money;
            print(save.upgrades[0]);
            nitrogenProduction = 1.0f + (save.upgrades[0] * 25f) / 100; // 5% per upgrade level, make to percentage
            maxEnergy = maxEnergy + (save.upgrades[2] * 10f);
            energyGain = energyGain * (1.0f + (save.upgrades[1] * 10f) / 100f);
            liftingBags = save.upgrades[3];
            energy = maxEnergy;

            for(int i = 0; i < liftingBags; i++)
            {
                GameObject tmpIcon = (GameObject)Instantiate(bagIcon);
                tmpIcon.transform.SetParent(bagsUI.transform, false);
            }
        }
    }

    void SaveTreasure()
    {
        save.money = cashMoney;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.diver");


        print(save.upgrades[0]);

        bf.Serialize(file, save);

        file.Close();
    }

    void UpdateNitrogenLevels()
    {
        if (nitrogen >= 100f) { GameManager.gm.GameOver(); nitrogen = 0f; }

        if (Mathf.Abs(currentDepth) > 1f)
        {
            nitrogen += Mathf.Abs(currentDepth) / (20 * nitrogenProduction);
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
        //timeDiveTotal = Time.time - timeStart;

        UpdateUI();
	}

    void UpdateUI ()
    {
        depthText.text = Mathf.Abs(Mathf.RoundToInt(currentDepth)).ToString();
        weightBar.gameObject.GetComponent<StatusBar>().UpdateBar(energy, maxEnergy);
        nitrogenBar.gameObject.GetComponent<StatusBar>().UpdateBar(nitrogen, 100);
        cashText.text = cashMoney.ToString();
    }

    void FixedUpdate()
    {
        if (GameManager.gm.gameOver || GameManager.gm.win) return;

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

            if (Input.GetKey(KeyCode.S) && energy >= energyConsumption * Time.deltaTime)
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

    /* DEBUGGING
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
    }*/
    void GotoMenu ()
    {
        Application.LoadLevel("Menu");
    }

    void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision != null)
        {
            if (_collision.gameObject.tag == "Surface")
            {
                SaveTreasure();
                GameManager.gm.Win(.4f);
                Invoke("GotoMenu", 5);
            }
        }
    }

    void OnTriggerEnter2D (Collider2D _col)
    {
        if (_col.gameObject.name == "Win")
        {
            GameManager.gm.Win(.15f);
        }
    }
}
