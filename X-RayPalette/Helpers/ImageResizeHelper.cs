namespace X_RayPalette.Helpers;

using System.Numerics;

public class ImageResizer
{
    public static Vector2 ResizeImage(Vector2 originalSize, float desiredWidth, float desiredHeight)
    {
        // Original dimensions
        float originalWidth = originalSize.X;
        float originalHeight = originalSize.Y;

        // Calculate the aspect ratio
        float aspectRatio = originalWidth / originalHeight;

        // Initialize new dimensions
        float newWidth;
        float newHeight;

        // Determine which dimension to constrain by
        if (desiredWidth / aspectRatio <= desiredHeight)
        {
            // Constrain by width
            newWidth = desiredWidth;
            newHeight = desiredWidth / aspectRatio;
        }
        else
        {
            // Constrain by height
            newWidth = desiredHeight * aspectRatio;
            newHeight = desiredHeight;
        }

        // Return the new size as a Vector2
        return new Vector2(newWidth, newHeight);
    }
}
