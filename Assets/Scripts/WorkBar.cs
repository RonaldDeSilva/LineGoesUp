using UnityEngine;

public class WorkBar : MonoBehaviour
{
    public Sprite workBar0;
    public Sprite workBar1;
    public Sprite workBar2;
    public Sprite workBar3;
    public Sprite workBar4;
    public Sprite workBar5;
    public Sprite workBar6;
    public Sprite workBar7;
    public Sprite workBar8;
    public Sprite workBar9;
    private GameObject employee;
    private SpriteRenderer workBarSprite;
    private bool working = false;
    private float timer;
    public GameObject moneyPrefab;
    private MoneyBarScript MBS;

    void Start()
    {
        workBarSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        workBarSprite.gameObject.SetActive(false);
        MBS = GameObject.Find("MoneyBar").GetComponent<MoneyBarScript>();
    }

    void Update()
    {
        if (working)
        {
            workBarSprite.gameObject.SetActive(true);
            if (employee != null)
            {
                if (timer <= 1)
                {
                    workBarSprite.sprite = workBar0;
                }
                else if (timer <= 2)
                {
                    workBarSprite.sprite = workBar1;
                }
                else if (timer <= 3)
                {
                    workBarSprite.sprite = workBar2;
                }
                else if (timer <= 4)
                {
                    workBarSprite.sprite = workBar3;
                }
                else if (timer <= 5)
                {
                    workBarSprite.sprite = workBar4;
                }
                else if (timer <= 6)
                {
                    workBarSprite.sprite = workBar5;
                }
                else if (timer <= 7)
                {
                    workBarSprite.sprite = workBar6;
                }
                else if (timer <= 8)
                {
                    workBarSprite.sprite = workBar7;
                }
                else if (timer <= 9)
                {
                    workBarSprite.sprite = workBar8;
                }
                else if (timer <= 10)
                {
                    workBarSprite.sprite = workBar9;
                    MBS.money += employee.GetComponent<EmployeeScript>().productivity * employee.GetComponent<EmployeeScript>().experience / (employee.GetComponent<EmployeeScript>().burnout);
                    timer = 0;
                }
                else
                {
                    timer = 0;
                }

                timer += 0.01f * (employee.GetComponent<EmployeeScript>().productivity * employee.GetComponent<EmployeeScript>().experience) / employee.GetComponent<EmployeeScript>().burnout;
            }
        }
        else
        {
            workBarSprite.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Employee"))
        {
            if (employee == null)
            {
                employee = collision.gameObject;
            }
            working = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Employee"))
        {
            working = false;
        }
    }
}
