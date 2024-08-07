using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUI : Attribute
{
    [Header("Buttons")]
    [SerializeField] private Button property1Button;
    [SerializeField] private Button property2Button;
    [SerializeField] private Button property3Button;
    [SerializeField] private Button property4Button;
    [Header("Golds")]
    [SerializeField] private TextMeshProUGUI property1Gold;
    [SerializeField] private TextMeshProUGUI property2Gold;
    [SerializeField] private TextMeshProUGUI property3Gold;
    [SerializeField] private TextMeshProUGUI property4Gold;
    [Header("Exps")]
    [SerializeField] private TextMeshProUGUI property1Exp;
    [SerializeField] private TextMeshProUGUI property2Exp;
    [SerializeField] private TextMeshProUGUI property3Exp;
    [SerializeField] private TextMeshProUGUI property4Exp;
    [Header("MyGold")]
    [SerializeField] private TextMeshProUGUI myGold;



    private void Awake()
    {
        property1Button.onClick.AddListener(HpUpButtonClicked);
        property2Button.onClick.AddListener(StrengthUpButtonClicked);
        property3Button.onClick.AddListener(AgilltyUpButtonClicked);
        property4Button.onClick.AddListener(IntelligenceUpButtonClicked);
    }

    private void OnEnable()
    {
        GameDataManager.FundsUpdated += MyGoldUpdate;
    }
    private void OnDisable()
    {
        GameDataManager.FundsUpdated -= MyGoldUpdate;
    }

    private void MyGoldUpdate(PlayerInfo info)
    {
        myGold.text = info.gold.ToString();
    }
}
