using Chondria.Math;
using Chondria.Rendering.Drawers;

namespace Chondria.Rendering.Software
{
    // test drawer/pipeline
    public class TestPatternDrawer : IDrawer
    {
        public void Draw(PixelBuffer buffer)
        {
            buffer.Clear(0, 0, 0, 255);

            camera.Traverse();

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3 v0 = cube[indices[i]];
                Vector3 v1 = cube[indices[i + 1]];
                Vector3 v2 = cube[indices[i + 2]];

                Vector3 center = new(0, 0, 3);

                v0 += center;
                v1 += center;
                v2 += center;

                v0 = camera.WorldToCamera(v0);
                v1 = camera.WorldToCamera(v1);
                v2 = camera.WorldToCamera(v2);

                //float time = (float)DateTime.Now.TimeOfDay.TotalSeconds;

                //v0 = Mathf.RotateY(v0, time);
                //v1 = Mathf.RotateY(v1, time);
                //v2 = Mathf.RotateY(v2, time);

                Vector2 p0 = buffer.Project(v0, 100, buffer.Width, buffer.Height);
                Vector2 p1 = buffer.Project(v1, 100, buffer.Width, buffer.Height);
                Vector2 p2 = buffer.Project(v2, 100, buffer.Width, buffer.Height);

                buffer.FillTriangle(p0, p1, p2, 255, 255, 255, 255);
            }
        }

        TempCamera camera = new TempCamera
        {
            Position = new Vector3(0, 0, -5),
            Yaw = 0,
            Pitch = 0
        };

        int[] indices =
        {
            0, 1, 2,  0, 2, 3, // front
            1, 5, 6,  1, 6, 2, // right
            5, 4, 7,  5, 7, 6, // back
            4, 0, 3,  4, 3, 7, // left
            3, 2, 6,  3, 6, 7, // top
            4, 5, 1,  4, 1, 0  // bottom
        };

        Vector3[] cube =
        {
            // front face
            new Vector3(-1, -1,  1),
            new Vector3( 1, -1,  1),
            new Vector3( 1,  1,  1),
            new Vector3(-1,  1,  1),

            // back face
            new Vector3(-1, -1, -1),
            new Vector3( 1, -1, -1),
            new Vector3( 1,  1, -1),
            new Vector3(-1,  1, -1),
        };
    }
}
