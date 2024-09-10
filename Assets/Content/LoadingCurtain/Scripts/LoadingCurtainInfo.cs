namespace Content.LoadingCurtain.Scripts
{
    public static class LoadingCurtainInfo
    {
        public const float FadeInAnimationDuration = 0.4f;
        public const float ColorChangeShowAnimationDuration = 0.9f;
        public const float ShowAnimationDuration = FadeInAnimationDuration + ColorChangeShowAnimationDuration;

        public const float FadeOutAnimationDuration = 0.7f;
        public const float ColorChangeHideAnimationDuration = 1.3f;
        public const float HideAnimationDuration = FadeOutAnimationDuration + ColorChangeHideAnimationDuration;
        
        public const float AddDotAnimationDelay = 0.3f;
        public const int DotsToAddInAnimation = 3;

        public const int LoadingTimeoutInSeconds = 10;
    }
}