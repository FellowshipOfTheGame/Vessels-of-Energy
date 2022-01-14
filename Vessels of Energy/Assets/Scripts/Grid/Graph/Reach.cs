using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reach : GraphTool {
    // main function, called by GridManager
    public GridManager.Grid getReach(HexGrid origin, int minDistance, int maxDistance, bool obstacle, params string[] blocks) {
        GridManager.Grid reach = new GridManager.Grid(origin);
        considerObstacle = obstacle;

        List<HexGrid> depth = new List<HexGrid>();
        List<HexGrid> sweep = new List<HexGrid>();
        depth.Add(origin);
        sweep.Add(origin);
        if (minDistance < 1) reach.grid.Add(new GridManager.GridPoint(origin, 0));

        this.blocks = blocks;
        reach = calcReach(reach, depth, sweep, minDistance, maxDistance, 1);
        return reach;
    }

    // auxiliar function to go depth by depth
    GridManager.Grid calcReach(GridManager.Grid reach, List<HexGrid> currDepth, List<HexGrid> sweep, int min, int max, int count) {
        if (count > max || currDepth.Count == 0) return reach;

        List<HexGrid> nextDepth = new List<HexGrid>();
        foreach (HexGrid hex in currDepth) {
            foreach (HexGrid neighbor in hex.neighbors) {
                if (!sweep.Contains(neighbor) && isAvailable(neighbor)) {
                    nextDepth.Add(neighbor);
                    sweep.Add(neighbor);
                    if (count >= min) reach.grid.Add(new GridManager.GridPoint(neighbor, count));
                }
            }
        }

        return calcReach(reach, nextDepth, sweep, min, max, count + 1);
    }

    // alternative that also gives solid objects within range, called by GridManager
    public GridManager.Grid getReachWithBorder(HexGrid origin, int minDistance, int maxDistance, bool obstacle, out GridManager.Grid border, int minBorderDistance, int maxBorderDistance, params string[] blocks) {
        GridManager.Grid reach = new GridManager.Grid(origin);
        GridManager.Grid solid = new GridManager.Grid(origin);
        considerObstacle = obstacle;

        List<HexGrid> depth = new List<HexGrid>();
        List<HexGrid> sweep = new List<HexGrid>();
        depth.Add(origin);
        sweep.Add(origin);
        if (minDistance < 1) reach.grid.Add(new GridManager.GridPoint(origin, 0));

        this.blocks = blocks;
        List<HexGrid> sweepBorder = new List<HexGrid>();
        reach = calcReachWithBorder(reach, depth, sweep, solid, minDistance, maxDistance, 1, out border, minBorderDistance, maxBorderDistance);
        return reach;
    }

    // auxiliar function to go depth by depth and register any solid object along the way
    GridManager.Grid calcReachWithBorder(GridManager.Grid reach, List<HexGrid> currDepth, List<HexGrid> sweep, GridManager.Grid solid, int min, int max, int count, out GridManager.Grid border, int minBorder, int maxBorder) {
        if (currDepth.Count == 0) {
            border = solid;
            return reach;
        }
        if (count > max) {
            considerObstacle = false;
            border = calcReach(solid, currDepth, sweep, minBorder, maxBorder, 1); ;
            return reach;
        }

        List<HexGrid> nextDepth = new List<HexGrid>();
        foreach (HexGrid hex in currDepth) {
            foreach (HexGrid neighbor in hex.neighbors) {
                if (!sweep.Contains(neighbor)) {
                    if (isAvailable(neighbor)) {
                        nextDepth.Add(neighbor);
                        sweep.Add(neighbor);
                        if (count >= min) reach.grid.Add(new GridManager.GridPoint(neighbor, count));
                    } else if (count >= minBorder) {
                        solid.grid.Add(new GridManager.GridPoint(neighbor, count));
                    }
                }
            }
        }

        return calcReachWithBorder(reach, nextDepth, sweep, solid, min, max, count + 1, out border, minBorder, maxBorder);
    }
}
