using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
//
// code based on "How to Add Swimming to Your Unity VR Game" by Justin P Barnett
// https://www.youtube.com/watch?v=ViQzKZvYdgE&t=320s
//
{
    [Header("Values")]
    [SerializeField] float swimForce = 2f;
    [SerializeField] float dragForce = 1f;
    [SerializeField] float minForce = 0f;
    [SerializeField] float minTimeBetweenStrokes;
    [SerializeField] float minTimeBetweenReports = 5;
    [Header("References")]
    [SerializeField] InputActionReference leftControllerSwimReference;
    [SerializeField] InputActionReference leftControllerVelocity;
    [SerializeField] InputActionReference rightControllerSwimReference;
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] Transform trackingdReference; // (XR Origin, where head is pointing)

    Rigidbody _rigidbody;
    float _cooldownTimer; // help us calculate time between strokes
    float _reportTimer; // delay time between log reports

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation; // avoid rotation to prevent nausea
    }
    // we use because we are using physics
    // both grips have to be pressed for this to work
    void FixedUpdate()
    {
        _cooldownTimer += Time.fixedDeltaTime;
        if (_cooldownTimer > minTimeBetweenStrokes
            && leftControllerSwimReference.action.IsPressed()
            && rightControllerSwimReference.action.IsPressed())
        {
            var leftHandVelocity = leftControllerVelocity.action.ReadValue<Vector3>();
            var rightHandVelocity = rightControllerVelocity.action.ReadValue<Vector3>();
            Vector3 localVelocity = leftHandVelocity + rightHandVelocity;
            localVelocity *= -1; // force is opposite our hand/controller gesture for a nautural swim experience

            if (localVelocity.sqrMagnitude > minForce * minForce) // we use sqrMagnitude for better performance
                // and we then square the force because we need you need to perform things on both sides of the equation
            {
                // APPLY the force 
                Vector3 worldVelocity = trackingdReference.TransformDirection(localVelocity);
                _rigidbody.AddForce(worldVelocity * swimForce, ForceMode.Acceleration); // rigidbody requires world velocity
                _cooldownTimer = 0f;
                _reportTimer += Time.fixedDeltaTime;
                if (_reportTimer > minTimeBetweenReports)
                {
                    Debug.Log("Swimmer: worldVelocity=" + worldVelocity + " swimForce=" + swimForce);
                    _reportTimer = 0f;
                }
            }
        }
        // apply opposite force to implement slowdown when not actively swimming
        if (_rigidbody.velocity.sqrMagnitude > 0.01f)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration); // we need drag so we slow down without movement
        }


    }
}
