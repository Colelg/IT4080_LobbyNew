using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace It4080 {
    public class ConnectedPlayers : MonoBehaviour {
        public PlayerCard playerCardPrefab;
        private Hashtable playerCards = new Hashtable();


        public PlayerCard AddPlayer(string playerName, ulong id) {
            PlayerCard pd = null;
            if (playerCardPrefab == null) {
                Debug.Log("It is null, cannot make panel");
            } else {
                pd = Instantiate(playerCardPrefab);
                pd.transform.SetParent(transform);
                pd.SetPlayerName(playerName);
                pd.setClientId(id);
                playerCards.Add(id, pd);
            }
            return pd;
        }


        public void Clear() {
            playerCards.Clear();

            var kids = transform.GetComponentsInChildren<PlayerCard>();
            foreach (PlayerCard kid in kids) {
                Destroy(kid.gameObject);
            }
        }


        public PlayerCard[] GetPlayerCards() {
            return transform.GetComponentsInChildren<PlayerCard>();
        }


        public PlayerCard GetCardForId(ulong id) {
            PlayerCard toReturn = null;

            if (playerCards.Contains(id)) {
                toReturn = (PlayerCard)playerCards[id];
            }
            return toReturn;
        }
    }

}
