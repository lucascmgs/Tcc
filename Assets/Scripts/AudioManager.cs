using System;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] _sounds;

        private Dictionary<string, Sound> soundBase;

        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            //DontDestroyOnLoad(gameObject);
            soundBase = new Dictionary<string, Sound>();
            foreach (Sound s in _sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.pitch = s.pitch;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
                soundBase.Add(s.name, s);
            }
        }

        private void Start()
        {
            if (GameOptions.playMusic)
            {
                Play("Theme");
            }
        }


        public void Play(string soundName)
        {
            if (GameOptions.playSounds)
            {
                var sound = soundBase[soundName];
                if (sound == null)
                {
                    Debug.Log("There is no sound with the name " + soundName);
                    return;
                }

                sound.source.Play();
            }
        }
    }
}