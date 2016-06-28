using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour
{

    public void LoadScene(int level)
    {
        GameObject fader = GameObject.FindGameObjectWithTag("Fader");
        FadeOut fade;
        if (fader != null)
        {
            fade = fader.GetComponent<FadeOut>();
            if (fade != null)
            {
                fade.LoadLevelFaded(level);
            }
            else
            {
                Application.LoadLevel(level);
            }
        }
        else
        {
            Application.LoadLevel(level);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
