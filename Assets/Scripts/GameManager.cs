using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _darkScreenUI;
    [SerializeField] private Image _ImageNextUI;
    [SerializeField] private TextMeshProUGUI _textScoreUI;
    [SerializeField] private TextMeshProUGUI _textBestScoreUI;
    [SerializeField] private TextMeshProUGUI _textLevelUI;
    [SerializeField] private TextMeshProUGUI _textLinesUI;
    [SerializeField] private TextMeshProUGUI _gameOverUI;
    [SerializeField] private TextMeshProUGUI _playAgainUI;
    [SerializeField] private List<TextMeshProUGUI> _textStatisticsUI;

    [SerializeField] private Sprite[] _spriteShapes;
    [SerializeField] private Image[] _imageStatisticsUI;
    private int _score, _bestScore, _amount;

    private int T, L, O, I, Z, rL, rZ;

    public int _level;
    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(800, 600, false);
        CheckPlayerPrefs();
    }
    void Start()
    {
        SetStats();
        SetUI();
    }

    private void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("BestScore"))
            _bestScore = PlayerPrefs.GetInt("BestScore");
        else
            _bestScore = 0;
    }
    private void SetUI()
    {
        _textScoreUI.text = _score.ToString();
        _textBestScoreUI.text = _bestScore.ToString();
        //GameObject.Find("ScreenTransition").GetComponent<Image>().enabled = false;
    }
    private void SetStats()
    {
        _amount = 0;
        _score = 0;
        _level = 0;
        T = 0; L = 0; O = 0; I = 0; Z = 0; rL = 0; rZ = 0;
    }
    public void UpdateScore(int score)
    {
        _score += score;
        _amount += score;
        _textScoreUI.text = _score.ToString();
        if (_score >= _bestScore)
        {
            _bestScore = _score;
            _textBestScoreUI.text = _bestScore.ToString();
        }
        UpdateLevel();
    }
    private void UpdateLevel()
    {
        if (_amount == 20)
        {
            _amount = 0;
            _level++;
            _textLevelUI.text = _level.ToString();
        }

    }
    public void UpdateStatistics(int number)
    {
        switch (number)
        {
            case 0:
                T++;
                _textStatisticsUI[0].text = T.ToString();
                break;
            case 1:
                rL++;
                _textStatisticsUI[1].text = rL.ToString();
                break;
            case 2:
                Z++;
                _textStatisticsUI[2].text = Z.ToString();
                break;
            case 3:
                O++;
                _textStatisticsUI[3].text = O.ToString();
                break;
            case 4:
                rZ++;
                _textStatisticsUI[4].text = rZ.ToString();
                break;
            case 5:
                L++;
                _textStatisticsUI[5].text = L.ToString();
                break;
            case 6:
                I++;
                _textStatisticsUI[6].text = I.ToString();
                break;
            default:
                return;
        }
    }
    public void UpdateNext(int number)
    {
        switch (number)
        {
            case 0:
                _ImageNextUI.sprite = _spriteShapes[0];
                break;
            case 1:
                _ImageNextUI.sprite = _spriteShapes[1];
                break;
            case 2:
                _ImageNextUI.sprite = _spriteShapes[2];
                break;
            case 3:
                _ImageNextUI.sprite = _spriteShapes[3];
                break;
            case 4:
                _ImageNextUI.sprite = _spriteShapes[4];
                break;
            case 5:
                _ImageNextUI.sprite = _spriteShapes[5];
                break;
            case 6:
                _ImageNextUI.sprite = _spriteShapes[6];
                break;
            default:
                return;
        }
    }
    public void UpdateNextByLevel(Sprite[] currentLevel)
    {
        _spriteShapes = currentLevel;
    }
    public void UpdateImgStatistics(Sprite[] currentLevel)
    {
        for (int i = 0; i < _imageStatisticsUI.Count(); i++)
        {
            _imageStatisticsUI[i].sprite = currentLevel[i];
        }
    }
    public void NewGame()
    {
        CheckBestScore();
        SceneManager.LoadScene("SampleScene");
    }
    private void CheckBestScore()
    {
        CheckPlayerPrefs();
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
    }

    public void UpdateLines(int lineCount) => _textLinesUI.text = lineCount.ToString();
    public void ShowGameOver() => StartCoroutine("EndGame");
    IEnumerator EndGame()
    {
        SoundController.Instance.PlayBackGround(9);
        _darkScreenUI.SetActive(true);
        float elapsedTime = 0f, timeDuration = 0.5f, timeRestart = 20f;
        //GameOver 
        while (elapsedTime < timeDuration)
        {
            elapsedTime += Time.deltaTime;
            _gameOverUI.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(-1000, 0, elapsedTime / timeDuration), 0, 0);
            yield return null;
        }
        _gameOverUI.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        //PlayAgain
        elapsedTime = 0f;
        while (elapsedTime < timeDuration)
        {
            elapsedTime += Time.deltaTime;
            _playAgainUI.GetComponent<RectTransform>().localPosition = new Vector3(0, Mathf.Lerp(-1000, -50, elapsedTime / timeDuration), 0);
            yield return null;
        }
        _playAgainUI.GetComponent<RectTransform>().localPosition = new Vector3(0, -50, 0);

        //Blinking
        elapsedTime = 0f;
        while (elapsedTime < timeRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                RestartGame();
            elapsedTime += Time.deltaTime;
            _playAgainUI.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
            yield return null;
        }
        RestartGame(true);
    }

    private void RestartGame(bool introStage = false) => StartCoroutine(LoadGame(introStage));

    IEnumerator LoadGame(bool backToIntro)
    {
        // GameObject.Find("ScreenTransition").GetComponent<Image>().enabled = true;
        GameObject.Find("ScreenTransition").GetComponent<Animator>().SetTrigger("Trigger");
        yield return new WaitForSeconds(1);
        if (backToIntro)
            SceneManager.LoadScene("IntroScene");
        else
            SceneManager.LoadScene("SampleScene");
    }
}
