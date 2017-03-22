using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains data for the difficulty, random-gen, etc. of a level in the tower.
/// </summary>
[System.Serializable]
public class Level {
    /// <summary>
    /// Manually assigned reference to a pre-loaded room.
    /// </summary>
    public Room startingRoom;
    public int targetRoomCount;
}
