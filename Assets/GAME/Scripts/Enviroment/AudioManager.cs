using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip1;
    [SerializeField] private AudioClip audioClip2;
    [SerializeField] private AudioClip audioClip3;

    [SerializeField] private float volume1 = 1f; // Volume do áudio 1
    [SerializeField] private float volume2 = 1f; // Volume do áudio 2
    [SerializeField] private float volume3 = 1f; // Volume do áudio 3


    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;


    private void Start()
    {
        // Criar o primeiro componente AudioSource
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource1.clip = audioClip1;
        audioSource1.volume = volume1;
        audioSource1.loop = true; // Ativar o loop
        audioSource1.Play();

        // Criar o segundo componente AudioSource
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = audioClip2;
        audioSource2.volume = volume2;
        audioSource2.loop = true; // Ativar o loop
        audioSource2.Play();

        // Criar o terceiro componente AudioSource
        audioSource3 = gameObject.AddComponent<AudioSource>();
        audioSource3.clip = audioClip3;
        audioSource3.volume = volume3;
        audioSource3.loop = true; // Ativar o loop
        audioSource3.Play();
    }
}
