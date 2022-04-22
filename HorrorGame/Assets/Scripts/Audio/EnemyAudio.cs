using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio
{
    public AudioSource loopAudioSource { get; set; }
    public AudioSource effectAudioSource { get; set; }
    public int hashCode { get; set; }
    public Vector3 movementVec { get; set; }
    public Vector3 enemyPos { get; set; }
    public EnemyAudio(int _hashCode, Vector3 _movementVec, Vector3 _enemyPos, AudioSource _loopAudioSource, AudioSource _effectAudioSource)
    {
        hashCode = _hashCode;
        movementVec = _movementVec;
        enemyPos = _enemyPos;
        loopAudioSource = _loopAudioSource;
        effectAudioSource = _effectAudioSource;
    }
}
