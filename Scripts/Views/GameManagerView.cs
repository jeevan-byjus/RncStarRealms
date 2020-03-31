using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Byjus.Gamepod.RncStarRealms.Controllers;

namespace Byjus.Gamepod.RncStarRealms.Views {

    public class GameManagerView : MonoBehaviour, IGameManagerView {
        [SerializeField] Vector2 attackerStartPos;
        [SerializeField] Vector2 attackerStep;
        [SerializeField] Vector2 defenseStartPos;
        [SerializeField] Vector2 defenseStep;
        [SerializeField] Vector2 defenseSizeInc;
        [SerializeField] Vector2 aiAttackerStartPos;
        [SerializeField] Vector2 aiDefenseStartPos;
        [SerializeField] GameObject attackerPrefab;
        [SerializeField] GameObject defensePrefab;

        [SerializeField] Text readyText;

        public IGameManagerCtrl ctrl;

        public void CreateAttacker(bool player, int xIndex, int yIndex, Action<GameObject> onDone) {
            if (player) {
                var pos = attackerStartPos + new Vector2(xIndex * attackerStep.x, yIndex * attackerStep.y);
                var attack = Instantiate(attackerPrefab, pos, Quaternion.identity, transform);

                onDone(attack);

            } else {
                var pos = aiAttackerStartPos + new Vector2(xIndex * attackerStep.x, -yIndex * attackerStep.y);
                var attack = Instantiate(attackerPrefab, pos, Quaternion.identity, transform);
                attack.transform.Rotate(new Vector3(0, 0, 180));

                onDone(attack);
            }
        }

        public void CreateDefense(bool player, int xIndex, int yIndex, Action<GameObject> onDone) {
            if (player) {
                var pos = defenseStartPos + new Vector2(xIndex * defenseStep.x, yIndex * defenseStep.y);
                var defense = Instantiate(defensePrefab, pos, Quaternion.identity, transform);

                var spr = defense.GetComponent<SpriteRenderer>();
                spr.size += defenseSizeInc;

                onDone(defense);

            } else {
                var pos = aiDefenseStartPos + new Vector2(xIndex * defenseStep.x, -yIndex * defenseStep.y);
                var defense = Instantiate(defensePrefab, pos, Quaternion.identity, transform);
                defense.transform.Rotate(new Vector3(0, 0, 180));

                var spr = defense.GetComponent<SpriteRenderer>();
                spr.size += defenseSizeInc;

                onDone(defense);
            }
        }

        public void DestroyShip(bool player, ShipType type, GameObject go, Action onDone) {
            Destroy(go);

            onDone();
        }

        public void StartPlayerTurn(float timer) {
            readyText.text = "READY";

            StartCoroutine(StartTurn(timer));
        }

        IEnumerator StartTurn(float timer) {
            yield return new WaitForSeconds(1);

            while (timer > 0) {
                readyText.text = timer + "";
                yield return new WaitForSeconds(1);

                timer--;
            }

            ctrl.PlayerTurnEnded();
        }

        public void Wait(float time, Action onDone) {
            StartCoroutine(WaitUntil(time, onDone));
        }

        IEnumerator WaitUntil(float time, Action onDone) {
            yield return new WaitForSeconds(time);

            onDone();
        }
    }

    public interface IGameManagerView {
        void CreateAttacker(bool player, int xIndex, int yIndex, Action<GameObject> onDone);
        void CreateDefense(bool player, int xIndex, int yIndex, Action<GameObject> onDone);
        void DestroyShip(bool player, ShipType type, GameObject go, Action onDone);
        void StartPlayerTurn(float timer);
        void Wait(float time, Action onDone);
    }
}