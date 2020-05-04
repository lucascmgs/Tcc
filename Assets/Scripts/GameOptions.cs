namespace DefaultNamespace
{
    public static class GameOptions
    {
        public static Gamestate gameState = Gamestate.NotStarted;
        public static float gameTime = 120;
        public static bool playMusic = true;
        public static bool playSounds = true;
    }

    public enum Gamestate
    {
        SubOwn,
        PhoneOwn,
        NotStarted
    }
}