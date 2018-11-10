using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    public enum AgeChar { PREHISTORY = 0, ORIENTAL, VICTORIAN, FUTURE }
    private AgeChar typeOfCharacter;

    public Dictionary<AgeChar, string> Skills = new Dictionary<AgeChar, string>();
    private PlayerController pc;

    private Robot robot;
    private Mimic mimic;
    private Painter painter;

    private void Start()
    {
        Skills.Add(AgeChar.PREHISTORY, "ActiveDash");
        Skills.Add(AgeChar.ORIENTAL, "ActiveMimic");
        Skills.Add(AgeChar.VICTORIAN, "ActivePainter");
        Skills.Add(AgeChar.FUTURE, "ActiveRobot");

        pc = GetComponent<PlayerController>();
        //get enum from the pc when the enum will be in the playercontroller
        typeOfCharacter = AgeChar.FUTURE;

        robot = GetComponentInChildren<Robot>();
        mimic = GetComponent<Mimic>();
        painter = GetComponent<Painter>();
    }

    public void ActiveSkill()
    {
        if (robot)
            robot.ActivateSkill();
        else if (mimic)
        else if (painter)

    }
    

    public void DectivateSkill() {

    }


}
