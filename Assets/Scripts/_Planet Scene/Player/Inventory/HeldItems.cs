using System;

[System.Flags]
public enum HeldItems
{
    //allows iteam storage but a single pickup per iteam
    None = 0,
    Rock = 1 << 0,
    Pear = 1 << 1,
    Stick = 1 << 2,
    //add iteams and shift them accordignly
}