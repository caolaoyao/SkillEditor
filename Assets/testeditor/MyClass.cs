using UnityEngine;
using System.Collections;

[System.Serializable]
public class Person
{
    public string name;
    public int age;
}
public class MyClass : MonoBehaviour {
    public Person[] person;
    [SerializeField]
    private float myScale;
}
