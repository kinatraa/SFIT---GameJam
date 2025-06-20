using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : Singleton<AudioManager>, IMessageHandle
{
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private AudioSource _musicSource;

    [Header("Music Clips")] 
    public AudioClip _musicMainMenu;
    public AudioClip _musicGamePlay;


    [Header("SFX Clips")] 
    public AudioClip _sfxGameOver;
    public AudioClip _sfxButtonClick;
    public AudioClip _sfxCollectCoin;
    public AudioClip _sfxHitEnemy;
    public AudioClip _sfxPopupShow;
    
    
    
    private List<AudioSource> _audioSourcePool;
    private AudioClip _currentMusicClip;

    private float _musicVolume = 1f;
    private float _sfxVolume = 0.5f;

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            _musicSource.volume = value;
        }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            foreach (var source in _audioSourcePool)
            {
                source.volume = value;
            }
        }
    }

    private void OnEnable()
    {
        _musicSource.volume = MusicVolume;
        InitializeAudioSourcePool();
        
        MessageManager.Instance.AddSubcriber(MessageType.OnGameLose, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnGameStart, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnCollectItem, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnHitEnemy, this);
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(MessageType.OnGameLose, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnGameStart, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnCollectItem, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnHitEnemy, this);
    }

    private void InitializeAudioSourcePool()
    {
        _audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < _poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = SFXVolume;
            _audioSourcePool.Add(source);
        }
    }

    public void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = GetAvailableAudioSource();
        source.volume = volume;
        source.pitch = pitch;
        source.PlayOneShot(clip);
    }
   

    public void PlayMusic(AudioClip clip, bool isLoop = true)
    {
        StartCoroutine(FadeOutAndIn(_musicSource, clip, isLoop));
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in _audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        // If no available source, create a new one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = SFXVolume;
        _audioSourcePool.Add(newSource);
        return newSource;
    }

    private IEnumerator FadeOutAndIn(AudioSource audioSource, AudioClip newClip, bool isLoop)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / 1f);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.loop = isLoop;
        audioSource.Play();

        currentTime = 0;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, MusicVolume, currentTime / 1f);
            yield return null;
        }

        _currentMusicClip = newClip;
    }

    public AudioClip GetCurrentMusicClip()
    {
        return _currentMusicClip;
    }

    public void Handle(Message message)
    {
        // Debug.Log($"AudioManager: Handle message {message.type.ToString()}");
        switch (message.type)
        {
            //MUSIC
            case MessageType.OnGameStart:
                PlayMusic(_musicGamePlay, true);
                break;
            
            //SFX
            case MessageType.OnGameLose:
                PlaySfx(_sfxGameOver);
                break;
            case MessageType.OnCollectItem:
                PlaySfx(_sfxCollectCoin);
                break;
            case MessageType.OnHitEnemy:
                PlaySfx(_sfxHitEnemy);
                break;
        }
    }
}