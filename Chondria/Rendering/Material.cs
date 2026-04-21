namespace Chondria.Rendering;

public class Material
{
    public Color Color = new(1f, 0.5f, 0.3f);

    public float SpecularStrength = 0.5f;
    public float Shininess = 32f;

    public Shader Shader = Shader.DefaultLit;

    /*public void Apply(Shader shader)
    {
        shader.SetVector3("objectColor", Color);
        shader.SetFloat("specularStrength", SpecularStrength);
        shader.SetFloat("shininess", Shininess);
    }*/
}
