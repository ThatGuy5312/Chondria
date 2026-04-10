using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Scene;

namespace Chondria.Core;

internal class CameraController(Camera camera)
{
    Camera camera = camera;

    float acceleration = 1f;
    float normalMoveSpeed = 1f;

    float moveSpeed = 1f;
    float mouseSensitivity = .2f;

    Vector3 euler;
    Vector2 lastMousePos;

    bool firstMouse = true;

    public void Update(float deltaTime)
    {
        var mousePos = Input.MousePosition;

        if (firstMouse)
        {
            lastMousePos = mousePos;
            firstMouse = false;
        }

        if (Input.IsMouseDown(1))
        {
            var delta = mousePos - lastMousePos;
            lastMousePos = mousePos;

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
}
