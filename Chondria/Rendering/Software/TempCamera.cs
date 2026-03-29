using Assimp;
using Chondria.InputSystem;
using Chondria.Math;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Chondria.Rendering.Software
{
    public class TempCamera
    {
        public Vector3 Position;
        public float Yaw;
        public float Pitch;

        public Vector3 WorldToCamera(Vector3 point)
        {
            // move world relative to camera
            Vector3 v = point - Position;

            // rotate opposite of camera rotation
            v = Mathf.RotateY(v, -Yaw);
            v = Mathf.RotateX(v, -Pitch);

            return v;
        }

        public Vector3 Forward()
        {
            float cosY = (float)System.Math.Cos(Yaw);
            float sinY = (float)System.Math.Sin(Yaw);

            return new(sinY, 0, cosY);
        }

        public Vector3 Right()
        {
            float cosY = (float)System.Math.Cos(Yaw);
            float sinY = (float)System.Math.Sin(Yaw);

            return new(cosY, 0, -sinY);
        }

        public void Traverse()
        {
            float speed = 0.1f;

            if (Input.IsKeyDown(Keys.W)) Position += Forward() * speed;
            if (Input.IsKeyDown(Keys.S)) Position -= Forward() * speed;
            if (Input.IsKeyDown(Keys.A)) Position -= Right() * speed;
            if (Input.IsKeyDown(Keys.D)) Position += Right() * speed;
        }
    }
}
