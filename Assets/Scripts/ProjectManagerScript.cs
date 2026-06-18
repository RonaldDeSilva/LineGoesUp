using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectManagerScript : MonoBehaviour
{
    //Current projects 
    public GameObject CurrentProject1;
    public GameObject CurrentProject2;
    public GameObject CurrentProject3;
    public GameObject NoProjectsText;
    public GameObject CurrentProjectPool;
    public GameObject PotentialProjectPool;
    public GameObject TimeClock;
    public GameObject ProjectScreen;
    public GameObject CurrentEmployeePool;

    //Project Browser
    public GameObject PotentialProject1;
    public GameObject PotentialProject2;
    public GameObject PotentialProject3;
    public GameObject ProjectTemplate;

    public Sprite[] ProjectIcons;
    public string[] ProjectNames;
    public string[] ProjectDescriptions;
    public float ProjectRewardsMin;
    public float ProjectRewardsMax;
    public int numActiveProjects;

    // Project acceptance screen
    public List<GameObject> Employees = new List<GameObject>();
    public GameObject Employee1;
    public GameObject Employee2;
    public GameObject Employee3;

    public void StartUp()
    {
        if (!transform.GetChild(0).gameObject.activeSelf && !transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (numActiveProjects == 3)
            {
                NoProjectsText.SetActive(false);
                CurrentProject3.SetActive(true);
                CurrentProject2.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    var proj = CurrentProjectPool.transform.GetChild(i).gameObject;

                    if (i == 0)
                    {
                        CurrentProject1.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                        CurrentProject1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                    else if (i == 1)
                    {
                        CurrentProject2.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                        CurrentProject2.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject2.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                    else if (i == 2)
                    {
                        CurrentProject3.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject3.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                        CurrentProject3.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject3.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                }
            }
            else if (numActiveProjects == 2)
            {
                NoProjectsText.SetActive(false);
                CurrentProject3.SetActive(false);
                CurrentProject2.SetActive(true);
                for (int i = 0; i < 2; i++)
                {
                    var proj = CurrentProjectPool.transform.GetChild(i).gameObject;

                    if (i == 0)
                    {
                        CurrentProject1.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                        CurrentProject1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                    else if (i == 1)
                    {
                        CurrentProject2.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                        CurrentProject2.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject2.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                }
            }
            else if (numActiveProjects == 1)
            {
                NoProjectsText.SetActive(false);
                CurrentProject3.SetActive(false);
                CurrentProject2.SetActive(false);
                var proj = CurrentProjectPool.transform.GetChild(0);
                CurrentProject1.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                CurrentProject1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                CurrentProject1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                CurrentProject1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
            }
            else
            {
                NoProjectsText.SetActive(true);
                CurrentProject3.SetActive(false);
                CurrentProject2.SetActive(false);
                CurrentProject1.SetActive(false);
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void ProjectBrowserButton()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                var proj = Instantiate(ProjectTemplate, PotentialProjectPool.transform);
                proj.GetComponent<ProjectScript>().ProjectIcon = ProjectIcons[Random.Range(0, ProjectIcons.Length)];
                proj.GetComponent<ProjectScript>().ProjectName = ProjectNames[Random.Range(0, ProjectIcons.Length)];
                proj.GetComponent<ProjectScript>().ProjectDescription = ProjectDescriptions[Random.Range(0, ProjectIcons.Length)];
                proj.GetComponent<ProjectScript>().ProjectReward = Random.Range(ProjectRewardsMin, ProjectRewardsMax);

                if (i == 0)
                {
                    PotentialProject1.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                    PotentialProject1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                    PotentialProject1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                    PotentialProject1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                }
                else if (i == 1)
                {
                    PotentialProject2.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                    PotentialProject2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                    PotentialProject2.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                    PotentialProject2.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                }
                else if (i == 2)
                {
                    PotentialProject3.transform.GetChild(1).GetComponent<Image>().sprite = proj.GetComponent<ProjectScript>().ProjectIcon;
                    PotentialProject3.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectName;
                    PotentialProject3.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectDescription;
                    PotentialProject3.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = proj.GetComponent<ProjectScript>().ProjectReward.ToString();
                }
            }

        }
        else
        {
            if (PotentialProjectPool.transform.childCount > 0)
            {
                for (int i = PotentialProjectPool.transform.childCount; i > -1; i--)
                {
                    Destroy(PotentialProjectPool.transform.GetChild(i));
                }
            }
            /*
            if (CurrentProjectPool.transform.childCount > 0)
            {
                for (int i = 0; i < CurrentProjectPool.transform.childCount; i++)
                {
                    if (i == 0)
                    {
                        CurrentProject1.transform.GetChild(1).GetComponent<Image>().sprite = CurrentProjectPool.transform.GetChild(0).GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(0).GetComponent<ProjectScript>().ProjectName;
                        CurrentProject1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(0).GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(0).GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                    else if (i == 1)
                    {
                        CurrentProject2.transform.GetChild(1).GetComponent<Image>().sprite = CurrentProjectPool.transform.GetChild(1).GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(1).GetComponent<ProjectScript>().ProjectName;
                        CurrentProject2.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(1).GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject2.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(1).GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                    else if (i == 2)
                    {
                        CurrentProject3.transform.GetChild(1).GetComponent<Image>().sprite = CurrentProjectPool.transform.GetChild(2).GetComponent<ProjectScript>().ProjectIcon;
                        CurrentProject3.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(2).GetComponent<ProjectScript>().ProjectName;
                        CurrentProject3.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(2).GetComponent<ProjectScript>().ProjectDescription;
                        CurrentProject3.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = CurrentProjectPool.transform.GetChild(2).GetComponent<ProjectScript>().ProjectReward.ToString();
                    }
                }
            }
            */
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void AcceptProjectButton(Button butt)
    {
        if (PotentialProjectPool.transform.childCount == 3)
        {
            if (butt.gameObject.CompareTag("Button1"))
            {
                var proj = PotentialProjectPool.transform.GetChild(0).gameObject;
                proj.transform.parent = CurrentProjectPool.transform;
                butt.interactable = false;
                ProjectAssignment(proj);
            }
            else if (butt.gameObject.CompareTag("Button2"))
            {
                var proj = PotentialProjectPool.transform.GetChild(1).gameObject;
                proj.transform.parent = CurrentProjectPool.transform;
                butt.interactable = false;
                ProjectAssignment(proj);
            }
            else if (butt.gameObject.CompareTag("Button3"))
            {
                var proj = PotentialProjectPool.transform.GetChild(2).gameObject;
                proj.transform.parent = CurrentProjectPool.transform;
                butt.interactable = false;
                ProjectAssignment(proj);
            }
        }
        else if (PotentialProjectPool.transform.childCount == 2)
        {
            if (butt.gameObject.CompareTag("Button1"))
            {
                var proj = PotentialProjectPool.transform.GetChild(0).gameObject;
                proj.transform.parent = CurrentProjectPool.transform;
                butt.interactable = false;
                ProjectAssignment(proj);
            }
            else if (butt.gameObject.CompareTag("Button2"))
            {
                if (butt.transform.parent.GetChild(2).GetComponent<Button>().interactable)
                {
                    var proj = PotentialProjectPool.transform.GetChild(1).gameObject;
                    proj.transform.parent = CurrentProjectPool.transform;
                    butt.interactable = false;
                    ProjectAssignment(proj);
                }
                else
                {
                    var proj = PotentialProjectPool.transform.GetChild(0).gameObject;
                    proj.transform.parent = CurrentProjectPool.transform;
                    butt.interactable = false;
                    ProjectAssignment(proj);
                }
            }
            else if (butt.gameObject.CompareTag("Button3"))
            {
                var proj = PotentialProjectPool.transform.GetChild(1).gameObject;
                proj.transform.parent = CurrentProjectPool.transform;
                butt.interactable = false;
                ProjectAssignment(proj);
            }
        }
        else if (PotentialProjectPool.transform.childCount == 1)
        {
            var proj = PotentialProjectPool.transform.GetChild(0).gameObject;
            proj.transform.parent = CurrentProjectPool.transform;
            butt.interactable = false;
            ProjectAssignment(proj);
        }
    }


    public void ProjectAssignment(GameObject proj)
    {
        ProjectScreen.SetActive(true);
        
        for (int i = 0; i < CurrentEmployeePool.transform.childCount; i++)
        {
            Employees.Add(CurrentEmployeePool.transform.GetChild(i).gameObject);
        }

        if (Employees.Count > 2)
        {
            Employee3.SetActive(true);
            Employee2.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    Employee1.transform.GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    Employee1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    Employee1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().age.ToString();
                    Employee1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    Employee1.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString();
                }
                else if (i == 1)
                {
                    Employee2.transform.GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    Employee2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    Employee2.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().age.ToString();
                    Employee2.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    Employee2.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString();
                }
                else if (i == 2)
                {
                    Employee3.transform.GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    Employee3.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    Employee3.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().age.ToString();
                    Employee3.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    Employee3.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString();
                }
            }
        }
        else if (Employees.Count == 2)
        {
            Employee3.SetActive(false);
            Employee2.SetActive(true);
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    Employee1.transform.GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    Employee1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    Employee1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().age.ToString();
                    Employee1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    Employee1.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString();
                }
                else if (i == 1)
                {
                    Employee2.transform.GetChild(1).GetComponent<Image>().sprite = Employees[i].GetComponent<SpriteRenderer>().sprite;
                    Employee2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().name;
                    Employee2.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().age.ToString();
                    Employee2.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyName;
                    Employee2.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Employees[i].GetComponent<EmployeeScript>().SpecialtyLevel.ToString();
                }
            }
        }
        else
        {
            Employee1.transform.GetChild(1).GetComponent<Image>().sprite = Employees[0].GetComponent<SpriteRenderer>().sprite;
            Employee1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().name;
            Employee1.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().age.ToString();
            Employee1.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().SpecialtyName;
            Employee1.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Employees[0].GetComponent<EmployeeScript>().SpecialtyLevel.ToString();
            Employee3.SetActive(false);
            Employee2.SetActive(false);
        }
    }    
}

