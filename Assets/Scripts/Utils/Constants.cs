public static class Constants
{
    public static class Grid
    {
        public const int Width = 5;
        public const int Height = 5;
    }

    public static class Scoring
    {
        public const int ScorePerLevel = 10;
    }

    public static class Pool
    {
        public const int ItemPoolInitialSize = 30;
    }

    public static class Item
    {
        public const int StartingLevel = 0;
    }

    public static class Text
    {
        public const string EmptyScore = "";
        public const string MaxLevelWarning = "Max level!";
        public const string GridFullWarning = "Grid is full!";
    }

    public static class Animation
    {
        public const float DragScale = 1.1f;
        public const int DragSortingOrder = 10;

        public const float TweenDuration = 0.2f;
        public const float SquishScale = 0.9f;
        public const float SquishDuration = 0.16f;

        public const float SpawnScaleDuration = 0.3f;

        public const float WarningFadeInDuration = 0.25f;
        public const float WarningFadeOutDuration = 0.5f;
        public const float WarningFloatDistance = 50f;
    }
    
}
