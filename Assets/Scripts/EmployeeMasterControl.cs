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

    public GameObject EmployeeTemplate;
    public Sprite[] EmployeeSprites;
    private string[] EmployeeFirstNames;
    private string[] EmployeeNickNames;
    private string[] EmployeeLastNames;
    public int[] EmployeeAges;
    public int[] EmployeeHappiness;
    public string[] EmployeeSpecialties;
    public float maxSpecialtyStartingLevel;
    public int HiringPool;
    public GameObject HireScreen;
    public GameObject CurrentEmployeePool;
    public List<GameObject> Employees = new List<GameObject>();
    public int numEmployees = 0;
    public int maxEmployees;
    private GameObject FiringScreen;

    private int moveTimer;
    public GameObject[] waterCoolerEmployeeSpots = new GameObject[5];
    public GameObject[] breakRoomEmployeeSpots = new GameObject[8];
    public GameObject[] stockRoomEmployeeSpots = new GameObject[4];
    public GameObject[] conferenceRoomEmployeeSpots = new GameObject[12];
    public GameObject[] restRoomEmployeeSpots = new GameObject[2];
    public bool bossOfficOccupied = false;
    public bool QueueQueryRunning = false;
    public bool QueueLeaveRunning = false;

    private int fireableEmployee1;
    private int fireableEmployee2;
    private int fireableEmployee3;
    private int CurrentPage;
    public string buttonNum;

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

        if (moveTimer % 15 == 0 && CurrentEmployeePool.transform.childCount > 0)
        {
            var ran = Random.Range(0, CurrentEmployeePool.transform.childCount);
            if (!CurrentEmployeePool.transform.GetChild(ran).gameObject.GetComponent<EmployeeScript>().isMoving)
            {
                CurrentEmployeePool.transform.GetChild(ran).gameObject.GetComponent<EmployeeScript>().chosen = true;
            }
            else
            {
                for (int i = 0; i < CurrentEmployeePool.transform.childCount; i++)
                {
                    if (!CurrentEmployeePool.transform.GetChild(i).gameObject.GetComponent<EmployeeScript>().isMoving)
                    {
                        CurrentEmployeePool.transform.GetChild(i).gameObject.GetComponent<EmployeeScript>().chosen = true;
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
            if (Employees.Count > 0)
            {
                for (int i = 0; i < Employees.Count; i++)
                {
                    if (Employees[i] == null)
                    {
                        Employees[i] = emp;
                        emp.GetComponent<EmployeeScript>().workStation = i + 1;
                        emp.GetComponent<EmployeeScript>().Awaken(this.GetComponent<EmployeeMasterControl>());
                        return;
                    }
                }
            }

            Employees.Add(emp);
            emp.GetComponent<EmployeeScript>().workStation = numEmployees;
            emp.GetComponent<EmployeeScript>().Awaken(this.GetComponent<EmployeeMasterControl>());
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
            CurrentPage = 0;
            if (numEmployees == 1)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(false);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false); 
                FiringScreen.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = CurrentEmployeePool.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                FiringScreen.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = CurrentEmployeePool.transform.GetChild(0).GetComponent<EmployeeScript>().name;
                FiringScreen.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + CurrentEmployeePool.transform.GetChild(0).GetComponent<EmployeeScript>().age + " Years Old";
                FiringScreen.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + CurrentEmployeePool.transform.GetChild(0).GetComponent<EmployeeScript>().SpecialtyNames[0];
                FiringScreen.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + CurrentEmployeePool.transform.GetChild(0).GetComponent<EmployeeScript>().SpecialtyLevels[0].ToString("F2");
                FiringScreen.transform.GetChild(0).GetComponent<Button>().interactable = true;
                FiringScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                fireableEmployee1 = 0;
            }
            else if (numEmployees == 2)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(false);
                for (int i = 0; i < 2; i++)
                {                    
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = CurrentEmployeePool.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().SpecialtyNames[0];
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().SpecialtyLevels[0].ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    fireableEmployee2 = 1;
                }
            }
            else if (numEmployees >= 3)
            {
                FiringScreen.transform.GetChild(1).gameObject.SetActive(true);
                FiringScreen.transform.GetChild(2).gameObject.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    FiringScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = CurrentEmployeePool.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite;
                    FiringScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().name;
                    FiringScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().age + " Years Old";
                    FiringScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().SpecialtyNames[0];
                    FiringScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + CurrentEmployeePool.transform.GetChild(i).GetComponent<EmployeeScript>().SpecialtyLevels[0].ToString("F2");
                    FiringScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    FiringScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Fire";
                    fireableEmployee3 = 2;
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
            Destroy(CurrentEmployeePool.transform.GetChild(0 + (CurrentPage * 3)).gameObject);

        }
        if (butt.CompareTag("Button2"))
        {
            Destroy(CurrentEmployeePool.transform.GetChild(1 + (CurrentPage * 3)).gameObject);
        }
        if (butt.CompareTag("Button3"))
        {
            Destroy(CurrentEmployeePool.transform.GetChild(2 + (CurrentPage * 3)).gameObject);
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
            HireScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialty: " + list[i].GetComponent<EmployeeScript>().SpecialtyNames[0];
            HireScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Level in Specialty: " + list[i].GetComponent<EmployeeScript>().SpecialtyLevels[0].ToString("F2");
            HireScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
            HireScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Hire";
        }
    }

    public GameObject[] HiringPhase()
    {
        var empList = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            var emp = PotentialHires(Random.Range(0, EmployeeSprites.Length), Random.Range(0, EmployeeFirstNames.Length),
                Random.Range(0, EmployeeNickNames.Length), Random.Range(0, EmployeeLastNames.Length), Random.Range(20, 121),
                Random.Range(0, EmployeeHappiness.Length), Random.Range(0, EmployeeSpecialties.Length), Random.Range(0, maxSpecialtyStartingLevel));
            emp.transform.position = new Vector3(-500, -1000, emp.transform.position.z);
            emp.transform.parent = HireScreen.transform.GetChild(i);
            empList[i] = emp;
        }

        return empList;
    }

    public GameObject PotentialHires(int spriteNum, int firstNameNum, int nickNameNum, int lastNameNum, int ageNum, int happNum, int specNum, float lvlNum)
    {
        var Emp = Instantiate(EmployeeTemplate);
        Emp.GetComponent<SpriteRenderer>().sprite = EmployeeSprites[spriteNum];
        Emp.GetComponent<EmployeeScript>().name = EmployeeFirstNames[firstNameNum] + " \"" + EmployeeNickNames[nickNameNum] + "\" " + EmployeeLastNames[lastNameNum];
        Emp.GetComponent<EmployeeScript>().age = ageNum;
        Emp.GetComponent<EmployeeScript>().happiness = EmployeeHappiness[happNum];
        Emp.GetComponent<EmployeeScript>().SpecialtyNames.Add(EmployeeSpecialties[specNum]);
        Emp.GetComponent<EmployeeScript>().SpecialtyLevels.Add(lvlNum);
        Emp.GetComponent<EmployeeScript>().conferenceRoom = 16;
        Emp.GetComponent<EmployeeScript>().breakRoom = 17;
        Emp.GetComponent<EmployeeScript>().waterCooler = 19;
        Emp.GetComponent<EmployeeScript>().bossOffice = 18;
        Emp.GetComponent<EmployeeScript>().restRoom = 20;
        Emp.GetComponent<EmployeeScript>().stockRoom = 21;

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
