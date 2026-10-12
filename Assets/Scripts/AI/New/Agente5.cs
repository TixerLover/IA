using UnityEngine;

public class Agente5 : AgenteBase
{
    public AgenteBase agente4Pursuit; 

    protected override Vector2 CalcularComportamiento()
    {
        if (agente4Pursuit == null) return Vector2.zero;

        Vector2 distanciaAlObjetivo = agente4Pursuit.transform.position - transform.position;

        float tiempoPrediccion = distanciaAlObjetivo.magnitude / velocidadMaxima;

        Vector2 posicionFuturaAmenaza = (Vector2)agente4Pursuit.transform.position + (agente4Pursuit.ObtenerVelocidad() * tiempoPrediccion);

        Vector2 deseada = ((Vector2)transform.position - posicionFuturaAmenaza).normalized * velocidadMaxima;
        return deseada - velocidad;
    }
}