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
    private int prevPrevNode = 24;
    private int targetNode;
    private int targetPosition;
    private Transform[] Positions = new Transform[22];
    private Transform[] QueuePositions = new Transform[33];
    private float speed;
    private EmployeeMasterControl EMC;
    private int currentQueuePosition;
    public bool movingToQueuePosition = false;
    private Rigidbody2D rb;
    public bool chosen = false; 
    public bool isMoving = false;

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
            if (collision.gameObject.GetComponent<Node>().nodeNum == targetNode)
            {
                prevNode = targetNode;
                chosen = false;
                isMoving = false;
                StartCoroutine("Behaviors");
                if (prevNode == breakRoom)
                {
                    targetPosition = currentQueuePosition - 1;
                    movingToQueuePosition = true;
                }
                else if (prevNode == waterCooler)
                {
                    targetPosition = currentQueuePosition + 26;
                    movingToQueuePosition = true;
                }
                else if (prevNode == bossOffice)
                {
                    targetPosition = 8;
                    movingToQueuePosition = true;
                }
                else if (prevNode == conferenceRoom)
                {
                    targetPosition = currentQueuePosition + 8;
                    movingToQueuePosition = true;
                }
                else if (prevNode == stockRoom)
                {
                    targetPosition = currentQueuePosition + 22;
                    movingToQueuePosition = true;
                }
                else if (prevNode == restRoom)
                {
                    targetPosition = currentQueuePosition + 20;
                    movingToQueuePosition = true;
                }
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
            if (workStation < 13)
            {
                targetNode = workStation + 1;
            }
            else
            {
                targetNode = workStation;
            }
            targetPosition = workStation;
            decisionTime = Random.Range(10f, 30f);
        }
        else if (chosen == true)
            {
            if (Rando == 1 && EMC.breakRoomQueue < 8)
            {
                targetNode = breakRoom;
                targetPosition = 17;
                decisionTime = Random.Range(15f, 20f);
            }
            else if (Rando == 2 && EMC.waterCoolerQueue < 6)
            {
                targetNode = waterCooler;
                targetPosition = 19;
                decisionTime = Random.Range(15f, 20f);
            }
            else if (Rando == 3 && EMC.bossOfficeQueue < 1)
            {
                targetNode = bossOffice;
                targetPosition = 18;
                decisionTime = Random.Range(30f, 50f);
            }
            else if (Rando == 4 && EMC.conferenceRoomQueue < 12)
            {
                targetNode = conferenceRoom;
                targetPosition = 16;
                decisionTime = Random.Range(10f, 15f);
            }
            else if (Rando == 5 && EMC.stockRoomQueue < 4)
            {
                targetNode = stockRoom;
                targetPosition = 21;
                decisionTime = Random.Range(3f, 7f);
            }
            else if (Rando == 6 && EMC.restRoomQueue < 2)
            {
                targetNode = restRoom;
                targetPosition = 20;
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

        if (prevNode == targetNode)
        {
            decisionTime = 0.001f;
            StartCoroutine("Behaviors");
            yield break;
        }
        else
        {
            chosen = false;
            movingToQueuePosition = false;
            PathFinder.instance.FindShortestPathOfPoints(transform.position, Positions[targetPosition].position, PathLineType.CatmullRomCurve, Execution.Synchronous,
                SearchMode.Simple,
                delegate (List<Vector3> thepoints)
                {
                    OnPathFound(thepoints);
                }
            );
        }

        if (prevNode != workStation && targetNode != workStation)
        {
            firstRun = true;
        }
    }

    void OnPathFound(List<Vector3> points)
    {
        isMoving = true;
        chosen = false;
        PathFollowerUtility.FollowPath(this.transform, points, speed, false);
        if (targetNode == breakRoom)
        {
            EMC.breakRoomQueue += 1;
            currentQueuePosition = EMC.breakRoomQueue;
            Debug.Log("Break Room Queue: " + EMC.breakRoomQueue);
        }
        else if (targetNode == restRoom)
        {
            EMC.restRoomQueue += 1;
            currentQueuePosition = EMC.restRoomQueue;
            Debug.Log("rest room Queue: " + EMC.restRoomQueue);
        }
        else if (targetNode == conferenceRoom)
        {
            EMC.conferenceRoomQueue += 1;
            currentQueuePosition = EMC.conferenceRoomQueue;
            Debug.Log("conference Room Queue: " + EMC.conferenceRoomQueue);
        }
        else if (targetNode == waterCooler)
        {
            EMC.waterCoolerQueue += 1;
            currentQueuePosition = EMC.waterCoolerQueue;
            Debug.Log("water cooler Queue: " + EMC.waterCoolerQueue);
        }
        else if (targetNode == bossOffice)
        {
            EMC.bossOfficeQueue += 1;
            currentQueuePosition = EMC.bossOfficeQueue;
            Debug.Log("boss office Queue: " + EMC.bossOfficeQueue);
        }
        else if (targetNode == stockRoom)
        {
            EMC.stockRoomQueue += 1;
            currentQueuePosition = EMC.stockRoomQueue;
            Debug.Log("stock Room Queue: " + EMC.stockRoomQueue);
        }

        if (prevNode == breakRoom)
        {
            EMC.breakRoomQueue -= 1;
            Debug.Log("Break Room Queue: " + EMC.breakRoomQueue);
        }
        else if (prevNode == restRoom)
        {
            EMC.restRoomQueue -= 1;
            Debug.Log("rest room Queue: " + EMC.restRoomQueue);
        }
        else if (prevNode == conferenceRoom)
        {
            EMC.conferenceRoomQueue -= 1;
            Debug.Log("conference Room Queue: " + EMC.conferenceRoomQueue);
        }
        else if (prevNode == waterCooler)
        {
            EMC.waterCoolerQueue -= 1;
            Debug.Log("water cooler Queue: " + EMC.waterCoolerQueue);
        }
        else if (prevNode == bossOffice)
        {
            EMC.bossOfficeQueue -= 1;
            Debug.Log("boss office Queue: " + EMC.bossOfficeQueue);
        }
        else if (prevNode == stockRoom)
        {
            EMC.stockRoomQueue -= 1;
            Debug.Log("stock Room Queue: " + EMC.stockRoomQueue);
        }
        
    }

    #endregion
}
