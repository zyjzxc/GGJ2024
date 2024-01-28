using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

using System.IO;
using TMPro;
using UnityEngine.Video;
using Unity.VisualScripting;



public enum GameState
{
    Lines,
    Pending,
    Result,
    EndingLines
}

[System.Serializable]
public class LevelLine
{
    public string name;
    public string line;
}

[System.Serializable]
public class LevelLines
{
    public LevelLine[] levelLines;
}

public class GameMgr : Singleton<GameMgr>
{
    private int currentLevel = 0;
    public GameObject BackGround;
    public GameObject Face;

    public GameObject Panel;

    public GameObject nameText;
    public GameObject line;

    public GameObject dialogue;

    public Image Preview;

    private SpriteRenderer backSprite;
    private PanelController panelController;
    private OptionSelector optionSelector;
    private VideoPlayer videoPlayer;


    public Slider optionSlider;
    public Button NextBtn;

    public GameState state = GameState.Lines;

    private int option0 = 0;
    private int option1 = 0;

    private const string ResultPathTmp = "Textures/{0}/{1}_{2}bg";
    private const string BGTmp = "Textures/{0}/{1}_bg";
    private const string LvPath = "Assets/Resources/Textures/{0}";

    private int[] faces = new int[9] { 3, 8, 7, 0, 1, 6, 4, 2, 5 };

    int choosedFace = 0;

    LevelLines endLines = new LevelLines();
    LevelLines lines = new LevelLines();
    int currtentLine = 0;

    bool gameStarted = false;


