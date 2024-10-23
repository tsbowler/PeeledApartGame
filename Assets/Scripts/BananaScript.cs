using System.Collections;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    private GameObject monkey;
    private GeneralLogic generalLogic;
    private SoundPlayer soundPlayer;

    private float lifespan = 30f; // Time before the banana self-destructs
    private SpriteRenderer bananaRenderer; // Reference to the SpriteRenderer
    private bool isFading = false; // To prevent multiple fade coroutines from starting

    void Start()
    {
        // Get the SpriteRenderer component
        bananaRenderer = GetComponent<SpriteRenderer>();

        // Destroy the banana after its lifespan
        Invoke("DestroyBanana", lifespan);

        // Find the SoundPlayer in the scene
        soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.PlayBanana();

        // Start fading the banana when there's 6 seconds left
        Invoke("StartFading", lifespan - 5.5f);
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
        Destroy(gameObject);
    }

    void PlayRandomSound()
    {
        if (soundPlayer != null)
        {
            int randomSound = Random.Range(0, 3);
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
        while (true) // Continue fading in and out until the banana is destroyed
        {
            yield return StartCoroutine(FadeTo(0f, fadeDuration)); // Fade out
            yield return StartCoroutine(FadeTo(1f, fadeDuration)); // Fade in
        }
    }

    // Coroutine to smoothly change the alpha value of the SpriteRenderer's color
    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color color = bananaRenderer.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            bananaRenderer.color = color; // Apply the color change to the SpriteRenderer
            yield return null;
        }

        // Ensure the final alpha is set precisely
        color.a = targetAlpha;
        bananaRenderer.color = color;
    }
}
