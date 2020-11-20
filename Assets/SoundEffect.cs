using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public AudioSource src;
    public AudioClip clip;

    public SoundEffect(AudioSource _src, AudioClip _clip)
    {
        src = _src;
        clip = _clip;
    }

    public void PlayClip(bool loop)
    {
        src.loop = loop;
        src.clip = clip;
        src.Play();
    }

    public void StopClip()
    {
        if (src.isPlaying)
            src.Stop();
        else Debug.LogWarning("this source isn't playing nothing");
    }
}

[System.Serializable]
public class SoundCollection
{
    public List<SoundEffect> playerSounds = new List<SoundEffect>();

    public void PlayClip(int index, bool shouldLoop)
    {
        playerSounds[index].PlayClip(shouldLoop);
    }

    public void PlayClip(string name, bool shouldLoop)
    {
        foreach(SoundEffect sound in playerSounds)
        {
            if (name != sound.name) continue;
            sound.PlayClip(shouldLoop);
        }
        Debug.LogError(name + " is not a valid sound effect");
    }

    public void StopClip(string name)
    {
        foreach (SoundEffect sound in playerSounds)
        {
            if (name != sound.name) continue;
            sound.StopClip();
            return;
        }
        Debug.LogError(name + " is not a valid sound effect");
    }

    public void StopClip(int index)
    {
        playerSounds[index].StopClip();
    }
}


