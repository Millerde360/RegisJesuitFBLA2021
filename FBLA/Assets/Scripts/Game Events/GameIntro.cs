using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Operates the intro portion of the game
public class GameIntro : MonoBehaviour
{
    public Image WhiteOut;
    public Text interactText;
    bool fadedIn = false;
    public PlayerMovement player;
    bool levelOver;

    public AudioClip introVoice;
    public AudioClip openingBreath;
    bool audioclipStarted;

    [Header("Clips and Start Clips")]
    public bool oneOneStart;
    public bool oneOneStarted;
    public AudioClip oneOneClip;

    [Header("UI and others")]
    float time;
    public Text newScore;
    public Text highscore;
    public Text isHighscore;
    bool countingScore;
    

    // Start is called before the first frame update
    void Start()
    {
        Color whited = new Color(0, 0, 0, 255);
        WhiteOut.color = whited;
        player.enabled = false;
        interactText.text = "Press Space to Wake Up";

        if (PlayerPrefs.GetInt("high2") == 0)
            PlayerPrefs.SetInt("high2", 99999);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fadedIn && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(FadeIn());

        if (fadedIn && !audioclipStarted)
        {
            player.GetComponent<AudioSource>().clip = introVoice;
            player.GetComponent<AudioSource>().PlayOneShot(introVoice, .5f);
            audioclipStarted = true;
        }

        if (oneOneStart && !oneOneStarted)
        {
            player.GetComponent<AudioSource>().clip = oneOneClip;
            player.GetComponent<AudioSource>().PlayOneShot(oneOneClip, .5f);
            oneOneStarted = true;
        }

        if (countingScore)
            time += Time.deltaTime;
    }

    IEnumerator FadeIn()
    {
        player.GetComponent<AudioSource>().PlayOneShot(openingBreath, .5f);
        interactText.text = "";
        float time = .25f;
        while(WhiteOut.color.a > 0)
        {
            time -= Time.deltaTime;
            Color newColor = new Color(0, 0, 0, time / .25f);
            WhiteOut.color = newColor;
            yield return null;
        }
        player.enabled = true;
        yield return new WaitForSeconds(.5f);
        countingScore = true;
        fadedIn = true;
        yield break;
    }

    public IEnumerator endLevel(float timeScore, float highScore, float returnToHub)
    {
        Debug.Log("Ending Level");
        yield return new WaitForSeconds(timeScore);
        countingScore = false;
        Debug.Log("Score: " + ((int)(time * 10)).ToString());
        newScore.text = ((int)(time*10)).ToString();
        yield return new WaitForSeconds(highScore);
        highscore.text = PlayerPrefs.GetInt("high").ToString();
        isHighscore.text = "We'll Get It Next Time!";
        if(PlayerPrefs.GetInt("high") > ((int)(time * 10)))
        {
            PlayerPrefs.SetInt("high", ((int)(time * 10)));
            isHighscore.text = "NEW HIGHSCORE!!!!";
        }
        yield return new WaitForSeconds(returnToHub);
        SceneManager.LoadScene(1);
    }
}
