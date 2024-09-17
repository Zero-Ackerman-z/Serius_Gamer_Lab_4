using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShootAbility : PlayerAbility
{
    private GameObject projectilePrefab;
    private Transform firePoint;
    private float projectileSpeed;
    private bool canShoot = true;
    private float shootCooldown = 0.5f;

    private Color bulletColor; // Color de las balas
    private Sprite projectileSprite; // Sprite del proyectil
    private int numberOfProjectiles; // Cantidad de balas disparadas
    private Sprite upgradedProjectileSprite; // Nuevo sprite para el proyectil
    private Vector3 normalScale = new Vector3(0.5f, 0.5f, 0.5f); // Escala normal del proyectil
    private Vector3 upgradedScale = new Vector3(1.5f, 1.5f, 1.5f); // Escala mejorada del proyectil
    private int maxShots = 20; // Máximo de disparos antes de volver al estado base
    private int currentShots = 0; // Contador de disparos actuales

    private bool isUpgraded = false; // Indica si el proyectil está mejorado

    public ShootAbility(PlayerController player, GameObject projectilePrefab, Transform firePoint, float projectileSpeed, Color bulletColor, int numberOfProjectiles, Sprite projectileSprite, Sprite upgradedProjectileSprite) : base(player)
    {
        this.projectilePrefab = projectilePrefab;
        this.firePoint = firePoint;
        this.projectileSpeed = projectileSpeed;
        this.bulletColor = bulletColor;
        this.numberOfProjectiles = numberOfProjectiles;
        this.projectileSprite = projectileSprite;
        this.upgradedProjectileSprite = upgradedProjectileSprite;
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (currentShots >= maxShots)
        {
            ResetState(); // Resetear el estado al alcanzar el límite de disparos
        }

        float angleStep = 15f;
        float startAngle = -angleStep * (numberOfProjectiles - 1) / 2;
        Vector2 shootDirection = player.IsFacingRight ? Vector2.right : Vector2.left;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * shootDirection;
            float projectileRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject projectile = GameObject.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
            spriteRenderer.color = bulletColor;

            // Establecer el sprite del proyectil basado en si está mejorado o no
            spriteRenderer.sprite = isUpgraded ? upgradedProjectileSprite : projectileSprite;

            // Ajustar la rotación del proyectil
            projectile.transform.rotation = Quaternion.Euler(0, 0, projectileRotation);

            // Ajustar la escala del proyectil
            projectile.transform.localScale = isUpgraded ? upgradedScale : normalScale;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * projectileSpeed;

            currentShots++;
        }

        player.StartCoroutine(ShootingCooldown());
    }

    private IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public void UpdateBulletColor(Color newColor)
    {
        bulletColor = newColor;
    }

    public void UpdateProjectileCount(int newCount)
    {
        numberOfProjectiles = newCount;
    }

    public void UpdateProjectileSprite(Sprite newSprite)
    {
        projectileSprite = newSprite;
    }

    public void ChangeProjectileAppearance()
    {
        isUpgraded = true; // Activar el estado mejorado
    }

    public void ResetState()
    {
        isUpgraded = false; // Restablecer el estado mejorado
        currentShots = 0; // Reiniciar el contador de disparos
    }
}
