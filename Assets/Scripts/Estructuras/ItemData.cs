using UnityEngine;

[CreateAssetMenu(fileName = "NuevoObjeto", menuName = "Inventario/Objeto")]
public class ItemData : ScriptableObject
{
    public string nombre;
    [TextArea] public string descripcion;
}
