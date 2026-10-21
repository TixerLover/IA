using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory inventario;

    [Header("Base de datos de objetos (Asígnalos una vez)")]
    public ItemData objetoQ;
    public ItemData objetoW;
    public ItemData objetoE;

    [Header("Estado Actual")]
    public int slotSeleccionado = 0;

    void Start()
    {
        Debug.Log("CONTROLES:\n1. Presiona [1-5] para elegir un Slot.\n2. Presiona [Q, W, E] para mandar un objeto al slot elegido.\n3. Presiona [R] para vaciar el slot elegido.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SeleccionarSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SeleccionarSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SeleccionarSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SeleccionarSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SeleccionarSlot(4);

        if (Input.GetKeyDown(KeyCode.Q)) Asignar(objetoQ);
        if (Input.GetKeyDown(KeyCode.W)) Asignar(objetoW);
        if (Input.GetKeyDown(KeyCode.E)) Asignar(objetoE);

        if (Input.GetKeyDown(KeyCode.R))
        {
            inventario.RemoverObjeto(slotSeleccionado);
        }
    }

    private void SeleccionarSlot(int indice)
    {
        slotSeleccionado = indice;
        Debug.Log($"Slot [{slotSeleccionado}] seleccionado. Ahora presiona Q, W o E.");
    }

    private void Asignar(ItemData itemSeleccionado)
    {
        if (itemSeleccionado != null)
        {
            inventario.AsignarObjetoEnSlot(itemSeleccionado, slotSeleccionado);
        }
        else
        {
            Debug.LogWarning("No has asignado este objeto en el Inspector del Tester.");
        }
    }
}