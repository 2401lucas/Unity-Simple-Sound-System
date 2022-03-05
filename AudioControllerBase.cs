using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Example of inherited script as well as info is availible at <see href="http://stackoverflow.com">the git repository</see> </para>
/// <para> See <seealso cref="ExampleAudioTrack"></seealso> for an example</para>
/// </summary>
public class AudioControllerBase : MonoBehaviour
{
    List<AudioSource> playingAudioSources = new List<AudioSource>();

    private void FixedUpdate()
    {
        for (int i = 0; i < playingAudioSources.Count; i++)
        {
            if (playingAudioSources[i].isPlaying == false)  {
                AudioTrackPool.Instance.Recycle(playingAudioSources[i]);
                playingAudioSources.RemoveAt(i);
            }
        }
    }

    public void Stop() { 
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].Stop();
    }

    public void Pause() { 
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].Pause();
    }

    public void UnPause() { 
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].UnPause();
    }

    public void Mute() { 
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].mute = true;
    }

    public void UnMute() { 
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].mute = false;
    }

    public void SetVolume(float volume) { 
        float newVolume = Mathf.Clamp01(volume);

        for (int i = 0; i < playingAudioSources.Count; i++)
        {
            playingAudioSources[i].volume = newVolume;
        }
    }

    protected void Play(AudioClip audioClip, int[] worldPos = null)
    {
        if (audioClip == null)
            return;

        if (worldPos != null)
            PlayInWorldSpace(audioClip, worldPos);
        else
            Play(GetTrack(audioClip.length, null), audioClip);
    }
    
    void PlayInWorldSpace(AudioClip audioClip, int[] worldPos)
    {
        var track = GetTrack(audioClip.length, worldPos);

        Play(track, audioClip);
    }

    void Play(AudioSource track, AudioClip clip) { 
        track.clip = clip; 
        track.Play(); 
    }

    AudioSource GetTrack(float length, int[] worldPos)
    {
        var track = AudioTrackPool.Instance.Get();

        track.mute = track.mute;
        track.volume = track.volume;

        if (worldPos!= null)
        {
            track.transform.position = new Vector3(worldPos[0], worldPos[1], worldPos[2]);
            track.spatialBlend = 1.0f;
        }
        else
        {
            track.transform.position = Vector3.zero;
            track.spatialBlend = 0.0f;
        }

        track.gameObject.SetActive(true);

        return track;
    }
}