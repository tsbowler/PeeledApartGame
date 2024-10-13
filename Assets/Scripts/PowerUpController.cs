using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public MoveLionWithBFS lion;  
    public MoveMonkey monkey;     

    public SpriteRenderer freezeSprite;
    public SpriteRenderer speedSprite;
    public SpriteRenderer decoySprite;
    public SpriteRenderer portalSprite;
    //public SpriteRenderer wingsSprite;

    private bool isFrozen = false;            
    private bool isSpeedBoostActive = false;  
    private bool isPhaseThruActive = false;   
    private bool isDecoyActive = false;       
    private bool isTeleportSet = false;  
    private Vector3 teleportLocation;    

    public GameObject decoyPrefab;  
    private GameObject currentDecoy; 
    public GameObject portalPrefab;  
    private GameObject currentPortal; 

    private int monkeyLayer;         
    private int obstacleLayer;       

    public enum PowerUpType { Freeze, SpeedBoost, PhaseThru, Decoy, Teleport }
    private List<PowerUpType> storedPowerUps = new List<PowerUpType>();

    void Start()
    {
        monkeyLayer = LayerMask.NameToLayer("Monkey");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && storedPowerUps.Contains(PowerUpType.Freeze))
        {
            ActivateFreezePower();
            freezeSprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.Freeze);  // Remove the power after using
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && storedPowerUps.Contains(PowerUpType.SpeedBoost))
        {
            ActivateSpeedBoostPower();
            speedSprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.SpeedBoost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && storedPowerUps.Contains(PowerUpType.Decoy))
        {
            ActivateDecoyPower();
            decoySprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.Decoy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && storedPowerUps.Contains(PowerUpType.Teleport))
        {
            ActivateTeleportPower();
            if (!isTeleportSet)
            {
                portalSprite.enabled = false;
                storedPowerUps.Remove(PowerUpType.Teleport);
            }
        }

        /*
        // Check for input to activate Phase-thru Power (press '3')
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ActivatePhaseThruPower();
        }
        */
    }

    public void UnlockRandomPower()
    {
        List<PowerUpType> availablePowerUps = new List<PowerUpType> { PowerUpType.Freeze, PowerUpType.SpeedBoost, PowerUpType.Decoy, PowerUpType.Teleport };

        // Remove powers already stored
        availablePowerUps.RemoveAll(p => storedPowerUps.Contains(p));

        if (availablePowerUps.Count > 0)
        {
            PowerUpType randomPower = availablePowerUps[Random.Range(0, availablePowerUps.Count)];
            storedPowerUps.Add(randomPower);
            Debug.Log("Unlocked power: " + randomPower);
            switch(randomPower)
            {
                case PowerUpType.Freeze: 
                    freezeSprite.enabled = true;
                    break;
                case PowerUpType.SpeedBoost:
                    speedSprite.enabled = true;
                    break;
                case PowerUpType.Decoy:
                    decoySprite.enabled = true;
                    break;
                case PowerUpType.Teleport:
                    portalSprite.enabled = true;
                    break;
            }
        }
        else
        {
            Debug.Log("All powers already unlocked.");
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

        float originalSpeed = monkey.moveSpeed;  
        monkey.moveSpeed = boostedSpeed;  

        yield return new WaitForSeconds(boostDuration);  

        monkey.moveSpeed = originalSpeed;  

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

    void ActivateTeleportPower()
    {
        if (!isTeleportSet)
        {
            // Set the teleport location
            teleportLocation = SnapToGrid(monkey.transform.position);
            currentPortal = Instantiate(portalPrefab, teleportLocation, Quaternion.identity);
            isTeleportSet = true;
            Debug.Log("Teleport location set at: " + teleportLocation);
        }
        else
        {
            // Teleport the monkey to the set location
            monkey.transform.position = teleportLocation;
            monkey.UpdateTargetPosition(teleportLocation);
            isTeleportSet = false;
            Destroy(currentPortal);
            Debug.Log("Teleported to: " + teleportLocation);
        }
    }

        Vector3 SnapToGrid(Vector3 originalPosition)
    {
        float snapValue = 1.0f;  // The step between the fixed positions (e.g., 1.0 for 0.5, 1.5, etc.)
        float offset = -0.5f;    // Offset for the fixed values (-1.5, -0.5, 0.5, etc.)

        float snappedX = Mathf.Round((originalPosition.x - offset) / snapValue) * snapValue + offset;
        float snappedY = Mathf.Round((originalPosition.y - offset) / snapValue) * snapValue + offset;

        return new Vector3(snappedX, snappedY, originalPosition.z);
    }
}
