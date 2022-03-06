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

    float volume = 1f;
    bool mute = false;

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

    #region Public Audio Control

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
        mute = true;
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].mute = true;
    }

    public void UnMute() {
        mute = false;
        for (int i = 0; i < playingAudioSources.Count; i++)
            playingAudioSources[i].mute = false;
    }

    public void SetVolume(float volume) { 
        float newVolume = Mathf.Clamp01(volume);
        volume = newVolume;
        for (int i = 0; i < playingAudioSources.Count; i++)
        {
            playingAudioSources[i].volume = newVolume;
        }
    }
    #endregion

    protected void FadeInPlay(AudioSource audioTrack, float targetVolume, float fadeSpeed) => StartCoroutine(BeginFadeIn(audioTrack, targetVolume, fadeSpeed));

    protected void FadeOutStop(AudioSource audioTrack, float targetVolume, float fadeSpeed) => StartCoroutine(BeginFadeOut(audioTrack, targetVolume, fadeSpeed));

    protected void FadeInOut(AudioSource playingAudioTrack, AudioSource newAudioTrack, float targetVolume, float fadeSpeed) => StartCoroutine(BeginFadeInAndOut(playingAudioTrack, newAudioTrack, targetVolume, fadeSpeed));

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

    void Play(AudioSource track, AudioClip clip)
    {
        track.clip = clip;
        track.Play();
    }

    AudioSource GetTrack(float length, int[] worldPos)
    {
        var track = AudioTrackPool.Instance.Get();

        track.mute = mute;
        track.volume = volume;

        if (worldPos!= null)
        {
            track.transform.position = new Vector3(worldPos[0], worldPos[1], worldPos[2]);
            track.spatialBlend = 1.0f; //Enables 3D sound
        }
        else
        {
            track.transform.position = Vector3.zero;
            track.spatialBlend = 0.0f;  //Enables 2D sound
        }

        track.gameObject.SetActive(true);

        return track;
    }

    IEnumerator BeginFadeIn(AudioSource audioTrack, float targetVolume, float fadeSpeed)
    {
        while (audioTrack.volume >= targetVolume)
        {
            audioTrack.volume -= fadeSpeed * Time.time;

            yield return 0;
        }
    }
    
    IEnumerator BeginFadeOut(AudioSource audioTrack, float targetVolume, float fadeSpeed)
    {
        while (audioTrack.volume <= targetVolume)
        {
            audioTrack.volume -= fadeSpeed * Time.time;

            yield return 0;
        }

        audioTrack.Stop();
    }

    /// <summary>
    /// Used for transitioning between sounds
    /// </summary>
    /// <returns></returns>
    IEnumerator BeginFadeInAndOut(AudioSource oldAudioTrack, AudioSource newAudioTrack, float targetVolume, float fadeSpeed)
    {
        newAudioTrack.volume = 0f;
        newAudioTrack.Play();

        while (newAudioTrack.volume >= targetVolume && oldAudioTrack.volume == 0)
        {
            if (oldAudioTrack.volume > 0)
                oldAudioTrack.volume -= fadeSpeed * Time.time;

            if (newAudioTrack.volume < volume)
                newAudioTrack.volume += fadeSpeed * Time.time;

            yield return 0;
        }

        oldAudioTrack.Stop();
    }
}