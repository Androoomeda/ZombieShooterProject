using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public bool inputIsBlocked;
    [HideInInspector] public GameObject interacteObject { get; private set; }

    [SerializeField] private GameObject interacteButton;
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick aimingJoystick;

    private Vector2 movementDirection;
    private Vector2 aimingDirection;
    private float pickupCountDown;

    public Vector2 MoveInput 
    {
        get
        {
            if (!inputIsBlocked && movementJoystick.Direction.magnitude > 0.3f)
                return movementJoystick.Direction;
            return Vector2.zero;
        }
    }

    public Vector2 AimingInput
    {
        get
        {
            if (!inputIsBlocked)
                return aimingJoystick.Direction;
            return Vector2.zero;
        }
    }

    public bool isShooting 
    {
        get
        {
#if UNITY_ANDROID
            if (aimingJoystick.Direction.magnitude > 0.5f && !inputIsBlocked)
                return true;
#endif

#if UNITY_WINDOWS
            if (Input.GetMouseButton(0) && !inputIsBlocked)
                return true;
#endif
            return false;
        }
    }

    public bool isCanInteracte
    {
        get
        {
            if (interacteObject && !inputIsBlocked)
                return true;

            return false;
        }
    }

    private void Update()
    {
        if (pickupCountDown > 0)
            pickupCountDown -= Time.deltaTime;

#if UNITY_WINDOWS
        movementDirection.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimingDirection = mousePos - (Vector2)transform.position;
#endif
    }

#if UNITY_ANDROID
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Weapon>(out Weapon pickup) && pickupCountDown <= 0)
        {
            interacteButton.SetActive(true);
            interacteObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Weapon>(out Weapon weapon))
        {
            interacteButton.SetActive(false);
            interacteObject = null;
            pickupCountDown = 0.3f;
        }
    }
#endif 
}
