using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarBehavior : MonoBehaviour
{
    public Color low;
    public Color high;
    public Vector2 offset;
    EnemyAI enemyScript;
    playerController playerScript;
    public SpriteRenderer spriteRenderer;

    //public void SetHelath(float health, float maxHealth) {
    //    slider.value = health;
    //    slider.maxValue = maxHealth;
    //    slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    //}

    private void Start()
    {
        if (gameObject.name == "Player_hp") { playerScript = transform.parent.GetComponent<playerController>(); }
        else { enemyScript = transform.parent.GetComponent<EnemyAI>(); }
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (gameObject.name == "Player_hp") {
            transform.GetChild(0).localScale = new Vector3(playerScript.health / 100, 1, 0);
            spriteRenderer.color = Color.Lerp(low, high, playerScript.health / 100);
        }
        else {
            transform.GetChild(0).localScale = new Vector3((float)enemyScript.health / 100, 1, 0);
            spriteRenderer.color = Color.Lerp(low, high, (float)enemyScript.health / 100);
        }

        transform.position = new Vector2(transform.parent.position.x + offset.x, transform.parent.position.y + offset.y);
    }
}
