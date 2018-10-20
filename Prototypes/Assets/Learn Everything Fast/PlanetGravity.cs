
using UnityEngine;

public class PlanetGravity : MonoBehaviour {

    public float gravity=-12f;  //gravity power

    /// <summary>
    /// Attracts the object toward the core of the planet
    /// </summary>
    /// <param name="transform"> The transform of the GameObject that needs to be attracted to the planet</param>
    public void Attract(Transform playerTransform) {

      

        // direction the rigidbody has to face
        Vector3 targetDir = (playerTransform.position - transform.position).normalized; //it is the direction between the body and the center of the planet 
        Vector3 bodyUp = playerTransform.up;   //rigidbody current direction
        
        playerTransform.GetComponent<Rigidbody>().AddForce(targetDir * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, targetDir) * playerTransform.rotation;
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, 50f * Time.deltaTime);
      
    }
}
