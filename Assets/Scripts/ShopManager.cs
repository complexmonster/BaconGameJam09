using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ShopManager : MonoBehaviour {


    public static ShopManager shop;
    private SaveFile save = new SaveFile();

    public float baseCostNminus, baseCosteX, baseCostEPlus, baseCostBag;
    public float costFactor = 2f;

    public int money = 1000;

    private int nMinusLevel = 1, eXLevel = 1, ePlusLevel = 1, bagLevel = 1, maxLevelN, maxLevelEx, maxLevelEPlus, MaxLevelBags;
    private float nMinusCost = 200f, eXCost = 100f, ePlusCost= 250f, bagCost = 500f;

    public UnityEngine.UI.Text nMinusCostText, eXCostText, ePlusCostText, bagCostText, nMinusLevelText, eXLevelText, ePlusLevelText, bagLevelText, moneyText;
    public UnityEngine.UI.Button nMinusButton, exButton, ePlusButton, bagButton;



    void Awake()
    {
        if (shop == null) shop = this;

        LoadData();
    }
	
	// Update is called once per frame
	void Update () 
    {
        UpdateUI();
	}

    void UpdateUI ()
    {
        nMinusCostText.text = nMinusCost.ToString();
        ePlusCostText.text = ePlusCost.ToString();
        eXCostText.text = eXCost.ToString();
        bagCostText.text = bagCost.ToString();

        nMinusLevelText.text = "L" + nMinusLevel.ToString();
        eXLevelText.text = "L" + eXLevel.ToString();
        ePlusLevelText.text = "L" + ePlusLevel.ToString();
        bagLevelText.text = "L" + bagLevel.ToString();

        moneyText.text = money.ToString();

        CheckMoney();
    }

    void CheckMoney ()
    {
        if (money < nMinusCost) nMinusButton.interactable = false; else nMinusButton.interactable = true;
        if (money < ePlusCost) ePlusButton.interactable = false; else ePlusButton.interactable = true;
        if (money < eXCost) exButton.interactable = false; else exButton.interactable = true;
        if (money < bagCost) bagButton.interactable = false; else bagButton.interactable = true;

        if (nMinusLevel >= 99) nMinusButton.interactable = false; else nMinusButton.interactable = true;
        if (ePlusLevel >= 99) ePlusButton.interactable = false; else ePlusButton.interactable = true;
        if (eXLevel >= 99) exButton.interactable = false; else exButton.interactable = true;
        if (bagLevel >= 12) bagButton.interactable = false; else bagButton.interactable = true;
    }

    void SetPrice (int _id)
    {
        switch (_id) { 
            case 0:
                nMinusCost = nMinusCost + (baseCostNminus * nMinusLevel);
                nMinusLevel++;
                break;
            case 1:
                ePlusCost = ePlusCost + (baseCostEPlus * ePlusLevel);
                ePlusLevel++;
                break;
            case 2:
                eXCost = eXCost + (baseCosteX * eXLevel);
                eXLevel++;
                break;
            case 3:
                bagCost = bagCost + (baseCostBag * bagLevel);
                bagLevel++;
                break;
        }
    }

    public void Clicked (int _buttonid)
    {
        switch (_buttonid)
        {
            case 0:
                money -= (int)nMinusCost;
                
                SetPrice(0);
                break;
            case 1:
                money -= (int)ePlusCost;
                
                SetPrice(1);
                break;
            case 2:
                money -= (int)eXCost;
                
                SetPrice(2);
                break;
            case 3:
                money -= (int)bagCost;
                
                SetPrice(3);
                break;
            case 4:
                SaveData();
                Application.LoadLevel("Game");
                break;
            case 5:
                SaveData();
                Application.LoadLevel("Menu");
                break;
        }
    }

    void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.diver"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.diver", FileMode.Open);
            save = (SaveFile)bf.Deserialize(file);

            file.Close();

            ParseSaveData(save);
        }
        else
        {
            Debug.LogWarning("FILE COULD NOT BE LOADED, FNF");
        }
    }

    void ParseSaveData (SaveFile _save)
    {
        money = _save.money;
        nMinusLevel = _save.upgrades[0];
        eXLevel = _save.upgrades[1];
        ePlusLevel = _save.upgrades[2];
        bagLevel =_save.upgrades[3];
        nMinusCost = _save.costs[0];
        eXCost = _save.costs[1];
        ePlusCost = _save.costs[2];
        bagCost = _save.costs[3];

        print("Save data successfully parsed");
    }

    void SaveData()
    {
        save.money = money;
        save.upgrades[0] = nMinusLevel;
        save.upgrades[1] = eXLevel;
        save.upgrades[2] = ePlusLevel;
        save.upgrades[3] = bagLevel;
        save.costs[0] = nMinusCost;
        save.costs[1] = eXCost;
        save.costs[2] = ePlusCost;
        save.costs[3] = bagCost;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.diver");

        bf.Serialize(file, save);

        file.Close();
    }

    void DeleteSaveFile ()
    {
        File.Delete(Application.persistentDataPath + "/save.diver");

        Reset();
    }

    void Reset ()
    {
        nMinusCost = save.costs[0];
        eXCost = save.costs[1];
        ePlusCost = save.costs[2];
        bagCost = save.costs[3];
    }
    void OnGUI ()
    {
        //if (GUILayout.Button("DELETE SAVE")) DeleteSaveFile();
       // if (GUILayout.Button("Refresh")) SetPrices();
    }
}
