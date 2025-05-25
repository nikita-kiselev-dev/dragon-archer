using Content.DailyBonus.Scripts.Data;
using Content.Items.Gems.Scripts.Data;
using Content.Items.Gold.Scripts.Data;
using Content.SettingsPopup.Scripts.Data;
using Infrastructure.Game.Data;
using Infrastructure.Game.Tutorials.Data;
using MemoryPack;
using ItemData = Content.Items.Common.Scripts.Data.ItemData;

namespace Infrastructure.Service.SaveLoad
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