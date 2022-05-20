/// Class that holds info regarding enemy audio
/**
    This class holds the information regarding enemies.
    The audio manager uses this information in order
    to play the enemy sound correctly.
*/
using UnityEngine;

public class EnemyAudio
{
    //Properties
    public AudioSource loopAudioSource { get; set; }
    public AudioSource effectAudioSource { get; set; }
    public int hashCode { get; set; }
    public Vector3 movementVec { get; set; }
    public Vector3 enemyPos { get; set; }

    //Constructor
    public EnemyAudio(int _hashCode, Vector3 _movementVec, Vector3 _enemyPos, AudioSource _loopAudioSource, AudioSource _effectAudioSource)
    {
        hashCode = _hashCode;
        movementVec = _movementVec;
        enemyPos = _enemyPos;
        loopAudioSource = _loopAudioSource;
        effectAudioSource = _effectAudioSource;
    }
}
