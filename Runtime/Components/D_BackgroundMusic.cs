using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
namespace Dutil
{
    [RequireComponent(typeof(AudioSource))]

    public class D_BackgroundMusic : MonoBehaviour
    {
        public bool playOnStart = true;
        //float storedVolume;
        public List<D_BackgroundMusicData> music = new List<D_BackgroundMusicData>();
        public static UnityEvent<D_BackgroundMusicData> OnTrackChangedEvent = new UnityEvent<D_BackgroundMusicData>();

        public UnityEvent<D_BackgroundMusicData> OnTrackChangedLocalEvent = new UnityEvent<D_BackgroundMusicData>();

        AudioSource source;
        D_BackgroundMusicData currentlyPlaying;

        float trackTimeRemaining = -1;
        public static D_BackgroundMusic Instance { get; private set; }
        static float masterVolume = .5f;
        public static float MasterVolume
        {
            get { return masterVolume; }
            set
            {
                masterVolume = value;
                if (Instance != null)
                {
                    Instance.source.volume = MasterVolume * (Instance.currentlyPlaying?.volume ?? 1);
                }
            }
        }
        void Reset()
        {
            music.Add(new D_BackgroundMusicData());
        }
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
            source = GetComponent<AudioSource>();

        }
        void OnValidate()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = MasterVolume * currentlyPlaying?.volume ?? 1;
        }
        void Start()
        {
            bool allValid = music.TrueForAll(x => x.clip != null);
            if (!allValid)
            {
                Debug.LogError("All music clips must be set");
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            if (playOnStart)
            {
                PlayTrack();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (trackTimeRemaining > 0 && source.isPlaying)
            {
                trackTimeRemaining -= Time.deltaTime;
                if (trackTimeRemaining <= 0)
                {

                    PlayTrack(CurrentIndex + 1);
                    OnTrackChangedEvent.Invoke(currentlyPlaying);
                }
            }
        }
        int CurrentIndex { get { return music.IndexOf(currentlyPlaying); } }
        public static void PlayTrack(int index = 0)
        {
            int i = index % Instance.music.Count;
            Instance.source.clip = Instance.music[i].clip;
            Instance.source.volume = Instance.music[i].volume;
            Instance.source.Play();
            Instance.currentlyPlaying = Instance.music[i];
            Instance.trackTimeRemaining = Instance.music[i].clip.length;
            Debug.Log("Now playing: " + Instance.source.clip.name);
            Debug.Log("Track will play for " + Instance.trackTimeRemaining + " seconds");
        }
        public static void PauseTrack()
        {
            AutoLerp.Begin(.4f, Instance.source.volume, 0, (t, v) => Instance.source.volume = v).OnComplete((t) => Instance.source.Pause());
        }
        public static void UnPauseTrack()
        {
            Instance.source.UnPause();
            AutoLerp.Begin(.4f, 0, Instance.currentlyPlaying?.volume ?? 1, (t, v) => Instance.source.volume = v);
        }
        public static void StopTrack()
        {
            AutoLerp.Begin(.4f, Instance.source.volume, 0, (t, v) => Instance.source.volume = v).OnComplete((t) => Instance.source.Stop());
        }
        public static float TrackProgress()
        {
            return Instance.source.time / Instance.source.clip.length;
        }

        public static string GetTrackString(int index = -1)
        {
            index = index == -1 ? Instance.CurrentIndex : index;
            D_BackgroundMusicData data = Instance.music[index];
            //Application.OpenURL(data.urlLink);
            return data.ToString();
        }


    }
    [System.Serializable]
    public class D_BackgroundMusicData
    {
        public string songName, artistName, urlLink = "https://";
        public AudioClip clip;
        [Range(0, 1)]
        public float volume = 1;
        public override string ToString()
        {
            return songName + " - " + artistName;
        }
    }
}
