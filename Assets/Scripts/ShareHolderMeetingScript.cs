using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShareHolderMeetingScript : MonoBehaviour
{

    public float monthlyGoal;
    public float quarterlyGoal;
    public float finalGoal;
    public string playerName;

    void Start()
    {
        GetComponent<Image>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        var text = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "Hello " + playerName + ", we are pleased to see you. This month we expect you to make $" +
            monthlyGoal + ". This quarter we expect you to make $" + quarterlyGoal + ". And finally, " +
            "in order to have a successful year we expect that you will make $" + finalGoal;
    }

    public void OKButton()
    {
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(4).GetChild(0).gameObject.GetComponent<TimeScript>().timePaused = false;
    }
}
