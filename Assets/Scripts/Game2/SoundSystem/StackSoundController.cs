using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    private Coroutine _playPitchCo;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayWithPitch()
    {   
        _audioSource.PlayOneShot(_audioSource.clip);
        _playPitchCo = StartCoroutine(ResetPitchCoroutine());
        _audioSource.pitch -= 0.05f;
    }

    private IEnumerator ResetPitchCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        _audioSource.pitch = 1f;
    }
}
