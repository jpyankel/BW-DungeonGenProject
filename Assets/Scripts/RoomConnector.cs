using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// A room connector is just a transform with some additional info.
/// This additional info can determine what types of rooms can be connected to this point.
/// </summary>
public class RoomConnector : MonoBehaviour {

    /// <summary>
    /// The size value of the connector.
    /// Only connectors of the same size can connect with each other.
    /// These sizes are arbitrary and are meant to be determined project specifics.
    /// </summary>
    [SerializeField]
    private int width = 1;

    /// <summary>
    /// The room connected to this connector.
    /// Used to check if this connector has already been used.
    /// </summary>
    private Room connectedRoom; //TODO: This may be used to transition the player's camera to target a specific room if a trigger is attached to this object.

    public int getWidth() {
        return width;
    }

    /// <summary>
    /// Returns the room that this connector is a part of.
    /// </summary>
    public Room getRoom () {
        return transform.parent.GetComponent<Room>();
    }

    public Room getConnectedRoom () {
        return connectedRoom;
    }

    public void setConnectedRoom (Room connectedRoom) {
        this.connectedRoom = connectedRoom;
    }

    /// <summary>
    /// Closes off this room's connector with a wall or something.
    /// Simply sets the render-disabled child of this object to active.
    /// </summary>
    public void closeOffConnector () {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
