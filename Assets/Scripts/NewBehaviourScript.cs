using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private Button yesBtn;
    private Button noBtn;
    private Button maybeBtn;

    private void Awake() {
        textMeshPro = transform.Find("DialogueBox").GetComponent<TextMeshProUGUI>();
        yesBtn = transform.Find("Yes Button").GetComponent<Button>();
        noBtn = transform.Find("No Button").GetComponent<Button>();
        maybeBtn = transform.Find("Maybe Button").GetComponent<Button>();

        ShowQuestion("Do you like this question?", () => Debug.Log("Yes"), 
            () => Debug.Log("No"), () => Debug.Log("Maybe"));
    }

    public void ShowQuestion(string QuestionText, Action yesAction, Action noAction, Action maybeAction) { 
        textMeshPro.text = QuestionText; 
        yesBtn.onClick.AddListener(() => {  
            Hide();
            yesAction();
        });
        noBtn.onClick.AddListener(() => {
            Hide();
            noAction();
        });
        maybeBtn.onClick.AddListener(() => {
            Hide();
            maybeAction();
        }); 
    }
    private void Hide() { 
        gameObject.SetActive(false);
    }

}
