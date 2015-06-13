using UnityEngine;
using System.Collections;

public class Diver : MonoBehaviour {

    public float startDepth;
    public float nitrogen = 0;
    public float weight = 0;
    public float swimPower = 200f;
    public int floatBags = 1;

    private float currentDepth;


    private float timeStart = 0f;
    private float timeDiveTotal = 0f;

    public UnityEngine.UI.Text depthText;
    public UnityEngine.UI.Image weightBar, nitrogenBar;

    private float degree, angle;

	// Use this for initialization
	void Start () {
        startDepth = transform.position.y;

        timeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        currentDepth = transform.position.y - startDepth;

        timeDiveTotal = Time.time - timeStart;

        depthText.text = Mathf.Abs(Mathf.RoundToInt(currentDepth)).ToString();

        UpdateUI();
	}

    void UpdateUI ()
    {
        weightBar.gameObject.GetComponent<StatusBar>().UpdateBar(weight, 100);
        nitrogenBar.gameObject.GetComponent<StatusBar>().UpdateBar(nitrogen, 100);
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
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10);

    }


    void OnGUI ()
    {
        GUILayout.Label("TIME: " + Mathf.RoundToInt(timeDiveTotal).ToString() + "secs");
        GUILayout.Label("TOXICITY: " + nitrogen.ToString());
        GUILayout.Label("VELOCITY: " + GetComponent<Rigidbody2D>().velocity.y.ToString());

        if (GUILayout.Button ("TREASURE!"))
        {
            weight += 10f;
        }

        if (GUILayout.Button("NITROGEN!"))
        {
            nitrogen += 10f;
        }
    }
}
