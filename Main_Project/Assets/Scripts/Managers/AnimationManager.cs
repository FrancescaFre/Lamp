using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationManager
{

//------------------------------------------------------------------------------
    public static float Anim_LenghtAnim(Animator anim, string s) {

        /*Animator m_Animator;
        string m_ClipName;
        AnimatorClipInfo[] m_CurrentClipInfo;

        float m_CurrentClipLength;
        //Get them_Animator, which you attach to the GameObject you intend to animate.
        m_Animator = trans.GetComponentInChildren<Animator>();
        //Fetch the current Animation clip information for the base layer
        m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
        //Access the Animation clip name
        m_ClipName = m_CurrentClipInfo[0].clip.name;
        Debug.Log("NAME " + m_ClipName);
        if (m_ClipName.Equals(s))
            //Access the current length of the clip
            return m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
        else
            return 0; 
        */
        
        float time = 0;
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            //Debug.Log(ac.animationClips[i].name);
            if (ac.animationClips[i].name.Equals(s))        //If it has the same name as your clip
            {
                time = ac.animationClips[i].length;
                //Debug.Log("time " + time + " name " + ac.animationClips[i].name);
            }
        }
        return time;
    }    
//------------------------------------------------------------------------------
    public static bool Anim_CheckPlay(Animator anim, string s)
    {
        return !anim.GetCurrentAnimatorStateInfo(0).IsName(s);   
    }
//------------------------------------------------------------------------------
    public static bool Anim_CheckBool(Animator anim, string s)
    {
        return anim.GetBool(s);
    }
 //------------------------------------------------------------------------------
    public static void Anim_StarDigging(Animator anim)
    {
        anim.SetTrigger("isDigging");
    } 
//------------------------------------------------------------------------------
    public static void Anim_OpenDoor(Animator anim)
    {
        anim.SetTrigger("OpenDoor");
        if (Anim_CheckBool(anim, "IsMovingStanding"))
            Anim_StopMovingStanding(anim);
        if (Anim_CheckBool(anim, "IsMovingSneaky"))
            Anim_StopMovingSneaky(anim);
    }
//------------------------------------------------------------------------------
    public static void Anim_StartMovingStanding(Animator anim)
    {
        anim.SetBool("IsMovingStanding", true);
    }
//------------------------------------------------------------------------------
    public static void Anim_StopMovingStanding(Animator anim)
    {
        anim.SetBool("IsMovingStanding", false);
    }
//------------------------------------------------------------------------------
    public static void Anim_StartMovingSneaky(Animator anim)
    {
        anim.SetBool("IsMovingSneaky", true);
    }
//------------------------------------------------------------------------------
    public static void Anim_StopMovingSneaky(Animator anim)
    {
        anim.SetBool("IsMovingSneaky", false);
    }
//------------------------------------------------------------------------------
    public static void Anim_StartMovingRunning(Animator anim)
    {
        anim.SetBool("IsMovingRunning", true);
    }
 //------------------------------------------------------------------------------
    public static void Anim_StopMovingRunning(Animator anim)
    {
        anim.SetBool("IsMovingRunning", false);
    }
}