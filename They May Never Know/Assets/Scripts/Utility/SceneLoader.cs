using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static Scene curScene = Scene.game;

    public enum Scene
    {
        game,
        option,
        pause,
        menu,
        
    }

    public static void loadScene(Scene s)
    {
        curScene = s;
        SceneManager.LoadSceneAsync((int)curScene);
    }

    public static void loadOptionScene()
    {
        curScene = Scene.option;
        SceneManager.LoadSceneAsync((int)Scene.option, LoadSceneMode.Additive);

    }

    public static void unloadOptionScene()
    {
        curScene = Scene.game;
        SceneManager.UnloadSceneAsync((int)Scene.option);
    }

    public static bool optionSceneActive()
    {
        return curScene == Scene.option;
    }
}
