using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [Header("Music Sources")]
    [SerializeField] private AudioSource sfxPrefab;
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private AudioClip mainMusicClip;
    [SerializeField] private AudioSource bossMusicSource;
    [SerializeField] private AudioClip bossMusicClip;
    [SerializeField] private float musicFadeTime = 3f;

    [Header("Player SFX")]
    [SerializeField] private AudioClip playerWalkClip;
    [SerializeField] private AudioClip playerAttackClip;
    [SerializeField] private AudioClip playerDamageClip;

    [SerializeField] private AudioClip NPCTalkClip;

    // [SerializeField] private AudioClip healClip

    [Header("Witch SFX")]
    [SerializeField] private AudioClip witchScreamClip;
    [SerializeField] private AudioClip witchMagicAttackClip;
    [SerializeField] private AudioClip potionAttackClip;
    [SerializeField] private AudioClip spawnAttackClip;

    [Header("Magic attack SFX")]
    [SerializeField] private AudioClip magicAttackClip;

    [Header("Bat")]
    [SerializeField] private AudioClip batFlyingClip;
    [SerializeField] private AudioClip batAttackClip;
    [SerializeField] private AudioClip batDeathClip;

    [Header("Spawn Attack SFX")]
    [SerializeField] private AudioClip spawnClip;


    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 0.25f;
    [Range(0f, 1f)] public float musicVolume = 0.25f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;
    
    [Header("NPC")]
    [SerializeField] private AudioSource npcTalkSource;


    [Header("Internal")]
    [SerializeField] private int sfxPoolSize = 5;
    private List<AudioSource> sfxPool;
    private int nextSfxIndex = 0;
    
    void Awake()
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

        InitializeSFXManager();

        //Initializing SFX pool
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = Instantiate(sfxPrefab, transform);
            src.playOnAwake = false;
            sfxPool.Add(src);
        }

        if(mainMusicSource != null)
        {
            mainMusicSource.loop = true;
            mainMusicSource.playOnAwake = false;
        }
        if(bossMusicSource != null)
        {
            bossMusicSource.loop = true;
            bossMusicSource.playOnAwake = false;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform)
    {
        if (audioClip == null) return;

        AudioSource src = sfxPool[nextSfxIndex];
        nextSfxIndex = (nextSfxIndex + 1) % sfxPool.Count;

        src.transform.position = spawnTransform.position;
        src.volume = sfxVolume * masterVolume;  // <-- global scaling
        src.PlayOneShot(audioClip);
    }

    //PlaySoundFX with Volume Damping | 2 = 50%, 4 = 25% etc.
    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volumeDamping)
    {
        if (audioClip == null) return;

        AudioSource src = sfxPool[nextSfxIndex];
        nextSfxIndex = (nextSfxIndex + 1) % sfxPool.Count;

        src.transform.position = spawnTransform.position;
        src.volume = (sfxVolume * masterVolume) / volumeDamping;  // <-- global scaling
        src.PlayOneShot(audioClip);
    }

    public void PlayNPCTalkSFX()
    {
        if (NPCTalkClip == null) return;

        npcTalkSource.Stop();      // ensures no overlap
        npcTalkSource.clip = NPCTalkClip;
        npcTalkSource.Play();
    }

    public void PlayPlayerAttackSFX()
    {
        PlaySoundFX(playerAttackClip, transform);
    }

    public void PlayPlayerDamageSFX()
    {
        PlaySoundFX(playerDamageClip, transform);
    }

    public void PlayWitchMagicAttackSFX()
    {
        PlaySoundFX(magicAttackClip, transform);
    }
    

    public void PreloadMusic(AudioClip audioClip)
    {
        audioClip.LoadAudioData();
    }

    public void PlayMainMusic()
    {
        
        if (mainMusicSource == null || mainMusicClip == null)
        {
            Debug.LogWarning("[SoundFXManager] Missing music source or clip");
            return;
        }
        mainMusicSource.gameObject.SetActive(true);
        mainMusicSource.clip = mainMusicClip;
        mainMusicSource.volume = musicVolume * masterVolume;
        mainMusicSource.loop = true;
        mainMusicSource.Play();
    }

    public void StartBossBattleMusic()
    {
        StartCoroutine(CrossfadeMusic(mainMusicSource, bossMusicSource, bossMusicClip, musicFadeTime));
    }

    public void StopBossBattleMusic()
    {
        StartCoroutine(CrossfadeMusic(bossMusicSource, mainMusicSource, mainMusicClip, musicFadeTime));
    }

    private IEnumerator CrossfadeMusic(AudioSource fromSource, AudioSource toSource, AudioClip newClip, float fadeDuration)
    {
        if (newClip == null) yield break;

        float timer = 0f;

        toSource.clip = newClip;
        toSource.volume = 0f;
        toSource.loop = true;
        toSource.Play();

        float startFromVolume = fromSource.volume;
        float targetVolume = masterVolume * musicVolume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            //Fade out
            fromSource.volume = Mathf.Lerp(startFromVolume, 0f, t);

            //Fade in
            toSource.volume = Mathf.Lerp(0f, targetVolume, t);

            yield return null;
        }

        //Ensure the volume is correct
        fromSource.volume = 0f;
        fromSource.Stop();
        toSource.volume = targetVolume;
    }

    // VOLUME SETTINGS
        public void SetMasterVolume(float value)
    {
        masterVolume = value;
        ApplyMusicVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyMusicVolumes();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
    }

    private void ApplyMusicVolumes()
    {
        float v = masterVolume * musicVolume;
        if (mainMusicSource != null) mainMusicSource.volume = v;
        if (bossMusicSource != null) bossMusicSource.volume = v;
    }

    private void InitializeSFXManager()
    {
        //Audio Sources
        sfxPrefab = Resources.Load<AudioSource>("SoundFX/SFXPrefab");
        mainMusicSource = transform.Find("MainMusicSource").GetComponent<AudioSource>();
        bossMusicSource = transform.Find("BossMusicSource").GetComponent<AudioSource>();

        //Music
        mainMusicClip = Resources.Load<AudioClip>("SoundFX/MainMusic");
        bossMusicClip = Resources.Load<AudioClip>("SoundFX/WitchBattleMusic");

        //Player
        playerAttackClip = Resources.Load<AudioClip>("SoundFX/PlayerAttackSFX");
        playerDamageClip = Resources.Load<AudioClip>("SoundFX/PlayerDamageSFX");
        NPCTalkClip = Resources.Load<AudioClip>("SoundFX/NPCTalkSFX");
        GameObject NPCSource = GameObject.Find("VillagerAudioSource");
        npcTalkSource = NPCSource.GetComponent<AudioSource>();
    }
}