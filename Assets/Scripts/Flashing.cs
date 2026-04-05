using UnityEngine;

public class Flashing : MonoBehaviour
{
    public Sprite whiteOutline;
    public Sprite blackOutline;
    public bool flashing = false;
    private float flashTimer;

    // Update is called once per frame
    void Update()
    {
        if (flashing)
        {
            
            if (flashTimer % 24 == 0)
            {
                GetComponent<SpriteRenderer>().sprite = whiteOutline;
            }
            
            if(flashTimer % 50 == 0)
            {
                GetComponent<SpriteRenderer>().sprite = blackOutline;
            }

            if (flashTimer >= 101)
            {
                flashTimer = 0;
            }
            else
            {
                flashTimer += 0.5f;
            }

        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
