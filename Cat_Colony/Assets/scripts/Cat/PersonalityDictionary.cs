using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// the personality structure, will contain the type (tough, alert, ect.) 
// eventually contain how much stat bonus from the personality
public struct Personality
{
    public PersonalityType type;
    public float chance;
}

// Enum is organized below
// Category name    Indexes
// Strength         0-1
// Agile            2
// Cute             3
public enum PersonalityType
{
    Tough,
    Brave,
    Alert,
    Derpy
}

// three dictionaries that contain all of the different type of personalities
// each has a const float that is the percent chance that it will be chosen
public class PersonalityDictionary {

    // the strength dictionary
    // Organize least chance to greatest chance, in order for recursion to work
    public const float STRENGTH_CHANCE = 1f;
    public static Dictionary<PersonalityType, Personality> Strength = new Dictionary<PersonalityType, Personality>()
    {
        {PersonalityType.Tough, new Personality { type = PersonalityType.Tough, chance = 0.5f} },
        {PersonalityType.Brave, new Personality { type = PersonalityType.Brave, chance = 1f} }
    };

    // the agile dictionary
    public const float AGILE_CHANCE = 0.5f;
    public static Dictionary<PersonalityType, Personality> Agile = new Dictionary<PersonalityType, Personality>()
    {
        {PersonalityType.Alert, new Personality { type = PersonalityType.Alert, chance = 1} }
    };

    // the cute dictionary
    public const float CUTE_CHANCE = 0.1f;
    public static Dictionary<PersonalityType, Personality> Cute = new Dictionary<PersonalityType, Personality>()
    {
        {PersonalityType.Derpy, new Personality { type = PersonalityType.Derpy, chance = 1} }
    };
}
