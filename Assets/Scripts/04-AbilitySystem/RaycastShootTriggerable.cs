using UnityEngine;
using System.Collections;

public class RaycastShootTriggerable : MonoBehaviour
{

    [HideInInspector] public int gunDamage = 1;
    // Set the number of hitpoints that this gun will take away from shot objects with a health script.
    [HideInInspector] public float weaponRange = 50f;
    // Distance in unity units over which the player can fire.
    [HideInInspector] public float hitForce = 100f;
    // Amount of force which will be added to objects with a rigidbody shot by the player.
    public Transform gunEnd;
    // Holds a reference to the gun end object, marking the muzzle location of the gun.
    [HideInInspector] public LineRenderer laserLine;
    // Reference to the LineRenderer component which will display our laserline.

    private Camera fpsCam;
    // Holds a reference to the first person camera.
    private WaitForSeconds shotDuration = new WaitForSeconds (.07f);
    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible.


    public void Initialize ()
    {
        //Get and store a reference to our LineRenderer component
        laserLine = GetComponent<LineRenderer> ();

        //Get and store a reference to our Camera
        fpsCam = GetComponentInParent<Camera> ();
    }

    public void Fire ()
    {

        //Create a vector at the center of our camera's near clip plane.
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (.5f, .5f, 0));

        //Draw a debug line which will show where our ray will eventually be
        Debug.DrawRay (rayOrigin, fpsCam.transform.forward * weaponRange, Color.green);

        //Declare a raycast hit to store information about what our raycast has hit.
        RaycastHit hit;

        //Start our ShotEffect coroutine to turn our laser line on and off
        StartCoroutine (ShotEffect ());

        //Set the start position for our visual effect for our laser to the position of gunEnd
        laserLine.SetPosition (0, gunEnd.position);

        //Check if our raycast has hit anything
        if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange)) {
            // Some code
        } else {
            //if we did not hit anything, set the end of the line to a position directly away from
            laserLine.SetPosition (1, fpsCam.transform.forward * weaponRange);
        }
    }

    private IEnumerator ShotEffect ()
    {

        //Turn on our line renderer
        laserLine.enabled = true;
        //Wait for .07 seconds
        yield return shotDuration;

        //Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}