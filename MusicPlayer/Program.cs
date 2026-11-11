using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MusicPlayer
{
    // Song library contains all songs available, and a playlist is the list of songs the user likes
    // Users can queue and play whatever songs they want from their library, and also remove song from their library

    class Song
    {
        // Stores song information
        public string songID; // used for hashtable key
        public string title;
        public string artist;
        public string duration;

        // constructor to initialise new song object
        public Song(string sid, string t, string a, string d)
        {
            songID = sid;
            title = t;
            artist = a;
            duration = d;
        }

        // Function to format song information for display
        public string Info()
        {
            return string.Format("{0,-10} | {1,-30} | {2,-35} | {3,-8}", songID, title, artist, duration);
        }
    }

    // Each node stores the normal Left & Right nodes, Data string for sorting but also the Song object
    class Node
    {
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public string Data { get; set; }
        public Song song { get; set; }
    }

    class BinaryTree
    {
        public Node Root { get; set; } // Binary tree starts from the root

        // Adds song object to correct BST position
        public bool Add(Song s)
        {
            Node before = null; // previous node
            Node after = this.Root; // current node

            while (after != null) // while there is an existing node
            {
                before = after; // store its position
                int compare = s.title.CompareTo(after.Data); // compare title of new song with the current song

                if (compare < 0)
                {  // if its value is less
                    after = after.LeftNode; // set current as left node
                }
                else if (compare > 0)
                { // if its greater
                    after = after.RightNode; // set current as right node
                }
                else
                {
                    return false; // if its a duplicate, do not add
                }
            }

            Node newNode = new Node(); // create a new node
            newNode.Data = s.title; // set its Data as the title
            newNode.song = s; // store the entire song object

            if (this.Root == null) // if there no root, there is no BST
            {
                this.Root = newNode; // so make one by setting the root as the new node
            }
            else
            {
                if (s.title.CompareTo(before.Data) < 0) // if new node is less than previous node
                {
                    before.LeftNode = newNode; // it goes on the left
                }
                else
                {
                    before.RightNode = newNode; // if not, it goes on the right
                }
            }
            return true; // return true once done
        }

        // public Find function
        public Node Find(string title)
        {
            return this.Find(title, this.Root); // Calls the private Find function and passes in the input title and current BST root
        }

        private Node Find(string title, Node parent)
        {
            if (parent != null) // if current node exists
            {
                int compare = title.CompareTo(parent.Data); // compare input title and the current nodes Data

                if (compare == 0) // if equal, we found it
                {
                    return parent; // return current node
                }
                if (compare < 0) // if search is less
                {
                    return Find(title, parent.LeftNode); // recurse left
                }
                else
                {
                    return Find(title, parent.RightNode); // if greater, recurse right
                }
            }
            return null; // if not found, return null
        }

        // public Remove function
        public void Remove(string value)
        {
            this.Root = Remove(this.Root, value); // calls the private Remove with current BST root and passed in value
        }

        private Node Remove(Node parent, string key)
        {
            if (parent == null) // if current node is null
            {
                return parent; // return null
            }
            if (key.CompareTo(parent.Data) < 0) // if input is less than current nodes data
            {
                parent.LeftNode = Remove(parent.LeftNode, key); // recurse Remove on the left node
            }
            else if (key.CompareTo(parent.Data) > 0)
            {
                parent.RightNode = Remove(parent.RightNode, key); // greater then recurse on the right node
            }
            else // if we find the node to remove, we need to replace parent with the next node
            {
                // node with only one child or no child
                if (parent.LeftNode == null)
                {
                    return parent.RightNode; // returns right node and if no right node, returns null which is what we want
                }
                else if (parent.RightNode == null)
                {
                    return parent.LeftNode; // similarly, this either returns left or null
                }

                // if 2 children, we need to replace parent with the smallest value in the right nodes

                Node next = MinValue(parent.RightNode); // get the smallest value

                // replace current parent with the smallest node
                parent.Data = next.Data;
                parent.song = next.song;

                // remove smallest from the right side
                parent.RightNode = Remove(parent.RightNode, parent.Data);
            }
            return parent;
        }

        // finds node with the smallest value
        private Node MinValue(Node node)
        {
            while (node.LeftNode != null) // while left node exists
            {
                node = node.LeftNode; // go left
            }
            return node; // the return left-most node (smallest)
        }

        // counts how many times the tree goes down (counts paths not nodes)
        public int maxDepth(Node node)
        {
            if (node == null) // if node is null
            {
                return -1; // return -1 to only count the paths
            }
            else
            {
                int lDepth = maxDepth(node.LeftNode); // recurse left to get depth of the left side
                int rDepth = maxDepth(node.RightNode); // recurse right to get depth of right side
                if (lDepth > rDepth) // find the deeper side
                {
                    return (lDepth + 1); // return depth + 1
                }
                else
                {
                    return (rDepth + 1);
                }
            }
        }

        // PreOrder(Root, Left, Right)
        public void TraversePreOrder(Node parent)
        {
            if (parent != null)
            {
                Console.WriteLine(parent.song.Info());
                TraversePreOrder(parent.LeftNode);
                TraversePreOrder(parent.RightNode);
            }
        }

        // InOrder(Left, Root, Right), this gives A-Z sorting
        public void TraverseInOrder(Node parent)
        {
            if (parent != null)
            {
                TraverseInOrder(parent.LeftNode);
                Console.WriteLine(parent.song.Info());
                TraverseInOrder(parent.RightNode);
            }
        }

        // Postorder(Left, Right, Root)
        public void TraversePostOrder(Node parent)
        {
            if (parent != null)
            {
                TraversePostOrder(parent.LeftNode);
                TraversePostOrder(parent.RightNode);
                Console.WriteLine(parent.song.Info());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // create all data structures we need
            Hashtable songs = new Hashtable(); // for searching by SongID
            BinaryTree tree = new BinaryTree(); // sorting and searching by title
            LinkedList<Song> playlist = new LinkedList<Song>(); // playlist
            Stack<Song> recentlyPlayed = new Stack<Song>(); // recently played songs
            Queue<Song> upNext = new Queue<Song>(); // songs to be played next

            // choice variables
            string choice = "";
            string choice2 = "";

            // loop until user inputs "E"
            while (choice != "E")
            {
                Console.WriteLine("=== MUSIC PLAYER ===");
                Console.WriteLine("(1) Load Songs");
                Console.WriteLine("(2) Browse Library");
                Console.WriteLine("(3) Search Song");
                Console.WriteLine("(4) Manage Playlist");
                Console.WriteLine("(5) Queue Song");
                Console.WriteLine("(6) Play Next Song");
                Console.WriteLine("(7) Show History");
                Console.WriteLine("(8) Delete Song from Library");
                Console.WriteLine("(E) Exit");
                Console.Write("Enter choice: ");
                choice = Console.ReadLine().ToUpper();
                Console.WriteLine();

                if (choice == "1") // Load songs
                {
                    if (songs.Count > 0)
                    {
                        Console.WriteLine("Songs already loaded");
                    }
                    else
                    {
                        try
                        {
                            // read songs text file
                            string[] lines = System.IO.File.ReadAllLines("songs.txt");

                            foreach (string line in lines)
                            {
                                // get all song info from each line
                                string[] words = line.Split(',');
                                string sid = words[0];
                                string t = words[1];
                                string a = words[2];
                                string d = words[3];
                                Song sobj = new Song(sid, t, a, d); // create new song object

                                songs.Add(sid, sobj); // add to hashtable
                                tree.Add(sobj); // add to BST
                            }

                            // read playlist text file
                            string[] plines = System.IO.File.ReadAllLines("playlist.txt");
                            foreach (string line in plines)
                            {
                                // text file only stores song ID's
                                Song song = songs[line] as Song; // get song objects from song library loaded into hashtable
                                if (song != null) // if song exists
                                {
                                    playlist.AddLast(song); // add to playlist
                                }
                            }
                            Console.WriteLine("Songs and playlist loaded");
                        }
                        catch
                        {
                            Console.WriteLine("Error reading songs.txt or playlist.txt");
                        }
                    }
                }
                else if (choice == "E") // have to check if input is "E"
                {
                    break;
                }
                // because this line checks for loaded songs even if they want to quit, and i didn't want to have this check for EVERY option
                else if (songs.Count == 0)
                {
                    Console.WriteLine("Load songs using Option 1 first");
                }
                else if (choice == "2") //Browse Library
                {
                    choice2 = ""; // second choice option for the sub-menu

                    Console.WriteLine("Tree Depth: " + tree.maxDepth(tree.Root)); // print depth of BST
                    while (choice2 != "E")
                    {
                        Console.WriteLine("(A) In-Order (A-Z)");
                        Console.WriteLine("(B) Pre-Order");
                        Console.WriteLine("(C) Post-Order");
                        Console.WriteLine("(E) Exit");
                        Console.Write("Choose: ");
                        choice2 = Console.ReadLine().ToUpper();
                        Console.WriteLine();

                        // Print BST traversal
                        if (choice2 == "A")
                        {
                            Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                            tree.TraverseInOrder(tree.Root);
                            Console.WriteLine();
                        }
                        else if (choice2 == "B")
                        {
                            Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                            tree.TraversePreOrder(tree.Root);
                            Console.WriteLine();
                        }
                        else if (choice2 == "C")
                        {
                            Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                            tree.TraversePostOrder(tree.Root);
                            Console.WriteLine();
                        }
                    }
                }
                else if (choice == "3") // Search Songs
                {
                    choice2 = "";
                    while (choice2 != "E")
                    {
                        Console.WriteLine("(A) Search by ID");
                        Console.WriteLine("(B) Search by Title");
                        Console.WriteLine("(E) Exit");
                        Console.Write("Choose: ");
                        choice2 = Console.ReadLine().ToUpper();
                        Console.WriteLine();

                        if (choice2 == "A") // search by ID
                        {
                            Console.Write("Enter song ID: ");
                            string ID = Console.ReadLine().ToUpper(); // ToUpper to make it all uppercase, more convenient

                            Song song = songs[ID] as Song; // find song in hashtable
                            if (song != null) // if found
                            {
                                Console.WriteLine("Found:");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(song.Info());
                            }
                            else
                            {
                                Console.WriteLine("Song not found");
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "B") // search by title
                        {
                            Console.Write("Enter song title: ");
                            string title = Console.ReadLine();

                            Node result = tree.Find(title); // use Find function search for song
                            if (result != null) // print result if song is found
                            {
                                Console.WriteLine("Found:");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(result.song.Info());
                            }
                            else
                            {
                                Console.WriteLine("Song not found");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                else if (choice == "4") // Manage Playlist
                {
                    choice2 = "";
                    while (choice2 != "E")
                    {
                        Console.WriteLine("(A) Display Playlist");
                        Console.WriteLine("(B) Add to End");
                        Console.WriteLine("(C) Add to Start");
                        Console.WriteLine("(D) Remove by ID");
                        Console.WriteLine("(F) Remove First");
                        Console.WriteLine("(G) Remove Last");
                        Console.WriteLine("(H) Save Playlist");
                        Console.WriteLine("(E) Exit");
                        Console.Write("Choose: ");
                        choice2 = Console.ReadLine().ToUpper();
                        Console.WriteLine();

                        if (choice2 == "A") // Display Playlist
                        {
                            if (playlist.Count == 0)
                            {
                                Console.WriteLine("Playlist is empty!");
                            }
                            else
                            {
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                foreach (Song song in playlist)
                                {
                                    Console.WriteLine(song.Info());
                                }
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "B") // Add to End
                        {
                            Console.Write("Enter song ID: ");
                            string input = Console.ReadLine().ToUpper();
                            Song song = songs[input] as Song; // search for song in hashtable
                            if (song != null)
                            {
                                playlist.AddLast(song); // Adds to end

                                Console.WriteLine("Added to end: ");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(song.Info());
                            }
                            else
                            {
                                Console.WriteLine("Song not found");
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "C") // Add to Start
                        {
                            Console.Write("Enter song ID: ");
                            string input = Console.ReadLine().ToUpper();
                            Song song = songs[input] as Song;
                            if (song != null)
                            {
                                playlist.AddFirst(song); // Adds to start

                                Console.WriteLine("Added to top: ");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(song.Info());
                            }
                            else
                            {
                                Console.WriteLine("Song not found");
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "D") // Remove by ID
                        {
                            Console.Write("Enter song ID: ");
                            string input = Console.ReadLine().ToUpper();
                            Song song = songs[input] as Song;
                            if (song != null)
                            {
                                playlist.Remove(song); // removes specific song object (found using it's ID in hashtable)

                                Console.WriteLine("Removed: ");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(song.Info());
                            }
                            else
                            {
                                Console.WriteLine("Song not found");
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "F") // Remove First
                        {
                            if (playlist.Count == 0)
                            {
                                Console.WriteLine("Playlist is empty");
                            }
                            else
                            {
                                Song song = playlist.First.Value; // gets the first song in playlist

                                playlist.Remove(song);  // removes first

                                Console.WriteLine("Removed from start: ");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(song.Info());
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "G") // Remove last
                        {
                            if (playlist.Count == 0)
                            {
                                Console.WriteLine("Playlist is empty");
                            }
                            else
                            {
                                Song song = playlist.Last.Value; // gets the last song in playlist

                                playlist.Remove(song); // removes last

                                Console.WriteLine("Removed from end: ");
                                Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                Console.WriteLine(song.Info());
                            }
                            Console.WriteLine();
                        }
                        else if (choice2 == "H") // Save Playlist
                        {
                            // write playlist to the playlist.txt file
                            try
                            {
                                using (StreamWriter sw = new StreamWriter("playlist.txt"))
                                {
                                    foreach (Song song in playlist)
                                    {
                                        sw.WriteLine(song.songID);
                                    }
                                }
                                Console.WriteLine("Playlist saved!");
                            }
                            catch
                            {
                                Console.WriteLine("Error saving playlist");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                else if (choice == "5") // Queue Song
                {
                    string input = "";
                    while (input != "E")
                    {
                        Console.Write("Enter song ID (E to stop): ");
                        input = Console.ReadLine().ToUpper();
                        Console.WriteLine();

                        if (input == "E")
                        {
                            break;
                        }

                        Song song = songs[input] as Song; // get song from hashtable using ID

                        if (song != null)
                        {
                            upNext.Enqueue(song); // adds song to queue

                            Console.WriteLine("Song queued: ");
                            Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                            Console.WriteLine(song.Info());
                        }
                        else
                        {
                            Console.WriteLine("Song not found");
                        }
                    }
                }
                else if (choice == "6") // Play Next Song
                {
                    if (upNext.Count == 0)
                    {
                        Console.WriteLine("Queue is empty");
                    }
                    else
                    {
                        Song song = upNext.Dequeue(); // remove song from queue

                        Console.WriteLine("Now playing: ");
                        Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                        Console.WriteLine(song.Info());

                        recentlyPlayed.Push(song); // adds song to the recentlyPlayed stack

                        if (upNext.Count > 0)
                        {
                            Console.WriteLine("Up next: ");
                            Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");

                            Console.WriteLine(upNext.Peek().Info()); // use peek to show next song without dequeuing
                        }
                        else
                        {
                            Console.WriteLine("Nothing queued after this");
                        }
                    }
                    Console.WriteLine();
                }
                else if (choice == "7") // Show History
                {
                    choice2 = "";
                    if (recentlyPlayed.Count == 0)
                    {
                        Console.WriteLine("No song history");
                    }
                    else
                    {
                        Console.WriteLine("Last played: ");
                        Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");

                        Console.WriteLine(recentlyPlayed.Peek().Info()); // use peek to show most recently played song without removing

                        while (choice2 != "E")
                        {
                            Console.WriteLine("(A) View Full History");
                            Console.WriteLine("(B) Re-queue Last Played");
                            Console.WriteLine("(E) Exit");
                            Console.Write("Choose: ");
                            choice2 = Console.ReadLine().ToUpper();
                            Console.WriteLine();

                            if (choice2 == "A") // display all songs in recentlyPlayed
                            {
                                if (recentlyPlayed.Count == 0)
                                {
                                    Console.WriteLine("No song history");
                                }
                                else
                                {
                                    Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                    foreach (Song song in recentlyPlayed)
                                    {
                                        Console.WriteLine(song.Info());
                                    }
                                }
                                Console.WriteLine();
                            }
                            else if (choice2 == "B") // requeue last played song
                            {
                                if (recentlyPlayed.Count == 0)
                                {
                                    Console.WriteLine("No songs to re-queue");
                                }
                                else
                                {
                                    Song song = recentlyPlayed.Pop(); // removes and stores the most recently added song
                                    upNext.Enqueue(song); // then adds it back to queue

                                    Console.WriteLine("Re-queued: ");
                                    Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                                    Console.WriteLine(song.Info());
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }
                else if (choice == "8") // Delete Song from Library
                {
                    Console.Write("Enter song ID: ");
                    string input = Console.ReadLine().ToUpper();
                    Console.WriteLine();

                    if (songs.ContainsKey(input))
                    {
                        Song song = songs[input] as Song; // get song
                        songs.Remove(input); // remove from hashtable
                        tree.Remove(song.title); // remove from BST
                        playlist.Remove(song); // remove from playlist

                        // Write updated song library and playlist songs.txt and playlist.txt files 
                        try
                        {
                            using (StreamWriter sw = new StreamWriter("songs.txt"))
                            {
                                foreach (DictionaryEntry entry in songs) // use dictionary entry to loop through Hashtable
                                {
                                    Song s = entry.Value as Song;
                                    sw.WriteLine(s.songID + "," + s.title + "," + s.artist + "," + s.duration);
                                }
                            }

                            using (StreamWriter sw = new StreamWriter("playlist.txt"))
                            {
                                foreach (Song s in playlist)
                                {
                                    sw.WriteLine(s.songID);
                                }
                            }

                            Console.WriteLine("Deleted: ");
                            Console.WriteLine("{0,-10} | {1,-30} | {2,-35} | {3,-8}", "ID", "Title", "Artist", "Duration");
                            Console.WriteLine(song.Info());
                            Console.WriteLine("Library and playlist saved");
                        }
                        catch
                        {
                            Console.WriteLine("Error saving song library or playlist");
                        }
                    }
                    else Console.WriteLine("Song not found");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}