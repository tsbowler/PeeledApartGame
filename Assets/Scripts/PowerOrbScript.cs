using System.Collections;
using UnityEngine;

public class PowerOrbScript : MonoBehaviour
{
    private GameObject monkey;
    private PowerUpController powerUpController;
    private SoundPlayer soundPlayer; 

    private float lifespan = 30f;
    private SpriteRenderer orbRenderer; 
    private float collectionRadius = 0.5f;
    private bool isFading = false;

    void Start() // invoke fading and destruction at specified times
    {
        orbRenderer = GetComponent<SpriteRenderer>();

        Invoke("DestroyPowerOrb", lifespan);

        soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.PlayOrb();

        Invoke("StartFading", lifespan - 5.5f);
    }

    void Update()
    {
        // monkey collects when touching
        if (monkey != null && Vector3.Distance(transform.position, monkey.transform.position) < collectionRadius)
        {
            CollectPowerOrb();
        }
    }

    public void Initialize(GameObject monkeyRef, PowerUpController powerUpControllerRef)
    {
        // assign references dynamically when the Power Orb is spawned
        monkey = monkeyRef;
        powerUpController = powerUpControllerRef;
    }

    void CollectPowerOrb()
    {
        if (powerUpController != null)
        {
            powerUpController.UnlockRandomPower();
        }
        DestroyPowerOrb();
    }

    void DestroyPowerOrb()
    {
        Destroy(gameObject);
    }

    void StartFading()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutAndIn());
        }
    }

    IEnumerator FadeOutAndIn() // opacity drops to 0 and rises to 100 per duration
    {
        float fadeDuration = 0.5f;
        while (true) 
        {
            yield return StartCoroutine(FadeTo(0f, fadeDuration)); // Fade out
            yield return StartCoroutine(FadeTo(1f, fadeDuration)); // Fade in
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration) // steadily controls opacity over time
    {
        Color color = orbRenderer.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            orbRenderer.color = color; 
            yield return null;
        }
        color.a = targetAlpha;
        orbRenderer.color = color;
    }
}
