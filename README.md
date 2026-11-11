# Music Player

A C# console music player. It keeps one song library in several data structures, each chosen for the job it does. Built as a school project.

| Data structure | Used for |
|---|---|
| Hashtable | Finding a song by its ID |
| Binary search tree (own implementation) | Searching by title, and listing the library A to Z |
| Linked list | The playlist, which can be added to or trimmed at either end |
| Queue | Songs up next, played in the order they were queued |
| Stack | Play history, with the last song played on top |

The brief required all five of these in one program, along with reading and writing files, so each one was given its own job.

## Features

- Browse the library in-order (A to Z), pre-order or post-order, with the tree depth shown.
- Search by song ID or by title.
- Build a playlist: add to the start or end, remove by ID or from either end, and save it.
- Queue songs, play the next one, and see what is up next.
- View play history and re-queue the last song played.
- Delete a song from the library. This removes it from the hashtable, the tree and the playlist, then saves both files. Removing a tree node handles all three cases: no children, one child, or two children (replaced by the smallest node in its right subtree).

## Sample run

```
Enter choice: 5

Enter song ID (E to stop): S027

Song queued: 
ID         | Title                          | Artist                              | Duration
S027       | Iris                           | The Goo Goo Dolls                   | 4:49    
Enter song ID (E to stop): S040

Song queued: 
ID         | Title                          | Artist                              | Duration
S040       | Creep                          | Radiohead                           | 3:58    
Enter song ID (E to stop): E

...

Enter choice: 6

Now playing: 
ID         | Title                          | Artist                              | Duration
S027       | Iris                           | The Goo Goo Dolls                   | 4:49    
Up next: 
ID         | Title                          | Artist                              | Duration
S040       | Creep                          | Radiohead                           | 3:58    
```

## Running it

Open `MusicPlayer.sln` in Visual Studio and run it. The project targets .NET Framework 4.7.2.

`songs.txt` holds the library, one song per line as `ID,Title,Artist,Duration`. `playlist.txt` holds the saved playlist as one song ID per line. Both are copied next to the program when it builds.
