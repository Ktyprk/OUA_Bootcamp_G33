using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransform : MonoBehaviour, ISceneTransform
{
    public string sceneName;
    public void TransformScene()
    {
        SceneManager.LoadScene(sceneName );
    }
}

public interface ISceneTransform
{
   public void TransformScene();
}


