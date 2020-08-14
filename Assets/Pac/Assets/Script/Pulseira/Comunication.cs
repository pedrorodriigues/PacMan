using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Comunication : MonoBehaviour {

    //FoundDeviceListScript2 foundDeviceListScript;

    public GameObject connectButton;
    public List<string> Services;
    public List<string> Characteristics;

    public void OnScanClick()
    {
        BSNHardwareInterface.FindBSN();


       
       
       //FOR TEST BSNHARDWAREINTERFACE

        // //List BT
        // int buttonID1 = 0;
        // foreach (DeviceObject device in FoundDeviceListScript2.DeviceAddressList)
        // {
        //     //Buttons[buttonID++].text = device.Name;
        //     Debug.Log(device.Name);
        //     if (buttonID1 == 10)
        //         break;
        // }
    }

    string FullUUID(string uuid)
    {
        if (uuid.Length == 4)
            return "E40F" + uuid + "-1FBF-11E8-B467-0ED5F89F718B";

        return uuid;
    }

    public void OnConnectClick()
    {

        BSNHardwareInterface.ConnectBSN();

        //FOR TEST BSNHARDWAREINTERFACE
        // Debug.Log("TENTANDO CONECTAR");
        // DeviceObject device = null;

        // int buttonID = 0;

        // foreach (DeviceObject deviceBiox in FoundDeviceListScript2.DeviceAddressList)
        // {
        //     //Buttons[buttonID++].text = device.Name
        //     if (deviceBiox.Name.Contains("Biox") || deviceBiox.Name.Contains("BIOX") || deviceBiox.Name.Contains("bsn") || deviceBiox.Name.Contains("BSN"))
        //     {
        //         device = deviceBiox;
        //         buttonID++;
        //         break;
        //     }

        // }
        // //Text button = Buttons[buttonID];
        // //device = FoundDeviceListScript.DeviceAddressList[buttonID];
        // //string subscribedService = Services[buttonID];
        // //string subscribedCharacteristic = Characteristics[buttonID];

        // if (device != null)
        // {
        //     BluetoothLEHardwareInterface.ConnectToPeripheral(device.Address, (address) => {

        //     }, null, (address, service, characteristic) => {

        //         Debug.Log("VTNC A " + address + " VTNC S " + service + " VTNC C " + characteristic);

        //         if (string.IsNullOrEmpty(Services[buttonID]) && string.IsNullOrEmpty(Characteristics[buttonID]))
        //         {
        //             Debug.Log("Entrou!!!");
        //             Services[buttonID] = FullUUID(service);
        //             Characteristics[buttonID] = FullUUID(characteristic);
        //             //button.text = device.Name + " connected";
        //         }

        //     }, null);


        //     /*
        //     if (button.text.Contains("connected"))
        //     {
        //         if (!string.IsNullOrEmpty(subscribedService) && !string.IsNullOrEmpty(subscribedCharacteristic))
        //         {
        //             BluetoothLEHardwareInterface.UnSubscribeCharacteristic(device.Address, subscribedService, subscribedCharacteristic, (characteristic) => {

        //                 Services[buttonID] = null;
        //                 Characteristics[buttonID] = null;

        //                 BluetoothLEHardwareInterface.DisconnectPeripheral(device.Address, (disconnectAddress) => {

        //                    // button.text = device.Name;
        //                 });
        //             });
        //         }
        //         else
        //         {
        //             BluetoothLEHardwareInterface.DisconnectPeripheral(device.Address, (disconnectAddress) => {

        //                 //button.text = device.Name;
        //             });
        //         }
        //     }
        //     else
        //     {
        //         BluetoothLEHardwareInterface.ConnectToPeripheral(device.Address, (address) => {

        //         }, null, (address, service, characteristic) => {

        //             if (string.IsNullOrEmpty(Services[buttonID]) && string.IsNullOrEmpty(Characteristics[buttonID]))
        //             {
        //                 Services[buttonID] = FullUUID(service);
        //                 Characteristics[buttonID] = FullUUID(characteristic);
        //                 //button.text = device.Name + " connected";
        //             }

        //         }, null);
        //     }*/
        // }


       

    }


    // Use this for initialization

    //FOR TEST BSNHARDWAREINTERFACE
    // void Start () {
    //     //get BT


    //     BluetoothLEHardwareInterface.Initialize(true, false, () => {

    //         FoundDeviceListScript2.DeviceAddressList = new List<DeviceObject>();

    //         BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {

    //             FoundDeviceListScript2.DeviceAddressList.Add(new DeviceObject(address, name));

    //         }, null);

    //     }, (error) => {

    //         BluetoothLEHardwareInterface.Log("BLE Error: " + error);

    //     });


    //     //Debug.Log(">>>>>>" + FoundDeviceListScript2.DeviceAddressList.Count);
        
    // }


    // Update is called once per frame
    void Update () {
        if (BSNHardwareInterface.bsnDevice != null){
            connectButton.SetActive(true);
        }
        //Debug.Log("OLA");
	}
}
