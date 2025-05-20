using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraMovement : MonoBehaviour
{
    public Transform player;
    public SceneTransition loadScene;

    public int i;

    public float damping;
    public playerController playerScript;

    public Image bossHealthBar;

    Vector2 movePosition = new Vector2(-50,0);

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        
    }

    void Update()
    {
        int i = FindObjectsOfType<EnemyAI>().Length;

        if (transform.position.x < -45) {

            if (playerScript.health <= 0)
            {
                switch (loadScene.whichScene) {

                    case 1:
                        loadScene.LoadScene1();
                    break;

                    case 2:
                        loadScene.LoadScene2();
                        break;

                    case 3:
                        loadScene.LoadScene3();
                        break;

                }
            }

            if (playerScript.health > 0) {
                switch (loadScene.whichScene)
                {

                    case 1:
                        loadScene.LoadScene2();
                        break;

                    case 2:
                        loadScene.LoadScene3();
                        break;

                    case 3:
                        loadScene.LoadScene4();
                        break;

                }
            }
        }


    }

    void FixedUpdate()
    {

        Vector3 movePosition = Vector3.ClampMagnitude(playerScript.direction, 2);


        switch (loadScene.whichScene)
        {
            case 1:
                if (playerScript.health > 0 && FindObjectsOfType<EnemyAI>().Length > 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(movePosition.x + player.position.x, movePosition.y + player.position.y, -10), ref velocity, damping);
                }
                if (playerScript.health <= 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-50, 0, -10), ref velocity, damping);
                }
                if ((FindObjectsOfType<EnemyAI>().Length == 0))
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-50, 0, -10), ref velocity, damping);
                }
                break;

            case 2:
                if (playerScript.health > 0 && FindObjectsOfType<EnemyAI>().Length != 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(movePosition.x + player.position.x, movePosition.y + player.position.y, -10), ref velocity, damping);
                }
                if (playerScript.health <= 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-50, 0, -10), ref velocity, damping);
                }
                if ((FindObjectsOfType<EnemyAI>().Length == 0))
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-50, 0, -10), ref velocity, damping);
                }
                break;

            case 3:
                if (playerScript.health > 0 && bossHealthBar.fillAmount != 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(movePosition.x + player.position.x, movePosition.y + player.position.y, -10), ref velocity, damping);
                }
                if (playerScript.health <= 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-50, 0, -10), ref velocity, damping);
                }
                if (bossHealthBar.fillAmount <= 0)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(-50, 0, -10), ref velocity, damping);
                }
                break;
        }




    }
}
