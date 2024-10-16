using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // AudioSource components for different music
    public AudioSource menuMusicSource;
    public AudioSource gameplayMusicSource;

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
        else if (scene.name == "Gameplay")
        {
            PlayGameplayMusic();
        }
    }

    // Play the menu music and stop other music
    private void PlayMenuMusic()
    {
        if (!menuMusicSource.isPlaying)
        {
            menuMusicSource.Play();
        }

        // Stop gameplay music if it was playing
        if (gameplayMusicSource.isPlaying)
        {
            gameplayMusicSource.Stop();
        }
    }

    // Play the gameplay music and stop other music
    private void PlayGameplayMusic()
    {
        if (!gameplayMusicSource.isPlaying)
        {
            gameplayMusicSource.Play();
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
}
