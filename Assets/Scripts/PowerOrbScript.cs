using UnityEngine;

public class PowerOrbScript : MonoBehaviour
{
    private GameObject monkey;  // Reference to the monkey
    private PowerUpController powerUpController;  // Reference to PowerUpController
    private float lifespan = 20f;  // Time before the Power Orb self-destructs

    // Distance threshold for collecting the power orb
    private float collectionRadius = 0.5f;

    void Start()
    {
        // Start the countdown to destroy the Power Orb after its lifespan
        Invoke("DestroyPowerOrb", lifespan);
    }

    void Update()
    {
        // Check if the monkey is within the collection radius of the power orb
        if (monkey != null && Vector3.Distance(transform.position, monkey.transform.position) < collectionRadius)
        {
            CollectPowerOrb();
        }
    }

    public void Initialize(GameObject monkeyRef, PowerUpController powerUpControllerRef)
    {
        // Assign references dynamically when the Power Orb is spawned
        monkey = monkeyRef;
        powerUpController = powerUpControllerRef;
    }

    void CollectPowerOrb()
    {
        // Unlock a random power in PowerUpController
        if (powerUpController != null)
        {
            powerUpController.UnlockRandomPower();
        }

        // Destroy the Power Orb after it is collected
        Destroy(gameObject);
    }

    void DestroyPowerOrb()
    {
        // If the Power Orb is not collected in time, destroy it
        Destroy(gameObject);
    }
}
