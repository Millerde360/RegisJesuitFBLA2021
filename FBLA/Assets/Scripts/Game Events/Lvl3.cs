using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lvl3 : MonoBehaviour
{
    public AudioClip start;
    public AudioSource Player;
    public Image WhiteOut;

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayOneShot(start, .5f);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        WhiteOut.color = new Color(0, 0, 0, .01f / .25f);
        float time = .25f;
        while (WhiteOut.color.a > 0)
        {
            time -= Time.deltaTime;
            Color newColor = new Color(0, 0, 0, time / .25f);
            WhiteOut.color = newColor;
            yield return null;
        }
        yield break;
    }
}
