using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpController : MonoBehaviour
{
    public MoveLionWithBFS lion;  
    public MoveMonkey monkey;     
    public SoundPlayer soundPlayer;
    public GeneralLogic generalLogic;
    public Tilemap obstaclesTilemap;

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

    void Update() // detect user input to use available powerups when valid
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && storedPowerUps.Contains(PowerUpType.Freeze))
        {
            ActivateFreezePower();
            freezeSprite.enabled = false;
            storedPowerUps.Remove(PowerUpType.Freeze);
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

    // choose random power to store and display
    public void UnlockRandomPower()
    {
        soundPlayer.PlayPowerup();

        List<PowerUpType> availablePowerUps = new List<PowerUpType> { PowerUpType.Freeze, PowerUpType.SpeedBoost, PowerUpType.Decoy, PowerUpType.Teleport, PowerUpType.PhaseThru };

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
            StartCoroutine(FreezeLion(3f));  // stop lion movement for x seconds
        }
    }

    IEnumerator FreezeLion(float freezeDuration)
    {
        isFrozen = true;
        soundPlayer.PlayFreeze();
        lion.moveDistance = 0f;  
        lion.animator.SetBool("isWalking", false);  
        generalLogic.canMonkeyDie = false;

        yield return new WaitForSeconds(freezeDuration);  

        generalLogic.canMonkeyDie = true;
        lion.moveDistance = 1f;
        lion.animator.SetBool("isWalking", true);

        isFrozen = false;
    }

    void ActivateSpeedBoostPower()
    {
        if (!isSpeedBoostActive)
        {
            StartCoroutine(SpeedBoost(3f, 4f));  // for 3 seconds, boost monkey speed to 4
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
        soundPlayer.PlayRunning();
        isSpeedBoostActive = false;
    }

    void ActivatePhaseThruPower()
    {
        if (!isPhaseThruActive)
        {
            StartCoroutine(PhaseThru(7f));  // allow monkey to phase through obstacles and lion for x seconds
        }
    }

    IEnumerator PhaseThru(float phaseDuration)
    {
        isPhaseThruActive = true;
        soundPlayer.PlayFlapWings();
        monkey.isFlying = true;
        generalLogic.canMonkeyDie = false;

        yield return new WaitForSeconds(phaseDuration); 

        monkey.isFlying = false;
        generalLogic.canMonkeyDie = true;
        soundPlayer.PlayFlapWings();
        isPhaseThruActive = false;
    }

    void ActivateDecoyPower()
    {
        StartCoroutine(ActivateDecoy()); // lion moves to decoy until it makes contact with decoy
    }

   IEnumerator ActivateDecoy()
    {
        if (!isDecoyActive) // place marker on first use
        {
            decoyPosition = SnapToGrid(monkey.transform.position);
            Vector2 decoyPosition2D = new Vector2(decoyPosition.x, decoyPosition.y);
            if (IsObstacleAtPosition(decoyPosition2D))
            {
                yield break;
            }
            soundPlayer.PlaySpawnMagic();
            isDecoyActive = true;
            decoyMarker = Instantiate(decoyMarkerPrefab, decoyPosition, Quaternion.identity);
            decoyPosition = monkey.transform.position;
        }
        else
        {   
            if (decoyMarker != null) // on second use, lion is distracted toward placed marker
            {
                Destroy(decoyMarker);
            }
            GameObject decoy = Instantiate(decoyPrefab, decoyPosition, Quaternion.identity);

            soundPlayer.PlayUseDecoy();

            if (lion.gameObject.activeInHierarchy)
                lion.SetTarget(decoy.transform.position);

            while (Vector3.Distance(lion.transform.position, decoy.transform.position) > 0.5f)
            {
                yield return null;
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
        if (!isTeleportSet) // place portal on first use
        {
            soundPlayer.PlaySpawnMagic();
            teleportLocation = SnapToGrid(monkey.transform.position);
            currentPortal = Instantiate(portalPrefab, teleportLocation, Quaternion.identity);
            isTeleportSet = true;
        }
        else // teleport to placed portal on second us
        {
            soundPlayer.PlayUsePortal();
            monkey.transform.position = teleportLocation;
            monkey.UpdateTargetPosition(teleportLocation);
            isTeleportSet = false;
            Destroy(currentPortal);
        }
    }

    Vector3 SnapToGrid(Vector3 originalPosition) // ensures placements are centralized on tiles
    {
        float snapValue = 1.0f;  
        float offset = -0.5f;    

        float snappedX = Mathf.Round((originalPosition.x - offset) / snapValue) * snapValue + offset;
        float snappedY = Mathf.Round((originalPosition.y - offset) / snapValue) * snapValue + offset;

        return new Vector3(snappedX, snappedY, originalPosition.z);
    }

    bool IsObstacleAtPosition(Vector2 position)
    {
        Vector3Int cellPosition = obstaclesTilemap.WorldToCell(position);
        TileBase tile = obstaclesTilemap.GetTile(cellPosition);
        return tile != null;
    }
}
