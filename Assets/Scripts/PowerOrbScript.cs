using System.Collections;
using UnityEngine;

public class PowerOrbScript : MonoBehaviour
{
    private GameObject monkey;  // Reference to the monkey
    private PowerUpController powerUpController;  // Reference to PowerUpController
    private SoundPlayer soundPlayer;  // Reference to the SoundPlayer (shared across scene)

    private float lifespan = 30f;  // Time before the Power Orb self-destructs
    private SpriteRenderer orbRenderer;  // Reference to the SpriteRenderer for the Power Orb
    private bool isFading = false;  // To prevent multiple fade coroutines from starting

    // Distance threshold for collecting the power orb
    private float collectionRadius = 0.5f;

    void Start()
    {
        // Get the SpriteRenderer component
        orbRenderer = GetComponent<SpriteRenderer>();

        // Start the countdown to destroy the Power Orb after its lifespan
        Invoke("DestroyPowerOrb", lifespan);

        soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.PlayOrb();

        // Start fading the orb when there's 6 seconds left
        Invoke("StartFading", lifespan - 5.5f);
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

    // Start the fading coroutine
    void StartFading()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutAndIn());
        }
    }

    // Coroutine to handle the fading effect
    IEnumerator FadeOutAndIn()
    {
        float fadeDuration = 0.5f;
        while (true) // Continue fading in and out until the orb is destroyed
        {
            yield return StartCoroutine(FadeTo(0f, fadeDuration)); // Fade out
            yield return StartCoroutine(FadeTo(1f, fadeDuration)); // Fade in
        }
    }

    // Coroutine to smoothly change the alpha value of the SpriteRenderer's color
    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color color = orbRenderer.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            orbRenderer.color = color; // Apply the color change to the SpriteRenderer
            yield return null;
        }

        // Ensure the final alpha is set precisely
        color.a = targetAlpha;
        orbRenderer.color = color;
    }
}
