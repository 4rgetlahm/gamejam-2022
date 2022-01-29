using System.Collections;
using System.Collections.Generic;
using System;

public class CoinToss
{
    public Outcome TossCoin(int winProb){
        var rand = new Random();
        var probability = rand.Next(1, 100);
        if(winProb > probability){
            return Outcome.Lose;
        }
        return Outcome.Win;
    }
}