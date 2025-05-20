using System.Collections;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 40);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
            Destroy(gameObject);
    }
}
