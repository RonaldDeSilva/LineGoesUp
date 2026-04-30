using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    public int numEmployees = 0;
    public int maxEmployees;

    public int restRoomQueue;
    public int waterCoolerQueue;
    public int bossOfficeQueue;
    public int breakRoomQueue;
    public int stockRoomQueue;
    public int conferenceRoomQueue;
    private int moveTimer;
    public EmployeeScript[] waterCoolerEmployeeSpots = new EmployeeScript[5];
    public EmployeeScript[] breakRoomEmployeeSpots = new EmployeeScript[8];
    public EmployeeScript[] stockRoomEmployeeSpots = new EmployeeScript[4];
    public EmployeeScript[] conferenceRoomEmployeeSpots = new EmployeeScript[12];
    public EmployeeScript[] restRoomEmployeeSpots = new EmployeeScript[2];

    void Start()
    {
        HireScreen = transform.GetChild(0).gameObject;
        HireScreen.SetActive(false);
        EmployeeFirstNames = File.ReadAllLines("Assets\\EmployeeInfoDocs\\First Names.txt");
        EmployeeNickNames = File.ReadAllLines("Assets\\EmployeeInfoDocs\\Nick Names.txt");
        EmployeeLastNames = File.ReadAllLines("Assets\\EmployeeInfoDocs\\Last Names.txt");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M) && !HireScreen.activeSelf)
        {
            HiringScreenFillUp(HiringPhase());
            HireScreen.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Escape) && HireScreen.activeSelf)
        {
            for (int i = 0; i < 6; i++)
            {
                try 
                { 
                    Destroy(HireScreen.transform.GetChild(i).GetChild(8).gameObject);
                }
                catch { continue; }
            }
            HireScreen.SetActive(false);
        }

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
        

        restRoomQueue = Mathf.Clamp(restRoomQueue, 0, 2);
        stockRoomQueue = Mathf.Clamp(stockRoomQueue, 0, 4);
        conferenceRoomQueue = Mathf.Clamp(conferenceRoomQueue, 0, 12);
        breakRoomQueue = Mathf.Clamp(breakRoomQueue, 0, 8);
        bossOfficeQueue = Mathf.Clamp(bossOfficeQueue, 0, 1);
        waterCoolerQueue = Mathf.Clamp(waterCoolerQueue, 0, 6);
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
            emp.GetComponent<EmployeeScript>().workStation = numEmployees;
            emp.GetComponent<EmployeeScript>().Awaken(this.GetComponent<EmployeeMasterControl>());
            numEmployees++;
        }
    }

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
}
