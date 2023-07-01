using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public int GetCurrentSceneID()
    {
        //TODO implement
        return 0;
    }

}