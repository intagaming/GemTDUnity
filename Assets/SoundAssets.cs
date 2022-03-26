using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SoundAssets
{
    public string name;
    public AudioClip clip;
    [Range(.1f, 3f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
   
}
