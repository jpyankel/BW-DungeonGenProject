using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Generates one level of a tower, using a target-rooms value.
/// </summary>
public class TowerGenerator : MonoBehaviour {

    /// <summary>
    /// This is the list of pre-loaded rooms to which we generate off of for each level.
    /// This is used for balancing reasons (e.g. if we only want one starting branch for the first few levels).
    /// </summary>
    [SerializeField]
    private List<Level> levels;

    [SerializeField]
    private List<Room> roomList;

    public void generateLevel (int level) {
        //TODO: Delete old level (if any).
        Level chosenLevel = levels[level];
        Room startingRoom = Instantiate(chosenLevel.startingRoom, Vector3.zero, Quaternion.identity) as Room;
        StartCoroutine(generationProcess(chosenLevel.targetRoomCount, startingRoom));
    }
    private IEnumerator generationProcess (int targetRoomCount, Room firstRoom) {
        int currentRoomCount = 1; //Count our starting room.
        List<RoomConnector> connectorSet = firstRoom.getRoomConnectors();
        while (currentRoomCount < targetRoomCount) {
            List<RoomConnector> nextConnectorSet = new List<RoomConnector>();
            foreach (RoomConnector initialConnector in connectorSet) {
                Room initialRoom = initialConnector.getRoom();
                if (initialConnector.getConnectedRoom() != null) { //If, for some reason, a connector is already occupied (set manually?), skip this iteration.
                    continue;
                }
                List<Room> possibleRooms = getConnectableRooms(initialConnector.getWidth()).ToList();
                //Until we reach our targetRoomCount, we must try and pick at least one room with more than one connector per iteration.
                List<Room> filteredRooms = new List<Room>();
                if (currentRoomCount < targetRoomCount) {
                    filteredRooms = getBranchingRooms(possibleRooms).ToList();
                }
                else {
                    filteredRooms = possibleRooms;
                }
                while (filteredRooms.Count > 0 && initialConnector.getConnectedRoom() == null) {
                    //Instantiate the rooms, filter the ones from the list that cause bad collisions, etc.
                    //If there are no possible working results, close off this connector.
                    Room newRoomPrefab = filteredRooms[Random.Range(0, filteredRooms.Count)];
                    //Randomly filter through connectors until one works:
                    List<RoomConnector> filteredConnectors = newRoomPrefab.getRoomConnectors().ToList();
                    while (filteredConnectors.Count > 0 && initialConnector.getConnectedRoom() == null) {
                        RoomConnector chosenConnector = filteredConnectors[Random.Range(0, filteredConnectors.Count)];
                        //Initialize and test the chosen room:
                        Room newRoom = Instantiate(newRoomPrefab, Vector3.zero, Quaternion.identity) as Room;
                        Transform newRoomConnector = newRoom.getRoomConnectors()[newRoomPrefab.getRoomConnectors().IndexOf(chosenConnector)].transform;
                        alignRoom(firstRoom.transform, initialConnector.transform, newRoom.transform, newRoomConnector);
                        //Start collision check:
                        newRoom.activateBoundsDetection();
                        yield return new WaitForFixedUpdate(); //Wait for any collisions to register.
                        if (!newRoom.getBoundsValidity()) {
                            filteredConnectors.Remove(chosenConnector); //This connector does not work, remove it from the list.
                            Destroy(newRoom.gameObject);
                        }
                        else {
                            //Set these rooms as connected to each other.
                            newRoom.getRoomConnectors()[newRoomPrefab.getRoomConnectors().IndexOf(chosenConnector)].setConnectedRoom(initialRoom);
                            initialConnector.setConnectedRoom(newRoom); //This step will break the loop.
                            newRoom.completeSetup();
                            currentRoomCount++;
                            nextConnectorSet.AddRange(newRoom.getUnusedRoomConnectors());
                        }
                    }
                    if (initialConnector.getConnectedRoom() == null) { //If this connector doesn't have a room yet, the newRoomPrefab cannot work in this instance.
                        filteredRooms.Remove(newRoomPrefab);
                    }
                }
                if (initialConnector.getConnectedRoom() == null) { //If this connector still does not have a room, we close it off.
                    initialConnector.closeOffConnector();
                    //TODO: Set this connector's connected room to a dummy value to indicate that it is filled with a door and should not be processed again.
                }
            }
            connectorSet = nextConnectorSet;
            if (connectorSet.Count <= 0) {
                break;
            }

            //Just before the next iteration, check if we are going to stop - and if so, we need to close off all of the nextConnectors:
            if (currentRoomCount >= targetRoomCount) {
                foreach (RoomConnector connector in connectorSet) {
                    connector.closeOffConnector();
                }
            }
        }
    }

    public List<Room> getConnectableRooms (int connectorWidth) {
        List<Room> possibleRooms = new List<Room>();
        foreach (Room room in roomList) {
            foreach(RoomConnector connector in room.getRoomConnectors()) {
                if (connector.getWidth() == connectorWidth) {
                    possibleRooms.Add(room);
                    break;
                }
            }
        }
        return possibleRooms;
    }

    /// <summary>
    /// Filters out any rooms that have 1 or less connectors.
    /// </summary>
    public List<Room> getBranchingRooms (List<Room> rooms) {
        List<Room> possibleRooms = new List<Room>();
        foreach (Room room in rooms) {
            if (room.getRoomConnectors().Count > 1) {
                possibleRooms.Add(room);
            }
        }
        return possibleRooms;
    }

    /// <summary>
    /// Returns a list of rooms with only one connector, i.e., ending rooms.
    /// </summary>
    public List<Room> getEndingRooms(List<Room> rooms) {
        List<Room> possibleRooms = new List<Room>();
        foreach (Room room in rooms) {
            if (room.getRoomConnectors().Count <= 1) {
                possibleRooms.Add(room);
            }
        }
        return possibleRooms;
    }

    /// <summary>
    /// Aligns the newRoom to the initialRoom using the given connectors.
    /// </summary>
    private void alignRoom(Transform initialRoom, Transform initialConnector, Transform newRoom, Transform newConnector) {
        newRoom.rotation = Quaternion.AngleAxis(initialConnector.rotation.eulerAngles.y + 180 - newConnector.rotation.eulerAngles.y, Vector3.up);
        newRoom.position = initialRoom.position + initialConnector.position - newConnector.position;
    }
}