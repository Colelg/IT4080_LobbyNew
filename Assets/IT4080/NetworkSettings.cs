using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

namespace It4080
{
    public class NetworkSettings : MonoBehaviour
    {
        public event Action<IPAddress, ushort> startServer;
        public event Action<IPAddress, ushort> startHost;
        public event Action<IPAddress, ushort> startClient;

        private IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
        private ushort port = 0;

        public Button btnHost;
        public Button btnServer;
        public Button btnClient;
        public TMPro.TMP_InputField inIpAddress;
        public TMPro.TMP_InputField inPort;
        public TMPro.TMP_Text txtStatus;
        public GameObject ControlsContainer;

        private bool showHost = true;
        public bool ShowHost {
            get => showHost;
            set => setShowHost(value);
        }

        private bool showServer = true;
        public bool ShowServer {
            get => showServer;
            set => setShowServer(value);
        }
        
        private void setShowHost(bool val)
        {
            showHost = val;
            btnServer.gameObject.SetActive(val);
        }

        private void setShowServer(bool val){
            showServer = val;
            btnServer.gameObject.SetActive(val);
        }

        void Start()
        {
            btnHost.onClick.AddListener(BtnHostOnClick);
            btnServer.onClick.AddListener(BtnServerOnClick);
            btnClient.onClick.AddListener(BtnClientOnClick);
        }

        private bool populateVars()
        {
            bool isValid = false;

            try {
                ipAddress = IPAddress.Parse(inIpAddress.text);
                port = ushort.Parse(inPort.text);
                isValid = true;
            } catch(Exception e) {
                Debug.LogError($"Ip/Port format:  {e.Message}");
            }

            return isValid;
        }


        // ----------------------
        // Events
        // ----------------------
        private void BtnHostOnClick()
        {
            if (populateVars())
            {
                startHost.Invoke(ipAddress, port);
            }            
        }


        private void BtnServerOnClick()
        {
            if (populateVars())
            {
                startServer.Invoke(ipAddress, port);
            }                
        }


        private void BtnClientOnClick()
        {
            if (populateVars())
            {
                startClient.Invoke(ipAddress, port);
            }                
        }

        // ----------------------
        // Public
        // ----------------------
        public void hide(bool should = true)
        {            
            ControlsContainer.SetActive(!should);
        }

        public void show(bool should = true)
        {
            this.hide(!should);
        }

        public void setStatusText(string msg)
        {
            txtStatus.text = msg;
        }
    }
}
