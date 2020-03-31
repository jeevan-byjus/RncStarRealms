using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Byjus.Gamepod.RncStarRealms.Verticals {
    /// <summary>
    /// Standalone variant of the Vision Service
    /// Generates random number of blue and red cubes in a range when queried for objects
    /// </summary>
    public class StandaloneVisionService : IVisionService {

        public void Init() {

        }

        public List<ExtInput> GetVisionObjects() {
            var numRed = Random.Range(0, 20);
            var numBlue = Random.Range(0, 3);

            var ret = new List<ExtInput>();
            for (int i = 0; i < numBlue; i++) {
                ret.Add(new ExtInput {
                    type = TileType.BLUE_ROD,
                    id = i,
                });
            }

            for (int i = 0; i < numRed; i++) {
                ret.Add(new ExtInput {
                    type = TileType.RED_CUBE,
                    id = i + 1000,
                });
            }

            return ret;
        }
    }
}