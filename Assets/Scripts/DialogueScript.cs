using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI Dialogue;
    public GameObject yesBtn;
    public GameObject noBtn;
    public GameObject maybeBtn;
    public GameObject continueButton;
    public GameObject DialogueBox;
    private string[] DialogueOptions;
    private int latestStringNum;
    private bool displayingText = false;
    private bool doOnce = false;

    private void Update()
    {
        if (!displayingText && doOnce)
        {
            for (int i = 0; i < GameObject.Find("Current Employee Pool").transform.childCount; i++)
            {
                if (GameObject.Find("Current Employee Pool").transform.GetChild(i).gameObject.GetComponent<EmployeeScript>().hasQuery)
                {
                    GameObject.Find("Current Employee Pool").transform.GetChild(i).gameObject.GetComponent<EmployeeScript>().hasQuery = false;
                    GameObject.Find("Current Employee Pool").transform.GetChild(i).gameObject.GetComponent<EmployeeScript>().QueryIcon.SetActive(false);
                }
            }

            doOnce = false;
        }
    }

    #region Display Text
    public void DisplayText()
    {
        DialogueBox.SetActive(true);
        continueButton.SetActive(false);
        DialogueOptions = File.ReadAllLines("Assets\\DialogueScripts\\dialogue.txt");
        var numQueries = DialogueOptions.Length / 11;
        var ran = Random.Range(0, numQueries);
        if (ran == 0)
        {
            Dialogue.text = DialogueOptions[1];
            yesBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[2];
            noBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[5];
            maybeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[8];
            latestStringNum = 2;
        }
        else
        {
            Dialogue.text = DialogueOptions[(ran * 11) + 1];
            yesBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[(ran * 11) + 2];
            noBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[(ran * 11) + 5];
            maybeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[(ran * 11) + 8];
            latestStringNum = (ran * 11) + 2;
        }
        doOnce = true;
        displayingText = true;
    }

    #endregion
    public void YesButton()
    {
        Dialogue.text = DialogueOptions[latestStringNum + 1];
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        maybeBtn.SetActive(false);
        continueButton.SetActive(true);
    }

    public void NoButton()
    {
        Dialogue.text = DialogueOptions[latestStringNum + 4];
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        maybeBtn.SetActive(false);
        continueButton.SetActive(true);
    }

    public void MaybeButton()
    {
        Dialogue.text = DialogueOptions[latestStringNum + 7];
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        maybeBtn.SetActive(false);
        continueButton.SetActive(true);
    }

    public void ContinueButton()
    {
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
        maybeBtn.SetActive(true);
        DialogueBox.SetActive(false);
        displayingText = false;
    }
}
