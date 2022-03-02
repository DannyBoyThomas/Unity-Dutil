# Unity-Dutil

Utility Extension Functions

## Colours

Access all Material colours and their shades(0-8).

Create any colour from a hex code. 3 digit or 6 digit. '#' is ignored if used

Tweak color values

```
Colours.Red
Color lightBlue = Colours.Blue.Shade(1);
List<Color> shades = Colours.Green.Shades();
Color grey = Colours.Hex("333");
Color purple = Colours.Red.WithBlue(1);
```

## List Extensions

```
List<Vector3> points = new List(){...};
points.Print();
points.Any();
points.AnyUnique(3);   //returns 3 random items from list without duplicates
points.AddUnique(anotherPoint) // Only adds if it doesn't already exist
points.ClosestPoint(otherPoint);
points.ClosestPointOnPath(otherPoint);
points.Average();
points.Smooth();
points.Cluster(3); // Separate points into 3 groups based on distance. Uses K-means algorithm

```

## Tracking

Use to store static references to your game objects/components.
Tracking ensures you dont slow your game down using functions such as; FindObjectsOfType().

To track;

```
GameObject spawnManagerObj = new GameObject("Spawn Manager");
D.Track("spawn_manager",spawnManagerObj);

```

To find;

```
GameObject spawnManager = D.TrackFirst("spawn_manager");
```

Many objects can be tracked under the same ID;

```
D.Track("spawned_objects",obj1,obj2,obj3...);
List<GameObject> spawnedObjects = D.Track("spawned_objects");
```

Remove tracked objects;

```
D.Untrack("spawned_objects"); //untrack all
D.Untrack("spawned_objects",obj3) // only untrack obj3
```

## Flood Fill

Easily implement a flood fill on your own data types

```
List<Vector2Int> island = D.FloodFill(allCoords, startCoord, (list, currentItem) =>{
     return currentItem.GetNeighbours();
});
```

## Schedule

Automate delayed tasks

```
Schedule.Add(3,(event)=>{
  //Run this code after 3 seconds
});
```

Automate repetitive tasks

```
Schedule.Add(3,(event)=>{
  //Run this code after 3 seconds and repeat every 10 seconds
}, 10);
```

## AutoLerp

Automate Continuous tasks
Set it and forget it.

```
AutoLerp.Begin(3,(task,value)=>{
  //returns values 0 to 1 over 3 seconds
});

AutoLerp.Begin(2, Color.red, Color.blue, (task,value)=>{
  transform.GetComponent<Renderer>().material.color = value;
  // Object colour transitions from red to blue over 2 seconds
});
```

To add easing;

```
AutoLerp.Begin(2, Vector3.zero, Vector3.right*7, (task,value)=>{
  transform.position = value;
  // Move to the right with easing
},true);
```

Accepted variable types; float, int, Vector2, Vector3, Quaternion, Color

GameObjects and Transforms can use these functions directly;

```
gameObject.AutoLerpPosition(Vector3.one*7, 3, true);
gameObject.AutoLerpScale(Vector3.one*2, 3, true);
transform.AutoLerpRotation(new Vector3(0,0,270), 3, true);

```

## Marking

A new tag system
Allows objects to hold multiple tags (marks)

```
gameObject.Mark("Sticky");
gameObject.UnMark("Slippery");
if( gameObject.HasMark("Bouncy"))
{
    //bounce
    gameObject.ClearMarks();
}
```

## Pathfinding

#### **GridNetwork**

Create a GridNetwork, specifying the dimensions as a Vector2Int. You also have the option to assign whether a tile is walkable or not, in the callback.

```
GridNetwork grid = new GridNetwork(new Vector2Int(10,10),(coord)=>{
    return Random.Range(0,1f)>0.5f;
});
```

Finding the shortest path

```
Vector2Int startCoord = new Vector2Int(0,0);
Vector2Int endCoord = new Vector2Int(9,9);
List<GridTile> path = grid.FindPath(startCoord,endCoord);
```

## Vector Extensions

```
Vector2 vec2 = new Vector3(1,2);
Vector3 vec3 = vec2.XY(3); // (1,2,3)
vec3.GetRight();
vec3.WithX(4);
vec2.OffsetY(-4);
vec3.SetY(7);
4.X() + 3.Y() // Vector3(4,3,0)
5.XYZ() // Vector3(5,5,5)
```

## Animated Line Renderer

```
List<Vector3> points = new List(){...};
DLine line = points.Renderer();
line.ShowAnimated();
line.HideInstantly();
```

## Plus more

```
gameObject.GetOrAddComponent<>();
gameObject.Kill(); // Animation then destroyed
gameObject.KillChildren();
enum.Next();
enum.Previous();
D.Chance(.5f);
D.RandomColour();
D.PointsOnCircle(3);
D.BezierPoints();
D.Hash(8);

```
