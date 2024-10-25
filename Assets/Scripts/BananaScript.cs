using System.Collections;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    private GameObject monkey;
    private GeneralLogic generalLogic;
    private SoundPlayer soundPlayer;

    private float lifespan = 30f; 
    private SpriteRenderer bananaRenderer; 
    private bool isFading = false;

    void Start() // invoke fading and destruction at specified times
    {
        bananaRenderer = GetComponent<SpriteRenderer>();

        Invoke("DestroyBanana", lifespan);

        soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.PlayBanana();

        Invoke("StartFading", lifespan - 5.5f);
    }

    void Update()
    {
        if (monkey != null && Vector3.Distance(transform.position, monkey.transform.position) < 0.5f)
        {
            CollectBanana(); // monkey collects when touching
        }
    }

    public void Initialize(GameObject monkeyRef, GeneralLogic generalLogicRef)
    {
        // assign references dynamically when the banana is spawned
        monkey = monkeyRef;
        generalLogic = generalLogicRef;
    }

    void CollectBanana()
    {
        PlayRandomSound();

        if (generalLogic != null)
        {
            generalLogic.AddScore(1);
        }

        DestroyBanana();
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
            yield return StartCoroutine(FadeTo(0f, fadeDuration)); 
            yield return StartCoroutine(FadeTo(1f, fadeDuration)); 
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration) // steadily controls opacity over time
    {
        Color color = bananaRenderer.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            bananaRenderer.color = color; 
            yield return null;
        }

        color.a = targetAlpha;
        bananaRenderer.color = color;
    }
}
