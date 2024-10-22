using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // AudioSource components for different music
    public AudioSource menuMusicSource;
    public AudioSource gameplayMusicSource;
    public AudioSource hardImpMusicSource;
    public AudioSource menuHoverSound;
    public AudioSource menuClickSound;

    private SetupScript setupScript;

    private void Awake()
    {
        // Ensure there's only one AudioManager that persists between scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
            return; // Return to prevent running code in duplicate objects
        }

        // Register to listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Get the SetupScript reference to access lionSpeed
        setupScript = SetupScript.instance;  // Assuming SetupScript is a Singleton
    }

    private void Start()
    {
        // Optionally, play the menu music initially if the game starts at the main menu
        PlayMenuMusic();
    }

    // When the scene changes, check what music to play
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play menu music if it's the MainMenu scene
        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        // Play gameplay music if it's the Gameplay scene
        else
        {
            PlayGameplayMusicBasedOnDifficulty();
        }
    }

    // Play the menu music and stop other music
    private void PlayMenuMusic()
    {
        if (!menuMusicSource.isPlaying)
        {
            menuMusicSource.Play();
        }

        // Stop gameplay and hard/imp music if they were playing
        gameplayMusicSource.Stop();
        hardImpMusicSource.Stop();
    }

    // Play the correct gameplay music based on the lion speed (difficulty)
    private void PlayGameplayMusicBasedOnDifficulty()
    {
        // Get the current lion speed from SetupScript (difficulty)
        float lionSpeed = setupScript.GetLionSpeed();

        // If difficulty is Hard or Impossible (lionSpeed >= 2.0), play hardImpMusicSource
        if (lionSpeed >= 2.0f)
        {
            if (!hardImpMusicSource.isPlaying)
            {
                hardImpMusicSource.Play();
            }

            // Stop other music sources
            gameplayMusicSource.Stop();
        }
        // Otherwise, play the regular gameplay music
        else
        {
            if (!gameplayMusicSource.isPlaying)
            {
                gameplayMusicSource.Play();
            }

            // Stop hard/imp music if it was playing
            hardImpMusicSource.Stop();
        }

        // Stop menu music if it was playing
        if (menuMusicSource.isPlaying)
        {
            menuMusicSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Methods to mute and unmute all music
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

    public void AssignButtonSounds(Button button)
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
