using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Management;

namespace Chondria.Core;

// the controller for scene camera movement
internal class CameraController(Camera camera)
{
    Camera camera = camera;

    public enum CameraMode { Free, Orbit }

    public CameraMode Mode = CameraMode.Free;

    float acceleration = 1f;
    float normalMoveSpeed = 1f;

    float moveSpeed = 1f;
    float mouseSensitivity = .2f;

    Vector3 euler;
    Vector2 lastMousePos;

    bool firstMouse = true;

    Vector3 Target = Vector3.Zero;
    float Distance = 5f;

    public void Update(float deltaTime)
    {
        if (Mode == CameraMode.Free)
            HandleFreeCamera(deltaTime);
        else
            HandleOrbitalCamera(deltaTime);

        if (Input.WasKeyPressed(Key.F))
        {
            Mode = Mode == CameraMode.Free ? CameraMode.Orbit : CameraMode.Free;
        }
    }

    void HandleFreeCamera(float deltaTime)
    {
        var mousePos = Input.MousePosition;

        var delta = mousePos - lastMousePos;
        lastMousePos = mousePos;

        if (firstMouse)
        {
            lastMousePos = mousePos;
            firstMouse = false;
        }

        if (Input.IsMouseDown(1))
        {
            euler.X -= delta.X * mouseSensitivity;
            euler.Y -= delta.Y * mouseSensitivity;

            camera.Rotation = Quaternion.FromEulerAngles(euler);
        }
        else
        {
            firstMouse = true;
        }

        // camera movement (WASDQE)
        if (Input.IsKeyDown(Key.W))
            camera.Position += camera.Forward * moveSpeed * deltaTime;
        if (Input.IsKeyDown(Key.S))
            camera.Position -= camera.Forward * moveSpeed * deltaTime;
        if (Input.IsKeyDown(Key.A))
            camera.Position -= camera.Right * moveSpeed * deltaTime;
        if (Input.IsKeyDown(Key.D))
            camera.Position += camera.Right * moveSpeed * deltaTime;
        if (Input.IsKeyDown(Key.E))
            camera.Position += camera.Up * moveSpeed * deltaTime;
        if (Input.IsKeyDown(Key.Q))
            camera.Position -= camera.Up * moveSpeed * deltaTime;

        if (Input.IsKeyDown(Key.LeftShift))
            moveSpeed += acceleration * deltaTime;
        else
            moveSpeed = normalMoveSpeed;
    }

    void HandleOrbitalCamera(float deltaTime)
    {
        var mousePos = Input.MousePosition;

        if (firstMouse)
        {
            lastMousePos = mousePos;
            firstMouse = false;
        }

        var delta = mousePos - lastMousePos;
        lastMousePos = mousePos;

        // orbitial rotation
        if (Input.IsMouseDown(2) && !Input.IsKeyDown(Key.LeftShift))
        {
            euler.X -= delta.X * mouseSensitivity;
            euler.Y -= delta.Y * mouseSensitivity;
        }

        //if (justRotate)
            //return;

        // panning
        if (Input.IsMouseDown(2) && Input.IsKeyDown(Key.LeftShift))
        {
            float panSpeed = Distance * 0.002f;

            Vector3 right = Vector3.Normalize(Vector3.Cross(Target - camera.Position, Vector3.UnitY));
            Vector3 up = Vector3.Normalize(Vector3.Cross(right, Target - camera.Position));

            Target -= right * delta.X * panSpeed;
            Target += up * delta.Y * panSpeed;
        }

        // zooming
        if (Input.ScrollDelta.Y != 0)
        {
            Distance *= (1f - Input.ScrollDelta.Y * 0.1f);
            Distance = Mathf.Clamp(Distance, 0.1f, 500f);
        }

        camera.Rotation = Quaternion.FromEulerAngles(euler);
        camera.Position = Target - camera.Forward * Distance;
    }
}
