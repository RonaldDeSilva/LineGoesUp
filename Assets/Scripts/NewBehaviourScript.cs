using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

public class NewBehaviourScript : MonoBehaviour
{
    public static NewBehaviourScript Instance { get; private set; }

    private TextMeshProUGUI textMeshPro;
    private Button yesBtn;
    private Button noBtn;
    private Button maybeBtn;

    private void Awake() { 
        Instance = this;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        yesBtn = transform.Find("Yes Button").GetComponent<Button>();
        noBtn = transform.Find("No Button").GetComponent<Button>();
        maybeBtn = transform.Find("Maybe Button").GetComponent<Button>();
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
