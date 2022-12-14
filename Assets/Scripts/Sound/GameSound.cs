using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class GameSound 
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [Range(0f, 255f)]
    public int priority;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
