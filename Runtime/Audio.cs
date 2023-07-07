using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{
    public class Audio : MonoBehaviour
    {
        public static void PlayAmbisonic(AudioClip clip, float volume)
        {
            GameObject audio = new GameObject("Ambisonic Audio");
            audio.transform.position = Vector3.zero;
            AudioSource source = audio.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.clip = clip;
            source.volume = volume;
            source.spatialBlend = 0;
            source.Play();
            Destroy(audio, clip.length + .1f);
        }
    }
}