using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTopLogic : MonoBehaviour
{
    //-------------------------------------------

    public GameObject cannon;
    public TMPro.TextMeshProUGUI ammoCountText;

    private bool isAlive;
    private int maxAmmo, ammoLeft;

    //-------------------------------------------

    private void Start()
    {
        init();
    }

    //-------------------------------------------

    public void init()
    {
        maxAmmo = 10;
        ammoLeft = maxAmmo;
        isAlive = true;

        setShotCountText();

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        cannon.transform.Find("Square").gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 1f);
        ammoCountText.color = Color.green;
    }

    //-------------------------------------------

    public bool getIsAlive()
    {
        return isAlive;
    }

    //-------------------------------------------

    private void Update()
    {
        if (isAlive && GameManager.instance.getGameState() == GameManager.GameState.inGame)
        {
            Vector2 moveDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (moveDirection != Vector2.zero)
            {
                float angle = (Mathf.Atan2(moveDirection.y, moveDirection.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
                if (-90f <= angle && angle <= 90f)
                {
                    cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        } // (isAlive)
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            disableTower();
        }
    }

    //-------------------------------------------

    public int getAmmoLeft()
    {
        return ammoLeft;
    }

    //-------------------------------------------

    public void shot()
    {
        ammoLeft--;
        setShotCountText();
    }

    //-------------------------------------------

    private void setShotCountText()
    {
        if (ammoLeft > 0)
        {
            ammoCountText.text = ammoLeft.ToString();
        }
        else
        {
            disableTower();
        }
    }

    //-------------------------------------------

    private void disableTower()
    {
        isAlive = false;
        
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
        cannon.transform.Find("Square").gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0.3f, 0f, 1f);

        GameManager.instance.removeTower();

        ammoCountText.color = Color.red;
        ammoCountText.text = "OUT";
    }

    //-------------------------------------------
}
