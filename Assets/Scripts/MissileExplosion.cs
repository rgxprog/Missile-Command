using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    //-------------------------------------------

    public AudioSource explosionSound;

    private bool exploding = false;
    private float alphaColor = 1f;

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

            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, alphaColor);

            if (alphaColor < 0f)
            {
                destroyMissile();
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

    private void destroyMissile()
    {
        GetComponentInParent<MissileLogic>().destroyMissile();
    }
    
    //-------------------------------------------
}
