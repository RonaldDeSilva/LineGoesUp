using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{
    public int currentMiniGame;
    private int prevMiniGame;
    public GameObject[] MiniGames;
    public GameObject MiniGameNonCanvas;
    public float timer;
    public float timerMaxTime;
    public float timerSpeed;
    public GameObject TimerGameObject;
    public bool activated;
    public Camera Cam;
    private bool timerStartUp;

    //MiniGame #1
    public GameObject MessageTextBox;
    public GameObject ResponseTextBox;
    public string[] Messages;
    public string[] Responses;
    private char[] responseCharacters;
    private int currentLetter = -5;
    private int sentEmails;
    private bool bootUp;
    private int random;
    private int prevMessage;
    private float typingDelay;
    public float typingDelayMaxTime;

    //MiniGame #2
    public GameObject DocumentTemplate;
    public Sprite[] documentSprites;
    private GameObject[] Documents = new GameObject[5];
    public GameObject Shredder;
    private bool holdingDocument;
    private GameObject heldDocument;
    private int destroyedDocs;
    private bool startUp;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && !activated)
        {
            BootUp();
        }
        
        if (activated)
        {
            if (!timerStartUp)
            {
                TimerGameObject.SetActive(true);
                timer = timerMaxTime;
                timerStartUp = true;
            }
            if (timer >= 0)
            {
                timer -= Time.deltaTime * timerSpeed;
                TimerGameObject.GetComponent<TextMeshProUGUI>().text = "Time Left: " + timer;
                if (currentMiniGame == 1)
                {
                    if (!startUp)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Documents[i] = Instantiate(DocumentTemplate, new Vector3(-5 + (2 * i), 2, transform.position.z), new Quaternion(0, 0, 0, 0), MiniGames[currentMiniGame].transform);
                            Documents[i].GetComponent<Image>().sprite = documentSprites[Random.Range(0, documentSprites.Length)];
                        }
                        startUp = true;
                    }

                    if (!holdingDocument)
                    {
                        for (int i = 0; i < Documents.Length; i++)
                        {
                            if (Documents[i] != null)
                            {
                                if (Input.GetMouseButtonDown(0) && Approx.FastApp(Documents[i].transform.position.x, Cam.ScreenToWorldPoint(Input.mousePosition).x, 0.5f) &&
                                    Approx.FastApp(Documents[i].transform.position.y, Cam.ScreenToWorldPoint(Input.mousePosition).y, 0.5f))
                                {
                                    heldDocument = Documents[i];
                                    holdingDocument = true;
                                }
                            }
                        }
                    }
                    
                    if (holdingDocument)
                    {
                        heldDocument.transform.position = new Vector3(Cam.ScreenToWorldPoint(Input.mousePosition).x, Cam.ScreenToWorldPoint(Input.mousePosition).y, heldDocument.transform.position.z);
                        if (Approx.FastApp(heldDocument.transform.position.x, Shredder.transform.position.x, 0.5f) && Approx.FastApp(heldDocument.transform.position.y, Shredder.transform.position.y, 0.5f))
                        {
                            Destroy(heldDocument);
                            destroyedDocs++;
                            holdingDocument = false;
                        }
                    }

                    if (destroyedDocs == 5)
                    {
                        prevMiniGame = currentMiniGame;
                        while (currentMiniGame == 1)
                        {
                            currentMiniGame = Random.Range(0, MiniGames.Length);
                        }
                        MiniGames[prevMiniGame].SetActive(false);
                        MiniGames[currentMiniGame].SetActive(true);
                        startUp = false;
                        timer += timerMaxTime / 5;
                        TimerGameObject.GetComponent<TextMeshProUGUI>().text = "Time Left: " + timer;
                    }
                }
                else if (currentMiniGame == 0)
                {
                    typingDelay -= Time.deltaTime;
                    if (!bootUp)
                    {
                        random = Random.Range(0, Messages.Length - 1);
                        MessageTextBox.GetComponent<TextMeshProUGUI>().text = Messages[random];
                        responseCharacters = Responses[random].ToCharArray();
                        bootUp = true;
                    }

                    if (Input.anyKey && currentLetter <= responseCharacters.Length && typingDelay <= 0)
                    {
                        currentLetter++;
                        typingDelay = typingDelayMaxTime;
                        if (currentLetter == 0)
                        {
                            ResponseTextBox.GetComponent<TextMeshProUGUI>().text = responseCharacters[0].ToString();
                        }
                        else if (currentLetter < responseCharacters.Length)
                        {
                            ResponseTextBox.GetComponent<TextMeshProUGUI>().text = "";
                            for (int i = 0; i <= currentLetter; i++)
                            {
                                ResponseTextBox.GetComponent<TextMeshProUGUI>().text = ResponseTextBox.GetComponent<TextMeshProUGUI>().text + responseCharacters[i].ToString();
                            }
                        }
                    }

                    if (currentLetter >= responseCharacters.Length)
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            prevMessage = random;
                            while (prevMessage == random)
                            {
                                random = Random.Range(0, Messages.Length);
                            }
                            MessageTextBox.GetComponent<TextMeshProUGUI>().text = Messages[random];
                            responseCharacters = Responses[random].ToCharArray();
                            currentLetter = -1;
                            sentEmails++;
                        }
                    }

                    if (sentEmails == 3)
                    {
                        prevMiniGame = currentMiniGame;
                        while (currentMiniGame == 0)
                        {
                            currentMiniGame = Random.Range(0, MiniGames.Length);
                        }
                        MiniGames[prevMiniGame].SetActive(false);
                        MiniGames[currentMiniGame].SetActive(true);
                        bootUp = false;
                        timer += timerMaxTime / 5;
                        TimerGameObject.GetComponent<TextMeshProUGUI>().text = "Time Left: " + timer;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MiniGames[currentMiniGame].SetActive(false);
            TimerGameObject.SetActive(false);
            activated = false;
        } 
    }

    public void BootUp()
    {
        currentMiniGame = Random.Range(0, MiniGames.Length);
        MiniGames[currentMiniGame].SetActive(true);
        activated = true;
    }
}
