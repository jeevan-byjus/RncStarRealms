using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Byjus.Gamepod.RncStarRealms.Util;

namespace Byjus.Gamepod.RncStarRealms.Verticals {
    public class InputParser : MonoBehaviour {
        public IExtInputListener inputListener;

        IVisionService visionService;
        int inputCount;

        List<ExtInput> currentObjects;

        public void Init() {
            visionService = Factory.GetVisionService();
            inputCount = 0;
            currentObjects = new List<ExtInput>();

            StartCoroutine(ListenForInput());
        }

        IEnumerator ListenForInput() {
            yield return new WaitForSeconds(Constants.INPUT_DELAY);
            inputCount++;
            
            inputListener.OnInputStart();
            var objs = visionService.GetVisionObjects();
            Process(objs);
            inputListener.OnInputEnd();

            StartCoroutine(ListenForInput());
        }

        void Process(List<ExtInput> objs) {
            objs.Sort((x, y) => x.id - y.id);

            Segregate(objs, out List<ExtInput> oldObjects, out List<ExtInput> commonObjects, out List<ExtInput> newObjects);

            Debug.LogError("Input count: " + inputCount + ", Count: " + objs.Count + ", New: " + newObjects.Count + ", Old: " + oldObjects.Count);

            foreach (var old in oldObjects) {
                if (old.type == TileType.BLUE_ROD) {
                    inputListener.OnBlueRodRemoved();
                } else {
                    inputListener.OnRedCubeRemoved();
                }
            }

            foreach (var newO in newObjects) {
                if (newO.type == TileType.BLUE_ROD) {
                    inputListener.OnBlueRodAdded();
                } else {
                    inputListener.OnRedCubeAdded();
                }
            }

            currentObjects = objs;
        }

        void Segregate(List<ExtInput> visionObjects, out List<ExtInput> oldObjects, out List<ExtInput> commonObjects, out List<ExtInput> newObjects) {
            oldObjects = new List<ExtInput>();
            commonObjects = new List<ExtInput>();
            newObjects = new List<ExtInput>();

            int i = 0, j = 0;
            while (i < currentObjects.Count && j < visionObjects.Count) {
                var curr = currentObjects[i];
                var newO = visionObjects[j];
                if (curr.id == newO.id) {
                    commonObjects.Add(curr);
                    i++;
                    j++;
                } else if (curr.id > newO.id) {
                    newObjects.Add(newO);
                    j++;
                } else {
                    oldObjects.Add(curr);
                    i++;
                }
            }
            while (i < currentObjects.Count) {
                oldObjects.Add(currentObjects[i]);
                i++;
            }
            while (j < visionObjects.Count) {
                newObjects.Add(visionObjects[j]);
                j++;
            }
        }
    }

    public interface IExtInputListener {
        void OnInputStart();
        void OnRedCubeAdded();
        void OnRedCubeRemoved();
        void OnBlueRodAdded();
        void OnBlueRodRemoved();
        void OnInputEnd();
    }

}