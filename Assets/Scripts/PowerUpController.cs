using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public MoveLionWithBFS lion;  
    public MoveMonkey monkey;     
    public SoundPlayer soundPlayer;

    public SpriteRenderer freezeSprite;
    public SpriteRenderer speedSprite;
    public SpriteRenderer decoySprite;
    public SpriteRenderer portalSprite;
    public SpriteRenderer wingsSprite;

    private bool isFrozen = false;            
    private bool isSpeedBoostActive = false;  
    private bool isPhaseThruActive = false;   
    private bool isDecoyActive = false;       
    private bool isTeleportSet = false;  
    private Vector3 decoyPosition;
    private Vector3 teleportLocation;    

    public GameObject decoyMarkerPrefab;
    private GameObject decoyMarker;
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

        if (Input.GetKeyDown(KeyCode.Alpha2) && storedPowerUps.Contains(PowerUpType.SpeedBoost) && !isPhaseThruActive && !isSpeedBoostActive)
        {
            ActivateSpeedBoostPower();
            speedSprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.SpeedBoost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && storedPowerUps.Contains(PowerUpType.Decoy))
        {
            ActivateDecoyPower();
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

        if (Input.GetKeyDown(KeyCode.Alpha5) && storedPowerUps.Contains(PowerUpType.PhaseThru) && !isSpeedBoostActive && !isPhaseThruActive)
        {
            ActivatePhaseThruPower();
            wingsSprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.PhaseThru);
        }
    
    }

    public void UnlockRandomPower()
    {
        soundPlayer.PlayPowerup();

        List<PowerUpType> availablePowerUps = new List<PowerUpType> { PowerUpType.Freeze, PowerUpType.SpeedBoost, PowerUpType.Decoy, PowerUpType.Teleport, PowerUpType.PhaseThru };

        // Remove powers already stored
        availablePowerUps.RemoveAll(p => storedPowerUps.Contains(p));

        if (availablePowerUps.Count > 0)
        {
            PowerUpType randomPower = availablePowerUps[Random.Range(0, availablePowerUps.Count)];
            storedPowerUps.Add(randomPower);
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
                case PowerUpType.PhaseThru:
                    wingsSprite.enabled = true;
                    break;
            }
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
        soundPlayer.PlayFreeze();
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
        soundPlayer.PlayRunning();
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
            StartCoroutine(PhaseThru(7f));  // Allow monkey to phase through obstacles for x seconds
        }
    }

    IEnumerator PhaseThru(float phaseDuration)
    {
        isPhaseThruActive = true;
        soundPlayer.PlayFlapWings();
        // Ignore collisions between Monkey and Obstacles
        monkey.isFlying = true;

        yield return new WaitForSeconds(phaseDuration);  // Wait for the phase-thru duration

        // Restore normal collisions between Monkey and Obstacles
        monkey.isFlying = false;

        soundPlayer.PlayFlapWings();
        isPhaseThruActive = false;
    }

    void ActivateDecoyPower()
    {
        StartCoroutine(ActivateDecoy());
    }

   IEnumerator ActivateDecoy()
    {
        if (!isDecoyActive)
        {
            soundPlayer.PlaySpawnMagic();
            isDecoyActive = true;
            decoyPosition = monkey.transform.position;
            decoyMarker = Instantiate(decoyMarkerPrefab, decoyPosition, Quaternion.identity);
        }
        else
        {   
            if (decoyMarker != null)
            {
                Destroy(decoyMarker);
            }
            GameObject decoy = Instantiate(decoyPrefab, decoyPosition, Quaternion.identity);

            soundPlayer.PlayUseDecoy();

            if (lion.gameObject.activeInHierarchy)
                lion.SetTarget(decoy.transform.position);

            while (Vector3.Distance(lion.transform.position, decoy.transform.position) > 0.5f)
            {
                yield return null;  // Wait until the lion reaches the decoy
            }

            soundPlayer.PlayUseDecoy();
            isDecoyActive = false;
            Destroy(decoy);
            lion.ResetTargetToMonkey();
            decoySprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.Decoy);
        }
        
    }

    void ActivateTeleportPower()
    {
        if (!isTeleportSet)
        {
            soundPlayer.PlaySpawnMagic();
            // Set the teleport location
            teleportLocation = SnapToGrid(monkey.transform.position);
            currentPortal = Instantiate(portalPrefab, teleportLocation, Quaternion.identity);
            isTeleportSet = true;
        }
        else
        {
            // Teleport the monkey to the set location
            soundPlayer.PlayUsePortal();
            monkey.transform.position = teleportLocation;
            monkey.UpdateTargetPosition(teleportLocation);
            isTeleportSet = false;
            Destroy(currentPortal);
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
