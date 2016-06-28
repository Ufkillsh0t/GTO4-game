using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1; // The direction to fade : in -1 or out = 1;

	void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    /// <summary>
    /// sets fadeDire to the direction parameter making the scene fade in if -1 and out if 1;
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    /// <summary>
    /// OnLevelWasLoaded is called when a new scene has been loaded.
    /// </summary>
    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }

    public void LoadLevelFaded(int level)
    {
        StartCoroutine(WaitLoadLevel(BeginFade(1), level));
    }


    IEnumerator WaitLoadLevel(float seconds, int level)
    {
        yield return new WaitForSeconds(seconds);
        Application.LoadLevel(level);
    }
}
