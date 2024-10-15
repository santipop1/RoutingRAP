using UnityEngine;

public class Carta
{
    public string Titulo { get; private set; }
    public string Descripcion1 { get; private set; }
    public string Descripcion2 { get; private set; }
    public Sprite Imagen { get; private set; }

    // Constructor de la clase
    public Carta(string titulo, string descripcion1, string descripcion2, Sprite imagen)
    {
        Titulo = titulo;
        Descripcion1 = descripcion1;
        Descripcion2 = descripcion2;
        Imagen = imagen;
    }
}
