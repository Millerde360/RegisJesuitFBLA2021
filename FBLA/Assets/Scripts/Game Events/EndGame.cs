using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public AudioClip line;
    bool played = false;

    public Text title;
    public Text author;
    public Text fbla;
    public Text thanks;

    Color transparent = new Color(0, 0, 0, 0);

    private void Start()
    {
        title.color = transparent;
        author.color = transparent;
        fbla.color = transparent;
        thanks.color = transparent;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !played)
        {
            transform.parent.GetComponent<Lvl3>().Player.PlayOneShot(line, 1f);
            transform.parent.GetComponent<Lvl3>().Player.GetComponent<PlayerMovement>().enabled = false;
            transform.parent.GetComponent<Lvl3>().Player.GetComponent<PlayerMovement>().camera.GetComponent<CameraLook>().enabled = false;
            StartCoroutine(UIappearances());
            played = true;
        }
    }

    IEnumerator UIappearances()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(fadeIn(title));
        yield return new WaitForSeconds(1);
        StartCoroutine(fadeIn(author));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(fadeIn(fbla));
        yield return new WaitForSeconds(3f);
        StartCoroutine(fadeIn(thanks));
        yield return new WaitForSeconds(25f);
        SceneManager.LoadScene(0);

    }

    IEnumerator fadeIn(Text text)
    {
        float fadeTime = 2;
        float fadeTimeCur = 0;
        while(text.color.a < 1)
        {
            Color newCol = new Color(0, 0, 0, fadeTimeCur / fadeTime);
            text.color = newCol;

            fadeTimeCur += Time.deltaTime;
            yield return null;
        }
    }
}
