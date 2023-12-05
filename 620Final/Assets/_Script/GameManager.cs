using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public enum SceneIndexes
{
    MANAGER = 0,
    TITLE = 1,
    GAMEPLAY = 2,
    ENDING = 3
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Scene currentScene;
    string sceneName;
    public GameObject loadingScreen;
    public LoadingProgress bar;
    CanvasGroup cg;
    public TextMeshProUGUI textField;
    public TextMeshProUGUI tipText;
    public CanvasGroup tipCg;
    public string[] tips;
    bool gameStart;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        cg = loadingScreen.GetComponent<CanvasGroup>();
        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE, LoadSceneMode.Additive);
    }

    void Start()
    {
        gameStart = false; 
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName != "GameplayScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadNewGame()
    {
        cg.alpha = 1;
        StartCoroutine(GenerateTips());
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.GAMEPLAY, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.ENDING, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
        StartCoroutine(GetTotalProgress());
    }

    float totalSceneProgress;
    float totalSpawnProgress;

    public IEnumerator GetSceneLoadProgress()
    {
        for(int i = 0; i <scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }
                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                textField.text = string.Format("LOADING ENVIRONMENT: {0}%", totalSceneProgress);
                yield return null;                
            }          
        }
    }

    public IEnumerator GetTotalProgress()
    {
        float totalProgress = 0;

        while (Spawner.spawn==null || Spawner.spawn.isDone)
        {
            if(Spawner.spawn == null)
            {
                totalSpawnProgress = 0;
            }
            else
            {
                totalSpawnProgress = Mathf.Round(Spawner.spawn.progress * 100f);
                textField.text = string.Format("LOADING FISH POOLS: {0}%", totalSpawnProgress);
            }
            totalProgress = Mathf.Round((totalSceneProgress + totalSpawnProgress) / 2f);
            bar.current = Mathf.RoundToInt(totalSceneProgress);
            yield return null;
        }
        cg.alpha = 0;
        bar.current = 0;
        gameStart = true; 
    }
    public int tipCount; 
    IEnumerator GenerateTips()
    {
        tipCount = Random.Range(0, tips.Length);
        tipText.text = tips[tipCount];
        while(loadingScreen.activeInHierarchy){
            yield return new WaitForSeconds(1f);
            tipCg.alpha = 0f;
            yield return new WaitForSeconds(0.5f);
            tipCount++;
            if(tipCount>= tips.Length)
            {
                tipCount = 0;
            }
            tipText.text = tips[tipCount];
            tipCg.alpha = 1f;
        }
    }
}
