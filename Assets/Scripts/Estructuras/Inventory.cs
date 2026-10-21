using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemData[] slots = new ItemData[5];

    public void AgregarObjeto(ItemData nuevoObjeto)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = nuevoObjeto;
                Debug.Log($"¡{nuevoObjeto.nombre} añadido al slot [{i}]!");
                MostrarInventario();
                return;
            }
        }
        Debug.LogWarning("¡Inventario lleno! No se pudo añadir: " + nuevoObjeto.nombre);
    }

    public void AsignarObjetoEnSlot(ItemData nuevoObjeto, int indiceSlot)
    {
        if (indiceSlot >= 0 && indiceSlot < slots.Length)
        {
            slots[indiceSlot] = nuevoObjeto;
            string nombreItem = nuevoObjeto != null ? nuevoObjeto.nombre : "[Vacío]";
            Debug.Log($"¡{nombreItem} asignado directamente al slot [{indiceSlot}]!");

            MostrarInventario();
        }
        else
        {
            Debug.LogWarning($"El slot [{indiceSlot}] no existe. El inventario solo tiene {slots.Length} espacios.");
        }
    }

    public void RemoverObjeto(int indiceSlot)
    {
        if (indiceSlot >= 0 && indiceSlot < slots.Length)
        {
            if (slots[indiceSlot] != null)
            {
                Debug.Log($"Eliminado {slots[indiceSlot].nombre} del slot [{indiceSlot}].");
                slots[indiceSlot] = null;
                MostrarInventario();
            }
            else
            {
                Debug.Log($"El slot [{indiceSlot}] ya estaba vacío.");
            }
        }
    }

    public void MostrarInventario()
    {
        string estadoInventario = "--- ESTADO DEL INVENTARIO ---\n";
        for (int i = 0; i < slots.Length; i++)
        {
            string nombreItem = slots[i] != null ? slots[i].nombre : "[Vacío]";
            estadoInventario += $"Slot [{i}]: {nombreItem}\n";
        }
        estadoInventario += "-----------------------------";
        Debug.Log(estadoInventario);
    }
}