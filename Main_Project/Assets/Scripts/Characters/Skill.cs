using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
 
    /// <summary>
    /// Activates the skill
    /// </summary>
    public virtual void ActivateSkill() { }

    /// <summary>
    /// Turns off the skill
    /// </summary>
    public virtual void DeactivateSkill() { }
        
}
