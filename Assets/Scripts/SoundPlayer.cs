using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource chomp;
    public AudioSource munch;
    public AudioSource gulp;
    public AudioSource roar;
    public AudioSource powerup;
    public AudioSource freeze;
    public AudioSource running;
    public AudioSource spawnMagic;
    public AudioSource useDecoy;
    public AudioSource usePortal;
    public AudioSource flapWings;
    public AudioSource loser;
    public AudioSource winner;
    public AudioSource bananaSound;
    public AudioSource orbSound;

    private bool isFlapWingsPlaying = false;
    private bool isUseDecoyPlaying = false;
    private bool isRunningPlaying = false;
    
    public void PlayChomp()
    {
        chomp.Play();
    }
    public void PlayMunch()
    {
        munch.Play();
    }
    public void PlayGulp()
    {
        gulp.Play();
    }

    public void PlayRoar()
    {
        roar.Play();
    }

    public void PlayPowerup()
    {
        powerup.Play();
    }
    public void PlayFreeze()
    {
        freeze.Play();
    }
    public void PlayRunning() // sounds that run too long can be called again to stop early
    {
        if(!isRunningPlaying)
        {
            running.Play();
            isRunningPlaying = true;
        } 
        else
        {
            running.Stop();
            isRunningPlaying = false;
        }
    }
    public void PlaySpawnMagic()
    {
        spawnMagic.Play();
    }
    public void PlayUseDecoy()
    {
        if(!isUseDecoyPlaying)
        {
            useDecoy.Play();
            isUseDecoyPlaying = true;
        } 
        else
        {
            useDecoy.Stop();
            isUseDecoyPlaying = false;
        }
    }
    public void PlayUsePortal()
    {
        usePortal.Play();
    }
    public void PlayFlapWings()
    {
        if(!isFlapWingsPlaying)
        {
            flapWings.Play();
            isFlapWingsPlaying = true;
        }
        else
        {
            flapWings.Stop();
            isFlapWingsPlaying = false;
        }
    }

    public void PlayLoser()
    {
        loser.Play();
    }
    public void PlayWinner()
    {
        winner.Play();
    }

    public void PlayBanana()
    {
        bananaSound.Play();
    }
    public void PlayOrb()
    {
        orbSound.Play();
    }
}
