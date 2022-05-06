using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLogic : MonoBehaviour
{
    //-------------------------------------------
    
    private Vector3 startPosition, target, moveDirection;
    private float speed, minSpeed, maxSpeed;
    private bool isAlive;

    public GameObject missileSprite;
    public GameObject explosion;
    public GameObject particles;

    //-------------------------------------------

    private void Awake()
    {
        setStartPosition(
            Random.Range(-8f, 8f),
            6f
        );

        setTarget(
            Random.Range(-7.5f, 7.5f),
            -4f
        );

        moveDirection = target - startPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = (Mathf.Atan2(moveDirection.y, moveDirection.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        minSpeed = 1f;
        maxSpeed = 3f;
        
        speed =
            (GameManager.instance.getCurrentLevel() - 1f) / (10f - 1f) * (maxSpeed - minSpeed) + 1;
        speed = speed > maxSpeed ? maxSpeed : speed;

        isAlive = true;
    }

    //-------------------------------------------

    private void Update()
    {
        if (isAlive)
        {
            transform.position += moveDirection.normalized * speed * Time.deltaTime;
        }
    }

    //-------------------------------------------

    public void setStartPosition(float x, float y)
    {
        startPosition = new Vector2(x, y);
        transform.position = startPosition;
    }

    public void setTarget(float x, float y)
    {
        target = new Vector2(x, y);
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isAlive = false;
        explosion.SetActive(true);
        explosion.GetComponent<MissileExplosion>().startExploding();
        particles.GetComponent<ParticleSystem>().Stop();
        missileSprite.SetActive(false);
    }

    //-------------------------------------------

    public void destroyMissile()
    {
        Destroy(gameObject);
    }

    //-------------------------------------------
}
