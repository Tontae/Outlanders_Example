using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.Enemy;

namespace Outlander.Player
{
    public class EnemyHpIndicator : PlayerElements
    {
        [SerializeField] private EnemyUIInfo enemyInfo;
        [SerializeField] private LayerMask targetLayer;

        // Start is called before the first frame update
        void Start()
        {
            UIManagers.Instance.playerCanvas.enemyHPBarGO.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer) return;

            RaycastCheck();
            DistanceCheck();

            if (enemyInfo != null)
            {
                if (enemyInfo.isBoss)
                    SetEnemyInfo(enemyInfo);
                else
                    UIManagers.Instance.playerCanvas.enemyHPBarGO.SetActive(false);
            }
            else
            {
                UIManagers.Instance.playerCanvas.enemyHPBarGO.SetActive(false);
            }
        }

        private void RaycastCheck()
        {
            if (Player.PlayerMatchManager.myManager == null) return;
            var cam = Camera.main.transform;

            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 15f, targetLayer))
            {
                //if(Player.PlayerMatchManager.myManager.GameObjectComponents.ContainsKey(hit.collider.gameObject))
                //enemyInfo = (Player.PlayerMatchManager.myManager.GameObjectComponents[hit.collider.gameObject] as Monster_Base).ui;
                if (hit.collider.TryGetComponent(out Monster_Base monster))
                    enemyInfo = monster.ui;
                else
                    enemyInfo = null;
            }
        }

        private void DistanceCheck()
        {
            if (enemyInfo == null) return;

            if (Vector3.Distance(enemyInfo.transform.root.gameObject.transform.position, this.transform.position) > 10f)
            {
                enemyInfo = null;
            }

        }

        public void SetEnemyInfo(EnemyUIInfo info)
        {
            enemyInfo = info;
            if (enemyInfo.HPFillAmount > 0f)
            {
                UIManagers.Instance.playerCanvas.enemyHPBarGO.SetActive(true);
                UIManagers.Instance.playerCanvas.enemyHPBar.fillAmount = enemyInfo.HPFillAmount;
                //enemyName.text = $"Lv.{enemyInfo.level} {enemyInfo.enemyName}";
                UIManagers.Instance.playerCanvas.enemyName.text = $"{enemyInfo.enemyName}";
            }
            else
            {
                UIManagers.Instance.playerCanvas.enemyHPBarGO.SetActive(false);
                enemyInfo = null;
            }
        }
    }
}

