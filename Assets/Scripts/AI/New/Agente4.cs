using UnityEngine;

public class Agente4 : AgenteBase
{
    public AgenteBase agente5Evade; 

    protected override Vector2 CalcularComportamiento()
    {
        if (agente5Evade == null) return Vector2.zero;

        Vector2 distanciaAlObjetivo = agente5Evade.transform.position - transform.position;

        float tiempoPrediccion = distanciaAlObjetivo.magnitude / velocidadMaxima;

        Vector2 posicionFutura = (Vector2)agente5Evade.transform.position + (agente5Evade.ObtenerVelocidad() * tiempoPrediccion);

        Vector2 deseada = (posicionFutura - (Vector2)transform.position).normalized * velocidadMaxima;
        return deseada - velocidad;
    }
}