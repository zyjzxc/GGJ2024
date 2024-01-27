using UnityEngine;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour
{
    public Button button;  // your button
    private AudioSource audioSource;

    void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(() => { audioSource.Play(); });
        // add click event listener
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        GameMgr.Instance.SetReuslt(1);
        // code to execute when button is clicked
        Debug.Log("Button has been clicked!");
    }
}