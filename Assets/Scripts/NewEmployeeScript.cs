using NUnit.Framework;
using QPathFinder;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class NewEmployeeScript : MonoBehaviour
{
    public float decisionTime = 0.001f;
    //
    public new string name;
    public int age = 20;
    public string reasonTermination;
    public float experience;
    public int productivity;
    public int loyalty;
    public int happiness;
    public int burnout;
    //
    public int waterCooler = 13;
    public int breakRoom = 34;
    public int restRoom = 23;
    public int stockRoom = 21;
    public int conferenceRoom = 26;
    public int workStation = 1;
    public int bossOffice = 27;
    public int entranceNode = 24;
    private bool firstRun = true;
    private int prevNode = 24;
    private int targetNode;
    private int targetPosition;
    public Transform[] Positions;
    private float speed; 

    public void Start()
    { 
        transform.position = new Vector3(6.290487f, 2.859212f, transform.position.z);
        if (age < 35)
        {
            speed = Random.Range(12.5f, 16f);
        }
        else if (age < 50)
        {
            speed = Random.Range(8f, 10f);
        }
        else if (age < 100)
        {
            speed = Random.Range(6f, 7.1f);
        }
        else
        {
            speed = 5;
        }

        StartCoroutine("Behaviors");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            if (collision.gameObject.GetComponent<Node>().nodeNum == targetNode)
            {
                prevNode = targetNode;
                StartCoroutine("Behaviors");
            }
        }
    }


    #region Behaviors

    IEnumerator Behaviors()
    {
        //potential behaviors: Working, Hungry, Thirsty, Talk with Boss, Conference, Printing, Gotta pee 
        yield return new WaitForSeconds(decisionTime);
        int Rando = 0;
        if (firstRun)
        {
            Rando = 0;
            firstRun = false;
        }
        else
        {
            Rando = Random.Range(0, 7);
        }

        if (Rando == 0)
        {
            targetNode = workStation;
            targetPosition = workStation - 1;
            decisionTime = Random.Range(1f, 3f);
        }
        else if (Rando == 1)
        {
            targetNode = breakRoom;
            targetPosition = 13;
            decisionTime = Random.Range(1f, 3f);
        }
        else if (Rando == 2)
        {
            targetNode = waterCooler;
            targetPosition = 15;
            decisionTime = Random.Range(1f, 3f);
        }
        else if (Rando == 3)
        {
            targetNode = bossOffice;
            targetPosition = 14;
            decisionTime = Random.Range(1f, 3f);
        }
        else if (Rando == 4)
        {
            targetNode = conferenceRoom;
            targetPosition = 12;
            decisionTime = Random.Range(1f, 3f);
        }
        else if (Rando == 5)
        {
            targetNode = stockRoom;
            targetPosition = 17;
            decisionTime = Random.Range(1f, 3f);
        }
        else if (Rando == 6)
        {
            targetNode = restRoom;
            targetPosition = 16;
            decisionTime = Random.Range(1f, 3f);
        }

        if (prevNode == targetNode)
        {
            decisionTime = 0.001f;
            StartCoroutine("Behaviors");
            yield break;
        }
        else
        {
            PathFinder.instance.FindShortestPathOfPoints(transform.position, Positions[targetPosition].position, PathLineType.CatmullRomCurve, Execution.Synchronous,
                SearchMode.Simple,
                delegate (List<Vector3> thepoints)
                {
                    OnPathFound(thepoints);
                }
            );
        }

    }

    void OnPathFound(List<Vector3> points)
    {
        PathFollowerUtility.FollowPath(this.transform, points, speed, false);
    }

    #endregion
}
