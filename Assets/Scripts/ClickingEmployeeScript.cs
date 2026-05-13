using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickingEmployeeScript : MonoBehaviour
{
    public GameObject selectedEmployee;
    private bool coolDown;
    private int timer;
    private GameObject CurrentEmps;
    private Camera Cam;
    private EmployeeMasterControl EMC;
    private Rigidbody2D rb;
    public float speed;
    public GameObject EmployeeInfo;
    private DialogueScript Dialogue;
    public MiniGameController MGC;

    private void Start()
    {
        CurrentEmps = GameObject.Find("Current Employee Pool");
        Cam = GetComponent<Camera>();
        EMC = GameObject.Find("Employee Master Control Canvas").GetComponent<EmployeeMasterControl>();
        rb = GetComponent<Rigidbody2D>();
        Dialogue = EMC.gameObject.transform.GetChild(2).gameObject.GetComponent<DialogueScript>();
    }

    private void Update()
    {
        if (CurrentEmps.transform.childCount != 0 && selectedEmployee == null)
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
                            if (!CurrentEmps.transform.GetChild(i).gameObject.GetComponent<EmployeeScript>().hasQuery)
                            {
                                selectedEmployee = CurrentEmps.transform.GetChild(i).gameObject;
                                EmployeeInfo.SetActive(true);
                                EmployeeInfo.transform.GetChild(0).GetComponent<Image>().sprite = selectedEmployee.GetComponent<SpriteRenderer>().sprite;
                                EmployeeInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedEmployee.GetComponent<EmployeeScript>().name;
                                EmployeeInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Age: " + selectedEmployee.GetComponent<EmployeeScript>().age;
                                EmployeeInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Happiness: " + selectedEmployee.GetComponent<EmployeeScript>().happiness;
                                for (int f = 0; f < selectedEmployee.GetComponent<EmployeeScript>().SpecialtyNames.Count; f++)
                                {
                                    EmployeeInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = EmployeeInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text +
                                        selectedEmployee.GetComponent<EmployeeScript>().SpecialtyNames[f] + " LVL-" + selectedEmployee.GetComponent<EmployeeScript>().SpecialtyLevels[f].ToString("F2");
                                }
                                Cam.orthographicSize = 2;
                                EmployeeInfo.SetActive(true);
                                coolDown = true;
                                timer = 60;
                            }
                            else
                            {
                                Dialogue.DisplayText();
                                selectedEmployee = CurrentEmps.transform.GetChild(i).gameObject;
                                EmployeeInfo.SetActive(true);
                                EmployeeInfo.transform.GetChild(0).GetComponent<Image>().sprite = selectedEmployee.GetComponent<SpriteRenderer>().sprite;
                                EmployeeInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedEmployee.GetComponent<EmployeeScript>().name;
                                EmployeeInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Age: " + selectedEmployee.GetComponent<EmployeeScript>().age;
                                EmployeeInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Happiness: " + selectedEmployee.GetComponent<EmployeeScript>().happiness;
                                if (selectedEmployee.GetComponent<EmployeeScript>().SpecialtyNames.Count == 1)
                                {

                                }
                                else if (selectedEmployee.GetComponent<EmployeeScript>().SpecialtyNames.Count >= 2)
                                {
                                    for (int f = 0; f < selectedEmployee.GetComponent<EmployeeScript>().SpecialtyNames.Count; f++)
                                    {
                                        EmployeeInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = EmployeeInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text +
                                            selectedEmployee.GetComponent<EmployeeScript>().SpecialtyNames[f] + " LVL: " + selectedEmployee.GetComponent<EmployeeScript>().SpecialtyLevels[f].ToString("F2") + ", ";
                                    }
                                }
                                Cam.orthographicSize = 2;
                                EmployeeInfo.SetActive(true);
                                coolDown = true;
                                timer = 60;
                            }
                        }
                    }
                }
            }
        }

        

        if (selectedEmployee != null)
        {
            rb.linearVelocity = new Vector2((selectedEmployee.transform.position.x - transform.position.x) * speed, (selectedEmployee.transform.position.y - transform.position.y) * speed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = new Vector3(0.62f, 0, -10);
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

    public void OnButtonClick()
    {
        if (selectedEmployee != null && !selectedEmployee.GetComponent<EmployeeScript>().controlled)
        {
            selectedEmployee.GetComponent<EmployeeScript>().StartControl();
            EmployeeInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Stop Controlling";

        }
        else if (selectedEmployee != null && selectedEmployee.GetComponent<EmployeeScript>().controlled)
        {
            selectedEmployee.GetComponent<EmployeeScript>().EndControl();
            EmployeeInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Control";
        }
    }

    public void DeSelectEmployee()
    {
        EmployeeInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Specialties: ";
        EmployeeInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Control";
        EmployeeInfo.SetActive(false);
        if (selectedEmployee.GetComponent<EmployeeScript>().controlled)
        {
            selectedEmployee.GetComponent<EmployeeScript>().EndControl();
        }
        if (selectedEmployee.GetComponent<EmployeeScript>().computerScreen.transform.GetChild(0).gameObject.activeSelf)
        {
            selectedEmployee.GetComponent<EmployeeScript>().computerScreen.transform.GetChild(0).gameObject.SetActive(false);
            EmployeeInfo.transform.localPosition = new Vector2(-704f, 387.2f);
            MGC.BootDown();
            EmployeeInfo.transform.GetChild(5).GetComponent<Button>().interactable = true;
        }
        selectedEmployee = null;
    }

    public void ComputerPowerButton()
    {
        MGC.BootDown();
        selectedEmployee.GetComponent<EmployeeScript>().computerScreen.transform.GetChild(0).gameObject.SetActive(false);
        selectedEmployee.GetComponent<EmployeeScript>().workingControlled = false;
        EmployeeInfo.transform.localPosition = new Vector2(-704f, 387.2f);
        EmployeeInfo.transform.GetChild(5).GetComponent<Button>().interactable = true;
    }
}