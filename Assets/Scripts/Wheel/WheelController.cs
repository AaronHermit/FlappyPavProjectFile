using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WheelController : MonoBehaviour
{
    public RectTransform wheel; // Reference to the RectTransform of the UI wheel
    public Button spinButton; // Reference to the Spin button
    public float spinDuration = 3f; // Duration of the spin in seconds
    private bool isSpinning = false;

    void Start()
    {
        // Add the listener to the Spin button
        spinButton.onClick.AddListener(SpinWheel);
    }

    void SpinWheel()
    {
        // Ensure the wheel is not spinning already
        if (!isSpinning)
        {
            StartCoroutine(Spin());
        }
    }

    IEnumerator Spin()
    {
        isSpinning = true;  // Mark that the wheel is currently spinning
        float timeElapsed = 0f;  // Track how much time has passed
        float startRotation = wheel.localEulerAngles.z;  // Get the initial z-axis rotation of the wheel
        float endRotation = startRotation + Random.Range(720, 1440);  // Randomize between 2 and 4 full rotations (in degrees)

        // Debugging log to ensure we're starting with the right rotation
        Debug.Log("Starting Rotation: " + startRotation);

        while (timeElapsed < spinDuration)
        {
            // Interpolate the rotation of the wheel
            float angle = Mathf.Lerp(startRotation, endRotation, timeElapsed / spinDuration);
            wheel.localEulerAngles = new Vector3(0, 0, angle);  // Apply the new rotation around the z-axis
            timeElapsed += Time.deltaTime;  // Increment time by the time that has passed
            yield return null;  // Wait until the next frame
        }

        // Ensure the final rotation is accurate
        wheel.localEulerAngles = new Vector3(0, 0, endRotation % 360);

        // Debugging log to check the final rotation
        Debug.Log("Final Rotation: " + wheel.localEulerAngles.z);

        isSpinning = false;  // Mark that the wheel has stopped spinning
    }
}
