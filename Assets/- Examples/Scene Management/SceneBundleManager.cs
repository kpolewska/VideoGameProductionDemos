using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBundleManager : MonoBehaviour {

    public SceneBundle[] sceneBundles;
    
    public bool loadOnStart = true;
    public SceneBundle defaultBundle;
    public string loadingScreenSceneName;

    //Singleton pattern
    private static SceneBundleManager _instance;

    public static SceneBundleManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneBundleManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    //End of singleton pattern

    private void Start()
    {
        //Load the default bundle if loadOnStart is true
        if(loadOnStart) LoadSceneBundle(defaultBundle.name);
    }

    //The public function LoadSceneBundle is accessible from any script now through the singleton using SceneBundleManager.Instance.LoadSceneBundle(stringNameOfBundle);
    public void LoadSceneBundle(string bundleName)
    {
        //Find which bundle is associated with the bundleName
        foreach(SceneBundle bundle in sceneBundles)
        {
            //If the name is found, start the coroutine that starts the loading process and break the foreach loop as it is not necessary to search for another bundle to load
            if (bundle.name == bundleName)
            {
                StartCoroutine(LoadingScenes(bundle));
                break;
            }
        }
        
    }

    IEnumerator LoadingScenes(SceneBundle sceneBundle)
    {
        //Check if the bundle to load is additive or not. If it is additive, the previous scene(s) won't be replaced by the newest bundle.
        LoadSceneMode loadMode = (sceneBundle.additiveLoad) ? LoadSceneMode.Additive : LoadSceneMode.Single;

        //Wait until loading screen is active and assigne the right loadmode set above
        yield return SceneManager.LoadSceneAsync(loadingScreenSceneName, loadMode);

        //Wait 1 second on the loading screen so that player can see what's happening
        yield return new WaitForSeconds(1);

        //Start another coroutine to start loading the scene bundle now that the loading screen is displayed. The loading screen is displayed until this new coroutine ends (ie. All scenes are loaded)
        yield return StartCoroutine(LoadAsyncSceneBundle(sceneBundle));

        //Unload loading screen once loading all layers is finished
        yield return SceneManager.UnloadSceneAsync(loadingScreenSceneName);
    }

    IEnumerator LoadAsyncSceneBundle(SceneBundle sceneBundle)
    {
        //Tracks progress of loading
        float loadProgress = 0;

        //THis is used to keep track of all async loads
        AsyncOperation[] asyncLayers = new AsyncOperation[sceneBundle.scenesNames.Length];
        int currentLayerIndex = 0;

        foreach (string scene in sceneBundle.scenesNames)
        {
            //Start asynchronous load of scene layers. Load mode is set to additive so that all scenes loading does not replace those already loaded
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false; //This is done

            while (!asyncLoad.isDone)
            {
                loadProgress += Mathf.Clamp01(asyncLoad.progress/ 0.9f) *100 / sceneBundle.scenesNames.Length;
                Debug.Log(loadProgress);

                //The progress of asyncLoad stops at 0.9 and never goes at 1...
                //This if condition is done to verify that the scene is loaded up to 0.9
                //Once this is detected, the asyncLoad is kept in an array for safekeeping
                //Since asyncLoad.allowSceneActivation is currently equal to false, this means that even if the scene is loaded, it won't be shown until we decide to switch this back to true
                //THis is done so that the layers all appears around the same time.
                if(asyncLoad.progress >= 0.9f)
                {
                    asyncLayers[currentLayerIndex] = asyncLoad;
                    currentLayerIndex++;
                    break;
                }

                yield return null;
            }
        }

        foreach(AsyncOperation asyncLayer in asyncLayers)
        {
            //Once all scenes are loaded in memory, enable scene activation so that all scenes apppears at the same time
            asyncLayer.allowSceneActivation = true;
        }

        yield return null;
    }
}
