using UnityEngine;
using UnityEngine.UI;

public class SetCharacterViewController : MonoBehaviour
{
    private Character character;

    public GameObject canvas;

    public GameObject prefabHpBar;
    GameObject hpBarObj;
    RectTransform hpBar;
    Image currentHpBarAmount;

    public GameObject prefabEnergyBar;
    GameObject energyBarObj;
    RectTransform energyBar;
    Image currentEnergyBarAmount;

    float height = 1f;

    private void Awake() 
    {
        character = GetComponent<Character>();    
    }

    private void Start()
    {
        InitHpBar();
    }

    private void Update()
    {
        CalPosHpBar();
        ControlViewHpBar();
        ControlActive();
    }

    public void InitHpBar()
    {
        hpBarObj = Instantiate(prefabHpBar, GameManager.Instance.dungeonManager.canvas.transform);
        hpBar = hpBarObj.GetComponent<RectTransform>();
        currentHpBarAmount = hpBar.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void CalPosHpBar()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(character.transform.position.x, character.transform.position.y + height, 0));
        hpBar.position = _hpBarPos;
    }

    public void ControlViewHpBar()
    {
        currentHpBarAmount.fillAmount = (float)character.currentHealth / (float)character.maxHealth;
    }

    public void ControlActive()
    {
        if(currentHpBarAmount.fillAmount <= 0)
        {
            hpBarObj.SetActive(false);
        }
        else if (currentHpBarAmount.fillAmount > 0 && !hpBarObj.activeSelf)
        {
            hpBarObj.SetActive(true);
        }
    }

}
