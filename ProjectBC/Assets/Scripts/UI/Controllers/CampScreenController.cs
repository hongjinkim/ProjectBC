using System;
using UnityEngine;
using UnityEngine.UI;

public class CampScreenController : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField] private Button portalBtn;
    [SerializeField] private Button portalBackBtn;
    [SerializeField] private GameObject campScreen_Portal;

    [Header("DailyStore")]
    [SerializeField] private Button dailyStoreBtn;
    [SerializeField] private Button dailyStoreBackBtn;
    [SerializeField] private GameObject campScreen_DailyStore;

    [Header("Forge")]
    [SerializeField] private Button forgeBtn;

    [Header("DailyQuest")]
    [SerializeField] private Button dailyQuestBtn;

    [Header("PvP")]
    [SerializeField] private Button PvpBtn;

    [Header("Stone")]
    [SerializeField] private Button StoneBtn;

    [Header("DailyDungeon")]
    [SerializeField] private Button DailyDungeonBtn;

    [SerializeField] private PlayerInfoBar playerInfoBar;
    private event Action OnDailyStoreOpened;
    private event Action OnDailyStoreClosed;

    private void Awake()
    {
        OnDailyStoreOpened += playerInfoBar.ShowOnlyFunds;
        OnDailyStoreClosed += playerInfoBar.ShowPlayerInfo;
    }

    private void Start()
    {
        portalBtn.onClick.AddListener(() => campScreen_Portal.SetActive(true));
        portalBackBtn.onClick.AddListener(() => campScreen_Portal.SetActive(false));

        dailyStoreBtn.onClick.AddListener(() => { campScreen_DailyStore.SetActive(true); OnDailyStoreOpened?.Invoke(); });
        dailyStoreBackBtn.onClick.AddListener(() => { campScreen_DailyStore.SetActive(false); OnDailyStoreClosed?.Invoke(); });

        forgeBtn.onClick.AddListener(() => ToastMsg.Instance.ShowMessage("개발중입니다", 0.5f));
        dailyQuestBtn.onClick.AddListener(() => ToastMsg.Instance.ShowMessage("개발중입니다", 0.5f));
        PvpBtn.onClick.AddListener(() => ToastMsg.Instance.ShowMessage("개발중입니다", 0.5f));
        StoneBtn.onClick.AddListener(() => ToastMsg.Instance.ShowMessage("개발중입니다", 0.5f));
        DailyDungeonBtn.onClick.AddListener(() => ToastMsg.Instance.ShowMessage("개발중입니다", 0.5f));
    }
}
