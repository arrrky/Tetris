using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; set; }

    private AudioSource song; 
    private Object[] allSongs;

    [SerializeField] private AudioClip clickSound;

    private readonly float maxVolume = 0.15f;
    private bool userInput = false;
    private bool mute = false;

    void Start()
    {
        song = GetComponent<AudioSource>();
        allSongs = Resources.LoadAll<AudioClip>("Music");
        song.clip = allSongs[Random.Range(0, allSongs.Length)] as AudioClip;
        //song.Play();

        GetComponent<AudioSource>().volume = 0;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //if (!song.isPlaying)
        //    PlayRandomSong();
        //// Если игрок не увеличивает громкость сам - она вырастет до максимального значения (maxVolume)
        //if (song.volume < maxVolume && usersInput == false)
        //    SlowlyIncreaseVolume(song.volume);
    }

    public void IncreaseVolumeLevel()
    {
        song.volume += 0.0005f;
        userInput = true;
    }

    public void DecreaseVolumeLevel()
    {
        song.volume -= 0.0005f;
        userInput = true;
    }

    private void SlowlyIncreaseVolume(float volume)
    {
        song.volume = Mathf.Lerp(song.volume, maxVolume, 0.01f);
    }

    public void PlayRandomSong()
    {
        song.clip = allSongs[Random.Range(0, allSongs.Length)] as AudioClip;
        song.Play();
    }

    public void MuteUnmute()
    {
        userInput = true;
        mute = !mute;
        if (mute)
            song.volume = 0;
        if (!mute)
            song.volume = 0.15f;
    }   
}

