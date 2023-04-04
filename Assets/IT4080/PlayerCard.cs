using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static It4080.Chat;

namespace It4080 {
    public class PlayerCard : MonoBehaviour {
        TMPro.TMP_Text lblPlayerName;
        TMPro.TMP_Text lblScore;
        TMPro.TMP_Text lblStatus;
        Button btnReady;
        Button btnKick;

        public event Action<bool> ReadyToggled;
        public event Action<ulong> KickPlayer;

        private bool isReady = false;
        private ulong clientId = ulong.MaxValue;

        // Start is called before the first frame update
        void Awake() {
            lblPlayerName = transform.Find("Row1/LblPlayerName").GetComponent<TMPro.TMP_Text>();
            lblScore = transform.Find("Row1/LblScore").GetComponent<TMPro.TMP_Text>();
            btnKick = transform.Find("Row1/BtnKick").GetComponent<Button>();
            btnKick.onClick.AddListener(OnBtnKickClicked);

            lblStatus = transform.Find("Row2/LblStatus").GetComponent<TMPro.TMP_Text>();
            btnReady = transform.Find("Row2/BtnReady").GetComponent<Button>();
            btnReady.onClick.AddListener(OnBtnReadyClicked);

            // Force update of buttons and any other actions taken
            // when ready is set.
            SetReady(isReady);
            lblPlayerName.text = "<Not Set>";
            SetScore(-1);
        }


        private void OnBtnReadyClicked() {
            SetReady(!isReady);
            if (ReadyToggled != null) {
                ReadyToggled.Invoke(isReady);
            }
        }


        private void OnBtnKickClicked() {
            if (KickPlayer != null) {
                KickPlayer.Invoke(clientId);
            }
        }


        public void SetScore(int newScore) {
            lblScore.text = $"{newScore} "; // hack in a space
        }


        public void SetPlayerName(string newName) {
            lblPlayerName.text = newName;
        }


        public void SetReady(bool ready) {
            isReady = ready;
            if (isReady) {
                btnReady.GetComponentInChildren<TextMeshProUGUI>().text = "Set Not Ready";
            } else {
                btnReady.GetComponentInChildren<TextMeshProUGUI>().text = "Set Ready";
            }
        }


        public void setClientId(ulong id) {
            clientId = id;
        }


        public ulong getClientId() {
            return clientId;
        }


        public void ShowKick(bool should) {
            btnKick.gameObject.SetActive(should);
        }


        public void ShowReady(bool should) {
            btnReady.gameObject.SetActive(should);
        }

        public void SetStatus(string newStatus)
        {
            lblStatus.text = $"Status:  {newStatus}";
        }
    }
}
