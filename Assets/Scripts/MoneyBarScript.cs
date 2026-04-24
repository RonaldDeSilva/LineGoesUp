using TMPro;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;

public class MoneyBarScript : MonoBehaviour
{
    public Sprite[] sprites;
    public float money;
    public float goalAmount;
    private Image sr;
    private float prevMoney;

    void Start()
    {
        sr = GetComponent<Image>();
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "$0 / " + goalAmount.ToString("C", new CultureInfo("en-US"));
    }

    void Update()
    {
        if (prevMoney != money)
        {
            sr.sprite = sprites[Mathf.Clamp((int)(money / goalAmount * sprites.Length), 0, 56)];
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = money.ToString("C", new CultureInfo("en-US")) + " / " + goalAmount.ToString("C", new CultureInfo("en-US"));
        }
    }
}
