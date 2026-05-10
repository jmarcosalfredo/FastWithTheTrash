using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    void Start()
    {
        UpdateSprite();
    }

    public void MuteBtn()
    {
        AudioManager.instance?.ToggleMute();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (AudioManager.instance == null) return;
        buttonImage.sprite = AudioManager.instance.IsMuted ? soundOffSprite : soundOnSprite;
    }
}