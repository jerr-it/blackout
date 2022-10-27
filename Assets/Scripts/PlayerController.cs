using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform rotationTarget;
    private string controlScheme;
    private Rigidbody2D rb;
    private Vector2 movement;

    [Space(10)]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;

    #region UIComponents

    [Space(10)]
    [SerializeField] private GameObject deviceLostPanel;
    #endregion

    void Start() {
        controlScheme = GetComponent<PlayerInput>().currentControlScheme;
        rb = GetComponent<Rigidbody2D>();
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
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotationTarget.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
    }

    public void OnPlayerMove(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
    }

    public void OnPlayerShoot(InputAction.CallbackContext context) {

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
