using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationManager
{
    public static bool Anim_CheckBool(Transform trans, string s)
    {
        return trans.GetComponent<Animator>().GetBool(s);
    }

    public static void Anim_StarDigging(Transform trans)
    {
        trans.GetComponent<Animator>().SetTrigger("isDigging");
    }
//------------------------------------------------------------------------------
    public static void Anim_StartMovingStanding(Transform trans)
    {
        trans.GetComponent<Animator>().SetBool("IsMovingStanding", true);
    }
//------------------------------------------------------------------------------
    public static void Anim_StopMovingStanding(Transform trans)
    {
        trans.GetComponent<Animator>().SetBool("IsMovingStanding", false);
    }
//------------------------------------------------------------------------------
    public static void Anim_StartMovingSneaky(Transform trans)
    {
        trans.GetComponent<Animator>().SetBool("IsMovingSneaky", true);
    }
//------------------------------------------------------------------------------
    public static void Anim_StopMovingSneaky(Transform trans)
    {
        trans.GetComponent<Animator>().SetBool("IsMovingSneaky", false);
    }
}