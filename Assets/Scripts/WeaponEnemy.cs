using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponEnemy : MonoBehaviour
{
    public List<GameObject> bloodTraces = new List<GameObject>();
    public GameObject bloodPrefab;
    public GameObject bloodsplash;

    public Transform muzzleFlash;
    public Transform muzzleFlashPoint;

    public Transform bulletTrail;
    public Transform bulletPoint;

    public AudioSource src;

    EnemyAI enemyScript;

    public LayerMask whatToHit;

    RaycastHit2D hit;

    float[] fireRate = { 2, 1f, 1f };
    int[] damage = { 12, 16, 24 };

    //current weapon
    int currentWeapon = 0;

    float timeToFire = 0;

    void Start()
    {
        enemyScript = GetComponentInParent<EnemyAI>();
        if (transform.parent.name == "Big_enemy") {
            currentWeapon = 2;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2 && transform.parent.name == "Green_enemy") {
            currentWeapon = 1;
        }
    }

    void Update()
    {
        if (enemyScript.canShoot && Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate[currentWeapon];
            src.Play();
            Shoot();
        }
    }

    void Shoot()
    {

        hit = Physics2D.Raycast(enemyScript.firePoint, enemyScript.rb.velocity, 100, whatToHit);

        Effect();

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            bloodTraces.Add(bloodPrefab);
            Instantiate(bloodTraces[bloodTraces.Count - 1], hit.transform.position, Quaternion.Euler(0, 0, enemyScript.rb.rotation));
            Instantiate(bloodsplash, hit.transform.position, Quaternion.identity);
            hit.collider.GetComponent<playerController>().health -= damage[currentWeapon];
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
}
