using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Operates the intro portion of the game
public class Lvl2 : MonoBehaviour
{
    float time;
    public Text newScore;
    public Text highscore;
    public Text isHighscore;
    bool countingScore;
    public Image WhiteOut;
    public int sceneID;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        countingScore = true;

        if (PlayerPrefs.GetInt("high2") == 0)
            PlayerPrefs.SetInt("high2", 99999);
    }

    // Update is called once per frame
    void Update()
    {
        if (countingScore)
            time += Time.deltaTime;
    }

    public IEnumerator endLevel(float timeScore, float highScore, float returnToHub)
    {
        Debug.Log("Ending Level");
        yield return new WaitForSeconds(timeScore);
        countingScore = false;
        Debug.Log("Score: " + ((int)(time * 10)).ToString());
        newScore.text = ((int)(time*10)).ToString();
        yield return new WaitForSeconds(highScore);
        highscore.text = PlayerPrefs.GetInt("high2").ToString();
        isHighscore.text = "We'll Get It Next Time!";
        if(PlayerPrefs.GetInt("high2") > ((int)(time * 10)))
        {
            PlayerPrefs.SetInt("high2", ((int)(time * 10)));
            isHighscore.text = "NEW HIGHSCORE!!!!";
        }
        yield return new WaitForSeconds(returnToHub);
        SceneManager.LoadScene(sceneID);
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
