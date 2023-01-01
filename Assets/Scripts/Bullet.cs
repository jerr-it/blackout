using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private GameObject explosion;
    [SerializeField] private UnityEvent onCollission;
    private PlayerController player;

    private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.GameObject() == player.GameObject()) return; 
        Vector3 newdirection = Vector3.Reflect(transform.up, collision.GetContact(0).normal);

        Instantiate(explosion, transform.position, Quaternion.LookRotation(newdirection) * Quaternion.Euler(90, 0, 0));
        player.OnBulletCollision(this.GameObject());
    }

    private void OnEnable() {
        rb.velocity = transform.forward * speed;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = transform.parent.GameObject().GetComponent<PlayerController>();
    }
}
