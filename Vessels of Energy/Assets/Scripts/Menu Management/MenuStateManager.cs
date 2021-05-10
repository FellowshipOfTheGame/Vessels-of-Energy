using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStateManager : MonoBehaviour
{
    public bool stateIdle = true;
    Animator menuAnim;
    public SceneLoader sceneLoaderObj;
    public ShipPosition shipPositionScript;

    void Start()
    {
        menuAnim = GetComponent<Animator>();
    }

    public void MenuChange(int state)                           //função chamada pelos botões.
    {
        if (stateIdle)                                          //só permite uma mudança de estado caso outra não esteja acontecendo
        {   
            menuAnim.SetInteger("stateParameter", state);       //sinaliza a mudança de estado do menu
            shipPositionScript.PositionChange(state);
            stateIdle = false;                                  //sinaliza o início da animação de mudança de estado
            Debug.Log("Changing to state " + state);
        }
        else Debug.LogWarning("Menu not idle! Can't change state until it's finished changing states.");
    }

    public void MenuChangeComplete()                            //sinaliza o fim da mudança de estado
    {
        if (!stateIdle)
        { 
            stateIdle = true;
            Debug.Log("Finished! Menu is now idle.");
        }
    }

    public void QuitCall()
    {
        sceneLoaderObj.QuitGame();
    }


}