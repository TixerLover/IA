using UnityEngine;

public class Agente2 : AgenteBase
{
    protected override Vector2 CalcularComportamiento()
    {
        Vector2 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deseada = (Vector2)transform.position - posicionMouse;
        deseada = deseada.normalized * velocidadMaxima;
        return deseada - velocidad;
    }
}