using UnityEngine;
using Byjus.Gamepod.RncStarRealms.Views;
using Byjus.Gamepod.RncStarRealms.Verticals;
using System.Collections.Generic;

namespace Byjus.Gamepod.RncStarRealms.Controllers {
    public class GameManagerCtrl : IGameManagerCtrl, IExtInputListener {
        public IGameManagerView view;
        List<Ship> shipsInPlay;
        const int maxInLine = 10;
        int xMinIndex;
        int xMaxIndex;
        int yIndex;
        int defenseYIndex;

        public void Init() {
            shipsInPlay = new List<Ship>();
            xMinIndex = 0;
            xMaxIndex = 0;
            yIndex = 0;
        }

        public void OnInputStart() {

        }

        public void OnInputEnd() {

        }

        public void OnRedCubeAdded() {
            Debug.LogError("Starting with red cube: xmin: " + xMinIndex + ", xmax: " + xMaxIndex + ", y: " + yIndex);
            if (xMaxIndex * 2 == maxInLine) {
                yIndex++;
                xMaxIndex = 0;
                xMinIndex = 0;
            }

            if (-xMinIndex <= xMaxIndex) {
                xMinIndex--;
                view.CreateAttacker(xMinIndex, yIndex, (go) => {
                    shipsInPlay.Add(new Ship {
                        go = go,
                        type = ShipType.ATTACK,
                        xIndex = xMinIndex,
                        yIndex = yIndex
                    });
                });
            } else {
                xMaxIndex++;
                view.CreateAttacker(xMaxIndex, yIndex, (go) => {
                    shipsInPlay.Add(new Ship {
                        go = go,
                        type = ShipType.ATTACK,
                        xIndex = xMaxIndex,
                        yIndex = yIndex
                    });
                });
            }

            Debug.LogError("Ending with red cube: xmin: " + xMinIndex + ", xmax: " + xMaxIndex + ", y: " + yIndex);
        }

        public void OnRedCubeRemoved() {
            int toFindX = 0;
            if (-xMinIndex <= xMaxIndex) {
                toFindX = xMaxIndex;
            } else {
                toFindX = xMinIndex;
            }

            var shipInd = shipsInPlay.FindIndex(x => x.yIndex == yIndex && (x.xIndex == toFindX));
            if (shipInd == -1) {
                Debug.LogError("No attack ship to remove");
                return;
            }

            var ship = shipsInPlay[shipInd];
            view.DestroyShip(ShipType.ATTACK, ship.go, () => {
                if (ship.xIndex == xMinIndex) {
                    xMinIndex++;
                } else {
                    xMaxIndex--;
                }

                if (xMaxIndex == 0 && xMinIndex == 0) {
                    if (yIndex > 0) {
                        yIndex--;

                        if (yIndex >= 0) {
                            xMaxIndex = maxInLine / 2;
                            xMinIndex = -maxInLine / 2;
                        }

                    } else {

                    }
                }

                shipsInPlay.RemoveAt(shipInd);
                Debug.LogError("Destroying after cube: xmin: " + xMinIndex + ", xmax: " + xMaxIndex + ", y: " + yIndex);
            });
        }

        public void OnBlueRodAdded() {

        }

        public void OnBlueRodRemoved() {

        }
    }

    public enum ShipType { ATTACK, DEFENSE }

    public class Ship {
        public ShipType type;
        public int xIndex;
        public int yIndex;
        public GameObject go;
    }

    public interface IGameManagerCtrl {
        void Init();
    }
}