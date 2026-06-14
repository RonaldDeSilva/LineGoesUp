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
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }


}
