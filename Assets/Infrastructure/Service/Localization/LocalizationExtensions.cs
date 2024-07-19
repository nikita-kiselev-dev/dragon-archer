using UnityEngine.Localization;

namespace Infrastructure.Service.Localization
{
    public static class LocalizationExtensions
    {
        public static string Localize(this string localizationKey)
        {
            var localizedString = new LocalizedString(LocalizationInfo.MainStringTableName, localizationKey).GetLocalizedString();
            return localizedString;
        }
    }
}