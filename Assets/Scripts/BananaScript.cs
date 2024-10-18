using UnityEngine;

public class BananaScript : MonoBehaviour
{
    private GameObject monkey;  // Reference to the monkey
    private GeneralLogic generalLogic;  // Reference to GeneralLogic
    private SoundPlayer soundPlayer;  // Reference to SoundPlayer (shared across scene)

    private float lifespan = 20f;  // Time before the banana self-destructs

    void Start()
    {
        Invoke("DestroyBanana", lifespan);
        
        // Find the SoundPlayer in the scene
        soundPlayer = FindObjectOfType<SoundPlayer>();
        
        // Check if SoundPlayer was found
        if (soundPlayer == null)
        {
            Debug.LogError("SoundPlayer not found in the scene!");
        }
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
        // Play a random sound effect
        PlayRandomSound();

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

    void PlayRandomSound()
    {
        // Ensure soundPlayer is assigned
        if (soundPlayer != null)
        {
            int randomSound = Random.Range(0, 3);  // Generates a random number between 0 and 2
            switch (randomSound)
            {
                case 0:
                    soundPlayer.PlayChomp();
                    break;
                case 1:
                    soundPlayer.PlayMunch();
                    break;
                case 2:
                    soundPlayer.PlayGulp();
                    break;
            }
        }
    }
}
