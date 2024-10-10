using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public MoveLionWithBFS lion;
    public MoveMonkey monkey;  // Assuming you have a script that controls the monkey's movement

    private bool isFrozen = false;
    private bool isSpeedBoostActive = false;
    private bool isPhaseThruActive = false;

    // Define layers for Monkey and Obstacles (ensure these layers exist in Unity)
    private int monkeyLayer;
    private int obstacleLayer;

    void Start()
    {
        // Assign the layers by name
        monkeyLayer = LayerMask.NameToLayer("Monkey");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }

    void Update()
    {
        // Check for input to activate Freeze Power (press '1')
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateFreezePower();
        }

        // Check for input to activate Speed Boost Power (press '2')
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSpeedBoostPower();
        }

        /* Check for input to activate Phase-thru Power (press '3')
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivatePhaseThruPower();
        }
        */
    }

    void ActivateFreezePower()
    {
        if (!isFrozen)
        {
            StartCoroutine(FreezeLion(3f));  // Freeze lion for 3 seconds
        }
    }

    IEnumerator FreezeLion(float freezeDuration)
    {
        isFrozen = true;

        lion.moveDistance = 0f;  // Freeze lion movement
        lion.animator.SetBool("isWalking", false);  // Stop walking animation

        yield return new WaitForSeconds(freezeDuration);  // Wait for freeze duration

        lion.moveDistance = 1f;  // Restore lion's normal movement speed
        lion.animator.SetBool("isWalking", true);

        isFrozen = false;
    }

    void ActivateSpeedBoostPower()
    {
        if (!isSpeedBoostActive)
        {
            StartCoroutine(SpeedBoost(3f, 4f));  // Boost monkey's speed to 4 for 3 seconds
        }
    }

    IEnumerator SpeedBoost(float boostDuration, float boostedSpeed)
    {
        isSpeedBoostActive = true;

        float originalSpeed = monkey.moveSpeed;  // Save the original speed of the monkey
        monkey.moveSpeed = boostedSpeed;  // Set the monkey's speed to the boosted speed

        yield return new WaitForSeconds(boostDuration);  // Wait for the boost duration

        monkey.moveSpeed = originalSpeed;  // Restore the original speed of the monkey

        isSpeedBoostActive = false;
    }

    void ActivatePhaseThruPower()
    {
        if (!isPhaseThruActive)
        {
            StartCoroutine(PhaseThru(5f));  // Allow monkey to phase through obstacles for 5 seconds
        }
    }

    IEnumerator PhaseThru(float phaseDuration)
    {
        isPhaseThruActive = true;

        // Ignore collisions between Monkey and Obstacles
        Physics2D.IgnoreLayerCollision(monkeyLayer, obstacleLayer, true);

        yield return new WaitForSeconds(phaseDuration);  // Wait for the phase-thru duration

        // Restore normal collisions between Monkey and Obstacles
        Physics2D.IgnoreLayerCollision(monkeyLayer, obstacleLayer, false);

        isPhaseThruActive = false;
    }
}
