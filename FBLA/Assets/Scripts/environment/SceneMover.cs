using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public int sceneID;
    public Image WhiteOut;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag + "as");
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(FadeIn());
        }
    }
    IEnumerator FadeIn()
    {
        float time = 0f;
        while (WhiteOut.color.a <= 1)
        {
            time += Time.deltaTime;
            Color newColor = new Color(0, 0, 0, time / .25f);
            WhiteOut.color = newColor;
            yield return null;
        }
        SceneManager.LoadScene(sceneID);
        yield break;
    }
}
