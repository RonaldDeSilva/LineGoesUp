using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickingEmployeeScript : MonoBehaviour
{
    private GameObject selectedEmployee;
    private bool coolDown;
    private int timer;
    private GameObject CurrentEmps;
    private Camera Cam;
    private EmployeeMasterControl EMC;
    private Rigidbody2D rb;
    public float speed;
    private GameObject EmployeeInfo;

    private void Start()
    {
        CurrentEmps = GameObject.Find("Current Employee Pool");
        Cam = GetComponent<Camera>();
        EMC = GameObject.Find("Employee Master Control Canvas").GetComponent<EmployeeMasterControl>();
        rb = GetComponent<Rigidbody2D>();
        EmployeeInfo = GameObject.Find("Employee Master Control Canvas").transform.GetChild(1).gameObject;
        EmployeeInfo.SetActive(false);
    }

    private void Update()
    {
        if (CurrentEmps.transform.childCount != 0)
        {
            for (int f = 0; f < CurrentEmps.transform.childCount; f++)
            {
                if (Approx.FastApp(CurrentEmps.transform.GetChild(f).position.x, Cam.ScreenToWorldPoint(Input.mousePosition).x, 0.25f) && Approx.FastApp(CurrentEmps.transform.GetChild(f).position.y, Cam.ScreenToWorldPoint(Input.mousePosition).y, 0.25f))
                {
                    CurrentEmps.transform.GetChild(f).GetChild(0).gameObject.GetComponent<Flashing>().flashing = true;
                }
                else
                {
                    CurrentEmps.transform.GetChild(f).GetChild(0).gameObject.GetComponent<Flashing>().flashing = false;
                }
            }
        }
        
        if (Input.GetMouseButtonDown(0) && !EMC.HireScreen.activeSelf && selectedEmployee == null)
        {
            if (!coolDown)
            {
                if (CurrentEmps.transform.childCount != 0)
                {
                    for (int i = 0; i < CurrentEmps.transform.childCount; i++)
                    {
                        if (Approx.FastApp(CurrentEmps.transform.GetChild(i).position.x, Cam.ScreenToWorldPoint(Input.mousePosition).x, 0.25f) && Approx.FastApp(CurrentEmps.transform.GetChild(i).position.y, Cam.ScreenToWorldPoint(Input.mousePosition).y, 0.25f))
                        {
                            selectedEmployee = CurrentEmps.transform.GetChild(i).gameObject;
                            EmployeeInfo.SetActive(true);
                            EmployeeInfo.transform.GetChild(0).GetComponent<Image>().sprite = selectedEmployee.GetComponent<SpriteRenderer>().sprite;
                            //EmployeeInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedEmployee.GetComponent<EmployeeScript>().title; // skipping this one since its the title not implemented yet
                            EmployeeInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = selectedEmployee.GetComponent<EmployeeScript>().name;
                            EmployeeInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Age: " + selectedEmployee.GetComponent<EmployeeScript>().age;
                            EmployeeInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Reason For Leaving Previous Job: " + selectedEmployee.GetComponent<EmployeeScript>().reasonTermination;
                            EmployeeInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Happiness: " + selectedEmployee.GetComponent<EmployeeScript>().happiness;
                            EmployeeInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Experience: " + selectedEmployee.GetComponent<EmployeeScript>().experience;
                            EmployeeInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "Productivity: " + selectedEmployee.GetComponent<EmployeeScript>().productivity;
                            Cam.orthographicSize = 2;
                            EmployeeInfo.SetActive(true);
                            coolDown = true;
                            timer = 60;
                        }
                    }
                }
            }
        }

        if (selectedEmployee != null && Input.GetKey(KeyCode.Escape))
        {
            EmployeeInfo.SetActive(false);
            selectedEmployee = null;
        }

        if (selectedEmployee != null)
        {
            rb.linearVelocity = new Vector2((selectedEmployee.transform.position.x - transform.position.x) * speed, (selectedEmployee.transform.position.y - transform.position.y) * speed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = new Vector3(0, 0, -10);
            Cam.orthographicSize = 5;
        }

        if (timer > 0)
        {
            timer--;
        }
        else
        {
            coolDown = false;
        }
    }
}
