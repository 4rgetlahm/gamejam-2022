using System.Collections;
using System.Collections.Generic;
using System;

public class CoinToss
{
    public Outcome TossCoin(int winProb){
        var rand = new Random();
        var probability = rand.Next(0, 100);
        if(probability <= winProb){
            return Outcome.Win;
        }
        return Outcome.Lose;
    }
}