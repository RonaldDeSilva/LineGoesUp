using NUnit.Framework;
using QPathFinder;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EmployeeScript : MonoBehaviour
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
    public int waterCooler = 19;
    public int breakRoom = 17;
    public int restRoom = 20;
    public int stockRoom = 21;
    public int conferenceRoom = 16;
    public int workStation = 1;
    public int bossOffice = 18;
    private bool firstRun = true;
    private int prevNode = 24;
    public int targetPosition;
    private Transform[] Positions = new Transform[22];
    private Transform[] QueuePositions = new Transform[33];
    private float speed;
    private EmployeeMasterControl EMC;
    private int currentQueuePosition;
    public bool movingToQueuePosition = false;
    private Rigidbody2D rb;
    public bool chosen = false; 
    public bool isMoving = false;
    private int badPos = -1;
    private int prevRando = -1;

    public void Awaken(EmployeeMasterControl EmpMasCon)
    {
        EMC = EmpMasCon;
        transform.position = new Vector3(6.290487f, 2.859212f, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        var nodesPos = GameObject.Find("Nodes");
        for (int i = 0; i < nodesPos.transform.childCount; i++)
        {
            Positions[i] = nodesPos.transform.GetChild(i);
        }

        var queuePos = GameObject.Find("QueuePositions");
        for (int i = 0; i < queuePos.transform.childCount; i++)
        {
            QueuePositions[i] = queuePos.transform.GetChild(i);
        }

        if (age < 35)
        {
            speed = Random.Range(10f, 16f);
        }
        else if (age < 50)
        {
            speed = Random.Range(6f, 8f);
        }
        else if (age < 100)
        {
            speed = Random.Range(3f, 5f);
        }
        else
        {
            speed = 2;
        }

        StartCoroutine("Behaviors");
    }

    private void Update()
    {
        if (movingToQueuePosition && !Approx.FastApp(transform.position.x, QueuePositions[targetPosition].position.x, 0.1f) && !Approx.FastApp(transform.position.y, QueuePositions[targetPosition].position.y, 0.1f))
        {
            rb.linearVelocityX = (QueuePositions[targetPosition].position.x - transform.position.x) * speed * Time.deltaTime * 100;
            rb.linearVelocityY = (QueuePositions[targetPosition].position.y - transform.position.y) * speed * Time.deltaTime * 100;
        }
        else if (movingToQueuePosition && Approx.FastApp(transform.position.x, QueuePositions[targetPosition].position.x, 0.1f) && Approx.FastApp(transform.position.y, QueuePositions[targetPosition].position.y, 0.1f))
        {
            rb.linearVelocity = Vector2.zero;
            movingToQueuePosition = false;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            if (collision.gameObject.GetComponent<Node>().nodeNum == targetPosition)
            {
                prevNode = targetPosition;
                chosen = false;
                isMoving = false;
                StartCoroutine("Behaviors");
                StartCoroutine("WaitASec");
            }
        }
    }

    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(0.5f);
        if (prevNode == breakRoom)
        {
            for (int i = 0; i < EMC.breakRoomEmployeeSpots.Length; i++)
            {
                if (EMC.breakRoomEmployeeSpots[i] == null)
                {
                    targetPosition = i;
                    EMC.breakRoomEmployeeSpots[i] = GetComponent<EmployeeScript>();
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        else if (prevNode == waterCooler)
        {
            for (int i = 0; i < EMC.waterCoolerEmployeeSpots.Length; i++)
            {
                if (EMC.waterCoolerEmployeeSpots[i] == null)
                {
                    targetPosition = i + 27;
                    EMC.waterCoolerEmployeeSpots[i] = GetComponent<EmployeeScript>();
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        else if (prevNode == bossOffice)
        {
            targetPosition = 8;
            movingToQueuePosition = true;
        }
        else if (prevNode == conferenceRoom)
        {
            for (int i = 0; i < EMC.conferenceRoomEmployeeSpots.Length; i++)
            {
                if (EMC.conferenceRoomEmployeeSpots[i] == null)
                {
                    targetPosition = i + 9;
                    EMC.conferenceRoomEmployeeSpots[i] = GetComponent<EmployeeScript>();
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        else if (prevNode == stockRoom)
        {
            for (int i = 0; i < EMC.stockRoomEmployeeSpots.Length; i++)
            {
                if (EMC.stockRoomEmployeeSpots[i] == null)
                {
                    targetPosition = i + 23;
                    EMC.stockRoomEmployeeSpots[i] = GetComponent<EmployeeScript>();
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        else if (prevNode == restRoom)
        {
            for (int i = 0; i < EMC.restRoomEmployeeSpots.Length; i++)
            {
                if (EMC.restRoomEmployeeSpots[i] == null)
                {
                    targetPosition = i + 21;
                    EMC.restRoomEmployeeSpots[i] = GetComponent<EmployeeScript>();
                    break;
                }
            }
            movingToQueuePosition = true;
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
            targetPosition = workStation;
            decisionTime = Random.Range(10f, 30f);
        }
        else if (chosen == true)
            {
            if (Rando == 1 && EMC.breakRoomQueue < 8)
            {
                targetPosition = breakRoom;
                decisionTime = Random.Range(15f, 20f);
            }
            else if (Rando == 2 && EMC.waterCoolerQueue < 5)
            {
                targetPosition = waterCooler;
                decisionTime = Random.Range(15f, 20f);
            }
            else if (Rando == 3 && EMC.bossOfficeQueue < 1)
            {
                targetPosition = bossOffice;
                decisionTime = Random.Range(30f, 50f);
            }
            else if (Rando == 4 && EMC.conferenceRoomQueue < 12)
            {
                targetPosition = conferenceRoom;
                decisionTime = Random.Range(10f, 15f);
            }
            else if (Rando == 5 && EMC.stockRoomQueue < 4)
            {
                targetPosition = stockRoom;
                decisionTime = Random.Range(3f, 7f);
            }
            else if (Rando == 6 && EMC.restRoomQueue < 2)
            {
                targetPosition = restRoom;
                decisionTime = Random.Range(10f, 30f);
            }
            else
            {
                decisionTime = 0.001f;
                StartCoroutine("Behaviors");
                yield break;
            }
        }
        else
        {
            decisionTime = 0.001f;
            StartCoroutine("Behaviors");
            yield break;
        }

        if (prevNode == targetPosition)
        {
            decisionTime = 0.001f;
            StartCoroutine("Behaviors");
            yield break;
        }
        else
        {
            prevRando = Rando;
            chosen = false;
            movingToQueuePosition = false;

            if (targetPosition == breakRoom)
            {
                EMC.breakRoomQueue += 1;
            }
            else if (targetPosition == restRoom)
            {
                EMC.restRoomQueue += 1;
            }
            else if (targetPosition == conferenceRoom)
            {
                EMC.conferenceRoomQueue += 1;
            }
            else if (targetPosition == waterCooler)
            {
                EMC.waterCoolerQueue += 1;
            }
            else if (targetPosition == bossOffice)
            {
                EMC.bossOfficeQueue += 1;
            }
            else if (targetPosition == stockRoom)
            {
                EMC.stockRoomQueue += 1;
            }

            PathFinder.instance.FindShortestPathOfPoints(transform.position, Positions[targetPosition].position, PathLineType.CatmullRomCurve, Execution.Synchronous,
                SearchMode.Simple,
                delegate (List<Vector3> thepoints)
                {
                    OnPathFound(thepoints);
                }
            );

            if (prevNode == breakRoom)
            {
                EMC.breakRoomQueue -= 1;
                for (int i = 0; i < EMC.breakRoomEmployeeSpots.Length; i++)
                {
                    if (EMC.breakRoomEmployeeSpots[i] == GetComponent<EmployeeScript>())
                    {
                        EMC.breakRoomEmployeeSpots[i] = null;
                        break;
                    }
                }
            }
            else if (prevNode == restRoom)
            {
                EMC.restRoomQueue -= 1;
                for (int i = 0; i < EMC.restRoomEmployeeSpots.Length; i++)
                {
                    if (EMC.restRoomEmployeeSpots[i] == GetComponent<EmployeeScript>())
                    {
                        EMC.restRoomEmployeeSpots[i] = null;
                        break;
                    }
                }
            }
            else if (prevNode == conferenceRoom)
            {
                EMC.conferenceRoomQueue -= 1;
                for (int i = 0; i < EMC.conferenceRoomEmployeeSpots.Length; i++)
                {
                    if (EMC.conferenceRoomEmployeeSpots[i] == GetComponent<EmployeeScript>())
                    {
                        EMC.conferenceRoomEmployeeSpots[i] = null;
                        break;
                    }
                }
            }
            else if (prevNode == waterCooler)
            {
                EMC.waterCoolerQueue -= 1;
                for (int i = 0; i < EMC.waterCoolerEmployeeSpots.Length; i++)
                {
                    if (EMC.waterCoolerEmployeeSpots[i] == GetComponent<EmployeeScript>())
                    {
                        EMC.waterCoolerEmployeeSpots[i] = null;
                        break;
                    }
                }
            }
            else if (prevNode == bossOffice)
            {
                EMC.bossOfficeQueue -= 1;
            }
            else if (prevNode == stockRoom)
            {
                EMC.stockRoomQueue -= 1;
                for (int i = 0; i < EMC.stockRoomEmployeeSpots.Length; i++)
                {
                    if (EMC.stockRoomEmployeeSpots[i] == GetComponent<EmployeeScript>())
                    {
                        EMC.stockRoomEmployeeSpots[i] = null;
                    }
                }
            }
        }

        if (prevNode != workStation && targetPosition != workStation)
        {
            firstRun = true;
        }
    }

    void OnPathFound(List<Vector3> points)
    {
        isMoving = true;
        chosen = false;
        PathFollowerUtility.FollowPath(this.transform, points, speed, false);
    }

    #endregion
}
