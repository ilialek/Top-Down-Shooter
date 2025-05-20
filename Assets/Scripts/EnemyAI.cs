using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public string testValue;

    public Transform target;
    public Transform[] wayPoints;
    int currentWayPointWhenLooking = 0;

    public Vector2 firePoint;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject deadBody;
    public Transform powerUpPrefab;

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public AudioSource src1;

    public bool canShoot;

    public float speed = 300f;
    public float nextWayPointDistnce = 3f;

    bool spotted = false;

    public int health = 100;

    Animator animator;

    Path path;
    int currentWayPoint = 0;
    bool isReachedTheEnd = false;
    bool isInFieldOfView = false;
    bool reachedTheEnd = false;

    Seeker seeker;
    public Rigidbody2D rb;

    void Awake()
    {
        target = GameObject.Find("Player").transform;

        playerLayer = LayerMask.GetMask("Player");
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(FOVRoutine());

        InvokeRepeating("UdpatePath", 0f, .5f);
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {

        if (Physics2D.OverlapCircle(transform.position, radius, playerLayer))
        {

            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer))
                {
                    isInFieldOfView = true;
                    spotted = true;
                }
                else
                {
                    isInFieldOfView = false;
                }
            }
            else
            {
                isInFieldOfView = false;
            }
        }
        //else if (isPlayerFound)
        //    isPlayerFound = false;
    }


    void UdpatePath() {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWayPoint = 0;
        }
    }

    void Update()
    {


        if (health <= 0) {
            Instantiate(deadBody, transform.position, Quaternion.Euler(0, 0, rb.rotation));
            if (powerUpPrefab != null) {
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }
            src1.Play();
            Destroy(gameObject);
        }

        if (health != 100) {
            spotted = true;
        }

        // animations
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            animator.SetBool("toWalk", true);
        }
        else
        {
            animator.SetBool("toWalk", false);
        }

        // setting up the fire point
        if (gameObject.name == "Big_enemy")
        {
            firePoint = new Vector2(transform.position.x - .1f, transform.position.y);
        }
        else
        {
            firePoint = new Vector2(transform.position.x, transform.position.y);
        }

    }

    void FixedUpdate()
    {

        // check the path
        if (path == null) {
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
            isReachedTheEnd = true;
            return;
        }
        else {

            isReachedTheEnd = false;
        }


        if (spotted)
        {

            Following();
        }

        else {

            Watching();
        }


        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;


    }

    void Following() {
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        float distanceToWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        float distanceToPlayer = Vector2.Distance(rb.position, target.position);

        if (distanceToWaypoint < nextWayPointDistnce)
        {
            currentWayPoint++;
        }
        if (distanceToPlayer <= 6 && isInFieldOfView)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        rb.AddForce(force);
    }

    void Watching(){

        Vector2 direction = ((Vector2)wayPoints[currentWayPointWhenLooking].position - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, wayPoints[currentWayPointWhenLooking].position);



        if (!reachedTheEnd)
        {
            if (distance < nextWayPointDistnce)
            {
                if (currentWayPointWhenLooking == wayPoints.Length - 1)
                {
                    currentWayPointWhenLooking--;
                    reachedTheEnd = true;
                }
                else
                {
                    currentWayPointWhenLooking++;
                }

            }

        }
        else
        {
            if (distance < nextWayPointDistnce)
            {
                if (currentWayPointWhenLooking == 0)
                {
                    currentWayPointWhenLooking++;
                    reachedTheEnd = false;
                }
                else
                {
                    currentWayPointWhenLooking--;
                }
            }

        }
    }



}
