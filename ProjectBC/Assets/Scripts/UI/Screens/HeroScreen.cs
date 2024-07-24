using System;

public class HeroScreen : MenuScreen
{
    public static event Action<HeroInfo> HeroSelected;
    public event Action<HeroInfo> EquipmentUpdated;

    public void HeroSelect(HeroInfo info)
    {
        HeroSelected?.Invoke(info);
    }
}
