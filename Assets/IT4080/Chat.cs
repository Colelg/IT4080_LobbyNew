using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using System.Net;

namespace It4080 { 
    public class Chat : NetworkBehaviour
    {
        public const string MSG_SYSTEM = "SYSTEM";

        public class ChatMessage
        {
            public string to = null;
            public string from = null;
            public string message = null;
        }

        public Button btnSend;
        public TMPro.TMP_InputField inputMessage;
        public TMPro.TMP_Text txtChatLog;
        public event Action<ChatMessage> sendMessage;

        private ulong clientId = 0;


        public void Start() {
            btnSend.onClick.AddListener(BtnSendOnClick);
            inputMessage.onSubmit.AddListener(InputMessageOnSubmit);
            txtChatLog.text = "Super Chat 64 Plus v2\n ";
        }


        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            enabled = true;
            clientId = NetworkManager.Singleton.LocalClientId;
        }


        private void DisplayMessage(ChatMessage msg)
        {
            string from = msg.from;
            if(from == NetworkManager.Singleton.LocalClientId.ToString())
            {
                from = "you";
            } 

            if(msg.from == MSG_SYSTEM)
            {
                txtChatLog.text += $"<SYS> {msg.message}\n";
            } else
            {
                txtChatLog.text += $"[{from}] {msg.message}\n";
            }            
        }


        private void SendMessage()
        {
            ChatMessage msg = new ChatMessage();
            msg.message = inputMessage.text;
            inputMessage.text = "";
            if(sendMessage != null)
            {
                sendMessage.Invoke(msg);
            }            
        }


        private void InputMessageOnSubmit(string text)
        {
            SendMessage();
        }


        private void BtnSendOnClick()
        {
            SendMessage();
        }


        private void OnEnable() {
            enable(true);
        }


        private void OnDisable() {
            enable(false);
        }


        // ----------------
        // Public
        // ----------------
        public void enable(bool should = true)
        {
            inputMessage.enabled = should;
            btnSend.enabled = should;
        }


        public void ShowMessage(ChatMessage msg)
        {
            DisplayMessage(msg);
        }


        public void SystemMessage(string text)
        {
            ChatMessage msg = new ChatMessage();
            msg.message = text;
            msg.from = MSG_SYSTEM;
            DisplayMessage(msg);
        }
    }
}