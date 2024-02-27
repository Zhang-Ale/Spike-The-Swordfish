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

    void Start()
    {
        backMusic.SetActive(true);
        backAS = backMusic.GetComponent<AudioSource>();
        currentAS = currentMusic.GetComponent<AudioSource>();
        garbageAS = garbageMusic.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PM.metCurrent)
        {
            StartCoroutine(ActionOne(backAS, backAS.volume, mFaded ? 0.3f : 0.05f));
            currentMusic.SetActive(true);
            currentAS.Play(); 
            StartCoroutine(ActionOne(currentAS, currentAS.volume, mFaded ? 0f : 0.4f));
        }
        else
        {
            StartCoroutine(ActionOne(currentAS, currentAS.volume, mFaded ? 0.4f : 0f));
            StartCoroutine(ActionOne(backAS, backAS.volume, mFaded ? 0.05f : 0.3f));
        }

        if (PM.metGarbage)
        {
            StartCoroutine(ActionOne(backAS, backAS.volume, mFaded ? 0.3f : 0f));
            garbageMusic.SetActive(true);
            garbageAS.Play(); 
            StartCoroutine(ActionOne(garbageAS, garbageAS.volume, mFaded ? 0f : 0.4f));
        }
        else
        {
            StartCoroutine(ActionOne(garbageAS, garbageAS.volume, mFaded ? 0.5f : 0f));
            StartCoroutine(ActionOne(backAS, backAS.volume, mFaded ? 0.05f : 0.3f));
        }
    }

    public void FadeIn(AudioSource AS)
    {
        StartCoroutine(ActionOne(AS, AS.volume, mFaded ? 0 : 1));
    }

    public void FadeOut(AudioSource AS)
    {
        StartCoroutine(ActionOne(AS, AS.volume, mFaded ? 1 : 0));
    }

    public IEnumerator ActionOne(AudioSource AS, float start, float end)
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
