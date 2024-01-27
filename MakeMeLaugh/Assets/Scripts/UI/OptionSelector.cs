using UnityEngine;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour
{
    public Button option0Button;
    public Button option1Button;
    public Button option2Button;

    public Sprite On;
    public Sprite Off;

    void Start()
    {
        option0Button.onClick.AddListener(() => { SelectOption(0); });
        option1Button.onClick.AddListener(() => { SelectOption(1); });
        option2Button.onClick.AddListener(() => { SelectOption(2); });
    }

    void SelectOption(int option)
    {
        Debug.Log(option);
        switch (option)
        {
            case 0:
                option0Button.image.sprite = On;
                option1Button.image.sprite = Off;
                option2Button.image.sprite = Off;
                break;
            case 1:
                option1Button.image.sprite = On;
                option0Button.image.sprite = Off;
                option2Button.image.sprite = Off;
                break;
            case 2:
                option2Button.image.sprite = On;
                option0Button.image.sprite = Off;
                option1Button.image.sprite = Off;
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        option0Button.image.sprite = On;
        option1Button.image.sprite = Off;
        option2Button.image.sprite = Off;
    }
}