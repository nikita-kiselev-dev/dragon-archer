using System.Threading.Tasks;
using UnityEngine.Localization;

namespace Infrastructure.Service.Localization
{
    public static class LocalizationExtensions
    {
        public static async Task<string> LocalizeAsync(this string localizationKey)
        {
            var localizedString = new LocalizedString(LocalizationInfo.MainStringTableName, localizationKey);
            var localizedStringOperation = localizedString.GetLocalizedStringAsync();
            return await localizedStringOperation.Task;
        }
        
        public static  string Localize(this string localizationKey)
        {
            var localizedString = new LocalizedString(LocalizationInfo.MainStringTableName, localizationKey);
            var localizedStringOperation = localizedString.GetLocalizedStringAsync();
            return localizedStringOperation.Result;
        }
    }
}