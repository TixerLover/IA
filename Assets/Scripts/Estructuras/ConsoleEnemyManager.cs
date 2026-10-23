using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleEnemyManager : MonoBehaviour
{
    private List<string> enemies = new List<string>();

    private void Start()
    {
        enemies.Add("Orc");
        Debug.Log("Orc has been added");

        enemies.Add("Naga");
        Debug.Log("Naga has been added");

        enemies.Add("Dragon Lord");
        Debug.Log("Dragon Lord has been added");

        Debug.Log("-------------------------");

        ImprimirLista();
        Debug.Log("-------------------------");
        KillEnemy("Dragon Lord");
        Debug.Log("Dragon Lord has been slayed");
        Debug.Log("-------------------------");
        ImprimirLista();

    }

    void KillEnemy(string Enemy)
    {
        enemies.Remove(Enemy);
    }

    void ImprimirLista()
    {
        Debug.Log("Enemies");
        for (int i = 0; i < enemies.Count; i++)
        {
            Debug.Log($"Índice {i}: {enemies[i]}");
        }
    }
}