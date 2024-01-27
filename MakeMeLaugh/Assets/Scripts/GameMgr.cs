using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]
public class LevelData
{
    public int level;
    // key 为笑脸的index， value为结果的index（0~2）
    public Dictionary<int, int> datas;
}

[System.Serializable]
public class LevelResult
{
    string[] lines;
    //string[] resultBG;
}


public class GameMgr : Singleton<GameMgr>
{
    private int currentLevel = 0;
    public GameObject BackGround;
    public GameObject Face;

    public GameObject Panel;

    private SpriteRenderer backSprite;
    private PanelController panelController;

    private const string ResultPathTmp = "Textures/{0}/{1}_{2}bg";
    private const string BGTmp = "Textures/{0}/{1}_bg";

    // Start is called before the first frame update
    void Start()
    {
        backSprite = BackGround.GetComponent<SpriteRenderer>();
        panelController = Panel.GetComponent<PanelController>();
        LevelStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetReuslt(int face)
    {
        var path = string.Format(ResultPathTmp, currentLevel, currentLevel, face);
        Sprite newSprite = Resources.Load<Sprite>(path);
        backSprite.sprite = newSprite;
        panelController.Hide();
    }

    void NextLevel()
    {
        currentLevel++;
        LevelStart();
    }

    void GameRest()
    {
        currentLevel = 0;
    }

    void LevelStart ()
    {
        var path = string.Format(BGTmp, currentLevel, currentLevel);
        Sprite newSprite = Resources.Load<Sprite>(path);
        backSprite.sprite = newSprite;
        panelController.Show();
    }

}
