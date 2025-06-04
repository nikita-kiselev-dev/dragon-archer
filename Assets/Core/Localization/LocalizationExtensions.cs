using Cysharp.Threading.Tasks;
using UnityEngine.Localization;

namespace Core.Localization
{
    public static class LocalizationExtensions
    {
        public static async UniTask<string> Localize(this string localizationKey)
        {
            var localizedString = new LocalizedString(LocalizationInfo.MainStringTableName, localizationKey);
            var localizedStringOperation = localizedString.GetLocalizedStringAsync().ToUniTask();
            return await localizedStringOperation;
        }
        
        public static async UniTask<string> Localize(this string localizationKey, string localizationTableKey)
        {
            var localizedString = new LocalizedString(localizationTableKey, localizationKey);
            var localizedStringOperation = localizedString.GetLocalizedStringAsync().ToUniTask();
            return await localizedStringOperation;
        }
    }
}