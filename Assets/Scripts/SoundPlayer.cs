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
    public AudioSource impWin;

    private bool isFlapWingsPlaying = false;
    private bool isUseDecoyPlaying = false;
    
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
    public void PlayRunning()
    {
        running.Play();
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
    public void PlayImpWin()
    {
        impWin.Play();
    }
}
