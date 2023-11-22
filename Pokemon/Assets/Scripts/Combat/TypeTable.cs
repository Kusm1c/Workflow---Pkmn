using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeTable
{
    private static float[,] resistanceTable = {
        //AttackerNormal
        {
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, .5f, 0.0f, 1.0f
        },
        //AttackerFire
        {
            1.0f, .5f, .5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, .5f, 1.0f, .5f
        },
        //AttackerWater
        {
            1.0f, 2.0f, .5f, .5f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, .5f
        },
        //AttackerPlant
        {
            1.0f, .5f, 2.0f, .5f, 1.0f, 1.0f, 1.0f, .5f, 2.0f, .5f, 1.0f, .5f, 2.0f, 1.0f, .5f
        },
        //AttackerElectric
        {
            1.0f, 1.0f, 2.0f, .5f, .5f, 1.0f, 1.0f, 1.0f, 0.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, .5f
        },
        //AttackerIce
        {
            1.0f, 1.0f, .5f, 2.0f, 1.0f, .5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f
        },
        //AttackerFight
        {
            2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, .5f, 1.0f, .5f, .5f, .5f, 2.0f, 0.0f, 1.0f
        },
        //AttackerPoison
        {
            1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, .5f, .5f, 1.0f, 1.0f, 2.0f, .5f, .5f, 1.0f
        },
        //AttackerGround
        {
            1.0f, 2.0f, 1.0f, .5f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.0f, 1.0f, .5f, 2.0f, 1.0f, 1.0f
        },
        //AttackerFlight
        {
            1.0f, 1.0f, 1.0f, 2.0f, .5f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, .5f, 1.0f, 1.0f
        },
        //AttackerPsy
        {
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, .5f, 1.0f, 1.0f, 1.0f, 1.0f
        },
        //AttackerInsect
        {
            1.0f, .5f, 1.0f, 2.0f, 1.0f, 1.0f, .5f, 2.0f, 1.0f, .5f, 2.0f, 1.0f, 1.0f, .5f, 1.0f
        },
        //AttackerRock
        {
            1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, .5f, 1.0f, .5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f
        },
        //AttackerSpectre
        {
            0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 2.0f, 1.0f
        },
        //AttackerDragon
        {
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f
        }
    };

    public static float GetTypeDamageMultiplier(PokemonType attackerType, PokemonType defenderType)
    {
        return resistanceTable[(int) attackerType, (int) defenderType];
    }
}
