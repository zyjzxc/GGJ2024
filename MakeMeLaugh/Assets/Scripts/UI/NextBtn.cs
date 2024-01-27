using UnityEngine;
using UnityEngine.UI;

public class Next : MonoBehaviour
{
    public Button button;  // your button
    private AudioSource audioSource;

    void Start()
    {
        button = GetComponent<Button>();
        //audioSource = GetComponent<AudioSource>();
        //button.onClick.AddListener(() => { audioSource.Play(); });
        // add click event listener
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        GameMgr.Instance.NextState();
    }
}