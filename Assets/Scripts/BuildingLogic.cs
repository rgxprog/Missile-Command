using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLogic : MonoBehaviour
{
    //-------------------------------------------

    private bool isAlive;

    //-------------------------------------------

    private void Awake()
    {
        init();
    }

    //-------------------------------------------

    public void init()
    {
        isAlive = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            isAlive = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 1f);

            GameManager.instance.removeBuilding();
        }
    }
        
    //-------------------------------------------
}
