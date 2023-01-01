using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform rotationTarget;
    private string controlScheme;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isShooting;
    private Vector3 lookingAt;

    [Space(10)]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    
    [Space(10)]
    private ObjectPool<GameObject> _bulletPool;
    [SerializeField] private int bulletPoolSizeDefault = 10;
    [SerializeField] private int bulletPoolSizeMax = 500;

    #region UIComponents

    [Space(10)]
    [SerializeField] private GameObject deviceLostPanel;
    #endregion

    void Start() {
        controlScheme = GetComponent<PlayerInput>().currentControlScheme;
        rb = GetComponent<Rigidbody2D>();
        _bulletPool = new ObjectPool<GameObject>(
            () => Instantiate(bullet, transform), 
            (obj) => obj.SetActive(true), 
            (obj) => obj.SetActive(false),
            (obj) => Destroy(obj),
            false,
            bulletPoolSizeDefault,
            bulletPoolSizeMax
        );

    }

    void Update() {
        
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(Time.deltaTime * movement.x * movementSpeed, Time.deltaTime * movement.y * movementSpeed);
    }

    public void OnPlayerAim(InputAction.CallbackContext context) {
        Vector2 aimTarget = context.ReadValue<Vector2>();

        if (controlScheme == "Controller") {
            if (aimTarget.x * aimTarget.x + aimTarget.y * aimTarget.y < 0.1f) {
                return;
            }
            // Remap the controller's joystick values to the mouse's position
            // Controller joystick values are between -1 and 1 while mouse position is between 0 and width/height
            aimTarget = MapVector(aimTarget, new Vector2(-1f, 1f), new Vector2(0f, Screen.width), new Vector2(-1f, 1f), new Vector2(0f, Screen.height));
        }

        Vector3 dir = new Vector3(aimTarget.x, aimTarget.y, 0) - cam.WorldToScreenPoint(transform.position);
        lookingAt = dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotationTarget.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
    }

    public void OnPlayerMove(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
    }

    public void OnPlayerShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;
            StartCoroutine(ShootingInterval());
        }
        if (context.canceled) isShooting = false;
    }

    private IEnumerator ShootingInterval()
    {
        while (isShooting)
        {
            ShootBullet();
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void ShootBullet() {
        var currentBullet = _bulletPool.Get();
        currentBullet.transform.position = gun.transform.position;
        currentBullet.transform.rotation = gun.transform.rotation;

    }

    public void OnBulletCollision(GameObject hitBullet)
    {
        Debug.Log("pepega");
        _bulletPool.Release(hitBullet);
    }
    

    public void OnDeviceLost() {
        deviceLostPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = controlScheme + " lost connection";
        deviceLostPanel.SetActive(true);
    }

    public void OnDeviceRegained() {
        deviceLostPanel.SetActive(false);
    }

    private Vector2 MapVector(Vector2 input, Vector2 old_x_range, Vector2 new_x_range, Vector2 old_y_range, Vector2 new_y_range) {
        float x = Map(input.x, old_x_range.x, old_x_range.y, new_x_range.x, new_x_range.y);
        float y = Map(input.y, old_y_range.x, old_y_range.y, new_y_range.x, new_y_range.y);
        return new Vector2(x, y);
    }

    private float Map(float input, float old_min, float old_max, float new_min, float new_max) {
        return (input - old_min) / (old_max - old_min) * (new_max - new_min) + new_min;
    }
}
