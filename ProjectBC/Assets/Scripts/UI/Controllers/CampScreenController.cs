using System;
using UnityEngine;
using UnityEngine.UI;

public class CampScreenController : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField] private Button portalBtn;
    [SerializeField] private Button portalBackBtn;
    [SerializeField] private GameObject campPortal;

    [Header("DailyStore")]
    [SerializeField] private Button dailyStoreBtn;
    [SerializeField] private Button dailyStoreBackBtn;
    [SerializeField] private GameObject dailyStorePanel;

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
        portalBtn.onClick.AddListener(() => campPortal.SetActive(true));
        portalBackBtn.onClick.AddListener(() => campPortal.SetActive(false));

        dailyStoreBtn.onClick.AddListener(() => { dailyStorePanel.SetActive(true); OnDailyStoreOpened?.Invoke(); });
        dailyStoreBackBtn.onClick.AddListener(() => { dailyStorePanel.SetActive(false); OnDailyStoreClosed?.Invoke(); });
    }
}
