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
    private GameObject Canvas;

    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        HiringScreenFillUp(HiringPhase());
    }

    public void HiringScreenFillUp(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            Canvas.transform.GetChild(0).GetChild(i).GetChild(1).GetComponent<Image>().sprite = list[i].GetComponent<SpriteRenderer>().sprite;
            Canvas.transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = list[i].GetComponent<EmployeeScript>().name;
            Canvas.transform.GetChild(0).GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + list[i].GetComponent<EmployeeScript>().age + " Years Old";
            Canvas.transform.GetChild(0).GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Reason for terminating previous employment contract: " + list[i].GetComponent<EmployeeScript>().reasonTermination;
            Canvas.transform.GetChild(0).GetChild(i).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Years of experience in position: " + list[i].GetComponent<EmployeeScript>().experience + " Years";
            Canvas.transform.GetChild(0).GetChild(i).GetChild(6).GetComponent<TextMeshProUGUI>().text = "Productivity: " + list[i].GetComponent<EmployeeScript>().productivity;
            Canvas.transform.GetChild(0).GetChild(i).GetChild(7).GetComponent<TextMeshProUGUI>().text = "Loyalty to employer(Subject to change): " + list[i].GetComponent<EmployeeScript>().loyalty;
        }
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
