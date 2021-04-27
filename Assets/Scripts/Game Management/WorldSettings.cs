﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSettings : MonoBehaviour
{
    // Variables describing the various world settings
    public float gravity = 9.81f;

    public GameObject playerPrefab;
    public CheckpointList CheckpointList;

    public AudioSource audioSource;
    public AudioSource EnemyAudio;
    public AudioSource endingMusicSource;
    public AudioClip[] audioClipArray;
    // Audio Clips
    //0
    public AudioClip[] deathNoises;
    //1
    public AudioClip[] attackNoises;
    //2
    public AudioClip[] moveNoises;
    //3
    public AudioClip[] damageNoises;
    public AudioClip[] shootNoises;
    public AudioClip[] hammerNoises;
    public AudioClip[] sproutNoises;
    public AudioClip[] projectileBreak;
    public List<AudioClip[]> noisePacks;
    protected bool[] canPlay;
    public bool reachedEnd = false;
    private AudioClip currClip;
    public AudioClip endingMusic;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        //audioSource.loop = true;
        //audioSource.PlayOneShot(RandomClip());

        StartCoroutine(PlayBackgroundMusic());

        //Debug.Log("started");
        noisePacks = new List<AudioClip[]>();
        

        noisePacks.Add(deathNoises);
        noisePacks.Add(attackNoises);
        noisePacks.Add(moveNoises);
        noisePacks.Add(damageNoises);
        noisePacks.Add(shootNoises);
        noisePacks.Add(hammerNoises);
        noisePacks.Add(sproutNoises);
        noisePacks.Add(projectileBreak);

        canPlay = new bool[noisePacks.Count];
        for(int i = 0; i<canPlay.Length; i++)
        {
            canPlay[i] = true;
        }

        player = GameObject.Find("Player");
    }

    protected IEnumerator PlayBackgroundMusic()
    {
        while (!reachedEnd)
        {
            // Play random clip
            currClip = RandomClip();
            audioSource.PlayOneShot(currClip);

            // Yield wait for song to be over
            yield return new WaitForSeconds(currClip.length);
        }
    }

    public void PlayNoise(int ind, float delay)
    {
        if(player!=null) StartCoroutine(playNoiseOnDelay(ind, delay));
    }

    public void PlayNoise(int ind)
    {
        if(player!=null)
        {
            if(canPlay[ind])
            {
                canPlay[ind] = false;
                AudioClip[] noisePack = noisePacks[ind];
                if(noisePack!=null && noisePack.Length > 0) EnemyAudio.PlayOneShot(noisePack[Random.Range(0, noisePack.Length)]);
                canPlay[ind] = true;
            }
        }
    }

    public IEnumerator playNoiseOnDelay(int ind, float delay)
    {
        if(canPlay[ind])
        {
            canPlay[ind] = false;
            AudioClip[] noisePack = noisePacks[ind];
            if(noisePack!=null && noisePack.Length > 0) EnemyAudio.PlayOneShot(noisePack[Random.Range(0, noisePack.Length)]);
            yield return new WaitForSeconds(delay);
            canPlay[ind] = true;
        }
    }

    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length)];
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        OnDoneFading(audioSource);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void InstantiatePlayer(Vector3 lastpos, int health)
    // {
    //     Debug.Log("instantiae pos: "+lastpos);
    //     StartCoroutine(InstantiatePlayerAfterDelay(lastpos, new Quaternion()));
        

    // IEnumerator InstantiatePlayerAfterDelay(Vector3 lastpos1, Quaternion lastrot1)
    // {
    //     yield return new WaitForSeconds(2);
    //     Debug.Log("Instantiating Player");
    //     Debug.Log("instantiae pos: "+lastpos1);
    //     GameObject newPlayer = Instantiate(playerPrefab, lastpos1, lastrot1);
    //     Debug.Log("Instantiating Player - should be in game");
    //     //set new follow
    //     Follow followCam = GameObject.Find("Follow Camera").GetComponent<Follow>();
    //     followCam.player = newPlayer;
    // }
    // }

    public void GameOver()
    {

    }

    public void OnDoneFading(AudioSource audio)
    {
        if (audio != endingMusicSource)
        {
            // Play one shot clip
            endingMusicSource.PlayOneShot(endingMusic);

            // Fade back in
            StartCoroutine(StartFade(endingMusicSource, 1.5f, 0.202f));
        }
    }

    public void OnWin()
    {
        Debug.Log("Winning in world settings");

        // Tell it that we've reached the end
        reachedEnd = true;

        // Fade curr music out
        StartCoroutine(StartFade(audioSource, 1.5f, 0.0f));
    }
}
