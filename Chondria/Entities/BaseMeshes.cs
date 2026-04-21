namespace Chondria.Entities;

internal static class BaseMeshes
{
    public static Dictionary<BaseMesh, float[]> BASE_MESHES = [];
    public static Dictionary<string, float[]> CUSTOM_MESHES = [];

    static BaseMeshes()
    {
        BASE_MESHES[BaseMesh.Cube] = Cube;
        BASE_MESHES[BaseMesh.Sphere] = BuildSphere();
    }

    static float[] Cube =
    [
        -0.5f, -0.5f, 0.5f,  0, 0, 1,
        0.5f, -0.5f, 0.5f,  0, 0, 1,
        0.5f,  0.5f, 0.5f,  0, 0, 1,

        0.5f,  0.5f, 0.5f,  0, 0, 1,
        -0.5f,  0.5f, 0.5f,  0, 0, 1,
        -0.5f, -0.5f, 0.5f,  0, 0, 1,

        -0.5f, -0.5f, -0.5f,  0, 0, -1,
        0.5f, -0.5f, -0.5f,  0, 0, -1,
        0.5f, 0.5f, -0.5f,  0, 0, -1,

        0.5f, 0.5f, -0.5f,  0, 0, -1,
        -0.5f, 0.5f, -0.5f,  0, 0, -1,
        -0.5f, -0.5f, -0.5f,  0, 0, -1,

        -0.5f, 0.5f, 0.5f,  -1, 0, 0,
        -0.5f, 0.5f, -0.5f,  -1, 0, 0,
        -0.5f, -0.5f, -0.5f,  -1, 0, 0,

        -0.5f, -0.5f, -0.5f,  -1, 0, 0,
        -0.5f, -0.5f, 0.5f,  -1, 0, 0,
        -0.5f, 0.5f, 0.5f,  -1, 0, 0,

        0.5f, 0.5f, 0.5f,  1, 0, 0,
        0.5f, 0.5f, -0.5f,  1, 0, 0,
        0.5f, -0.5f, -0.5f,  1, 0, 0,

        0.5f, -0.5f, -0.5f,  1, 0, 0,
        0.5f, -0.5f, 0.5f,  1, 0, 0,
        0.5f, 0.5f, 0.5f,  1, 0, 0,

        -0.5f, 0.5f, -0.5f,  0, 1, 0,
        0.5f, 0.5f, -0.5f,  0, 1, 0,
        0.5f, 0.5f, 0.5f,  0, 1, 0,

        0.5f, 0.5f, 0.5f,  0, 1, 0,
        -0.5f, 0.5f, 0.5f,  0, 1, 0,
        -0.5f, 0.5f, -0.5f,  0, 1, 0,

        -0.5f, -0.5f, -0.5f,  0, -1, 0,
        0.5f, -0.5f, -0.5f,  0, -1, 0,
        0.5f, -0.5f, 0.5f,  0, -1, 0,

        0.5f, -0.5f, 0.5f,  0, -1, 0,
        -0.5f, -0.5f, 0.5f,  0, -1, 0,
        -0.5f, -0.5f, -0.5f,  0, -1, 0,
    ];

    static float[] BuildSphere()
    {
        List<float> vertices = [];
        int latBands = 16, longBands = 16;

        for (int lat = 0; lat < latBands; lat++)
        {
            float theta1 = lat * MathF.PI / latBands;
            float theta2 = (lat + 1) * MathF.PI / latBands;

            for (int lon = 0; lon < longBands; lon++)
            {
                float phi1 = lon * 2 * MathF.PI / longBands;
                float phi2 = (lon + 1) * 2 * MathF.PI / longBands;

                float x1 = MathF.Cos(phi1) * MathF.Sin(theta1);
                float y1 = MathF.Cos(theta1);
                float z1 = MathF.Sin(phi1) * MathF.Sin(theta1);

                float x2 = MathF.Cos(phi2) * MathF.Sin(theta1);
                float y2 = MathF.Cos(theta1);
                float z2 = MathF.Sin(phi2) * MathF.Sin(theta1);

                float x3 = MathF.Cos(phi2) * MathF.Sin(theta2);
                float y3 = MathF.Cos(theta2);
                float z3 = MathF.Sin(phi2) * MathF.Sin(theta2);

                float x4 = MathF.Cos(phi1) * MathF.Sin(theta2);
                float y4 = MathF.Cos(theta2);
                float z4 = MathF.Sin(phi1) * MathF.Sin(theta2);

                AddVertex(vertices, x1, y1, z1);
                AddVertex(vertices, x2, y2, z2);
                AddVertex(vertices, x3, y3, z3);

                AddVertex(vertices, x1, y1, z1);
                AddVertex(vertices, x3, y3, z3);
                AddVertex(vertices, x4, y4, z4);
            }
        }

        return [.. vertices];
    }

    static void AddVertex(List<float> vertices, float x, float y, float z)
    {
        vertices.Add(x);
        vertices.Add(y);
        vertices.Add(z);

        vertices.Add(x); // normal x
        vertices.Add(y); // normal y
        vertices.Add(z); // normal z
    }
}

public enum BaseMesh
{
    None,
    Cube,
    Sphere,
    Cylinder,
    Capsule,
    Plane,
    Quad,

    Custom,
}
