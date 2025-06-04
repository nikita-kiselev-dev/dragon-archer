using Core.SettingsPopup.Scripts.Data;
using Core.Tutorials.Data;
using MemoryPack;
using Project.Items.Gems.Scripts.Data;
using Project.Items.Gold.Scripts.Data;
using DailyBonusData = Core.DailyBonus.Scripts.Data.DailyBonusData;
using ItemData = Core.Items.Scripts.Data.ItemData;

namespace Core.SaveLoad
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(MainData))]
    [MemoryPackUnion(1, typeof(TutorialData))]
    [MemoryPackUnion(2, typeof(OnboardingTutorialData))]
    [MemoryPackUnion(3, typeof(ItemData))]
    [MemoryPackUnion(4, typeof(GoldData))]
    [MemoryPackUnion(5, typeof(GemsData))]
    [MemoryPackUnion(6, typeof(SettingsPopupData))]
    [MemoryPackUnion(7, typeof(DailyBonusData))]
    public abstract partial class Data
    {
        public string Name()
        {
            return GetType().Name;
        }

        public abstract void PrepareNewData();
    }
}