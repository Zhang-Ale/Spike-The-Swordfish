using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject backMusic, currentMusic, garbageMusic;
    AudioSource backAS, currentAS, garbageAS;
    public PlayerMovement PM;
    private bool mFaded = false;
    public float Duration = 1f;
    public PauseGame PG; 

    void Start()
    {
        backMusic.SetActive(true);
        backAS = backMusic.GetComponent<AudioSource>();
        currentAS = currentMusic.GetComponent<AudioSource>(); 
        garbageAS = garbageMusic.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!PG.gameIsPaused)
        {
            if (PM.metCurrent)
            {
                StartCoroutine(PlayAudio(backAS, 0.5f, 0f));
                currentMusic.SetActive(true);
                backMusic.SetActive(false);
                StartCoroutine(PlayAudio(currentAS, 0f, 0.5f));
            }
            else
            {
                StartCoroutine(PlayAudio(currentAS, 0.5f, 0f));
                currentMusic.SetActive(false);
                backMusic.SetActive(true);
                StartCoroutine(PlayAudio(backAS, 0f, 0.5f));
            }

            if (PM.metGarbage)
            {
                StartCoroutine(PlayAudio(backAS, 0.5f, 0f));
                currentMusic.SetActive(false);
                backMusic.SetActive(false);
                garbageMusic.SetActive(true);
                StartCoroutine(PlayAudio(garbageAS, 0f, 0.5f));
            }
            else
            {
                StartCoroutine(PlayAudio(garbageAS, 0.5f, 0f));
                garbageMusic.SetActive(false);
                backMusic.SetActive(true);
                StartCoroutine(PlayAudio(backAS, 0f, 0.5f));
            }
        }
        else
        {
            backAS.volume = 0.25f;
            garbageAS.volume = 0.25f;
            currentAS.volume = 0.25f;
        }
    }

    public void FadeIn(AudioSource AS)
    {
        StartCoroutine(PlayMusic(AS, AS.volume, mFaded ? 0 : 1));
    }

    public void FadeOut(AudioSource AS)
    {
        StartCoroutine(PlayMusic(AS, AS.volume, mFaded ? 1 : 0));
    }

    IEnumerator PlayAudio(AudioSource AS, float start, float end)
    {
        while (AS.volume >= start)
        {
            AS.volume = Mathf.Lerp(AS.volume, end, 1f * Time.deltaTime);
            yield return 0f;
        }
    }

    public IEnumerator PlayMusic(AudioSource AS, float start, float end)
    {
        float counter = 0f;
        yield return new WaitForSeconds(0.5f);
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            AS.volume = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
        }
    }
}
