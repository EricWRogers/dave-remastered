using UnityEngine.Audio;
using System;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public GameSound[] sounds;
    public PhysicsButton button;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.priority = s.priority;
            s.source.loop = s.loop;
        }
        
    }

    void Start()
    {
       Play("Wasteland Showdown");
    }

   public void Play(string name)
   {
        GameSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
   }

    public void Stop(string name)
    {
        GameSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

}
