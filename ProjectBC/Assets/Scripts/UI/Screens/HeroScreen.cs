using System;

public class HeroScreen : MenuScreen
{
    public event Action<HeroInfo> HeroSelected;
    public event Action<HeroInfo> EquipmentUpdated;

    public void HeroSelect(int idx)
    {
        //HeroSelected?.Invoke();
    }
}
