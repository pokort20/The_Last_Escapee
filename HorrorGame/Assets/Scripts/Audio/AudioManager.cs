using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public bool MainMenuScene;
    public static AudioManager instance;
    private GameManager gameManager;

    public AudioSource playerMovementAudioSource;
    public AudioSource playerBreathingAudioSource;
    public AudioSource ambientMenuAudioSource;
    public Transform player;
    [Range(0.0f, 1.0f)]
    public float masterVolume;
    [Range(0.0f, 1.0f)]
    public float ambientVolume;
    [Range(0.0f, 1.0f)]
    public float effectsVolume;
    public List<Audio> audios;

    public List<Audio> ambientEffectAudios;
    public List<Audio> thudEffectAudios;

    private List<EnemyAudio> enemies;
    private Audio enemyLoopAudio;
    private Audio menuAudio;
    private List<Tuple<AudioSource, bool>> sceneAudioSources;
    //properties
    public Vector3 movementVec { get; set; }
    public bool isGrounded { get; set; }
    public float generalVolume
    {
        get
        {
            return masterVolume;
        }
        set
        {
            masterVolume = value;
            if(gameManager != null && gameManager.isPaused)
            {
                ambientMenuAudioSource.volume = menuAudio.volume * ambientVolume * masterVolume;
            }
        }
    }
    void Awake()
    {
        Debug.Log("Started audio manager!");
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of AudioManager!");
        }
        instance = this;

        enemies = new List<EnemyAudio>();
        sceneAudioSources = new List<Tuple<AudioSource, bool>>();
    }
    void Start()
    {
        gameManager = GameManager.instance;
        gameManager.onGamePausedCallback += onGamePausedAudio;
        gameManager.onPlayerDeathCallback += onGameOverAudio;

        initMenuAudio();
        if(!MainMenuScene)
        {
            initPlayerMovementAudio();
            initPlayerBreathingAudio();
            initEnemyLoopAudio();
            Invoke("playAmbientEffect", Random.Range(5, 15));
        }
        else
        {
            ambientMenuAudioSource.volume = menuAudio.volume * ambientVolume * masterVolume;
        }
        generalVolume = masterVolume;
    }
    void Update()
    {
        if(!MainMenuScene)
        {
            handlePlayerMovementAudio();
            handlePlayerBreathAudio();
            handleEnemyAudios();
        }
    }
    private void initMenuAudio()
    {
        foreach(Audio audio in audios)
        {
            if(audio.name == "menu_ambient")
            {
                menuAudio = audio;
                ambientMenuAudioSource.clip = audio.audioClip;
                ambientMenuAudioSource.pitch = audio.pitch;
                ambientMenuAudioSource.loop = audio.loop;
                ambientMenuAudioSource.volume = 0.0f;
                ambientMenuAudioSource.Play();
            }
        }
       
    }
    private void initPlayerMovementAudio()
    {
        foreach(Audio audio in audios)
        {
            if(audio.name == "player_walk")
            {
                playerMovementAudioSource.clip = audio.audioClip;
                playerMovementAudioSource.volume = audio.volume;
                playerMovementAudioSource.loop = audio.loop;

                return;
            }
        }
        Debug.LogWarning("Can not find player_walk audio!");
    }
    private void initPlayerBreathingAudio()
    {
        foreach (Audio audio in audios)
        {
            if (audio.name == "player_breathe")
            {
                playerBreathingAudioSource.clip = audio.audioClip;
                playerBreathingAudioSource.volume = audio.volume;
                playerBreathingAudioSource.loop = audio.loop;

                return;
            }
        }
        Debug.LogWarning("Can not find player_breath audio!");
    }
    private void initEnemyLoopAudio()
    {
        foreach(Audio audio in audios)
        {
            if(audio.name == "enemy_walk")
            {
                enemyLoopAudio = audio;

                return;
            }
        }
    }
    private void handlePlayerMovementAudio()
    {
        if(movementVec.magnitude <= 0.0f || !isGrounded)
        {
            playerMovementAudioSource.Pause();
            return;
        }
        playerMovementAudioSource.volume = remap(1.0f, 6.0f, 0.0f, 0.2f, movementVec.magnitude) * effectsVolume * masterVolume;
        playerMovementAudioSource.pitch = remap(0.0f, 6.0f, 0.7f, 2.0f, movementVec.magnitude);
        if(!playerMovementAudioSource.isPlaying && !gameManager.isPaused && !gameManager.isPlayerDead)
        {
            playerMovementAudioSource.Play();
        }
    }
    private void handlePlayerBreathAudio()
    {
        //if(movementVec.magnitude < 3.5f && gameManager.stamina >= 0.9f * gameManager._maxStamina)
        //{
        //    playerBreathingAudioSource.Pause();
        //    return;
        //}
        playerBreathingAudioSource.volume = (remap(0.0f, 6.0f, 0.01f, 0.015f, movementVec.magnitude) + remap(0.0f, gameManager._maxStamina, 0.00f, 0.1f, gameManager._maxStamina - gameManager.stamina)) * effectsVolume * masterVolume;
        playerBreathingAudioSource.pitch = remap(0.0f, gameManager._maxStamina, 0.7f, 1.3f, gameManager._maxStamina - gameManager.stamina);
        if(!playerBreathingAudioSource.isPlaying && !gameManager.isPaused && !gameManager.isPlayerDead)
        {
            playerBreathingAudioSource.Play();
        }
    }
    public void playAudio(string name)
    {
        foreach (Audio audio in audios)
        {
            if (audio.name == name)
            {
                AudioSource.PlayClipAtPoint(audio.audioClip, player.position, audio.volume * effectsVolume * masterVolume);
                return;
            }
        }
        Debug.LogWarning("Audio with given name not found!");
    }
    public void playAudio(string name, AudioSource source)
    {
        if(!source.isActiveAndEnabled)
        {
            Debug.Log("Can not play sound, because this audio source is not enabled/active");
            return;
        }
        foreach (Audio audio in audios)
        {
            if (audio.name == name)
            {
                source.clip = audio.audioClip;
                source.loop = audio.loop;
                source.volume = audio.volume * effectsVolume * masterVolume;
                source.pitch = audio.pitch;
                source.Play();

                return;
            }
        }
        Debug.LogWarning("Audio with given name not found!");
    }
    public void playAudio(string name, Vector3 position)
    {
        foreach (Audio audio in audios)
        {
            if (audio.name == name)
            {
                AudioSource.PlayClipAtPoint(audio.audioClip, position, audio.volume * effectsVolume * masterVolume);
                return;
            }
        }
        Debug.LogWarning("Audio with given name not found!");
    }
    public void playThudAudio(Vector3 position)
    {
        int index = Random.Range(0, thudEffectAudios.Count);
        Audio audio = thudEffectAudios[index];
        AudioSource.PlayClipAtPoint(audio.audioClip, position, audio.volume * effectsVolume * masterVolume);
    }
    
    public void addEnemy(EnemyAudio ea)
    {
        enemies.Add(ea); //could be buggy, order of initialization - enemy / audio manager!
    }
    public void updateEnemy(int hash, Vector3 enemyPos, Vector3 movementVec)
    {
        foreach(EnemyAudio ea in enemies)
        {
            if(ea.hashCode == hash)
            {
                ea.movementVec = movementVec;
                ea.enemyPos = enemyPos;
                return;
            }
        }
    }
    private void handleEnemyAudios()
    {
        foreach (EnemyAudio ea in enemies)
        {
            if (Vector3.Distance(ea.enemyPos, player.position) > 15.0f || ea.movementVec.magnitude <= 0.0f)
            {
                if(ea.loopAudioSource.isPlaying)
                {
                    ea.loopAudioSource.Pause();
                }
                continue;
            }
            if (ea.loopAudioSource.clip == null)
            {
                ea.loopAudioSource.clip = enemyLoopAudio.audioClip;
            }
            ea.loopAudioSource.volume = enemyLoopAudio.volume; //to be changed
            ea.loopAudioSource.pitch = remap(0.0f, 6.0f, 0.7f, 1.5f, ea.movementVec.magnitude); //enemyLoopAudio.pitch; //to be changed

            if (!ea.loopAudioSource.isPlaying && !gameManager.isPaused && !gameManager.isPlayerDead)
            {
                ea.loopAudioSource.Play();
            }
        }
    }
    private void playAmbientEffect()
    {
        int index = Random.Range(0, ambientEffectAudios.Count);
        Audio audio = null;
        try
        {
            audio = ambientEffectAudios[index];
        }
        catch(System.Exception e)
        {
            Debug.LogError("effect ambient sound error:" + e.Message);
        }

        if (audio != null)
        {
            Debug.Log("Playing: " + audio.name);
            Vector3 pos = new Vector3(Random.Range(-2.0f, 2.0f), player.position.y + 0.5f, Random.Range(-2.0f, 2.0f));
            AudioSource.PlayClipAtPoint(audio.audioClip, pos, audio.volume * ambientVolume * masterVolume);
        }

        float nextTime = Random.Range(10, 22);

        Invoke("playAmbientEffect", nextTime);
    }
    private void onGameOverAudio()
    {
        Debug.Log("stopping all audio sources in the scene");
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.Stop();
        }
        playAudio("player_grunt2");
    }
    private void onGamePausedAudio()
    {
        Debug.Log("Audio sources in scene: " + FindObjectsOfType<AudioSource>().Length);
        if(gameManager.isPaused)
        {
            
            sceneAudioSources.Clear();
            foreach(AudioSource source in FindObjectsOfType<AudioSource>())
            {
                sceneAudioSources.Add(new Tuple<AudioSource, bool>(source, source.isPlaying));
                source.Pause();
            }
            
            ambientMenuAudioSource.volume = menuAudio.volume * ambientVolume * masterVolume;
            ambientMenuAudioSource.UnPause();
            Debug.Log("Audio sources paused!");
            if(!ambientMenuAudioSource.isPlaying)
            {
                Debug.LogError("Ambient audio source is not playing!!!");
            }
        }
        else
        {
            foreach(Tuple<AudioSource, bool> tuple in sceneAudioSources)
            {
                if(tuple.Item1 == null)
                {
                    //Audio source was destroyed
                    return;
                }
                if(tuple.Item2)
                {
                    tuple.Item1.UnPause();
                }
            }
            ambientMenuAudioSource.volume = 0.0f;
            Debug.Log("Audio sources unPaused!");
        }
        Debug.Log("AUDIO: game pause state changed!");
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
}
