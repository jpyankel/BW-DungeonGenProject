using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleImplementation : MonoBehaviour {

    private void Start () {
        GetComponent<TowerGenerator>().generateLevel(0); //Generate the first level (index 0).
    }
}
