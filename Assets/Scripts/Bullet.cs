using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private GameObject explosion;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        Destroy(gameObject, maxLifetime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Vector3 newdirection = Vector3.Reflect(transform.up, collision.GetContact(0).normal);

        Instantiate(explosion, transform.position, Quaternion.LookRotation(newdirection) * Quaternion.Euler(90, 0, 0));
        Destroy(gameObject);
    }
}
