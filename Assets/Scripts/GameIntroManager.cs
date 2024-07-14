using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pressAnyKeyUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
            StartCoroutine(StartGame());
        _pressAnyKeyUI.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
    }

    IEnumerator StartGame()
    {
        GameObject.Find("ScreenTransition").GetComponent<Animator>().SetTrigger("Trigger");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SampleScene");
    }
}
