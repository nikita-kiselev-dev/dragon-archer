using Cysharp.Threading.Tasks;
using UnityEngine.Localization;

namespace Infrastructure.Service.Localization
{
    public static class LocalizationExtensions
    {
        public static string Localize(this string localizationKey)
        {
            var localizedString = new LocalizedString(LocalizationInfo.MainStringTableName, localizationKey);
            var localizedStringResult = localizedString.GetLocalizedString();
            return localizedStringResult;
        }
        
        public static async UniTask<string> LocalizeAsync(this string localizationKey)
        {
            var localizedString = new LocalizedString(LocalizationInfo.MainStringTableName, localizationKey);
            var localizedStringOperation = localizedString.GetLocalizedStringAsync().ToUniTask();
            return await localizedStringOperation;
        }
    }
}