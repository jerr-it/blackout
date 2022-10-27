using UnityEngine;

public class Explode : MonoBehaviour {
    [SerializeField] private float lifetime;

    void Start() {
        Destroy(gameObject, lifetime);
    }
}
