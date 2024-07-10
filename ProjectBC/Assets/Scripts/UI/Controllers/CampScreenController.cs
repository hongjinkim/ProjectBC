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

    [SerializeField] private PlayerInfoBar playerInfoBar;
    private event Action OnDailyStoreOpened;
    private event Action OnDailyStoreClosed;

    private void Awake()
    {
        OnDailyStoreOpened += playerInfoBar.HideMenu;
        OnDailyStoreClosed += playerInfoBar.ShowMenu;
    }

    private void Start()
    {
        portalBtn.onClick.AddListener(() => campScreen_Portal.SetActive(true));
        portalBackBtn.onClick.AddListener(() => campScreen_Portal.SetActive(false));

        dailyStoreBtn.onClick.AddListener(() => { campScreen_DailyStore.SetActive(true); OnDailyStoreOpened?.Invoke(); });
        dailyStoreBackBtn.onClick.AddListener(() => { campScreen_DailyStore.SetActive(false); OnDailyStoreClosed?.Invoke(); });
    }
}
