using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public MoveLionWithBFS lion;  // Reference to the Lion controller (MoveLionWithBFS script)
    public MoveMonkey monkey;     // Reference to the Monkey controller (MoveMonkey script)

    private bool isFrozen = false;            // Indicates if the Freeze power is active
    private bool isSpeedBoostActive = false;  // Indicates if the Speed Boost power is active
    private bool isPhaseThruActive = false;   // Indicates if the Phase-Thru power is active
    private bool isDecoyActive = false;       // Indicates if the Decoy power is active

    public GameObject decoyPrefab;  // Prefab for the Decoy object
    private GameObject currentDecoy; // Reference to the currently active Decoy

    // Define layers for Monkey and Obstacles (ensure these layers exist in Unity)
    private int monkeyLayer;         // The layer for the Monkey
    private int obstacleLayer;       // The layer for Obstacles

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
/*
        // Check for input to activate Phase-thru Power (press '3')
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivatePhaseThruPower();
        }
*/
        // Check for input to activate Decoy Power (press '4')
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateDecoyPower();
        }
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

    void ActivateDecoyPower()
    {
        // Check if the decoy power is already active
        if (!isDecoyActive)
        {
            // Activate the decoy power
            StartCoroutine(ActivateDecoy());
        }
    }

   IEnumerator ActivateDecoy()
    {
        isDecoyActive = true;

        // Spawn a decoy at the monkey's current position
        Vector3 decoyPosition = monkey.transform.position;
        GameObject decoy = Instantiate(decoyPrefab, decoyPosition, Quaternion.identity);

        // Set the lion's target to the decoy's position
        lion.SetTarget(decoy.transform.position);

        // Wait until the lion reaches the decoy's position (or close enough)
        while (Vector3.Distance(lion.transform.position, decoy.transform.position) > 0.5f)
        {
            yield return null;  // Wait until the lion reaches the decoy
        }

        // Destroy the decoy once the lion reaches it
        Destroy(decoy);

        // Reset the lion's target back to the monkey
        lion.ResetTargetToMonkey();

        isDecoyActive = false;
    }

}
