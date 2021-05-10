using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Animator sceneAnim;
    int otherScene;
    int currentScene;

    void Start()
    {
        sceneAnim = GetComponent<Animator>();
        currentScene = SceneManager.GetActiveScene().buildIndex;

        switch (currentScene)
        {
            case 0:                          //em caso de entrada em cena do menu
                otherScene = 1;
                break;

            case 1:                          //em caso de entrada em cena do jogo
                otherScene = 0;
                sceneAnim.SetBool("enteringScene", true);
                break;
        }
    }

    public void SwitchScene()                           //é chamada por algum script ligado a um botão para iniciar a transição de cena
    {
        sceneAnim.SetBool("exitingScene", true);        //ativa a animação de saída
    }

    public void LoadNewScene()                          //é chamada pela animação de saída
    {
        Debug.Log("Loading scene " + otherScene + ".");
        sceneAnim.SetBool("exitingScene", false);
        SceneManager.LoadSceneAsync(otherScene);         // carrega a outra cena
    }

    public void SceneChangeComplete()
    {
        Debug.Log("Finished changing to scene " + currentScene + ".");
        sceneAnim.SetBool("enteringScene", false);
    }

    public void QuitGame()                              //fecha o jogo
    {
        Application.Quit();
        Debug.Log("Game would have quit.");
    }
}