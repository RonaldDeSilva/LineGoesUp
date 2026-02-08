using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMasterControl : MonoBehaviour
{
    #region Employee Definition

    //Employees Have a:
    // - Name
    // - Age
    // - Reason for quit/fired from job
    // - Experience in position
    // - Productivity
    // - Loyalty
    // - Happiness (Only revealed after hiring)
    // - Burnout (Only revealed after hiring)

    #endregion

    public GameObject EmployeeTemplate;
    public Sprite[] EmployeeSprites;
    public string[] EmployeeNames;
    public int[] EmployeeAges;
    public string[] EmployeeReasonTermination;
    public float[] EmployeeExperience;
    public int[] EmployeeProductivity;
    public int[] EmployeeLoyalty;
    public int[] EmployeeHappiness;
    public int[] EmployeeBurnout;
    public int HiringPool;
    private GameObject HireScreen;
    private GameObject[] CurrentHires;
    public GameObject CurrentEmployeePool;

    void Start()
    {
        HireScreen = transform.GetChild(0).gameObject;
        HireScreen.SetActive(false);
        
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
    }

    public void OnClick(Button button)
    {
        var emp = button.transform.GetChild(8).gameObject;
        emp.transform.parent = CurrentEmployeePool.transform;
        emp.transform.position = Vector3.zero;
        button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Sold American";
        button.GetComponent<Button>().interactable = false;
    }

    public void HiringScreenFillUp(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            HireScreen.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = list[i].GetComponent<SpriteRenderer>().sprite;
            HireScreen.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = list[i].GetComponent<EmployeeScript>().name;
            HireScreen.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + list[i].GetComponent<EmployeeScript>().age + " Years Old";
            HireScreen.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Reason for terminating previous employment contract: " + list[i].GetComponent<EmployeeScript>().reasonTermination;
            HireScreen.transform.GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Years of experience in position: " + list[i].GetComponent<EmployeeScript>().experience + " Years";
            HireScreen.transform.GetChild(i).GetChild(6).GetComponent<TextMeshProUGUI>().text = "Productivity: " + list[i].GetComponent<EmployeeScript>().productivity;
            HireScreen.transform.GetChild(i).GetChild(7).GetComponent<TextMeshProUGUI>().text = "Loyalty to employer(Subject to change): " + list[i].GetComponent<EmployeeScript>().loyalty;
            HireScreen.transform.GetChild(i).GetComponent<Button>().interactable = true;
            HireScreen.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Hire";
        }
        CurrentHires = list;
    }

    public GameObject[] HiringPhase()
    {
        int[] list = new int[] {Random.Range(0, HiringPool - 1), Random.Range(0, HiringPool - 1), Random.Range(0, HiringPool - 1),
            Random.Range(0, HiringPool - 1), Random.Range(0, HiringPool - 1), Random.Range(0, HiringPool - 1)};

        var empList = new GameObject[6];
        var noRepeatsList = new int[6] {-1, -1, -1, -1, -1, -1};
        for (int i = 0; i < list.Length; i++)
        {
            for (int g = 0; g < 6; g++)
            {
                while (list[i] == noRepeatsList[g])
                {
                    list[i] = Random.Range(0, HiringPool - 1);
                }
            }
            var emp = PotentialHires(list[i]);
            emp.transform.position = new Vector3(-500, -1000, emp.transform.position.z);
            emp.transform.parent = HireScreen.transform.GetChild(i);
            empList[i] = emp;
            noRepeatsList[i] = list[i];
        }

        return empList;
    }

    public GameObject PotentialHires(int EmpNum)
    {
        var Emp = Instantiate(EmployeeTemplate);
        Emp.GetComponent<SpriteRenderer>().sprite = EmployeeSprites[EmpNum];
        Emp.GetComponent<EmployeeScript>().name = EmployeeNames[EmpNum];
        Emp.GetComponent<EmployeeScript>().age = EmployeeAges[EmpNum];
        Emp.GetComponent<EmployeeScript>().reasonTermination = EmployeeReasonTermination[EmpNum];
        Emp.GetComponent<EmployeeScript>().experience = EmployeeExperience[EmpNum];
        Emp.GetComponent<EmployeeScript>().productivity = EmployeeProductivity[EmpNum];
        Emp.GetComponent<EmployeeScript>().loyalty = EmployeeLoyalty[EmpNum];
        Emp.GetComponent<EmployeeScript>().happiness = EmployeeHappiness[EmpNum];
        Emp.GetComponent<EmployeeScript>().burnout = EmployeeBurnout[EmpNum];

        return Emp;
    }
}
