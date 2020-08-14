using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comunication2 : MonoBehaviour {
    public InputField Send;
    private string _connectedID = null;
    private string _serviceUUID = "0300";
    private string _serviceUUID2 = "0400";
    private string _readCharacteristicUUID = "0407";
    private string _readCharacteristicUUIDQuat = "0401";
    private string _writeCharacteristicUUID = "0305";



    string FullUUID(string uuid)
    {
        if (uuid.Length == 4)
            return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";

        return uuid;
    }

    string FullUUID2(string uuid)
    {
        if (uuid.Length == 4)
            return "E40F" + uuid + "-1FBF-11E8-B467-0ED5F89F718B";

        return uuid;
    }

    void SendBytes(byte[] data)
    {
        BluetoothLEHardwareInterface.Log(string.Format("data length: {0} uuid: {1}", data.Length.ToString(), FullUUID2(_writeCharacteristicUUID)));
        BluetoothLEHardwareInterface.WriteCharacteristic(_connectedID, FullUUID2(_serviceUUID), FullUUID2(_writeCharacteristicUUID), data, data.Length, true, (characteristicUUID) => {

            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
        Debug.Log("ESCREVEU COM SUCESSO");
    }

    public void Enviar()
    {
        Debug.Log("TENTANDO ENVIAR");
        DeviceObject device = null;
        int buttonID = 0;

        foreach (DeviceObject deviceBiox in FoundDeviceList.DeviceAddressList)
        {
            //Buttons[buttonID++].text = device.Name
            //RECEBER COMO PARAMETRO O DEVICE
            if (deviceBiox.Name.Contains("Biox") || deviceBiox.Name.Contains("BIOX") || deviceBiox.Name.Contains("bsn") || deviceBiox.Name.Contains("BSN"))
            {
                device = deviceBiox;
                buttonID++;
                break;
            }
        }
        Debug.Log("NOME = " + device.Name);
        Debug.Log("ADRESS = " + device.Address);
        _connectedID = device.Address;
        
        Debug.Log("TEXTO DO INPUT = " + Send.text);
		byte[] bytes = { 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
		if (bytes.Length > 0)
        {
			Debug.Log("ENVIOU BYTES : " + bytes);
            Debug.Log("ENVIOU BYTES STRING : " + Encoding.ASCII.GetString(bytes));
            Debug.Log("ENVIOU BYTES STRING : " + Encoding.Default.GetString(bytes));
            Debug.Log("ENVIOU BYTES Lenght : " + bytes.Length);
            SendBytes(bytes);
        }
        //Send.text = "";
        
    }


    public void Receber()
    {
        Debug.Log("ENTROU NO RECEBER");
        DeviceObject device = null;
        int buttonID = 0;

        foreach (DeviceObject deviceBiox in FoundDeviceList.DeviceAddressList)
        {
            //Buttons[buttonID++].text = device.Name
            if (deviceBiox.Name.Contains("Biox") || deviceBiox.Name.Contains("BIOX") || deviceBiox.Name.Contains("bsn") || deviceBiox.Name.Contains("BSN"))
            {
                device = deviceBiox;
                buttonID++;
                break;
            }
        }
        _connectedID = device.Address;

        //byte[] bytes = { 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        Debug.Log("ANTES DO READ");
        //BluetoothLEHardwareInterface.ReadCharacteristic(_connectedID, FullUUID2(_serviceUUID), FullUUID2(_readCharacteristicUUID), (deviceAddress, data) => {
        /*
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedID, FullUUID2(_serviceUUID), FullUUID2(_readCharacteristicUUID), (deviceAddress, notification) => {


            Debug.Log("TENTOU RECEBER");
            BluetoothLEHardwareInterface.Log("id: " + _connectedID);
            if (deviceAddress.CompareTo(_connectedID) == 0)
            {
                BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                if (data.Length == 0)
                {
                    Debug.Log("NAO TEM DADOS");
                }
                else
                {
                    Debug.Log("TEM DADOS");
                    string s = ASCIIEncoding.UTF8.GetString(data);
                    BluetoothLEHardwareInterface.Log("data: " + s);
                    Debug.Log("PASSOS = "+s);
                }
            }

        });*/

        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedID, FullUUID2(_serviceUUID2), FullUUID2(_readCharacteristicUUID), (deviceAddress, notification) => {

        }, (deviceAddress2, characteristic, data) => {

            BluetoothLEHardwareInterface.Log("id: " + _connectedID);
            Debug.Log("DENTRO");
            if (deviceAddress2.CompareTo(_connectedID) == 0)
            {
                BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                if (data.Length == 0)
                {
                }
                else
                {
                    int passo = data[0];
                    Debug.Log("DATA = " + ((data[1]*256)+ data[0]));
                    Debug.Log("DATA LENGTH = " + data.Length);
                    string s = ASCIIEncoding.UTF8.GetString(data);
                    BluetoothLEHardwareInterface.Log("data: " + s);
                    Debug.Log("PASSOS 1 = " + s);
                    s = ASCIIEncoding.ASCII.GetString(data);
                    Debug.Log("PASSOS 2 = " + s);
                    s = ASCIIEncoding.UTF32.GetString(data);
                    Debug.Log("PASSOS 3 = " + s);
                    s = ASCIIEncoding.Unicode.GetString(data);
                    Debug.Log("PASSOS 4 = " + s);
                    s = ASCIIEncoding.UTF7.GetString(data);
                    Debug.Log("PASSOS 5 = " + s);
                    s = BitConverter.ToString(data);
                    Debug.Log("PASSOS HEX = " + s);
                    ushort a = BitConverter.ToUInt16(data,2);
                    Debug.Log("PASSOS INT = " + a);
                    ushort ap = BitConverter.ToUInt16(data, 2);
                    Debug.Log("PASSOS INT = " + ap);
                }
            }

        });
        Debug.Log("DEPOIS DO READ");
    }





    public void ReceberQuarternos()
    {
        Debug.Log("ENTROU NO RECEBER QUATERNOS");
        DeviceObject device = null;
        int buttonID = 0;

        foreach (DeviceObject deviceBiox in FoundDeviceList.DeviceAddressList)
        {
            //Buttons[buttonID++].text = device.Name
            if (deviceBiox.Name.Contains("Biox") || deviceBiox.Name.Contains("BIOX") || deviceBiox.Name.Contains("bsn") || deviceBiox.Name.Contains("BSN"))
            {
                device = deviceBiox;
                buttonID++;
                break;
            }
        }
        _connectedID = device.Address;

        //byte[] bytes = { 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        Debug.Log("ANTES DO READ QUATERNOS");
        //BluetoothLEHardwareInterface.ReadCharacteristic(_connectedID, FullUUID2(_serviceUUID), FullUUID2(_readCharacteristicUUID), (deviceAddress, data) => {
        /*
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedID, FullUUID2(_serviceUUID), FullUUID2(_readCharacteristicUUID), (deviceAddress, notification) => {


            Debug.Log("TENTOU RECEBER");
            BluetoothLEHardwareInterface.Log("id: " + _connectedID);
            if (deviceAddress.CompareTo(_connectedID) == 0)
            {
                BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                if (data.Length == 0)
                {
                    Debug.Log("NAO TEM DADOS");
                }
                else
                {
                    Debug.Log("TEM DADOS");
                    string s = ASCIIEncoding.UTF8.GetString(data);
                    BluetoothLEHardwareInterface.Log("data: " + s);
                    Debug.Log("PASSOS = "+s);
                }
            }

        });*/

        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedID, FullUUID2(_serviceUUID2), FullUUID2(_readCharacteristicUUIDQuat), (deviceAddress, notification) => {

        }, (deviceAddress2, characteristic, data) => {

            BluetoothLEHardwareInterface.Log("id: " + _connectedID);
            Debug.Log("DENTRO");
            if (deviceAddress2.CompareTo(_connectedID) == 0)
            {
                BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                if (data.Length == 0)
                {
                }
                else
                {
                    int passo = data[0];
                   // Debug.Log("DATA = " + ((data[1] * 256) + data[0]));
                    Debug.Log("DATA LENGTH = " + data.Length);
                    string s = ASCIIEncoding.UTF8.GetString(data);
                   BluetoothLEHardwareInterface.Log("data: " + s);
                    Debug.Log("PASSOS 1 = " + s);
                    s = ASCIIEncoding.ASCII.GetString(data);
                    Debug.Log("PASSOS 2 = " + s);
                    /*  s = ASCIIEncoding.UTF32.GetString(data);
                     Debug.Log("PASSOS 3 = " + s);
                     s = ASCIIEncoding.Unicode.GetString(data);
                     Debug.Log("PASSOS 4 = " + s);
                     s = ASCIIEncoding.UTF7.GetString(data);
                     Debug.Log("PASSOS 5 = " + s);*/
                    s = BitConverter.ToString(data);
                    Debug.Log("PASSOS HEX = " + s);
                    // ushort a = BitConverter.ToUInt16(data, 2);
                    // Debug.Log("PASSOS INT = " + a);
                    // ushort ap = BitConverter.ToUInt16(data, 2);
                    // Debug.Log("PASSOS INT = " + ap);
                    s = Convert.ToString(data[3], 2).PadLeft(8, '0');
                    Debug.Log("MEUS BITS = " + s);
                    Debug.Log("MEUS BITS = " + s.ToString());
                }
            }

        });
        Debug.Log("DEPOIS DO READ QUATERNOS");
    }
    // Update is called once per frame
    void Update () {
		
	}


    public void receberVetorGravidade()
    {
        BSNHardwareInterface.ReceiveGravityVector();
    }

    public void receberBateria()
    {
        BSNHardwareInterface.ReceiveBatteryStatus();
    }

    public void receberEuler()
    {
        //TODO OLHAR O PORQUE O VETOR DE GRAVIDADE PRECISA SER CHAMADO AQUI E NAO NO START DO SCRIPT DELE
       // BSNHardwareInterface.ReceiveGravityVector();
        BSNHardwareInterface.ReceiveEuler();
        // BSNHardwareInterface.ReceiveRawData();
        // BSNHardwareInterface.ReceiveQuaternions();
        //SceneManager.LoadScene("EstudoV3");
    }

    public void startDebugger()
    {
        //TODO OLHAR O PORQUE O VETOR DE GRAVIDADE PRECISA SER CHAMADO AQUI E NAO NO START DO SCRIPT DELE
        // BSNHardwareInterface.ReceiveGravityVector();
        //BSNHardwareInterface.ReceiveEuler();
        // BSNHardwareInterface.ReceiveRawData();
        // BSNHardwareInterface.ReceiveQuaternions();
        SceneManager.LoadScene("EstudoV3");
    }


}
