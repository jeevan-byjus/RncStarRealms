using System;
using System.Collections.Generic;
using UnityEngine;

namespace Byjus.Gamepod.RncStarRealms.Controllers {
    public class Player {
        public int maxInLine;
        public int shieldValue;
        public int health;
        public int energy;
        public float turnTime;
        public List<Ship> ships;
        public int attXMin;
        public int attXMax;
        public int attY;
        public int defY;

        public Player() {
            maxInLine = 10;
            attXMax = 0;
            attXMin = 0;
            attY = 0;
            defY = 0;
            energy = 0;
            health = 10;
            shieldValue = 10;
            turnTime = 40;
            ships = new List<Ship>();
        }

        public Ship AddAttack() {
            if (attXMax * 2 == maxInLine) {
                attY++;
                attXMax = 0;
                attXMin = 0;
            }

            Ship ret;

            if (-attXMin <= attXMax) {
                attXMin--;
                ret = new Ship { type = ShipType.ATTACK, xIndex = attXMin, yIndex = attY };
            } else {
                attXMax++;
                ret = new Ship { type = ShipType.ATTACK, xIndex = attXMax, yIndex = attY };
            }

            return ret;
        }

        public Ship RemoveAttack() {
            int toFindX = 0;
            if (-attXMin <= attXMax) {
                toFindX = attXMax;
            } else {
                toFindX = attXMin;
            }

            var ship = ships.Find(x => x.type == ShipType.ATTACK && x.yIndex == attY && (x.xIndex == toFindX));
            if (ship == null) {
                throw new Exception("No attack ship to remove");
            }

            if (ship.xIndex == attXMin) {
                attXMin++;
            } else {
                attXMax--;
            }

            if (attXMax == 0 && attXMin == 0) {
                if (attY > 0) {
                    attY--;

                    if (attY >= 0) {
                        attXMax = maxInLine / 2;
                        attXMin = -maxInLine / 2;
                    }

                } else {

                }
            }

            ships.Remove(ship);
            return ship;
        }

        public Ship AddDefense() {
            var ret = new Ship {
                type = ShipType.DEFENSE,
                xIndex = 0,
                yIndex = defY
            };

            ships.Add(ret);
            defY++;

            return ret;
        }

        public Ship RemoveDefense() {
            var toRemove = ships.Find(x => x.type == ShipType.DEFENSE && x.yIndex == defY - 1);
            if (toRemove == null) {
                throw new Exception("No defense ship to remove");
            }

            defY--;
            ships.Remove(toRemove);
            return toRemove;
        }
    }

    public enum ShipType { ATTACK, DEFENSE }

    public class Ship {
        public ShipType type;
        public int xIndex;
        public int yIndex;
        public GameObject go;
    }
}