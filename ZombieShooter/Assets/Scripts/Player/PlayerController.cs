using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Sprite bodyColts, bodyOtherGuns;
    [SerializeField] private GameObject body, legs;
    [SerializeField] private Text ammoAmountText;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int ammoAmount;

    [Header("AUDIO")]
    [SerializeField] private AudioSource takeAmmoSound;
    [SerializeField] private AudioSource runSound;
    [SerializeField] private AudioSource pickupWeaponSound;

    private PlayerInput playerInput;
    private Weapon weapon;
    private Rigidbody2D rigidbody;
    private Animator legsAnim; 
    private SpriteRenderer bodyRenderer;

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody2D>();
        legsAnim = legs.GetComponent<Animator>();
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        if (playerInput.isShooting)
            weapon.TryShoot();

        if (weapon.currentAmmoAmount <= 0 && !weapon.isReloading && ammoAmount > 0)
            ReloadWeapon();

        ammoAmountText.text = $"{weapon.currentAmmoAmount}/{ammoAmount}";
    }

    private void FixedUpdate()
    {
        Move();
        RotateBody();
    }

    private void ReloadWeapon()
    {
        if ((ammoAmount - weapon.maxAmmoAmount) < 0)
        {
            weapon.Reload(ammoAmount);
            ammoAmount = 0;
        }
        else
        {
            weapon.Reload(weapon.maxAmmoAmount);
            ammoAmount -= weapon.maxAmmoAmount;
        }
    }

    private void Move()
    {
        Vector2 movement = playerInput.MoveInput * movementSpeed;
        rigidbody.velocity = playerInput.MoveInput * movementSpeed;

        if (movement != Vector2.zero)
        {
            if(!runSound.isPlaying)
                runSound.Play();

            RotateLegs(movement);
            legsAnim.SetBool("isWalking", true);
        }
        else
        {
            runSound.Stop();
            legsAnim.SetBool("isWalking", false);
        }
    }

    private void RotateLegs(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        legs.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void RotateBody()
    {
        if (playerInput.AimingInput != Vector2.zero)
        {
            Vector2 direction = playerInput.AimingInput;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, targetRotation, 0.3f);
        }
        else if(playerInput.MoveInput != Vector2.zero)
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, legs.transform.rotation, 0.4f);
    }


    public void PickupWeapon()
    {
        GameObject weaponObject = playerInput.interacteObject;
        Weapon newWeapon = weaponObject.GetComponent<Weapon>();

        this.weapon.Drop();
        this.weapon = newWeapon;
        newWeapon.Pickup();

        pickupWeaponSound.Play();

        weaponObject.transform.parent = body.transform;
        weaponObject.transform.localPosition = Vector2.zero;
        weaponObject.transform.rotation = body.transform.rotation;

        if (weaponObject.gameObject.name.Contains("Colts"))
            bodyRenderer.sprite = bodyColts;
        else
            bodyRenderer.sprite = bodyOtherGuns;
    }

    public void TakeAmmo(int ammo)
    {
        takeAmmoSound.PlayOneShot(takeAmmoSound.clip);
        ammoAmount += ammo;
        ammoAmountText.text = $"{weapon.currentAmmoAmount}/{ammoAmount}";
    }

}
