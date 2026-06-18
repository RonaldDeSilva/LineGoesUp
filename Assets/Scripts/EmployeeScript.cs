using QPathFinder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeScript : MonoBehaviour
{
    public float decisionTime = 0.001f;
    //
    public new string name;
    public int age = 20;
    public int happiness;
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
    public bool movingToQueuePosition = false;
    private Rigidbody2D rb;
    public bool chosen = false; 
    public bool isMoving = false;
    public bool hasQuery = false;
    public GameObject QueryIcon;
    private GameObject redEyes;
    public bool controlled;
    public bool workingControlled;
    public GameObject computerScreen;
    private GameObject EmployeeInfo;
    public Sprite[] EmployeeSprites;
    private string currentAnimation = null;
    public float animationSpeed;
    private float animTimer;
    private Vector2 curPos;
    private Vector2 prevPos = Vector2.zero;

    //Specialization stuff

    public string SpecialtyName;
    public float SpecialtyLevel;

    public string projectAssignment = "";

    #region Awaken

    public void Awaken(EmployeeMasterControl EmpMasCon)
    {
        QueryIcon = transform.GetChild(1).gameObject;
        QueryIcon.SetActive(false);
        EmployeeInfo = GameObject.FindGameObjectWithTag("MasterCanvas");
        EMC = EmpMasCon;
        transform.position = new Vector3(6.290487f, 2.859212f, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        var nodesPos = GameObject.FindGameObjectWithTag("Nodes");
        redEyes = transform.GetChild(2).gameObject;
        redEyes.SetActive(false);
        computerScreen = GameObject.FindGameObjectWithTag("Computer");
        animTimer = 30;
        for (int i = 0; i < nodesPos.transform.childCount; i++)
        {
            Positions[i] = nodesPos.transform.GetChild(i);
        }

        var queuePos = GameObject.Find("QueuePositions");
        QueuePositions = new Transform[queuePos.transform.childCount];
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

    #endregion

    #region Navigation

    private void Update()
    {
        if (workingControlled && !controlled)
        {
            workingControlled = false;
        }

        if (movingToQueuePosition && !Approx.FastApp(transform.position.x, QueuePositions[targetPosition].position.x, 0.1f) && !Approx.FastApp(transform.position.y, QueuePositions[targetPosition].position.y, 0.1f))
        {
            rb.linearVelocityX = (QueuePositions[targetPosition].position.x - transform.position.x) * speed * Time.deltaTime * 100;
            rb.linearVelocityY = (QueuePositions[targetPosition].position.y - transform.position.y) * speed * Time.deltaTime * 100;
        }
        else if (movingToQueuePosition && Approx.FastApp(transform.position.x, QueuePositions[targetPosition].position.x, 0.1f) && Approx.FastApp(transform.position.y, QueuePositions[targetPosition].position.y, 0.1f))
        {
            rb.linearVelocity = Vector2.zero;
            StartCoroutine("Behaviors");
            movingToQueuePosition = false;
            currentAnimation = "Sitting";
        }

        if (controlled && !workingControlled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.linearVelocityY += speed / 500;
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.linearVelocityX -= speed / 500;
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.linearVelocityY -= speed / 500;
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.linearVelocityX += speed / 500;
            }

            Mathf.Clamp(rb.linearVelocityX, -speed / 50, speed / 50);
            Mathf.Clamp(rb.linearVelocityY, -speed / 50, speed / 50);

            if (!Input.anyKey)
            {
                rb.linearVelocityX -= rb.linearVelocityX / speed;
                rb.linearVelocityY -= rb.linearVelocityY / speed;
            }
        }

        if (currentAnimation != null)
        {
            if (currentAnimation == "Walking")
            {
                if (animTimer >= 33)
                {
                    if (GetComponent<SpriteRenderer>().sprite == EmployeeSprites[8])
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[9];
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[8];
                    }
                    curPos = transform.position;
                    if (prevPos == Vector2.zero)
                    {
                        prevPos = curPos;
                    }
                    else
                    {
                        if (curPos != prevPos)
                        {
                            Debug.Log(Mathf.Abs(curPos.x - prevPos.x) + " " + Mathf.Abs(curPos.y - prevPos.y) + " " + (Mathf.Abs(curPos.x - prevPos.x) > Mathf.Abs(curPos.y - prevPos.y)).ToString());
                            if (Mathf.Abs(curPos.x - prevPos.x) > Mathf.Abs(curPos.y - prevPos.y))
                            {
                                if (curPos.x > prevPos.x)
                                {
                                    transform.eulerAngles = new Vector3(0, 0, 90);
                                }
                                else
                                {
                                    transform.eulerAngles = new Vector3(0, 0, 270);
                                }
                            }
                            else if (Mathf.Abs(curPos.x - prevPos.x) < Mathf.Abs(curPos.y - prevPos.y))
                            {
                                if (curPos.y > prevPos.y)
                                {
                                    transform.eulerAngles = new Vector3(0, 0, 180);
                                }
                                else
                                {
                                    transform.eulerAngles = new Vector3(0, 0, 0);
                                }
                            }
                            else
                            {
                                if (curPos.x > prevPos.x)
                                {
                                    transform.eulerAngles = new Vector3(0, 0, 90);
                                }
                                else
                                {
                                    transform.eulerAngles = new Vector3(0, 0, 270);
                                }
                            }

                                prevPos = curPos;
                        }
                    }
                    animTimer = 0;
                }

                
            }
            else if (currentAnimation == "Working")
            {
                if (animTimer >= 35)
                {
                    if (GetComponent<SpriteRenderer>().sprite == EmployeeSprites[6])
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[7];
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[6];
                    }

                    if (workStation < 6 || workStation > 11 && workStation < 14)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 325);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 140);
                    }
                    
                    animTimer = 0;
                }
            }
            else if (currentAnimation == "Sitting")
            {
                if (animTimer >= 45)
                {
                    if (GetComponent<SpriteRenderer>().sprite == EmployeeSprites[4])
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[5];
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[4];
                    }

                    if (prevNode == conferenceRoom)
                    {
                        if (workStation == 0 || workStation > 13)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 90);
                        }
                        else if (workStation < 3)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 180);
                        }
                        else if (workStation > 11 && workStation < 14)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(0, 0, 270);
                        }
                    }
                    else if (prevNode == breakRoom)
                    {
                        if (targetPosition == 0 ||  targetPosition == 5)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else if (targetPosition == 1 || targetPosition == 4)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 90);
                        }
                        else if (targetPosition == 2 || targetPosition == 7)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 180);
                        }
                        else if (targetPosition == 3 || targetPosition == 6)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 270);
                        }
                    }
                    else if (prevNode == restRoom)
                    {
                        
                        transform.eulerAngles = new Vector3(0, 0, 270);

                    }
                    else if (prevNode == waterCooler)
                    {

                        transform.eulerAngles = new Vector3(0, 0, 90);

                    }
                    else if (prevNode == stockRoom)
                    {

                        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 359));

                    }
                    animTimer = 0;
                }
            }
            else if (currentAnimation == "Idle")
            {
                if (animTimer >= 60)
                {
                    if (GetComponent<SpriteRenderer>().sprite == EmployeeSprites[2])
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[3];
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = EmployeeSprites[2];
                    }

                    transform.eulerAngles = new Vector3(0, 0, 0);
                    animTimer = 0;
                }
            }
            animTimer += Time.deltaTime * speed * 50;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            if (collision.gameObject.GetComponent<Node>().nodeNum == targetPosition && !controlled && !movingToQueuePosition)
            {
                if (targetPosition == bossOffice)
                {
                    prevNode = targetPosition;
                    chosen = false;
                    isMoving = false;
                    hasQuery = true;
                    QueryIcon.SetActive(true);
                    StartCoroutine("Behaviors");
                    StartCoroutine("WaitASec");
                    currentAnimation = "Idle";
                    return;
                }

                if (targetPosition == workStation)
                {
                    prevNode = targetPosition;
                    chosen = false;
                    isMoving = false;
                    StartCoroutine("Behaviors");
                    currentAnimation = "Working";
                    return;
                }

                prevNode = targetPosition;
                chosen = false;
                isMoving = false;
                StartCoroutine("WaitASec");
            }

            if (controlled)
            {
                prevNode = collision.gameObject.GetComponent<Node>().nodeNum;
                chosen = false;
                isMoving = false;

                if (collision.gameObject.GetComponent<Node>().nodeNum == workStation)
                {
                    workingControlled = true;
                    EmployeeInfo.transform.GetChild(1).localPosition = new Vector2(-704f, -374.16f);
                    computerScreen.transform.GetChild(0).gameObject.SetActive(true);
                    EmployeeInfo.transform.GetChild(1).GetChild(5).GetComponent<Button>().interactable = false;
                }
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
                if (EMC.breakRoomEmployeeSpots[i] == this.gameObject)
                {
                    targetPosition = i;
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        else if (prevNode == waterCooler)
        {
            for (int i = 0; i < EMC.waterCoolerEmployeeSpots.Length; i++)
            {
                if (EMC.waterCoolerEmployeeSpots[i] == this.gameObject)
                {
                    targetPosition = i + 31;
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
        else if (prevNode == stockRoom)
        {
            for (int i = 0; i < EMC.stockRoomEmployeeSpots.Length; i++)
            {
                if (EMC.stockRoomEmployeeSpots[i] == this.gameObject)
                {
                    targetPosition = i + 27;
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        else if (prevNode == restRoom)
        {
            for (int i = 0; i < EMC.restRoomEmployeeSpots.Length; i++)
            {
                if (EMC.restRoomEmployeeSpots[i] == this.gameObject)
                {
                    targetPosition = i + 25;
                    break;
                }
            }
            movingToQueuePosition = true;
        }
        
        if (prevNode == 16)
        {
            for (int i = 0; i < EMC.conferenceRoomEmployeeSpots.Length; i++)
            {
                if (EMC.conferenceRoomEmployeeSpots[i] == this.gameObject)
                {
                    targetPosition = workStation + 9;
                    movingToQueuePosition = true;
                    break;
                }
                movingToQueuePosition = true;
            }
            movingToQueuePosition = true;
        }
    }

    #endregion

    #region Behaviors

    IEnumerator Behaviors()
    {
        //potential behaviors: Working, Hungry, Thirsty, Talk with Boss, Conference, Printing, Gotta pee 
        yield return new WaitForSeconds(decisionTime);
        if (!hasQuery && !EMC.QueueQueryRunning)
        {
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
                if (prevNode == targetPosition)
                {
                    decisionTime = 0.001f;
                    StartCoroutine("Behaviors");
                    yield break;
                }
            }
            else if (chosen == true)
            {
                if (Rando == 1)
                {
                    targetPosition = breakRoom;
                    decisionTime = Random.Range(15f, 20f);
                    if (prevNode == targetPosition)
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                    else if (!EMC.QueueQuery("BreakRoom", this.gameObject))
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                }
                else if (Rando == 2)
                {
                    targetPosition = waterCooler;
                    decisionTime = Random.Range(15f, 20f);
                    if (prevNode == targetPosition)
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                    else if (!EMC.QueueQuery("WaterCooler", this.gameObject))
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                }
                else if (Rando == 3)
                {
                    targetPosition = bossOffice;
                    decisionTime = Random.Range(0.1f, 1f);
                    if (prevNode == targetPosition)
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                    else if (!EMC.QueueQuery("BossOffice", this.gameObject))
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                }
                else if (Rando == 4)
                {
                    targetPosition = conferenceRoom;
                    decisionTime = Random.Range(10f, 15f);
                    if (prevNode == targetPosition)
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                    else if (!EMC.QueueQuery("ConferenceRoom", this.gameObject))
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                }
                else if (Rando == 5)
                {
                    targetPosition = stockRoom;
                    decisionTime = Random.Range(3f, 7f);
                    if (prevNode == targetPosition)
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                    else if (!EMC.QueueQuery("StockRoom", this.gameObject))
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                }
                else if (Rando == 6)
                {
                    targetPosition = restRoom;
                    decisionTime = Random.Range(10f, 30f);
                    if (prevNode == targetPosition)
                    {
                        decisionTime = 0.001f;
                        StartCoroutine("Behaviors");
                        yield break;
                    }
                    else if (!EMC.QueueQuery("RestRoom", this.gameObject))
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
            }
            else
            {
                decisionTime = 0.001f;
                StartCoroutine("Behaviors");
                yield break;
            }
            chosen = false;
            movingToQueuePosition = false;

            PathFinder.instance.FindShortestPathOfPoints(transform.position, Positions[targetPosition].position, PathLineType.CatmullRomCurve, Execution.Synchronous,
                SearchMode.Simple,
                delegate (List<Vector3> thepoints)
                {
                    OnPathFound(thepoints);
                }
            );
            StartCoroutine("LeavePrevQueue");
            currentAnimation = "Walking";

            if (prevNode != workStation && targetPosition != workStation)
            {
                firstRun = true;
            }
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            if (hasQuery)
            {
                yield return new WaitForSeconds(1f);
            }
            decisionTime = 0.001f;
            StartCoroutine("Behaviors");
        }
    }

    IEnumerator LeavePrevQueue()
    {
        if (!EMC.QueueLeaveRunning)
        {
            if (prevNode == breakRoom)
            {
                EMC.QueueLeave("BreakRoom", this.gameObject);
                yield break;
            }
            else if (prevNode == restRoom)
            {
                EMC.QueueLeave("RestRoom", this.gameObject);
                yield break;
            }
            else if (prevNode == conferenceRoom)
            {
                EMC.QueueLeave("ConferenceRoom", this.gameObject);
                yield break;
            }
            else if (prevNode == waterCooler)
            {
                EMC.QueueLeave("WaterCooler", this.gameObject);
                yield break;
            }
            else if (prevNode == stockRoom)
            {
                EMC.QueueLeave("StockRoom", this.gameObject);
                yield break;
            }
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.01f, 0.3f));
            StartCoroutine("LeavePrevQueue");
        }
    }

    IEnumerator LeaveCurrentQueue()
    {
        if (!EMC.QueueLeaveRunning)
        {
            if (targetPosition == breakRoom)
            {
                EMC.QueueLeave("BreakRoom", this.gameObject);
                yield break;
            }
            else if (targetPosition == restRoom)
            {
                EMC.QueueLeave("RestRoom", this.gameObject);
                yield break;
            }
            else if (targetPosition == conferenceRoom)
            {
                EMC.QueueLeave("ConferenceRoom", this.gameObject);
                yield break;
            }
            else if (targetPosition == waterCooler)
            {
                EMC.QueueLeave("WaterCooler", this.gameObject);
                yield break;
            }
            else if (targetPosition == stockRoom)
            {
                EMC.QueueLeave("StockRoom", this.gameObject);
                yield break;
            }
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.01f, 0.3f));
            StartCoroutine("LeaveCurrentQueue");
        }
    }

    void OnPathFound(List<Vector3> points)
    {
        isMoving = true;
        chosen = false;
        PathFollowerUtility.FollowPath(transform, points, speed, false);
    }

    public void StartControl()
    {
        PathFollowerUtility.StopFollowing(transform);
        PathFollowerUtility.StopFollowing(transform);
        rb.bodyType = RigidbodyType2D.Dynamic;
        movingToQueuePosition = false;
        redEyes.gameObject.SetActive(true);
        controlled = true;
        StartCoroutine("LeaveCurrentQueue");
    }

    public void EndControl()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        controlled = false;
        decisionTime = 0.001f;
        redEyes.SetActive(false);
        chosen = false;
        isMoving = false;
        workingControlled = false;
        StartCoroutine("Behaviors");
    }

    #endregion
}
