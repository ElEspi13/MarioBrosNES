using System;

[Serializable]
public class GameStats
{
    public enum Dificulty { Easy, Normal, Hard };
    public int score = 0;       // Puntos
    public int coins = 0;       // Monedas
    public int lives = 3;       // Vidas
    public float time = 400f;   // Tiempo en segundos
    public Dificulty difficulty = Dificulty.Normal;

    public void ResetGameStats()
    {
        score = 0;
        coins = 0;
        time = 400f;
        
        switch (difficulty)
        {
            case Dificulty.Easy:
                lives = 10;
                break;
            case Dificulty.Normal:
                lives = 3;
                break;
            case Dificulty.Hard:
                lives = 1;
                break;
        }
    }
}