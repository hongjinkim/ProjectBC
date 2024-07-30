using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSettingUI : MonoBehaviour
{
    public Image potionIcon;
    public Image potionBackground;
    public Image potionFrame;
    public TextMeshProUGUI potionCount;

    private HeroPotion heroPotion;

    private void Awake()
    {
        if (heroPotion != null)
            heroPotion = GetComponent<HeroPotion>();
    }

    private void Start()
    {

    }


}
