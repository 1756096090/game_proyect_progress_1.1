using System;
using UnityEngine;

enum AttackKeys
{
    // Second Player Attacks
    M,
    N,
    L,
    // First Player Attacks
    Z,
    X,
    C
}

static class AttackKeysExtensions
{
    public static KeyCode ToKey(this AttackKeys attackKey)
    {
        switch (attackKey)
        {
            // Second Player Attacks
            case AttackKeys.M: return KeyCode.M;
            case AttackKeys.N: return KeyCode.N;
            case AttackKeys.L: return KeyCode.L;
            // First Player Attacks
            case AttackKeys.Z: return KeyCode.Z;
            case AttackKeys.X: return KeyCode.X;
            case AttackKeys.C: return KeyCode.C;
            default: throw new ArgumentOutOfRangeException("Attack Key does not exists");
        }
    }

    public static int Index(this AttackKeys attackKey)
    {
        switch (attackKey)
        {
            // Second Player Attacks
            case AttackKeys.M: return 1;
            case AttackKeys.N: return 2;
            case AttackKeys.L: return 3;
            // First Player Attacks
            case AttackKeys.Z: return 1;
            case AttackKeys.X: return 2;
            case AttackKeys.C: return 3;
            default: throw new ArgumentOutOfRangeException("Attack Key does not exists");
        }
    }
}