using UnityEngine;
using UnityEngine.UI;

public class ColorLerpOnClick : MonoBehaviour
{
    public RawImage rawImageToModify;
    public float lerpDuration = 1.0f;
    public Color initialColor;
    public Color targetColor;
    public Color originalColor;
    private bool isLerping = false;
    private float lerpStartTime;

    private void Start()
    {
        initialColor = rawImageToModify.color;
        targetColor = initialColor;
        targetColor.a = 1.0f; // Set the target alpha to 1.0f (255 in the 0-1 range).
        originalColor = targetColor;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Start lerping when the mouse button is pressed down.
            StartLerp();
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Start lerping back to the initial color when the mouse button is released.
            StartLerpBack();
        }

        if (isLerping)
        {
            // Calculate the lerp progress.
            float lerpProgress = (Time.time - lerpStartTime) / lerpDuration;

            // Lerping the color.
            rawImageToModify.color = Color.Lerp(initialColor, targetColor, lerpProgress);

            // If the lerp is complete, stop lerping.
            if (lerpProgress >= 1.0f)
            {
                isLerping = false;
                targetColor = originalColor;
            }
        }
    }

    private void StartLerp()
    {
        isLerping = true;
        lerpStartTime = Time.time;
        //targetColor = originalColor;
    }

    private void StartLerpBack()
    {
        initialColor = rawImageToModify.color; // Set the initial color to the current color.
        targetColor = initialColor;
        targetColor.a = 0.235f; // Set the target alpha to the desired value (e.g., 60 in the 0-1 range).
        StartLerp();
    }
}
