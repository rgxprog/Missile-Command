using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMissileLogic : MonoBehaviour
{
    //-------------------------------------------

    private Vector3 startPosition, target, moveDirection;
    private float speed, originalDistance;
    private bool isAlive;

    public GameObject antiMissileSprite;
    public GameObject explosion;
    public GameObject particles;
    public AudioSource shotSound;

    //-------------------------------------------

    private void Awake()
    {
        speed = 4f;
        isAlive = true;
    }

    //-------------------------------------------

    private void Start()
    {
        init();
        shotSound.Play();
    }

    //-------------------------------------------

    private void Update()
    {
        if (isAlive)
        {
            transform.position += moveDirection.normalized * speed * Time.deltaTime;

            if (Vector2.Distance(startPosition, transform.position) >= originalDistance)
            {
                startExplosion();
            }
        }
    }

    //-------------------------------------------

    public void init()
    {
        startPosition = transform.position;
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0f;
        originalDistance = Vector2.Distance(startPosition, target);

        moveDirection = target - startPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = (Mathf.Atan2(moveDirection.y, moveDirection.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    //-------------------------------------------

    private void startExplosion()
    {
        isAlive = false;
        explosion.SetActive(true);
        explosion.GetComponent<AntiMissileExplosion>().startExploding();
        particles.GetComponent<ParticleSystem>().Stop();
        antiMissileSprite.SetActive(false);
    }

    //-------------------------------------------

    public void destroyAntiMissile()
    {
        Destroy(gameObject);
    }

    //-------------------------------------------

}
