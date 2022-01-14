using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTool {
    protected string[] blocks; //states or effects that block the graph calculation
    protected bool considerObstacle; //whether or not consider solid hexagons as a block

    protected bool isAvailable(HexGrid hex) {
        foreach (string state in blocks) {
            if (hex.getState() == state || hex.hasEffect(state))
                return false;
        }

        return !considerObstacle || !hex.isEmpty();
    }
}
