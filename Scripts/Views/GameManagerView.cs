using UnityEngine;
using System;
using Byjus.Gamepod.RncStarRealms.Controllers;

namespace Byjus.Gamepod.RncStarRealms.Views {

    public class GameManagerView : MonoBehaviour, IGameManagerView {
        [SerializeField] Vector2 attackerStartPos;
        [SerializeField] Vector2 attackerStep;
        [SerializeField] Vector2 defenseStartPos;
        [SerializeField] Vector2 defenseStep;
        [SerializeField] Vector2 defenseSizeInc;
        [SerializeField] GameObject attackerPrefab;
        [SerializeField] GameObject defensePrefab;

        public IGameManagerCtrl ctrl;

        public void CreateAttacker(int xIndex, int yIndex, Action<GameObject> onDone) {
            var pos = attackerStartPos + new Vector2(xIndex * attackerStep.x, yIndex * attackerStep.y);
            var attack = Instantiate(attackerPrefab, pos, Quaternion.identity, transform);

            onDone(attack);
        }

        public void CreateDefense(int xIndex, int yIndex, Action<GameObject> onDone) {
            var pos = defenseStartPos + new Vector2(xIndex * defenseStep.x, yIndex * defenseStep.y);
            var defense = Instantiate(defensePrefab, pos, Quaternion.identity, transform);

            var spr = defense.GetComponent<SpriteRenderer>();
            spr.size += defenseSizeInc;

            onDone(defense);
        }

        public void DestroyShip(ShipType type, GameObject go, Action onDone) {
            Destroy(go);

            onDone();
        }
    }

    public interface IGameManagerView {
        void CreateAttacker(int xIndex, int yIndex, Action<GameObject> onDone);
        void CreateDefense(int xIndex, int yIndex, Action<GameObject> onDone);
        void DestroyShip(ShipType type, GameObject go, Action onDone);
    }
}