using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgeChar { PREHISTORY = 0, ORIENTAL, VICTORIAN, FUTURE }

public abstract class Skill : MonoBehaviour {

    /// <summary>
    /// Activates the skill
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Turns off the skill
    /// </summary>
    public abstract void Deactivate();
        
}
