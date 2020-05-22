using UnityEngine;

namespace DefaultNamespace
{
    public static class GameOptions
    {
        public static Gamestate gameState = Gamestate.NotStarted;
        
        public static float GameTime
        {
            get
            {
                return PlayerPrefs.GetFloat("GameTime", 100);
            } set
            {
                PlayerPrefs.SetFloat("GameTime", value);
            }
        }

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