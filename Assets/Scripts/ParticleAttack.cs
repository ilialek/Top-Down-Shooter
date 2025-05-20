using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttack : MonoBehaviour
{

    public ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 3) {
            Debug.Log("particle hit the player");
            other.GetComponent<playerController>().health -= 10;
        }
    }

    void Update()
    {

    }
}
