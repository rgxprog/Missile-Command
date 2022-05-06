using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMissileExplosion : MonoBehaviour
{
    //-------------------------------------------

    private bool exploding = false;
    private float alphaColor = 1f;

    public AudioSource explosionSound;

    //-------------------------------------------

    private void Update()
    {
        if (exploding)
        {
            float ratio = 0.3f * Time.deltaTime;
            alphaColor -= 0.3f * Time.deltaTime;

            if (transform.localScale.x < 1.0f)
            {
                transform.localScale = new Vector3(transform.localScale.x + ratio, transform.localScale.y + ratio, transform.localScale.z);
            }

            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, alphaColor);

            if (alphaColor < 0f)
            {
                destroyAntiMissile();
            }
        }
    }

    //-------------------------------------------

    public void startExploding()
    {
        exploding = true;
        explosionSound.Play();
    }

    //-------------------------------------------

    private void destroyAntiMissile()
    {
        GetComponentInParent<AntiMissileLogic>().destroyAntiMissile();
    }

    //-------------------------------------------
}
