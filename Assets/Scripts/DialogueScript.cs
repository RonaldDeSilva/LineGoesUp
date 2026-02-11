using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI Dialogue;
    public Button yesBtn;
    public Button noBtn;
    public Button maybeBtn;
    private GameObject DialogueBox;
    private string[] DialogueOptions;
    private int latestStringNum;

    private void Start()
    {
        DialogueBox = transform.GetChild(0).gameObject;
        DialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            DialogueBox.SetActive(true);
            DisplayText("Assets\\DialogueScripts\\Dialogue_Example.txt", 0);
        }

        
    }

    public void DisplayText(string fileName, int lineNum)
    {
        DialogueOptions = File.ReadAllLines(fileName);
        Dialogue.text = DialogueOptions[lineNum];
        yesBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[lineNum + 1];
        noBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[lineNum + 2];
        maybeBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DialogueOptions[lineNum + 3];
        latestStringNum = lineNum + 3;
    }

    public void YesButton()
    {
        Dialogue.text = DialogueOptions[latestStringNum + 1];
    }

    public void NoButton()
    {
        Dialogue.text = DialogueOptions[latestStringNum + 2];
    }

    public void MaybeButton()
    {
        Dialogue.text = DialogueOptions[latestStringNum + 3];
    }
}
