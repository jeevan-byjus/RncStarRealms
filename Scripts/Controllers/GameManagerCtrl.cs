using UnityEngine;
using Byjus.Gamepod.RncStarRealms.Views;
using Byjus.Gamepod.RncStarRealms.Verticals;
using System.Collections.Generic;

namespace Byjus.Gamepod.RncStarRealms.Controllers {
    public class GameManagerCtrl : IGameManagerCtrl, IExtInputListener {
        public IGameManagerView view;

        Player player;
        Player ai;

        bool playerTurn;
        List<Vector2> aiActions;
        int turnNumber;

        public void Init() {
            player = new Player();
            player.turnTime = 60;

            ai = new Player();
            ai.turnTime = 5;

            playerTurn = false;

            aiActions = new List<Vector2> {
                new Vector2(1, 0),
                new Vector2(1, 5),
                new Vector2(0, 8),
                new Vector2(0, 7),
                new Vector2(1, 0),
                new Vector2(0, 5),
                new Vector2(1, 5),
                new Vector2(0, 10)
            };

            turnNumber = 0;
            DoAITurn();
        }

        void DoAITurn() {
            foreach (var ship in ai.ships) {
                if (ship.type == ShipType.ATTACK) { RemoveAttack(ai, false); } else { RemoveDefense(ai, false); }
            }

            var num = aiActions[turnNumber % aiActions.Count];

            for (int i = 0; i < num.x; i++) {
                CreateDefense(ai, false);
            }

            for (int i = 0; i < num.y; i++) {
                CreateAttack(ai, false);
            }

            view.Wait(ai.turnTime, () => {
                playerTurn = true;
                view.StartPlayerTurn(player.turnTime);
            });
        }

        public void PlayerTurnEnded() {
            playerTurn = false;

            turnNumber++;
            DoAITurn();
        }

        public void OnInputStart() {

        }

        public void OnInputEnd() {

        }

        public void OnRedCubeAdded() {
            if (!playerTurn) { return; }
            CreateAttack(player, true);
        }

        public void OnRedCubeRemoved() {
            if (!playerTurn) { return; }
            RemoveAttack(player, true);
        }

        public void OnBlueRodAdded() {
            if (!playerTurn) { return; }
            CreateDefense(player, true);
        }

        public void OnBlueRodRemoved() {
            if (!playerTurn) { return; }
            RemoveDefense(player, true);
        }


        void CreateAttack(Player pl, bool isPlayer) {
            int playerXMin = pl.attXMin;
            int playerXMax = pl.attXMax;
            var ship = pl.AddAttack();

            if (playerXMin != pl.attXMin) {
                view.CreateAttacker(isPlayer, pl.attXMin, pl.attY, (go) => { ship.go = go; });
            } else {
                view.CreateAttacker(isPlayer, pl.attXMax, pl.attY, (go) => { ship.go = go; });
            }
        }

        void RemoveAttack(Player pl, bool isPlayer) {
            var ship = pl.RemoveAttack();
            view.DestroyShip(true, ShipType.ATTACK, ship.go, () => { });
        }

        void CreateDefense(Player pl, bool isPlayer) {
            var ship = pl.AddDefense();
            view.CreateDefense(isPlayer, ship.xIndex, ship.yIndex, (go) => { ship.go = go; });
        }

        void RemoveDefense(Player pl, bool isPlayer) {
            var ship = pl.RemoveDefense();
            view.DestroyShip(true, ShipType.DEFENSE, ship.go, () => { });
        }
    }

    public interface IGameManagerCtrl {
        void Init();
        void PlayerTurnEnded();
    }
}