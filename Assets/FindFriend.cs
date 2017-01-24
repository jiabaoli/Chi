using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindFriend : MonoBehaviour
{
    public Material lineMat;
    public List<GameObject> lineCarriers = new List<GameObject>();
    // LineRenderer thisLine;
    public int Mode = 1;
	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void connect(Collider col_with)
    {
        GameObject lineCarrier = new GameObject("lineCarrier");
        lineCarrier.transform.parent = gameObject.transform;

        // --------Connection----------------
        lineCarrier.AddComponent<Connection>();
        Connection c = lineCarrier.GetComponent<Connection>();
        c.origin = gameObject;
        c.target = col_with.gameObject;

        // --------LineRenderer---------------

        if (Mode == 1)
        {
            
            lineCarrier.AddComponent<LineRenderer>();
            LineRenderer l = lineCarrier.GetComponent<LineRenderer>();

            l.material = lineMat;
            l.SetColors(Color.white, Color.white);
            l.SetWidth(0.1f, 0.1f);
            l.SetVertexCount(2);
            
        }
    }

    void OnTriggerEnter(Collider col)
    {
        connect(col);
    }

    void OnTriggerStay(Collider col)
    {
        if (Mode == 1)
        {
            foreach (LineRenderer l in gameObject.GetComponentsInChildren<LineRenderer>())
            {
                l.SetPosition(0, l.gameObject.GetComponent<Connection>().origin.transform.position);
                l.SetPosition(1, l.gameObject.GetComponent<Connection>().target.transform.position);
            }
        }

        else
        {
            foreach (Connection c in GetComponentsInChildren<Connection>())
            {
                c.origin = gameObject;
                c.target = col.gameObject;
            }
        }
           
    }

    void OnTriggerExit(Collider col)
    {
        // Destroy(thisLine);
    }
}
