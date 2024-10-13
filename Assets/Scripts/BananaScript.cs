using UnityEngine;

public class BananaScript : MonoBehaviour
{
    private GameObject monkey;  // Reference to the monkey
    private GeneralLogic generalLogic;  // Reference to GeneralLogic

    private float lifespan = 20f;  // Time before the banana self-destructs

    void Start()
    {
        // Start the countdown to destroy the banana after its lifespan
        Invoke("DestroyBanana", lifespan);
    }

    void Update()
    {
        // Check if the monkey has reached the banana's position
        if (monkey != null && Vector3.Distance(transform.position, monkey.transform.position) < 0.5f)
        {
            CollectBanana();
        }
    }

    public void Initialize(GameObject monkeyRef, GeneralLogic generalLogicRef)
    {
        // Assign references dynamically when the banana is spawned
        monkey = monkeyRef;
        generalLogic = generalLogicRef;
    }

    void CollectBanana()
    {
        // Add to score in GeneralLogic
        if (generalLogic != null)
        {
            generalLogic.AddScore(1);
        }

        // Destroy the banana
        Destroy(gameObject);
    }

    void DestroyBanana()
    {
        // If the banana is not collected in time, destroy it
        Destroy(gameObject);
    }
}
