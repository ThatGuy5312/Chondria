using Chondria.Math;
using OpenTK.Mathematics;

namespace Chondria.Scene;

// simple camera class that holds position and projection info.
public class Camera
{
    public Math.Vector3 Position;

    public float FOV = 60f;
    public float AspectRatio;
    public float Near = 0.1f;
    public float Far = 100f;

    public Camera(Math.Vector3 position, float aspectRatio)
    {
        Position = position;
        AspectRatio = aspectRatio;
    }

    public Matrix4 GetViewMatrix()
    {
        // Move the world opposite of the camera
        return Matrix4.CreateTranslation(-Position);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), AspectRatio, Near, Far);
    }
}
