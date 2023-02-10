using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RpgAdventure
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        public class SoundBank
        {
            public string name;
            public AudioClip[] clips;
        }
        public SoundBank soundbank = new SoundBank();
        private AudioSource m_AudioSource;

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomClip()
        {
            var clip = soundbank.clips[Random.Range(0, soundbank.clips.Length)];
            
            if (clip == null)
            {
                return;
            }
            m_AudioSource.clip = clip;
            m_AudioSource.Play();
        }
        

    }

}
