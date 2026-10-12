using UnityEngine;

public class Agente3 : AgenteBase
{
    public float radioWander = 2f;
    public float distanciaWander = 3f;

    [Tooltip("Qué tan bruscos son los giros (en grados por segundo)")]
    public float jitterWander = 180f;

    private float anguloWander = 0f;

    protected override Vector2 CalcularComportamiento()
    {
        anguloWander += Random.Range(-jitterWander, jitterWander) * Time.deltaTime;
        Vector2 puntoEnCirculo = new Vector2(Mathf.Cos(anguloWander * Mathf.Deg2Rad), Mathf.Sin(anguloWander * Mathf.Deg2Rad)) * radioWander;
        Vector2 posicionLocalObjetivo = puntoEnCirculo + new Vector2(distanciaWander, 0);
        Vector2 posicionMundoObjetivo = transform.TransformPoint(posicionLocalObjetivo);
        Vector2 deseada = (posicionMundoObjetivo - (Vector2)transform.position).normalized * velocidadMaxima;
        return deseada - velocidad;
    }
}