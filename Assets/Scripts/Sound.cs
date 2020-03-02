using System;
using UnityEngine.Audio;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class Sound
    {
        public string name;
        
        public AudioClip clip;

        [HideInInspector]
        public AudioSource source;

        [Range(0f,3f)]
        public float pitch = 1f;

        [Range(0f, 1f)] 
        public float volume = 1f;

        public bool loop = false;
    }
}