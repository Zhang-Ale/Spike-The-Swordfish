using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoad : MonoBehaviour
{
    public GameObject player; 
    public void TransitionToDestination(TransitionPosition tp)
    {
        switch (tp.transitionType)
        {
            case TransitionPosition.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, tp.destinationTag));
                break;
            case TransitionPosition.TransitionType.DifferentScene:
                //SceneManager.LoadSceneAsync("EndingScene");
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
        yield return null; 
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if(entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null; 
    }
}
