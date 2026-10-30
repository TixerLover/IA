using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class LinkedListTest : MonoBehaviour
{
    LinkedList<string> sentence = new LinkedList<string>(); //creacion de lista
    void Start()
    {
        sentence.AddFirst("Buenos"); //asigna el primer elemento

        LinkedListNode<string> currentNode = sentence.First; //accede al nodo principal

        sentence.AddLast("dias");

        Debug.Log("Ya no hay: ");
        PrintLink();

        currentNode.Value = "Malos";

        Debug.Log("Ni tampoco: ");
        PrintLink();

        sentence.RemoveFirst();

        Debug.Log("Solo hay: ");
        PrintLink();
    }

    private void PrintLink()
    {
        foreach (string s in sentence)
        {
            Debug.Log(s);
        }
    }

    
}
