using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class EmployeeMasterControl : MonoBehaviour
{
    #region Employee Definition

    //Employees Have a:
    // - Name
    // - Age
    // - Experience in position (Specialty)
    // - Happiness (Only revealed after hiring)

    #endregion
    //Employee stuff
    public GameObject EmployeeTemplate;
    public Sprite[] EmployeeSprites1;
    public Sprite[] EmployeeSprites2;
    public Sprite[] EmployeeSprites3;
    private string[] EmployeeFirstNames;
    private string[] EmployeeNickNames;
    private string[] EmployeeLastNames;
    public int[] EmployeeAges;
    public int[] EmployeeHappiness;
    public string[] EmployeeSpecialties;
    public float maxSpecialtyStartingLevel;
    //Hiring
    public int HiringPool;
    public GameObject HireScreen;
    public GameObject CurrentEmployeePool;
    public float HiringScreenAnimationSpeed;
    public List<GameObject> Employees = new List<GameObject>();
    public int numEmployees = 0;
    public int maxEmployees;
    private GameObject FiringScreen;
    private int moveTimer;
    private float animTimer;

    //Queue stuff
    public GameObject[] waterCoolerEmployeeSpots = new GameObject[5];
    public GameObject[] breakRoomEmployeeSpots = new GameObject[8];
    public GameObject[] stockRoomEmployeeSpots = new GameObject[4];
    public GameObject[] conferenceRoomEmployeeSpots = new GameObject[12];
    public GameObject[] restRoomEmployeeSpots = new GameObject[2];

    public bool bossOfficOccupied = false;
    public bool QueueQueryRunning = false;
    public bool QueueLeaveRunning = false;
    private int CurrentPage;
    public string buttonNum;
    public GameObject[] SeatingChart = new GameObject[16];
    private int MaxPages;

    void Start()
    {
        HireScreen = transform.GetChild(0).gameObject;
        FiringScreen = HireScreen.transform.GetChild(5).gameObject;
        EmployeeFirstNames = File.ReadAllLines("Assets\\EmployeeInfoDocs\\First Names.txt");
        EmployeeNickNames = File.ReadAllLines("Assets\\EmployeeInfoDocs\\Nick Names.txt");
        EmployeeLastNames = File.ReadAllLines("Assets\\EmployeeInfoDocs\\Last Names.txt");
    }

    private void Update()
    {

        if (HireScreen.activeSelf)
        {
            if (animTimer >= 30) 
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        if (HireScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite == HireScreen.transform.GetChild(i).GetChild(6).GetComponent<EmployeeScript>().EmployeeSprites[0])
                        {
                            HireScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = HireScreen.transform.GetChild(i).GetChild(6).GetComponent<EmployeeScript>().EmployeeSprites[1];
                        }
                        else
                        {
                            HireScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = HireScreen.transform.GetChild(i).GetChild(6).GetComponent<EmployeeScript>().EmployeeSprites[0];
                        }
                    }
                    catch { }
                }
                animTimer = 0;
            }
            animTimer += Time.deltaTime * Random.Range(1, 250);
        }

        if (moveTimer % 15 == 0 && Employees.Count > 0)
        {
            var ran = Random.Range(0, Employees.Count);
            if (!Employees[ran].GetComponent<EmployeeScript>().isMoving)
            {
                Employees[ran].GetComponent<EmployeeScript>().chosen = true;
            }
            else
            {
                for (int i = 0; i < Employees.Count; i++)
                {
                    if (!Employees[i].gameObject.GetComponent<EmployeeScript>().isMoving)
                    {
                        Employees[i].gameObject.GetComponent<EmployeeScript>().chosen = true;
                        break;
                    }
                }
            }
            
        }
        moveTimer++;
    }

    public void OnClick(Button button)
    {
        if (numEmployees < maxEmployees)
        {
            var emp = button.transform.GetChild(6).gameObject;
            emp.transform.parent = CurrentEmployeePool.transform;
            emp.transform.position = Vector3.zero;
            button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
            button.GetComponent<Button>().interactable = false;
            numEmployees++;
            for (int i = 0; i < SeatingChart.Length; i++)
            {
                if (SeatingChart[i] == null)
                {
                    SeatingChart[i] = emp;
                    emp.GetComponent<EmployeeScript>().workStation = i;
                    emp.GetComponent<EmployeeScript>().Awaken(this.GetComponent<EmployeeMasterControl>());
                    break;
                }
            }
            Employees.Add(emp);
        }
    }

    public void HireFireScreenButton()
    {
        if (!HireScreen.activeSelf)
        {
            HiringScreenFillUp(HiringPhase());
            HireScreen.SetActive(true);
            if (numEmployees == 0)
            {
                HireScreen.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                HireScreen.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Destroy(HireScreen.transform.GetChild(i).GetChild(6).gameObject);
                }
                catch { continue; }
            }
            HireScreen.SetActive(false);
        }
    }

    #region Firing Functions

    public void FireScreenButton()
    {
        if (!FiringScreen.activeSelf)
        {
            FiringScreen.SetActive(true);
            FiringScreen.transform.GetChild(0).gameObject.SetActive(true);
            CurrentPage = 0;
            MaxPages = numEmployees / 3;
            FiringScreen.transform.GetChild(6).GetComponent<Button>().interactable = false;
            if (numEmployees == 1)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false); 
                FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0].GetComponent<SpriteRenderer>().sprite;
                FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().name;
                FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0].GetComponent<EmployeeScript>().age + " Years Old";
                FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0].GetComponent<EmployeeScript>().SpecialtyName;
                FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
            }
            else if (numEmployees == 2)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {                    
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
            else if (numEmployees >= 3)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(true);

                if (numEmployees >= 4)
                {
                    FiringScreen.transform.GetChild(5).GetComponent<Button>().interactable = true;
                }

                for (int i = 0; i < 3; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
        }
        else
        {
            FiringScreen.SetActive(false);
        }
    }

    public void FireEmployeeButton(Button butt)
    {
        if (butt.CompareTag("Button1"))
        {
            if (CurrentPage == 1 && numEmployees % 3 == 1)
            {
                CurrentPage--;
                FiringScreen.transform.GetChild(6).GetComponent<Button>().interactable = false;
            }
            else if (CurrentPage > 1 && numEmployees % 3 == 1)
            {
                CurrentPage--;
            }
            Employees.Remove(Employees[0 + (CurrentPage * 3)]);
            Destroy(CurrentEmployeePool.transform.GetChild(0 + (CurrentPage * 3)).gameObject);
            numEmployees--;

            if (numEmployees == 0)
            {
                FiringScreen.transform.GetChild(0).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (numEmployees == 1)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0].GetComponent<SpriteRenderer>().sprite;
                FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().name;
                FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0].GetComponent<EmployeeScript>().age + " Years Old";
                FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0].GetComponent<EmployeeScript>().SpecialtyName;
                FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
            }
            else if (numEmployees == 2)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
            else if (numEmployees >= 3)
            {
                if (numEmployees == 3)
                {
                    FiringScreen.transform.GetChild(5).GetComponent<Button>().interactable = false;
                }
                
                if (numEmployees % 3 == 0)
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                    for (int i = 0; i < 3; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
                else if (numEmployees % 3 == 1 && CurrentPage == (int)(numEmployees / 3))
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                    FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0 + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
                else if (numEmployees % 3 == 2 && CurrentPage == (int)(numEmployees / 3))
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                    for (int i = 0; i < 2; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
                else
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                    for (int i = 0; i < 3; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
            }
        }
        if (butt.CompareTag("Button2"))
        {
            Employees.Remove(Employees[1 + (CurrentPage * 3)]);
            Destroy(CurrentEmployeePool.transform.GetChild(1 + (CurrentPage * 3)).gameObject);
            numEmployees--;

            if (numEmployees == 1)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0].GetComponent<SpriteRenderer>().sprite;
                FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().name;
                FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0].GetComponent<EmployeeScript>().age + " Years Old";
                FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0].GetComponent<EmployeeScript>().SpecialtyName;
                FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
            }
            else if (numEmployees == 2)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
            else if (numEmployees >= 3)
            {
                if (numEmployees % 3 == 0)
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                    for (int i = 0; i < 3; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
                else if (numEmployees % 3 == 1 && CurrentPage == (int)(numEmployees / 3))
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                    FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0 + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
                else if (numEmployees % 3 == 2 && CurrentPage == (int)(numEmployees / 3))
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                    for (int i = 0; i < 2; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
                else
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                    for (int i = 0; i < 3; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
            }
        }
        if (butt.CompareTag("Button3"))
        {
            Employees.Remove(Employees[2 + (CurrentPage * 3)]);
            Destroy(CurrentEmployeePool.transform.GetChild(2 + (CurrentPage * 3)).gameObject);
            numEmployees--;

            if (numEmployees == 2)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
            else if (numEmployees >= 3)
            {
                if (numEmployees % 3 == 0)
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                    for (int i = 0; i < 3; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
                else if (numEmployees % 3 == 1 && CurrentPage == (int)(numEmployees / 3))
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                    FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0 + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
                else if (numEmployees % 3 == 2 && CurrentPage == (int)(numEmployees / 3))
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                    for (int i = 0; i < 2; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
                else
                {
                    FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                    FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                    for (int i = 0; i < 3; i++)
                    {
                        FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                        FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                        FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                        FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                        FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                        FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                        FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    }
                }
            }
        }
    }

    public void ScrollFireScreen(bool right)
    {
        if (right)
        {
            CurrentPage++;
            FiringScreen.transform.GetChild(6).GetComponent<Button>().interactable = true;
            if (CurrentPage == MaxPages || CurrentPage == MaxPages - 1 && numEmployees % 3 == 0)
            {
                FiringScreen.transform.GetChild(5).GetComponent<Button>().interactable = false;
            }
            if (numEmployees % 3 == 1 && CurrentPage == (int)(numEmployees / 3))
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Employees[0 + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[0 + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
            }
            else if (numEmployees % 3 == 2 && CurrentPage == (int)(numEmployees / 3))
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
            else
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                }
            }
        }
        else
        {
            CurrentPage--;
            FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
            FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
            FiringScreen.transform.GetChild(5).GetComponent<Button>().interactable = true;

            if (CurrentPage == 0)
            {
                FiringScreen.transform.GetChild(6).GetComponent<Button>().interactable = false;
            }

            for (int i = 0; i < 3; i++)
            {
                FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Employees[i + (CurrentPage * 3)].GetComponent<SpriteRenderer>().sprite;
                FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().name;
                FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().age + " Years Old";
                FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyName;
                FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + Employees[i + (CurrentPage * 3)].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
                FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
            }
        }
    }


    #endregion

    #region Hiring Functions

    public void HiringScreenFillUp(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            HireScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = list[i].GetComponent<SpriteRenderer>().sprite;
            HireScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = list[i].GetComponent<EmployeeScript>().name;
            HireScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + list[i].GetComponent<EmployeeScript>().age + " Years Old";
            HireScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + list[i].GetComponent<EmployeeScript>().SpecialtyName;
            HireScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + list[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString("F2");
            HireScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
            HireScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Hire";
        }
    }

    public GameObject[] HiringPhase()
    {
        var empList = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            var emp = PotentialHires(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, EmployeeFirstNames.Length),
                Random.Range(0, EmployeeNickNames.Length), Random.Range(0, EmployeeLastNames.Length), Random.Range(20, 121),
                Random.Range(0, EmployeeHappiness.Length), Random.Range(0, EmployeeSpecialties.Length), Random.Range(0, maxSpecialtyStartingLevel));
            emp.transform.position = new Vector3(-500, -1000, emp.transform.position.z);
            emp.transform.parent = HireScreen.transform.GetChild(i);
            empList[i] = emp;
        }

        return empList;
    }

    public GameObject PotentialHires(int spriteNum, int secSpriteNum, int firstNameNum, int nickNameNum, int lastNameNum, int ageNum, int happNum, int specNum, float lvlNum)
    {
        var Emp = Instantiate(EmployeeTemplate);
        Emp.GetComponent<EmployeeScript>().name = EmployeeFirstNames[firstNameNum] + " \"" + EmployeeNickNames[nickNameNum] + "\" " + EmployeeLastNames[lastNameNum];
        Emp.GetComponent<EmployeeScript>().age = ageNum;
        Emp.GetComponent<EmployeeScript>().happiness = EmployeeHappiness[happNum];
        Emp.GetComponent<EmployeeScript>().SpecialtyName = EmployeeSpecialties[specNum];
        Emp.GetComponent<EmployeeScript>().SpecialtyLevel = lvlNum;
        Emp.GetComponent<EmployeeScript>().conferenceRoom = 16;
        Emp.GetComponent<EmployeeScript>().breakRoom = 17;
        Emp.GetComponent<EmployeeScript>().waterCooler = 19;
        Emp.GetComponent<EmployeeScript>().bossOffice = 18;
        Emp.GetComponent<EmployeeScript>().restRoom = 20;
        Emp.GetComponent<EmployeeScript>().stockRoom = 21;

        if (spriteNum == 0)
        {
            Emp.GetComponent<SpriteRenderer>().sprite = EmployeeSprites1[secSpriteNum * 10];
        }
        else if (spriteNum == 1)
        {
            Emp.GetComponent<SpriteRenderer>().sprite = EmployeeSprites2[secSpriteNum * 10];
        }
        else if (spriteNum == 2)
        {
            Emp.GetComponent<SpriteRenderer>().sprite = EmployeeSprites3[secSpriteNum * 10];
        }


        for (int i = 0; i < 10; i++)
        {
            var w = i + (secSpriteNum * 10);
            if (spriteNum == 0)
            {
                Emp.GetComponent<EmployeeScript>().EmployeeSprites[i] = EmployeeSprites1[w];
            }
            else if (spriteNum == 1)
            {
                Emp.GetComponent<EmployeeScript>().EmployeeSprites[i] = EmployeeSprites2[w];
            }
            else if (spriteNum == 2)
            {
                Emp.GetComponent<EmployeeScript>().EmployeeSprites[i] = EmployeeSprites3[w];
            }
        }

        return Emp;
    }

    public bool QueueQuery(string queue, GameObject emp)
    {
        QueueQueryRunning = true;
        bool val = false;

        if (queue == "RestRoom")
        {
            for (int i = 0; i < restRoomEmployeeSpots.Length; i++)
            {
                if (restRoomEmployeeSpots[i] == null) 
                {
                    restRoomEmployeeSpots[i] = emp;
                    QueueQueryRunning = false;
                    return true;
                }
            }
        }
        else if (queue == "StockRoom")
        {
            for (int i = 0; i < stockRoomEmployeeSpots.Length; i++)
            {
                if (stockRoomEmployeeSpots[i] == null)
                {
                    stockRoomEmployeeSpots[i] = emp;
                    QueueQueryRunning = false;
                    return true;
                }
            }
        }
        else if (queue == "ConferenceRoom")
        {
            for (int i = 0; i < conferenceRoomEmployeeSpots.Length; i++)
            {
                if (conferenceRoomEmployeeSpots[i] == null)
                {
                    conferenceRoomEmployeeSpots[i] = emp;
                    QueueQueryRunning = false;
                    return true;
                }
            }
        }
        else if (queue == "BreakRoom")
        {
            for (int i = 0; i < breakRoomEmployeeSpots.Length; i++)
            {
                if (breakRoomEmployeeSpots[i] == null)
                {
                    breakRoomEmployeeSpots[i] = emp;
                    QueueQueryRunning = false;
                    return true;
                }
            }
        }
        else if (queue == "BossOffice")
        {
            if (bossOfficOccupied)
            {
                QueueQueryRunning = false;
                return false;
            }
            else
            {
                bossOfficOccupied = true;
                QueueQueryRunning = false;
                return true;
            }
        }
        else if (queue == "WaterCooler")
        {
            for (int i = 0; i < waterCoolerEmployeeSpots.Length; i++)
            {
                if (waterCoolerEmployeeSpots[i] == null)
                {
                    waterCoolerEmployeeSpots[i] = emp;
                    QueueQueryRunning = false;
                    return true;
                }
            }
        }

        QueueQueryRunning = false;
        return val;
    }

    public void QueueLeave(string queue, GameObject emp)
    {
        QueueLeaveRunning = true;
        if (queue == "RestRoom")
        {
            for (int i = 0; i < restRoomEmployeeSpots.Length; i++)
            {
                if (restRoomEmployeeSpots[i] == emp)
                {
                    restRoomEmployeeSpots[i] = null;
                    break;
                }
            }
        }
        else if (queue == "StockRoom")
        {
            for (int i = 0; i < stockRoomEmployeeSpots.Length; i++)
            {
                if (stockRoomEmployeeSpots[i] == emp)
                {
                    stockRoomEmployeeSpots[i] = null;
                    break;
                }
            }
        }
        else if (queue == "ConferenceRoom")
        {
            for (int i = 0; i < conferenceRoomEmployeeSpots.Length; i++)
            {
                if (conferenceRoomEmployeeSpots[i] == emp)
                {
                    conferenceRoomEmployeeSpots[i] = null;
                    break;
                }
            }
        }
        else if (queue == "BreakRoom")
        {
            for (int i = 0; i < breakRoomEmployeeSpots.Length; i++)
            {
                if (breakRoomEmployeeSpots[i] == emp)
                {
                    breakRoomEmployeeSpots[i] = null;
                    break;
                }
            }
        }
        else if (queue == "BossOffice")
        {
            bossOfficOccupied = false;
        }
        else if (queue == "WaterCooler")
        {
            for (int i = 0; i < waterCoolerEmployeeSpots.Length; i++)
            {
                if (waterCoolerEmployeeSpots[i] == emp)
                {
                    waterCoolerEmployeeSpots[i] = null;
                    break;
                }
            }
        }
        QueueLeaveRunning = false;
    }

    #endregion
}
