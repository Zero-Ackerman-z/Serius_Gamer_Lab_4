using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    public Sprite defaultSprite;
    public Sprite upgradedSprite;
    public Sprite defaultProjectileSprite; // Sprite base del proyectil
    public Sprite upgradedProjectileSprite; // Nuevo sprite para el proyectil

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isFacingRight = true;

    private DashAbility dashAbility;
    private ShootAbility shootAbility;

    private SpriteRenderer spriteRenderer;
    private int numberOfProjectiles = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Crear habilidades
        dashAbility = new DashAbility(this, dashSpeed, dashDuration);
        shootAbility = new ShootAbility(this, projectilePrefab, firePoint, projectileSpeed, Color.white, numberOfProjectiles, defaultProjectileSprite, upgradedProjectileSprite); // Inicializar con el sprite por defecto
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        Move();

        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }

        dashAbility.Execute();
        shootAbility.Execute();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public bool IsFacingRight => isFacingRight;

    public void ChangeAppearanceAndShotCount(Sprite newSprite, int newNumberOfProjectiles, Sprite newProjectileSprite)
    {
        spriteRenderer.sprite = newSprite;
        numberOfProjectiles = newNumberOfProjectiles;
        shootAbility.UpdateProjectileCount(numberOfProjectiles);
        shootAbility.UpdateProjectileSprite(newProjectileSprite); // Actualizar el sprite del proyectil
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            ChangeAppearanceAndShotCount(upgradedSprite, 3, upgradedProjectileSprite); // Cambia el sprite del proyectil

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PowerUpTempol"))
        {
            shootAbility.ChangeProjectileAppearance(); // Cambiar la apariencia de las balas
            Destroy(collision.gameObject);
        }
    }
}
