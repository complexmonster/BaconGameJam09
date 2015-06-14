using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

[System.Serializable]
public class SaveFile {
    public int money;
    public int[] upgrades = { 1, 1, 1, 1 };
    public float[] costs = { 100f, 100, 250f, 500f };
}
