using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A room is a subcomponent attached to a prefab that designates the prefab as a spawnable room.
/// Rooms contain specific info on what can generate in a room, as well as how this room can connect to others.
/// </summary>
public class Room : MonoBehaviour {

    /// <summary>
    /// This is essentially a list of doors, or points to which other rooms can branch off of.
    /// </summary>
    [SerializeField]
    private List<RoomConnector> roomConnectors;

    [SerializeField]
    private List<RoomCollider> roomBounds;

    public List<RoomConnector> getRoomConnectors () {
        return roomConnectors;
    }

    public List<RoomConnector> getUnusedRoomConnectors () {
        List<RoomConnector> unusedConnectors = new List<RoomConnector>();
        foreach (RoomConnector roomConnector in roomConnectors) {
            if (roomConnector.getConnectedRoom() == null) {
                unusedConnectors.Add(roomConnector);
            }
        }
        return unusedConnectors;
    }

    /// <summary>
    /// Returns true if this room doesn't collide with any other rooms.
    /// Must be called at least one frame after room instantiation.
    /// </summary>
    public bool getBoundsValidity () {
        foreach(RoomCollider collider in roomBounds) {
            if (collider.isActivated) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Finalizes this room by generating extra random stuff, and switching the tags of the roomBounds so as to not interfere with other calculations.
    /// </summary>
    public void completeSetup () {
        foreach (RoomCollider collider in roomBounds) {
            collider.deactivateTrigger();
        }
        foreach (Transform child in transform.FindChild("Geometry")) {
            child.tag = "Untagged";
        }
    }

    public void activateBoundsDetection () {
        foreach (Transform child in transform.FindChild("Geometry")) {
            child.tag = "RoomBoundsTrigger";
        }
        foreach (RoomCollider collider in roomBounds) {
            collider.tag = "RoomBoundsTrigger";
        }
    }
}
