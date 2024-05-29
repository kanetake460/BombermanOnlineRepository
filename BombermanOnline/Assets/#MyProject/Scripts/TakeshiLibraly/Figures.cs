using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Figures
{
    private const int _zero = 0;
    private const int _one = 1;
    private const int _two = 2;

    /// <summary>0</summary>
    public static int Zero              => _zero;

    /// <summary>1</summary>
    public static int One               => _one;

    /// <summary>2</summary>
    public static int Two               => _two;

    /// <summary>4</summary>
    public static int TwoSquared        => _two * _two;

    /// <summary>8</summary>
    public static int TwoCubed          => _two * _two * _two;

    /// <summary>16</summary>
    public static int TwoPowerdByFour   => _two * _two * _two * _two;

    /// <summary>32</summary>
    public static int TwoPowerdByFive   => _two * _two * _two * _two * _two;

    /// <summary>62</summary>
    public static int TwoPowerdBySix    => _two * _two * _two * _two * _two * _two;
}
