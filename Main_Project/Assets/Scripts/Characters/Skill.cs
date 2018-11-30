using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    public PlayerController pc;
    public virtual void Awake() {
        pc = GetComponentInParent<PlayerController>();
    }
    /// <summary>
    /// Activates the skill
    /// </summary>
    public virtual void ActivateSkill() { }

    /// <summary>
    /// Turns off the skill
    /// </summary>
    public virtual void DeactivateSkill() { }
        
}
