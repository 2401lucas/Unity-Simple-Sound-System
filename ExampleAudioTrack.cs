using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAudioTrack : AudioControllerBase
{

    [SerializeField] AudioClips[] audioClips;

    #region Singleton
    public static ExampleAudioTrack Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion
    
    AudioClip GetAudioClip(ExampleSounds soundName)
    {
        for (int i = 0; i < audioClips.Length; i++)
            if (audioClips[i].audioClipName == soundName)
                return audioClips[i].audioClip;

        Debug.LogError($"Audio Clip Reference not found for sound: {soundName}");
        return null;
    }

    public void PlaySound(ExampleSounds audioClipName, int[] worldPos = null)
    {
        Play(GetAudioClip(audioClipName), worldPos);
    }

    public enum ExampleSounds
    {
        sound1,
        sound2
    }

    [System.Serializable]
    struct AudioClips
    {
        public ExampleSounds audioClipName;
        public AudioClip audioClip;
    }
}
