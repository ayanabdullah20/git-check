using UnityEngine;
using UnityEngine.InputSystem;

public class mover : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] ParticleSystem mainbooster;
    [SerializeField] ParticleSystem leftbooster;
    [SerializeField] ParticleSystem rightbooster;
    AudioSource rs;
    Rigidbody rb;

    [SerializeField] AudioClip mainengine;
    [SerializeField] int thrustforce;
    [SerializeField] int rotationforce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rs = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
    private void FixedUpdate()
    {
        {
            thrustapplier();
            processrotation();

        }
    }

    private void thrustapplier()
    {
        if (thrust.IsPressed())
        {

            rb.AddRelativeForce(Vector3.up * thrustforce * Time.fixedDeltaTime);
            mainbooster.Play();
            if (!rs.isPlaying)
            {
                rs.PlayOneShot(mainengine);
            }
        }
        else if (!thrust.IsPressed())
        {
            mainbooster.Stop();
            rs.Stop();
        }
    }
    private void processrotation()
    {
        float rotationinput = rotation.ReadValue<float>();
        if (rotationinput < 0)
        {
            leftbooster.Play();
            rb.freezeRotation = true;
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationforce);
            rb.freezeRotation = false;
        }
        else if (rotationinput > 0)
        {
            rightbooster.Play();
            rb.freezeRotation = true;
            transform.Rotate(-Vector3.forward * Time.deltaTime * rotationforce);
            rb.freezeRotation = false;
        }
        else if (rotationinput == 0)
        {
            rightbooster.Stop();
            leftbooster.Stop();
        }
    }
}