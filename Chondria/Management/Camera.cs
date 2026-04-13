using OpenTK.Mathematics;

namespace Chondria.Management;

// simple camera class that holds position and projection info.
public class Camera(Math.Vector3 position, float aspectRatio)
{
    public Math.Vector3 Position = position;

    public Math.Quaternion Rotation = Math.Quaternion.Identity;

    public Math.Vector3 Forward => Vector3.Transform(-Vector3.UnitZ, Rotation);
    public Math.Vector3 Right => Vector3.Normalize(Vector3.Cross(Forward, Up));
    public Math.Vector3 Up => Vector3.UnitY;//Vector3.Normalize(Vector3.Cross(Right, Forward));

    public float FOV = 60f;
    public float AspectRatio = aspectRatio;
    public float Near = 0.1f;
    public float Far = 100f;

    public Matrix4 View =>
        Matrix4.CreateTranslation(-Position) *
        Matrix4.CreateFromQuaternion(Quaternion.Invert(Rotation));

    public Matrix4 Projection => 
        Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), AspectRatio, Near, Far);
}
