using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DotsArray : MonoBehaviour
{
    GameObject Point;

    List<GameObject> PointArray;
    public Material lineMat;
    List<float> speed;
    public GameObject Dome;
    public GameObject whale;

    int pointCount = 0;
    List<Vector3> positionBeforeMove;
    int status = 0;
    
    // Use this for initialization
	void Start ()
    {
        pointCount = Dome.GetComponent<MeshFilter>().mesh.vertexCount;
        // pointCount = whale.GetComponent<SkinnedMeshRenderer>().sharedMesh.vertexCount;
        // pointCount = 200;

        Debug.Log(pointCount);

        PointArray = new List<GameObject>();
        speed = new List<float>();
        positionBeforeMove = new List<Vector3>();

        Point = Resources.Load("Primitive/Dot") as GameObject;
        

        if (Point != null)
        {
            for (int i = 0; i < pointCount; i++)
            {
                PointArray.Add(GameObject.Instantiate(Point, new Vector3(Random.Range(-30f, 30f), Random.Range(0f, 40f), Random.Range(-30f, 30f)), Quaternion.identity) as GameObject);
                speed.Add(Random.Range(0.02f, 0.15f));
                // PointArray[i].AddComponent<LineRenderer>();
            }
        }

        else
        {
            Debug.Log("Can't find the dot");
        }
        
        /*
        for (int i = 0; i < PointArray.Count; i++)
        {
            int indexOfClosest = 0;
            float minDist = 100000;
            speed.Add(Random.Range(0.02f, 0.15f));

            for (int j = 0; j < PointArray.Count; j++)
            {
                float thisDist = Vector3.Distance(PointArray[j].transform.position, PointArray[i].transform.position);
                if (thisDist < minDist && j != i)
                {
                    minDist = thisDist;
                    indexOfClosest = j;
                }    
            }

            LineRenderer thisLine = PointArray[i].GetComponent<LineRenderer>();
            thisLine.material = lineMat;
            thisLine.SetColors(Color.white, Color.white);
            thisLine.SetWidth(0.1f, 0.1f);
            thisLine.SetVertexCount(2);
            thisLine.SetPosition(0, PointArray[i].transform.position);
            thisLine.SetPosition(1, PointArray[indexOfClosest].transform.position);
        }
        */
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(status);

        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = 0; i < pointCount; i++)
            {
                positionBeforeMove.Add(PointArray[i].transform.position);
            }
            status = 1;
        }

        else if (Input.GetKeyDown(KeyCode.B))
        {
            status = 2;
        }

        /*
        else
        {
            for (int i = 0; i < PointArray.Count; i++)
            {
                PointArray[i].transform.RotateAround(Vector3.zero, Vector3.up, speed[i]);
            }
        }
        */

        switch (status)
        {
            case 0:
                for (int i = 0; i < PointArray.Count; i++)
                {
                    PointArray[i].transform.RotateAround(Vector3.zero, Vector3.up, speed[i]);
                }

                break;
            case 1:
                converge1(PointArray, whale);

                // converge(PointArray, Dome);
                break;
            case 2:
                goBack(PointArray);
                break;
            default:
                break;
        }

        /*
        if (converge1(PointArray, whale))
        {
            whale.GetComponent<SkinnedMeshRenderer>().enabled = true;   
            whale.GetComponentInParent<Animation>().enabled = true;
            
            whale.GetComponentInParent<Animation>().gameObject.SetActive(true);
        }
        */

        // DrawLine(PointArray);
    }

    void DrawLine(List<GameObject> Points)
    {
        for (int i = 0; i < Points.Count; i++)
        {
            LineRenderer thisLine = Points[i].GetComponent<LineRenderer>();

            int indexOfClosest = 0;
            float minDist = 100000;
            for (int j = 0; j < Points.Count; j++)
            {
                float thisDist = Vector3.Distance(Points[j].transform.position, Points[i].transform.position);
                if (thisDist < minDist && j != i)
                {
                    minDist = thisDist;
                    indexOfClosest = j;
                }
            }

            // thisLine = new LineRenderer();
            thisLine.SetVertexCount(2);
            thisLine.SetPosition(0, Points[i].transform.position);
            thisLine.SetPosition(1, Points[indexOfClosest].transform.position);
        }
    }

    void converge(List<GameObject> Points, GameObject targetObj)
    {

        // Debug.Log("Converging");
        for (int i = 0; i < Points.Count; i++)
        {
            Points[i].transform.position = Vector3.Slerp(Points[i].transform.position, targetObj.transform.TransformPoint(targetObj.GetComponent<MeshFilter>().mesh.vertices[i]), .04f);
        }
    }

    bool converge1(List<GameObject> Points, GameObject targetObj)
    {

        int arrive = 0;
        // Debug.Log("Converging");
        for (int i = 0; i < Points.Count; i++)
        {
            Points[i].transform.position = Vector3.Slerp(Points[i].transform.position, targetObj.transform.TransformPoint(targetObj.GetComponent<SkinnedMeshRenderer>().sharedMesh.vertices[i]), .04f);

            if (Vector3.Distance(Points[i].transform.position, targetObj.transform.TransformPoint(targetObj.GetComponent<SkinnedMeshRenderer>().sharedMesh.vertices[i])) <= 0.2f)
            {
                arrive++;
            }
        }



        if (arrive == Points.Count)
        {
            return true;
            foreach (GameObject o in PointArray)
            {
                o.SetActive(false);
                Destroy(o);
            }
        }
        return false;

    }

    void goBack(List<GameObject> Points)
    {
        int back = 0;
        for (int i = 0; i < Points.Count; i++)
        {
            if (Points[i].transform.position != positionBeforeMove[i])
            {
                Points[i].transform.position = Vector3.Slerp(Points[i].transform.position, positionBeforeMove[i], 0.1f);
            }
            else
            {
                back++;
            }
        }

        // Debug.Log(back);
        if (back != 0)
        {
            status = 0;
        }
    }
}
