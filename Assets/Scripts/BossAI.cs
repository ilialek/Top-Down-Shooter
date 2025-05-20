using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform playerTransform;

    public float targetAngle = 130f;
    public float rotateTime = 2f;
    float tolerance = 0.1f;

    private float currentTime = 0f;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    int whichAction = 0;

    public float fireRate = 1.5f;
    public float fireRateExplosion;
    float timeToFire = 0;
    public AudioSource src1;
    public AudioSource src2;
    public Transform firePoint;
    public LayerMask whatToHit;
    RaycastHit2D hit;

    public List<GameObject> bloodTraces = new List<GameObject>();
    public GameObject bloodPrefab;
    public GameObject bloodsplash;

    public Transform bulletTrail;
    public Transform bulletPoint;

    public Transform muzzleFlash;
    public Transform muzzleFlashPoint;

    public GameObject explosion;

    bool onceBool;
    int localCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    
    void Update()
    {

        if (whichAction == 0 && localCount == 4)
        {
            whichAction = 1;
            timeToFire = 0;
            localCount = 0;
        }

        if (whichAction == 1 && localCount == 4)
        {
            whichAction = 0;
            timeToFire = 0;
            localCount = 0;
        }

        if (whichAction == 0) {
            DoTheFirstAction();
        }

        if (whichAction == 1)
        {
            DoTheSecondAction();
        }
    }


    bool HasReachedRotation()
    {
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
        return angleDifference < tolerance;
    }
    void DoTheFirstAction() {

        if (!HasReachedRotation())
        {
            currentTime += Time.deltaTime;
            float t = currentTime / rotateTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
        }
        else
        {
            localCount++;
            onceBool = true;
            targetAngle = -targetAngle; // set target angle to opposite direction
            targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            startRotation = transform.rotation;
            currentTime = 0f;
        }

        if (Time.time > timeToFire && onceBool)
        {
            timeToFire = Time.time + 1 / fireRate;
            src1.Play();

            
            Shoot();
        }



    }

    void DoTheSecondAction()
    {
        if (Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / fireRateExplosion;
            src2.Play();

            Instantiate(explosion, new Vector2(0, 4.7f), Quaternion.identity);
            localCount++;
        }
    }



    void Shoot()
    {

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up);

        Effect();

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("Player has been hit!");
            bloodTraces.Add(bloodPrefab);
            Instantiate(bloodTraces[bloodTraces.Count - 1], hit.transform.position, Quaternion.Euler(0, 0, rb.rotation));
            Instantiate(bloodsplash, hit.transform.position, Quaternion.identity);
            hit.collider.GetComponent<playerController>().health -= 34;
        }

    }

    void Effect()
    {
        Instantiate(bulletTrail, bulletPoint.position, bulletPoint.rotation);
        Transform flashClone = Instantiate(muzzleFlash, muzzleFlashPoint.position, muzzleFlashPoint.rotation);
        flashClone.parent = muzzleFlashPoint;
        float size = Random.Range(0.6f, 1f);
        flashClone.localScale = new Vector3(size, size, 0);
        Destroy(flashClone.gameObject, 0.05f);
    }


    void FixedUpdate() {
        //Vector2 direction = (Vector2)playerTransform.position - (Vector2)transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
