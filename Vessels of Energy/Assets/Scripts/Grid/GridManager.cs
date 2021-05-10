using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public class GridPoint {
        public HexGrid hex;
        public float distance;
        public GridPoint(HexGrid hex, float distance) {
            this.hex = hex;
            this.distance = distance;
        }
    }
    public class Grid {
        public HexGrid origin;
        public List<GridPoint> grid;

        public Grid(HexGrid origin) {
            this.origin = origin;
            grid = new List<GridPoint>();
        }
    }

    public static GridManager instance;

    string[] blocks;

    private void Awake() {
        if (instance != null && instance != this) Destroy(this.gameObject);
        else instance = this;

        Token.selected = null;
        Token.locked = false;
    }

    //returns all the hex on reach given a origin and a distance
    public Grid getReach(HexGrid origin, int distance, params string[] blocks) {
        return getReach(origin, 1, distance, blocks);
    } //default
    public Grid getReach(HexGrid origin, int minDistance, int maxDistance, params string[] blocks) {
        Grid reach = new Grid(origin);

        List<HexGrid> depth = new List<HexGrid>();
        List<HexGrid> sweep = new List<HexGrid>();
        depth.Add(origin);
        sweep.Add(origin);
        if (minDistance < 1) reach.grid.Add(new GridPoint(origin, 0));

        this.blocks = blocks;
        reach = calcReach(reach, depth, sweep, minDistance, maxDistance, 1);
        return reach;
    }

    Grid calcReach(Grid reach, List<HexGrid> currDepth, List<HexGrid> sweep, int min, int max, int count) {
        if (count > max || currDepth.Count == 0) return reach;

        List<HexGrid> nextDepth = new List<HexGrid>();
        foreach (HexGrid hex in currDepth) {
            foreach (HexGrid neighbor in hex.neighbors) {
                if (!sweep.Contains(neighbor) && isAvailable(neighbor)) {
                    nextDepth.Add(neighbor);
                    sweep.Add(neighbor);
                    if (count >= min) reach.grid.Add(new GridPoint(neighbor, count));
                }
            }
        }

        return calcReach(reach, nextDepth, sweep, min, max, count + 1);
    }

    //get the shortest path, given origin and destination
    public Grid getPath(HexGrid origin, HexGrid destiny, params string[] blocks) {
        Grid path = new Grid(origin);

        List<HexGrid> depth = new List<HexGrid>();
        List<HexGrid> sweep = new List<HexGrid>();
        depth.Add(destiny);
        sweep.Add(destiny);

        this.blocks = blocks;
        path = calcPath(origin, path, depth, sweep);
        return path;
    }

    Grid calcPath(HexGrid origin, Grid path, List<HexGrid> currDepth, List<HexGrid> sweep) {
        if (currDepth.Count == 0) return null;

        List<HexGrid> nextDepth = new List<HexGrid>();
        List<HexGrid> reference = new List<HexGrid>();

        foreach (HexGrid hex in currDepth) {
            foreach (HexGrid neighbor in hex.neighbors) {
                if (neighbor == origin) {
                    path.grid.Add(new GridPoint(hex, 1));
                    return path;
                }

                if (!sweep.Contains(neighbor) && isAvailable(neighbor)) {
                    nextDepth.Add(neighbor);
                    sweep.Add(neighbor);
                    reference.Add(hex);
                }
            }
        }
        path = calcPath(origin, path, nextDepth, sweep);
        if (path != null) {
            GridPoint lastStep = path.grid[path.grid.Count - 1];
            if (nextDepth.Contains(lastStep.hex)) {
                HexGrid nextStep = reference[nextDepth.IndexOf(lastStep.hex)];
                path.grid.Add(new GridPoint(nextStep, path.grid.Count));
                return path;
            }
        }

        return null;
    }

    bool isAvailable(HexGrid hex) {
        foreach (string state in blocks) {
            if (hex.state.name == state)
                return false;
        }

        return true;
    }
}
