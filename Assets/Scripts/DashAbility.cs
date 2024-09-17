using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : PlayerAbility
{
    private float dashSpeed;
    private float dashDuration;
    private bool isDashing;

    public DashAbility(PlayerController player, float dashSpeed, float dashDuration) : base(player)
    {
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.isDashing = false;
    }

    public override void Execute()
    {
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift)) // Al presionar Shift se activa el dash
        {
            player.StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalSpeed = player.moveSpeed;
        player.moveSpeed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        player.moveSpeed = originalSpeed;
        isDashing = false;
    }
}
