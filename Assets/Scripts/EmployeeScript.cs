using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class EmployeeScript : MonoBehaviour
{
    public float decisionTime = 0;
    public new string name;
    public int age;
    public string reasonTermination;
    public float experience;
    public int productivity;
    public int loyalty;
    public int happiness;
    public int burnout;
    public Transform[] nodes;
    public int targetNode;
    public int waterCooler;
    public int breakRoom;
    public int restRoom;
    public int stockRoom;
    public int conferenceRoom;
    public int workStation;
    public int bossOffice;
    public int currentBehaviour = 0;
    public int HallwayNode1;
    public int HallwayNode2;
    public int HallwayNode3;
    public int HallwayNode4;
    public int entranceNode;
    private int speed;
    private bool arrived = false;
    private bool topHalfCubicle = false;
    public int currentNode;
    private bool firstRun = true;

    public void Awaken()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1);
        HallwayNode1 = 12;
        HallwayNode2 = 13;
        HallwayNode3 = 14;
        HallwayNode4 = 15;
        currentNode = entranceNode;
        transform.position = nodes[entranceNode].position;
        if (age < 35)
        {
            speed = 5;
        }
        else if (age < 50)
        {
            speed = 4;
        }
        else if (age < 100)
        {
            speed = 3;
        }
        else
        {
            speed = 1;
        }

        if (workStation == 0 || workStation == 1 || workStation == 2 || workStation == 3 || workStation == 4 || workStation == 5)
        {
            topHalfCubicle = true;
        }
        StartCoroutine("Behaviors");
    }

    public void Update()
    {
        #region Working
        if (currentBehaviour == 0)
        {
            if (!arrived)
            {
                if (transform.position.y > nodes[HallwayNode1].position.y)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode1].position.y - 0.1f, transform.position.z), Time.deltaTime * speed);
                }
                else if (transform.position.y < nodes[HallwayNode4].position.y)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode4].position.y + 0.1f, transform.position.z), Time.deltaTime * speed);
                }
                else
                {
                    if (topHalfCubicle)
                    {
                        if (Approx.FastApp(transform.position.y, nodes[HallwayNode1].position.y, 0.5f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[targetNode].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        }
                        else if (!Approx.FastApp(transform.position.x, nodes[HallwayNode3].position.x, 0.1f) && transform.position.y < nodes[targetNode].position.y)
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[HallwayNode3].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        }
                        else if (transform.position.y < nodes[HallwayNode1].position.y - 0.5f && Approx.FastApp(transform.position.x, nodes[HallwayNode3].position.x, 0.2f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode2].position.y, transform.position.z), Time.deltaTime * speed);
                        }

                        if (transform.position.y > nodes[targetNode].position.y &&  Approx.FastApp(transform.position.x, nodes[targetNode].position.x, 0.1f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[targetNode].position.y, transform.position.z), Time.deltaTime * speed);
                        }

                        if (Approx.FastApp(transform.position.y, nodes[workStation].position.y, 0.1f) && Approx.FastApp(transform.position.x, nodes[workStation].position.x, 0.75f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[targetNode].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        }
                    }
                    else
                    {
                        if (Approx.FastApp(transform.position.y, nodes[HallwayNode4].position.y, 0.5f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[targetNode].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        }
                        else if (!Approx.FastApp(transform.position.x, nodes[HallwayNode2].position.x, 0.1f) && transform.position.y > nodes[targetNode].position.y)
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[HallwayNode2].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        }
                        else if (transform.position.y > nodes[HallwayNode4].position.y + 0.5f && Approx.FastApp(transform.position.x, nodes[HallwayNode2].position.x, 0.2f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode3].position.y, transform.position.z), Time.deltaTime * speed);
                        }

                        if (transform.position.y < nodes[targetNode].position.y && Approx.FastApp(transform.position.x, nodes[targetNode].position.x, 0.1f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[targetNode].position.y, transform.position.z), Time.deltaTime * speed);
                        }

                        if (Approx.FastApp(transform.position.y, nodes[workStation].position.y, 0.1f) && Approx.FastApp(transform.position.x, nodes[workStation].position.x, 0.75f))
                        {
                            transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[targetNode].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        }
                    }
                }
            }
        }
        #endregion

        #region Hungry going to break room

        if (currentBehaviour == 1)
        {
            if (!arrived)
            {
                if (transform.position.y > nodes[HallwayNode3].position.y)
                {
                    if (transform.position.y > nodes[HallwayNode1].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode1].position.y, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("1");
                    }
                    else if (currentNode == workStation && Approx.FastApp(transform.position.x, nodes[workStation].position.x, 0.1f) && topHalfCubicle && transform.position.y < nodes[HallwayNode1].position.y && !Approx.FastApp(transform.position.y, nodes[HallwayNode4].position.y, 0.1f))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode1].position.y, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("2");
                    }
                    else if (currentNode == workStation && Approx.FastApp(transform.position.x, nodes[workStation].position.x, 0.1f) && !topHalfCubicle && transform.position.y > nodes[HallwayNode4].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode4].position.y - 0.1f, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("3");
                    }
                    else if (Approx.FastApp(transform.position.y, nodes[HallwayNode1].position.y, 0.3f) && !Approx.FastApp(transform.position.x, nodes[HallwayNode2].position.x, 0.05f))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[HallwayNode2].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("4");
                    }
                    else if (Approx.FastApp(transform.position.x, nodes[HallwayNode2].position.x, 0.1f) && transform.position.y > nodes[HallwayNode3].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode3].position.y - 0.1f, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("5");
                    }

                }
                else if (!Approx.FastApp(transform.position.y, nodes[HallwayNode3].position.y, 0.3f) && !Approx.FastApp(transform.position.x, nodes[targetNode].position.x, 0.1f))
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode3].position.y + 0.1f, transform.position.z), Time.deltaTime * speed);
                    Debug.Log("6");
                }
                else
                {
                    if (!Approx.FastApp(transform.position.x, nodes[targetNode].position.x, 0.1f))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[targetNode].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("7");
                    }
                    else if (transform.position.y > nodes[targetNode].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[targetNode].position.y - 0.1f, transform.position.z), Time.deltaTime * speed);
                        Debug.Log("8");
                    }
                }
            }
        }

        #endregion

        #region Thirsty going to water cooler

        if (currentBehaviour == 2)
        {
            if (!arrived)
            {
                if (transform.position.y < nodes[HallwayNode2].position.y)
                {
                    if (transform.position.y < nodes[HallwayNode4].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode4].position.y + 0.1f, transform.position.z), Time.deltaTime * speed);
                    }
                    else if (currentNode == workStation && Approx.FastApp(transform.position.x, nodes[workStation].position.x, 0.1f) && !topHalfCubicle && transform.position.y > nodes[HallwayNode4].position.y && !Approx.FastApp(transform.position.y, nodes[HallwayNode1].position.y, 0.1f))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode4].position.y - 0.1f, transform.position.z), Time.deltaTime * speed);
                    }
                    else if (currentNode == workStation && Approx.FastApp(transform.position.x, nodes[workStation].position.x, 0.1f) && topHalfCubicle && transform.position.y < nodes[HallwayNode1].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode1].position.y + 0.1f, transform.position.z), Time.deltaTime * speed);
                    }
                    else if (Approx.FastApp(transform.position.y, nodes[HallwayNode4].position.y, 0.3f) && !Approx.FastApp(transform.position.x, nodes[HallwayNode3].position.x, 0.05f))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[HallwayNode3].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                    }
                    else if (Approx.FastApp(transform.position.x, nodes[HallwayNode3].position.x, 0.1f) && transform.position.y < nodes[HallwayNode2].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode2].position.y + 0.1f, transform.position.z), Time.deltaTime * speed);
                    }
                }
                else if (!Approx.FastApp(transform.position.y, nodes[HallwayNode2].position.y, 0.3f) && !Approx.FastApp(transform.position.x, nodes[targetNode].position.x, 0.1f))
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[HallwayNode2].position.y + 0.1f, transform.position.z), Time.deltaTime * speed);
                }
                else
                {
                    if (!Approx.FastApp(transform.position.x, nodes[targetNode].position.x, 0.1f))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(nodes[targetNode].position.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                    }
                    else if (transform.position.y < nodes[targetNode].position.y)
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, nodes[targetNode].position.y - 0.1f, transform.position.z), Time.deltaTime * speed);
                    }
                }
            }
        }

        #endregion
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            currentNode = collision.gameObject.GetComponent<Node>().nodeNum;
            if (currentNode == targetNode)
            {
                arrived = true;
                decisionTime = Random.Range(4f, 11f);
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
            Rando = Random.Range(0, 3);
        }

        if (Rando == 0)
        {
            currentBehaviour = 0;
            targetNode = workStation;
        }
        else if (Rando == 1)
        {
            currentBehaviour = 1;
            targetNode = breakRoom;
        }
        else if (Rando == 2)
        {
            currentBehaviour = 2;
            targetNode = waterCooler;
        }
        else if (Rando == 3)
        {
            currentBehaviour = 3;
            targetNode = bossOffice;
        }
        else if (Rando == 4)
        {
            currentBehaviour = 4;
            targetNode = conferenceRoom;
        }
        else if (Rando == 5)
        {
            currentBehaviour = 5;
            targetNode = stockRoom;
        }
        else if (Rando == 6)
        {
            currentBehaviour = 6;
            targetNode = restRoom;
        }

        if (currentNode == targetNode)
        {
            StartCoroutine("Behaviors");
            yield break;
        }

        yield return new WaitForSeconds(Rando);
        arrived = false;
    }

    #endregion
}
