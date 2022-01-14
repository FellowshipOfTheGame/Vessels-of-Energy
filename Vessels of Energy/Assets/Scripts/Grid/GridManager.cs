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
    public static HexGrid[] arena;

    string[] blocks;

    Reach reachAlgorithm;

    private void Awake() {
        if (instance != null && instance != this) Destroy(this.gameObject);
        else instance = this;

        Token.selected = null;
        Token.locked = false;
        arena = FindObjectsOfType<HexGrid>();
        reachAlgorithm = new Reach();
    }

    //returns all the hex on reach given a origin and a distance
    public Grid getReach(HexGrid origin, int distance, bool obstacle, params string[] blocks) {
        return getReach(origin, 1, distance, obstacle, blocks);
    } //default
    public Grid getReach(HexGrid origin, int minDistance, int maxDistance, bool obstacle, params string[] blocks) {
        return reachAlgorithm.getReach(origin, minDistance, maxDistance, obstacle, blocks);
    }

    //return all the hex on reach given a origin and a distance, along with a list of hexagons within range
    public Grid getReachWithBorder(HexGrid origin, int distance, bool obstacle, out Grid border, int borderDistance, params string[] blocks) {
        return getReachWithBorder(origin, 1, distance, obstacle, out border, 1, borderDistance, blocks);
    }
    public Grid getReachWithBorder(HexGrid origin, int distance, bool obstacle, out Grid border, int minBorderDistance, int maxBorderDistance, params string[] blocks) {
        return getReachWithBorder(origin, 1, distance, obstacle, out border, minBorderDistance, maxBorderDistance, blocks);
    }
    public Grid getReachWithBorder(HexGrid origin, int minDistance, int maxDistance, bool obstacle, out Grid border, int borderDistance, params string[] blocks) {
        return getReachWithBorder(origin, minDistance, maxDistance, obstacle, out border, 1, borderDistance, blocks);
    }
    public Grid getReachWithBorder(HexGrid origin, int minDistance, int maxDistance, bool obstacle, out Grid border, int minBorderDistance, int maxBorderDistance, params string[] blocks) {
        return reachAlgorithm.getReachWithBorder(origin, minDistance, maxDistance, obstacle, out border, minBorderDistance, maxBorderDistance, blocks);
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
            if (hex.getState() == state || hex.hasEffect(state))
                return false;
        }

        return true;
    }
}
