using UnityEngine;

namespace Core.Utilities.Scripts
{
    public class ExternalSourcesController
    {
        private static ExternalSourcesController _instance;
        public static ExternalSourcesController Instance => _instance ??= new ExternalSourcesController();
        public void OpenPrivacyPolicy()
        {
            Application.OpenURL(ExternalSourcesInfo.PrivacyPolicyWebSite);
        }
    }
}