    void LoadLines()
    {
        string path = "Assets/Data/{0}.json";
        path = string.Format(path, currentLevel);

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            jsonString = "{\"levelLines\":" + jsonString + "}";
            lines = JsonUtility.FromJson<LevelLines>(jsonString);

            foreach (LevelLine line in lines.levelLines)
            {
                Debug.Log("Name: " + line.name + ", Score: " + line.line);
            }
        }
        else
        {
            lines.levelLines = new LevelLine[0];
        }
    }

    void LoadEndLines()
    {
        string path = "Assets/Data/Results/{0}/{1}.json";
        path = string.Format(path, currentLevel, choosedFace);

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            jsonString = "{\"levelLines\":" + jsonString + "}";
            endLines = JsonUtility.FromJson<LevelLines>(jsonString);

            foreach (LevelLine line in endLines.levelLines)
            {
                Debug.Log("Name: " + line.name + ", Score: " + line.line);
            }
        }
        else
        {
            endLines.levelLines = new LevelLine[0];
        }
    }

    void UpdataDiaglogue()
    {
        dialogue.SetActive(true);
        nameText.GetComponent<TextMeshProUGUI>().SetText(lines.levelLines[currtentLine].name);
        line.GetComponent<TextMeshProUGUI>().SetText(lines.levelLines[currtentLine].line);

    }

    void UpdataEndDiaglogue()
    {
        dialogue.SetActive(true);
        nameText.GetComponent<TextMeshProUGUI>().SetText(endLines.levelLines[currtentLine].name);
        line.GetComponent<TextMeshProUGUI>().SetText(endLines.levelLines[currtentLine].line);

    }

    public void NextState()
    {
        if (!gameStarted)
        {
            videoPlayer.Stop();
            EndOpening(videoPlayer);
            return;
        }
        //if (state != GameState.Lines)
        //    state = (GameState)(((int)GameMgr.Instance.state + 1) % System.Enum.GetNames(typeof(GameState)).Length);
        //else
        //{
        //    currtentLine++;
        //    if (currtentLine < lines.levelLines.Length)
        //    {
        //        UpdataDiaglogue();
        //    }
        //    else
        //    {
        //        dialogue.SetActive(false);
        //        state = (GameState)(((int)GameMgr.Instance.state + 1) % System.Enum.GetNames(typeof(GameState)).Length);
        //        currtentLine = 0;
        //    }
        //}
        //if (state == GameState.Pending)
        //{
        //    panelController.Show();
        //    NextBtn.interactable = false;
        //    NextBtn.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //}
        //else
        //{
        //    NextBtn.interactable = true;
        //    NextBtn.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //    if (state == GameState.Lines && currtentLine == 0)
        //    {
        //        NextLevel();
        //    }
        //}

        switch(state)
        {
            case GameState.Lines:
                currtentLine++;
                if (currtentLine < lines.levelLines.Length)
                {
                    UpdataDiaglogue();
                }
                else
                {
                    dialogue.SetActive(false);
                    state = (GameState)(((int)GameMgr.Instance.state + 1) % System.Enum.GetNames(typeof(GameState)).Length);
                    currtentLine = 0;
                }
                break;
            case GameState.Pending:
                state = (GameState)(((int)GameMgr.Instance.state + 1) % System.Enum.GetNames(typeof(GameState)).Length);
                panelController.Show();
                NextBtn.interactable = false;
                NextBtn.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                break;
            case GameState.Result:
                state = (GameState)(((int)GameMgr.Instance.state + 1) % System.Enum.GetNames(typeof(GameState)).Length);
                NextBtn.interactable = true;
                NextBtn.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                if (currtentLine < endLines.levelLines.Length)
                {
                    UpdataEndDiaglogue();
                }
                break;
            case GameState.EndingLines:
                currtentLine++;
                if (currtentLine < endLines.levelLines.Length)
                {
                    UpdataEndDiaglogue();
                }
                else
                {
                    dialogue.SetActive(false);
                    state = (GameState)(((int)GameMgr.Instance.state + 1) % System.Enum.GetNames(typeof(GameState)).Length);
                    currtentLine = 0;
                    NextLevel();
                }
                //if (state == GameState.Lines && currtentLine == 0)
                //{
                //    NextLevel();
                //}
                break;
            default: break;
        }
        Debug.Log(state);
    }

    public override void Awake()
    {
        base.Awake();
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndOpening;
        videoPlayer.Play();
        //videoPlayer.Prepare();


    }

    // Start is called before the first frame update
    void Start()
    {
        //videoPlayer.Play();
        backSprite = BackGround.GetComponent<SpriteRenderer>();
        panelController = Panel.GetComponent<PanelController>();
        optionSelector = gameObject.GetComponent<OptionSelector>();
        optionSlider.onValueChanged.AddListener(ShowPreview);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowPreview(float v)
    {
        option1 = (int)optionSlider.value;
        choosedFace = faces[option1 + option0 * 3];
        var previewPath = string.Format("Textures/Preview/ui_emoji_00{0}", choosedFace);
        Preview.sprite = Resources.Load<Sprite>(previewPath);
        Debug.Log(option1 + option0 * 3);
    }

    public void SetOption0(int i)
    {
        option0 = i;
        ShowPreview(0);
    }    

    public void SetReuslt()
    {
        option1 = (int)optionSlider.value;
        choosedFace = faces[option1 + option0 * 3];
        var path = string.Format(ResultPathTmp, currentLevel, currentLevel, choosedFace);
        Sprite newSprite = Resources.Load<Sprite>(path);
        backSprite.sprite = newSprite;
        panelController.Hide();
        BackGround.GetComponent<Animator>().CrossFade("shake", 0f);
        LoadEndLines();
        NextState();
    }

    void NextLevel()
    {
        currentLevel++;
        if (!Directory.Exists(string.Format(LvPath, currentLevel)))
            GameRest();
        LevelStart();
    }

    void GameRest()
    {
        currentLevel = 0;
    }

    void LevelStart ()
    {
        //panelController.Hide();
        option0 = 0;
        option1 = 0;
        optionSlider.value = 0;
        optionSelector.Reset();
        var path = string.Format(BGTmp, currentLevel, currentLevel);
        Sprite newSprite = Resources.Load<Sprite>(path);
        backSprite.sprite = newSprite;
        LoadLines();
        if (currtentLine < lines.levelLines.Length)
            UpdataDiaglogue();
    }

    void EndOpening(VideoPlayer vp)
    {
        gameStarted = true;
        videoPlayer.targetCamera = null;
        LevelStart();
    }

}
