using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.Menus
{
    public class ServerRegionEditor : MonoBehaviour
    {
        public Dictionary<string, string> regions = new Dictionary<string, string>()
        {
            {"Asia", "asia"},
            {"Australia", "au"},
            {"Canada", "cae"},
            {"China", "cn"},
            {"Europe", "eu"},
            {"India", "in"},
            {"Japan", "jp"},
            {"South Africa", "za"},
            {"South America", "sa"},
            {"South Korea", "kr"},
            {"Turkey", "tr"},
            {"USA East", "us"},
            {"USA West", "usw"}
        };

        public void ConnectToRegion(string region)
        {
            if (!regions.ContainsKey(region))
                return;

            foreach (KeyValuePair<string, string> pair in regions)
            {
                if (pair.Key != region)
                    continue;
                else
                    PhotonNetwork.ConnectToRegion(pair.Value);
            }
        }
    }
}
