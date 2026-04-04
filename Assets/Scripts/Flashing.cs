using UnityEngine;

public class Flashing : MonoBehaviour
{
    public Sprite whiteOutline;
    public Sprite blackOutline;
    public bool flashing = false;
    private int flashTimer;

    // Update is called once per frame
    void Update()
    {
        if (flashing)
        {
            Debug.Log("Flashing");
            if (flashTimer % 20 == 0)
            {
                GetComponent<SpriteRenderer>().sprite = whiteOutline;
            }
            
            if(flashTimer % 65 == 0)
            {
                GetComponent<SpriteRenderer>().sprite = blackOutline;
            }

            if (flashTimer >= 121)
            {
                flashTimer = 0;
            }
            else
            {
                flashTimer++;
            }

        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
