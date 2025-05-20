using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public List<GameObject> bloodTraces = new List<GameObject>();
    public GameObject bloodPrefab;
    public GameObject bloodsplash;

    public Transform muzzleFlash;
    public Transform muzzleFlashPoint;
    public Transform bulletTrail;
    public Transform bulletPoint;
    public Transform doorBreakPrefab;

    public Image healthBar;

    public AudioSource src;

    public Sprite[] weapons;
    SpriteRenderer spriteRenderer;

    public playerController playerScript;

    public Transform firePoint;
    public LayerMask whatToHit;

    RaycastHit2D hit;

    float[] fireRate = { 3, 8f, 8f };
    int[] damage = { 7, 4, 2 };

    //current weapon
    public int currentWeapon;

    float timeToFire = 0;
    float timeToLimit = 0;

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 1)
        {
            currentWeapon = 0;
        }
        else if (index == 2)
        {
            currentWeapon = 1;
        }
        else {
            currentWeapon = 2;
        }
    }

    void Update()
    {
        spriteRenderer.sprite = weapons[currentWeapon];

        if (Input.GetMouseButton(0) && Time.time > timeToFire) {
            timeToFire = Time.time + 1 / fireRate[currentWeapon];
            src.Play();
            Shoot();
        }
        
    }

    void Shoot() {

        hit = Physics2D.Raycast(firePoint.transform.position, playerScript.direction, 100, whatToHit);
        Effect();

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.DrawLine(firePoint.transform.position, hit.point, Color.black);
            bloodTraces.Add(bloodPrefab);
            Instantiate(bloodTraces[bloodTraces.Count - 1], hit.transform.position, Quaternion.Euler(0,0,playerScript.angle));
            Instantiate(bloodsplash, hit.transform.position, Quaternion.identity);
            hit.collider.GetComponent<EnemyAI>().health -= damage[currentWeapon];
        }

        if (hit.transform.gameObject.layer == 12)
        {
            if (healthBar != null) {
                healthBar.fillAmount -= 0.04f;
            }
            
        }

        if (hit.transform.gameObject.layer == 10)
        {
            Destroy(hit.transform.gameObject);
            Instantiate(doorBreakPrefab, hit.transform.position, Quaternion.identity);
        }
        if (hit.transform.gameObject.layer == 6) {
        }

    }

    void Effect() {
        Instantiate(bulletTrail, bulletPoint.position, bulletPoint.rotation);
        Transform flashClone = Instantiate(muzzleFlash, muzzleFlashPoint.position, muzzleFlashPoint.rotation);
        flashClone.parent = muzzleFlashPoint;
        float size = Random.Range(0.6f, 1f);
        flashClone.localScale = new Vector3(size, size, 0);
        Destroy(flashClone.gameObject, 0.05f);
    }

}
