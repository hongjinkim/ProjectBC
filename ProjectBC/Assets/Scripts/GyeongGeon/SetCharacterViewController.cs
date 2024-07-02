using UnityEngine;
using UnityEngine.UI;

public class SetCharacterViewController : MonoBehaviour
{
    private Character character;

    public GameObject canvas;
    public GameObject prefabHpBar;

    RectTransform hpBar;
    Image currentHpBarAmount;

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
    }

    public void InitHpBar()
    {
        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
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

}
