using UnityEngine;

public class Agente1 : AgenteBase
{
    public Transform puntoDestino;
    public float radioLlegada = 3f;

    protected override Vector2 CalcularComportamiento()
    {
        if (puntoDestino == null) return Vector2.zero;

        Vector2 deseada = (Vector2)puntoDestino.position - (Vector2)transform.position;
        float distancia = deseada.magnitude;

        if (distancia < 0.1f) return Vector2.zero;

        deseada.Normalize();
        if (distancia < radioLlegada)
        {
            deseada *= velocidadMaxima * (distancia / radioLlegada);
        }
        else
        {
            deseada *= velocidadMaxima;
        }

        return deseada - velocidad;
    }
}