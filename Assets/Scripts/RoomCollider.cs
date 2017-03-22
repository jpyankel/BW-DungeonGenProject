using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour {

    [HideInInspector]
    public bool isActivated = false;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("RoomBoundsTrigger")) {
            isActivated = true;
        }
    }

    public void deactivateTrigger () {
        gameObject.tag = "Untagged";
    }
}
