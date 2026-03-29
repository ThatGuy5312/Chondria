using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Scene;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Chondria.Core;

internal class CameraController(Camera camera)
{
    public void Update()
    {
        // Example camera movement (WASD)
        if (Input.IsKeyDown(Keys.W))
            camera.Position -= Vector3.UnitZ * .001f;
        if (Input.IsKeyDown(Keys.S))
            camera.Position += Vector3.UnitZ * .001f;
        if (Input.IsKeyDown(Keys.A))
            camera.Position -= Vector3.UnitX * .001f;
        if (Input.IsKeyDown(Keys.D))
            camera.Position += Vector3.UnitX * .001f;

        if (Input.IsKeyDown(Keys.E))
            camera.Position += Vector3.UnitY * .001f;
        if (Input.IsKeyDown(Keys.Q))
            camera.Position -= Vector3.UnitY * .001f;
    }
}
