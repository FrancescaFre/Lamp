using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ParticlesOverSphere : MonoBehaviour {

    public ParticleSystem ps;
    
	// Use this for initialization
	void Start () {

        ps = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
     // ps.Emit(new ParticleSystem.EmitParams() { position = Random.onUnitSphere*21}, 1);

	}
}
