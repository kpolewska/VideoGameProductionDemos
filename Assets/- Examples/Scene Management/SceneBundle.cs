using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new Scene Bundle", menuName = "Scene Bundle/New Scene Bundle", order = 1)]
public class SceneBundle : ScriptableObject {

    public string sceneBundleName;
    public bool additiveLoad = false;
    public string[] scenesNames;
}
