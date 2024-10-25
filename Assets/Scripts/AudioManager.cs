using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource menuMusicSource;
    public AudioSource gameplayMusicSource;
    public AudioSource hardImpMusicSource;
    public AudioSource menuHoverSound;
    public AudioSource menuClickSound;

    private SetupScript setupScript;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return; 
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        setupScript = SetupScript.instance;  
    }

    private void Start()
    {
        PlayMenuMusic();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameplayMusicBasedOnDifficulty();
        }
    }

    private void PlayMenuMusic()
    {
        if (!menuMusicSource.isPlaying)
        {
            menuMusicSource.Play();
        }

        gameplayMusicSource.Stop();
        hardImpMusicSource.Stop();
    }

    // play music based on the lion speed (difficulty)
    private void PlayGameplayMusicBasedOnDifficulty()
    {
        float lionSpeed = setupScript.GetLionSpeed();

        if (lionSpeed >= 2.0f)
        {
            if (!hardImpMusicSource.isPlaying)
            {
                hardImpMusicSource.Play();
            }
            gameplayMusicSource.Stop();
        }

        else
        {
            if (!gameplayMusicSource.isPlaying)
            {
                gameplayMusicSource.Play();
            }
            hardImpMusicSource.Stop();
        }

        if (menuMusicSource.isPlaying)
        {
            menuMusicSource.Stop();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void MusicOff()
    {
        menuMusicSource.mute = true;
        gameplayMusicSource.mute = true;
        hardImpMusicSource.mute = true;
    }

    public void MusicOn()
    {
        menuMusicSource.mute = false;
        gameplayMusicSource.mute = false;
        hardImpMusicSource.mute = false;
    }

    public void AssignButtonSounds(Button button) // gives buttons in gameplay scenes menu sound effects
    {
        button.onClick.AddListener(() => menuClick());
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((eventData) => menuHover());
        trigger.triggers.Add(entry);
    }

    public void menuHover()
    {
        menuHoverSound.Play();
    }

    public void menuClick()
    {
        menuClickSound.Play();
    }
}
