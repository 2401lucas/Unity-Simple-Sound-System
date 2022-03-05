# Unity Simple Sound System
A simple Sound System design, made to be easy and controllable with both 2D and 3D sounds.

## How Unity plays Sounds
To play a sound in Unity, you need 2 things, an [Audio Clip](https://docs.unity3d.com/ScriptReference/AudioClip.html) and an [Audio Source](https://docs.unity3d.com/ScriptReference/AudioSource.html). The Audio Source has 2 main ways of playing audio, Play, and PlayOneShot

### [Play](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html)
Play which requires you to first give reference to Audio Source of the clip you want, then to call Play.  This gives you full control to play, pause, stop, mute and change the volume, even while the sound is playing. You can only have one sound playing at a time per Audio Source.    

### [PlayOneShot](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html)
PlayOneShot takes one parameter, an Audio clip. Once PlayOneShot is called, that audio plays until it is done, and allows you to play up to 10-12 sounds at once. You cannot inturrupt or stop a sound once PlayOnShot is run.

### The Issue
While PlayOneShot sounds useful for small sound effects such as gunshots, I encountered that if we pause the game (Not using Time.TimeScale) that the sounds keep playing. I also want to be able keep this to be as simple and clean as possible. How would I be able to play multiple sounds without losing control over the Sound Source. 

### Resolution
My resolution to this issue is to have an object pool of audio sources, this allows me to not need to worry about conflicting sounds per Audio Source while still maintaining full control of Audio Sources, also allowing me to change the position of Audio Sources around the map to allow for 3D audio. 

## Must use an Object Pooler
In my example, [I use my simple Object pool which can be found here.](https://github.com/2401lucas/Unity-Object-Pooling-System) However, any object pool should work.

## The Setup
To Setup an AudioController, simple inherit from the AudioControllerBase.cs script. You are required to add these componets

#### A Singleton
A singleton is require to keep the AudioSource persitant and accessable throughout the scene

#### A public Enum of Sound names

#### A struct
A struct is required to create the refrence of the Sound name and the Audio clips, and a private SerializedField of the struct to manually create the AudioClip - name refrences
```
[SerializeField] AudioClips[] audioClips;
```
```
[System.Serializable]
struct AudioClips
{
    public ExampleSounds audioClipName;
    public AudioClip audioClip;
}
```
#### A PlaySound method
```
public void PlaySound(ExampleSounds audioClipName, int[] worldPos = null)
```

#### A GetAudioClip method
This is used to get the AudioClip based on the SoundName
```
AudioClip GetAudioClip(ExampleSounds soundName)
{
    for (int i = 0; i < audioClips.Length; i++)
        if (audioClips[i].audioClipName == soundName)
            return audioClips[i].audioClip;

    Debug.LogError($"Audio Clip Reference not found for sound: {soundName}");
    return null;
}
```
