using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    private GameObject monkey;  // Reference to the monkey
    private GeneralLogic generalLogic;  // Reference to GeneralLogic
    private SoundPlayer soundPlayer;  // Reference to SoundPlayer (shared across scene)

    private float lifespan = 30f;  // Time before the banana self-destructs
    private Material bananaMaterial;  // Reference to the banana's material
    private bool isFading = false;  // To prevent multiple fade coroutines from starting

    void Start()
    {
        // Get the material from the SpriteRenderer
        bananaMaterial = GetComponent<SpriteRenderer>().material;

        Invoke("DestroyBanana", lifespan);

        // Find the SoundPlayer in the scene
        soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.PlayBanana();

        // Start the fading coroutine when there's 6 seconds left
        Invoke("StartFading", lifespan - 6f);
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
        while (lifespan > 0 && lifespan <= 6f)
        {
            // Fade out
            yield return StartCoroutine(FadeTo(0f, 0.5f));  // Fade to 0 alpha over 0.5 seconds
            // Fade in
            yield return StartCoroutine(FadeTo(1f, 0.5f));  // Fade to full alpha over 0.5 seconds
        }
    }

    // Coroutine to smoothly change the alpha value of the material
    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color color = bananaMaterial.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            bananaMaterial.color = color;
            yield return null;
        }

        // Ensure the final alpha is set precisely
        color.a = targetAlpha;
        bananaMaterial.color = color;
    }
}
