using System;
using Infrastructure.Service.SaveLoad;
using Newtonsoft.Json;

namespace Infrastructure.Game.Tutorials.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class OnboardingTutorialData : TutorialData
    {
    }
}