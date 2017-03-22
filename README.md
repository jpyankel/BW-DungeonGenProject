# BW - Dungeon Generation Project
This project is a fully functional runtime dungeon layout generator for Unity3D. 
It simply pulls from a pool of user-defined dungeon room prefabs, and attempts to arrange them, randomly, to meet a target room count.
It is inspired from some of these ideas [here.](http://blog.elysianlegends.com/?p=11)
Although there are many dungeon generators out there, even for Unity3D, I had specific project requirements and artist pipelines I wanted to account for.

Feel free to learn from it, build onto it, or use it in your own projects!

## Getting Started
### Running the Example Implementation
1. Download the zip or clone the repository.
2. Open the scene "ExampleScene.unity".
3. Run the game and watch it generate a level!

**Note:** The MainCamera in the scene currently holds the ExampleImplementation monobehaviour; 
if you want to tweak some properties or add your own rooms, start by looking there.

### Importing and Implementing in Your Own Game
1. Download the zip or clone the repository.
2. Copy the "scripts" folder into your project hierarchy 
(I like to put mine in its own separate code folder to preserve a sense of modularity).
3. Attach TowerGenerator.cs MonoBehaviour to any GameObject (preferably your GameManager). You should modify its properties now
(I will also cover specifics and how to do it in [this wiki section]()). Be sure to assign one or more levels and multiple room prefabs.
4. Create a monobehaviour or other script that calls `TowerGenerator.generateLevel` when you want your dungeon to be generated. 
An integer must be passed specifying the index of the level you want to load from the list of levels created in step 3.
5. Run and playtest!

**Note:**
When you create the Room Prefabs, there is a certain procedure you must follow in order for the placement and generation to occur properly.
It will be covered in [this wiki section.]()
