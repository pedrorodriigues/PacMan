using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BSNHardwareInterface : MonoBehaviour {

    //Services available at BSN
    private static string _connectedID = null;
    private static string _serviceWriteUUID = "0300";
    private static string _serviceReadUUID = "0400";

    private static string _serviceBatteryUUID = "180F";


    //Characteristics available for writing
    private static string _writeStepCalibrationUUID = "0305";
    //Characteristics available for reading
    private static string _readQuaternionUUID = "0401";
    private static string _readRawDataUUID = "0402";
    private static string _readEulerUUID = "0403";
    private static string _readRotationMatrixUUID = "0404";
    private static string _readCompassUUID = "0405";
    private static string _readGravityVectorUUID = "0406";
    private static string _readStepUUID = "0407";
    private static string _readBatteryUUID = "2A19";
    public static DeviceObject bsnDevice = null;


    //Complete service with BSN address
    private static string FullBSNUUID(string uuid){
        if (uuid.Length == 4)
            return "E40F" + uuid + "-1FBF-11E8-B467-0ED5F89F718B";

        return uuid;
    }

    private static string FullBatteryBSNUUID(string uuid)
    {
        if (uuid.Length == 4)
            return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";

        return uuid;
    }

	//Start Bluetooth Low Energy and search BSN devices
    public static void FindBSN(){
        Debug.Log("TENTANDO ESCANEAR");
        FoundDeviceList.DeviceAddressList = new List<DeviceObject>();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
            {
                Debug.Log("ADRESS = " + address + " NAME = " + name);
                if (name.Contains("Biox") || name.Contains("BIOX") || name.Contains("bsn") || name.Contains("BSN"))
                {
                    bsnDevice = new DeviceObject(address, name);
                    FoundDeviceList.DeviceAddressList.Add(bsnDevice);
                    

                }
            }, null);
        }, (error) =>
        {
        });
        Debug.Log("FINALIZOU ESCANEAR");
        Debug.Log(">>>>>>" + FoundDeviceList.DeviceAddressList.Count);
    }



    public static void ConnectBSN(){

        foreach (DeviceObject deviceBiox in FoundDeviceList.DeviceAddressList)
        {
            //Buttons[buttonID++].text = device.Name
            if (deviceBiox.Name.Contains("Biox") || deviceBiox.Name.Contains("BIOX") || deviceBiox.Name.Contains("bsn") || deviceBiox.Name.Contains("BSN"))
            {
                bsnDevice = deviceBiox;
                Debug.Log(bsnDevice.Name);
                BluetoothLEHardwareInterface.ConnectToPeripheral(bsnDevice.Address, (address) =>
                {

                }, null, (address, service, characteristic) =>
                {

                    Debug.Log("Adress: " + address + " Service: " + service + " Characteristic: " + characteristic);

                }, null);
            }

        }

       

    }


    //Send a byte array to BSN
    public static void SendBytesBSN(byte[] data, string bsnAddress, string serviceUUID, string writeCharacteristicUUID){

        BluetoothLEHardwareInterface.Log(string.Format("data length: {0} uuid: {1}", data.Length.ToString(), FullBSNUUID(writeCharacteristicUUID)));
        BluetoothLEHardwareInterface.WriteCharacteristic(bsnAddress, FullBSNUUID(serviceUUID), FullBSNUUID(writeCharacteristicUUID), data, data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
        Debug.Log("ESCREVEU COM SUCESSO");
    }


    //Calibrate the step identifier of BSN
    public static void CalibrateBSN(){
        Debug.Log("TENTANDO CALIBRAR"); 
		Debug.Log("NOME = " + bsnDevice.Name);
		Debug.Log("ADRESS = " + bsnDevice.Address);
		byte[] bytes = { 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        SendBytesBSN(bytes, bsnDevice.Address, _serviceWriteUUID, _writeStepCalibrationUUID);
        Debug.Log("FIM DA CALIBRACAO");
    }

    //Receive Quaternions
    public static double[] ReceiveQuaternions(){

        double[] quaternions = new double[4];
        Debug.Log("ENTROU NO RECEBER QUATERNOS");
        Debug.Log("ANTES DO READ QUATERNOS");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readQuaternionUUID), (deviceAddress, notification) =>
        {

        }, (deviceAddress2, characteristic, data) =>
        {
            //Receive W data
            byte[] dataW = { data[0], data[1], data[2], data[3] };
            quaternions[0] = System.BitConverter.ToInt32(dataW, 0) / 65536.0;   // 44100.0
           // Debug.Log("W = " + quaternions[0]);

            //Receive X data
            byte[] dataX = { data[4], data[5], data[6], data[7] };
            quaternions[1] = System.BitConverter.ToInt32(dataX, 0) / 65536.0;   // 44100.0
           // Debug.Log("X = " + quaternions[1]);

            //Receive Y data
            byte[] dataY = { data[8], data[9], data[10], data[11] };
            quaternions[2] = System.BitConverter.ToInt32(dataY, 0) / 65536.0;   // 44100.0
           // Debug.Log("Y = " + quaternions[2]);

            //Receive Z data
            byte[] dataZ = { data[12], data[13], data[14], data[15] };
            quaternions[3] = System.BitConverter.ToInt32(dataZ, 0) / 65536.0;   // 44100.0
            //Debug.Log("Z = " + quaternions[3]);

            // Debug.Log("ENVIOU BYTES : " + data);
            // Debug.Log("ENVIOU BYTES STRING : " + Encoding.ASCII.GetString(data));
            // Debug.Log("ENVIOU BYTES STRING : " + Encoding.Default.GetString(data));
            // Debug.Log("ENVIOU BYTES Lenght : " + data.Length);
        });
        Debug.Log("DEPOIS DO READ QUATERNOS");

        return quaternions;
    }




    //Receive raw data of sensors (Accelerometer, Gyroscope, Compass)
    public static List<double[]> ReceiveRawData()
    {
        List<double[]> rawData = new List<double[]>();
        double[] accelerometer = new double[3];
        double[] gyroscope = new double[3];
        double[] compass = new double[3];
        rawData.Add(accelerometer);
        rawData.Add(gyroscope);
        rawData.Add(compass);
        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readRawDataUUID), (deviceAddress, notification) =>
        {
        }, (deviceAddress2, characteristic, data) =>
        {
			string aX,aY,aZ;
				//Receive Y data
				char[] acelX =  (System.Convert.ToString(System.Int32.Parse(data[0].ToString()), 2).PadLeft(8, '0') + System.Convert.ToString(System.Int32.Parse(data[1].ToString()), 2).PadLeft(8, '0')).ToCharArray() ;
				int accvarX = binTwosComplementToSignedDecimal(acelX, 16);
				accelerometer[0] = (accvarX );   // 44100.0
				byte[] dataX = { data[0], data[1] };
				aX = "X% = " + accelerometer[0]+" TESTE X"+ System.BitConverter.ToUInt16(dataX,0) +" X = "+accvarX +" DATA 0 = "+System.Convert.ToString(System.Int32.Parse(data[0].ToString()), 2).PadLeft(8, '0')+ " DATA 1 = "+System.Convert.ToString(System.Int32.Parse(data[1].ToString()), 2).PadLeft(8, '0');
			
            //Receive Y data
				char[] acelY = (System.Convert.ToString(System.Int32.Parse(data[2].ToString()), 2).PadLeft(8, '0') + System.Convert.ToString(System.Int32.Parse(data[3].ToString()), 2).PadLeft(8, '0')).ToCharArray() ;
				int accvarY = binTwosComplementToSignedDecimal(acelY, 16);

				accelerometer[1] = (accvarY);  // 44100.0
				aY ="Y% = " + accelerometer[1]+" Y = "+accvarY+" DATA 2 = "+System.Convert.ToString(System.Int32.Parse(data[2].ToString()), 2).PadLeft(8, '0')+ " DATA 3 = "+System.Convert.ToString(System.Int32.Parse(data[3].ToString()), 2).PadLeft(8, '0') ;

            //Receive Z data
				char[] acelZ =  (System.Convert.ToString(System.Int32.Parse(data[4].ToString()), 2).PadLeft(8, '0') + System.Convert.ToString(System.Int32.Parse(data[5].ToString()), 2).PadLeft(8, '0')).ToCharArray() ;
				int accvarZ = binTwosComplementToSignedDecimal(acelZ, 16);

				accelerometer[2] = ( accvarZ);  // 44100.0
				aZ ="Z% = " + accelerometer[2]+" Z = "+accvarZ +" DATA 4 = "+System.Convert.ToString(System.Int32.Parse(data[4].ToString()), 2).PadLeft(8, '0')+ " DATA 5 = "+System.Convert.ToString(System.Int32.Parse(data[5].ToString()), 2).PadLeft(8, '0') ;

			//Debug.Log(aX+" "+aY +" "+ aZ);

			string gX,gY,gZ;
				char[] gyroX = (System.Convert.ToString(System.Int32.Parse(data[6].ToString()), 2).PadLeft(8, '0') + System.Convert.ToString(System.Int32.Parse(data[7].ToString()), 2).PadLeft(8, '0')).ToCharArray() ;
				gyroscope[0] = binTwosComplementToSignedDecimal(gyroX, 16);
				gX = "gX = " + gyroscope[0] +" DATA 0 = "+System.Convert.ToString(System.Int32.Parse(data[6].ToString()), 2).PadLeft(8, '0')+ " DATA 1 = "+System.Convert.ToString(System.Int32.Parse(data[7].ToString()), 2).PadLeft(8, '0');

            //Receive Y data
				char[] gyroY = (System.Convert.ToString(System.Int32.Parse(data[8].ToString()), 2).PadLeft(8, '0') + System.Convert.ToString(System.Int32.Parse(data[9].ToString()), 2).PadLeft(8, '0')).ToCharArray() ;
				gyroscope[1] = binTwosComplementToSignedDecimal(gyroY, 16);
				gY ="gY = " + gyroscope[0] +" DATA 0 = "+System.Convert.ToString(System.Int32.Parse(data[8].ToString()), 2).PadLeft(8, '0')+ " DATA 1 = "+System.Convert.ToString(System.Int32.Parse(data[9].ToString()), 2).PadLeft(8, '0');
			
            //Receive Z data
				char[] gyroZ = (System.Convert.ToString(System.Int32.Parse(data[10].ToString()), 2).PadLeft(8, '0') + System.Convert.ToString(System.Int32.Parse(data[11].ToString()), 2).PadLeft(8, '0')).ToCharArray() ;
				gyroscope[2] = binTwosComplementToSignedDecimal(gyroZ, 16);
				gZ ="gZ = " + gyroscope[0] +" DATA 0 = "+System.Convert.ToString(System.Int32.Parse(data[10].ToString()), 2).PadLeft(8, '0')+ " DATA 1 = "+System.Convert.ToString(System.Int32.Parse(data[11].ToString()), 2).PadLeft(8, '0');

			//Debug.Log(gX +" "+ gY +" "+ gZ);

            byte[] compX = { data[12], data[13] };
				compass[0] = (System.BitConverter.ToUInt16(compX, 0) / 65536.0);   // 44100.0
			//	Debug.Log("X = " + compass[0]+" DATA 12 = "+data[12]+ " DATA 13 = "+data[13] + " Converte double"+ (double)System.BitConverter.ToInt16(compX, 0) + " Converte short"+ System.BitConverter.ToInt16(compX, 0));

            //Receive Y data
            byte[] compY = { data[14], data[15] };
				compass[1] = (System.BitConverter.ToUInt16(compY, 0) / 65536.0);  // 44100.0
			//	Debug.Log("Y = " + compass[1]+" DATA 14 = "+data[14]+ " DATA 15 = "+data[15] + " Converte double"+ (double)System.BitConverter.ToInt16(compY, 0) + " Converte short"+ System.BitConverter.ToInt16(compY, 0));

            //Receive Z data
            byte[] compZ = { data[16], data[17] };
				compass[2] = (System.BitConverter.ToUInt16(compZ, 0) / 65536.0);  // 44100.0
			//	Debug.Log("Y = " + compass[2]+" DATA 16 = "+data[16]+ " DATA 17 = "+data[17] + " Converte double"+ (double)System.BitConverter.ToInt16(compZ, 0) + " Converte short"+ System.BitConverter.ToInt16(compZ, 0));

        });
        Debug.Log("FIM DO RECEBER");
        return rawData;
    }

    public static double[] ReceiveRawDataTest()
    {

        double[] rawData = new double[3];
        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readRawDataUUID), (deviceAddress, notification) =>
        {
        }, (deviceAddress2, characteristic, data) =>
        {

            byte[] dataX = { data[0], data[1] };
            rawData[0] = (System.BitConverter.ToInt16(dataX, 0) / 65536.0) * 9.8;   // 44100.0
            //Debug.Log("X = " + rawData[0]);

            //Receive Y data
            byte[] dataY = { data[2], data[3] };
            rawData[1] = (System.BitConverter.ToInt16(dataY, 0) / 65536.0) * 9.8;  // 44100.0
            //Debug.Log("Y = " + rawData[1]);

            //Receive Z data
            byte[] dataZ = { data[4], data[5] };
            rawData[2] = (System.BitConverter.ToInt16(dataZ, 0) / 65536.0) * 9.8;  // 44100.0
            //Debug.Log("Z = " + rawData[2]);

            // //RECEIVE ACELEROMETER DATA
            // //Receive X data
            // rawData[0] = (data[1] * 256) + data[0];
            // Debug.Log("X = " + rawData[0]);

            // //Receive Y data
            // rawData[1] = (data[3] * 256) + data[2];  
            // Debug.Log("Y = " + rawData[1]);

            // //Receive Z data
            // rawData[2] = (data[5] * 256) + data[4];
            // Debug.Log("Z = " + rawData[2]);

        });
        Debug.Log("FIM DO RECEBER");
        return rawData;
    }



    //Receive Euler
    public static double[] ReceiveEuler()
    {

        double[] euler = new double[3];

        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readEulerUUID), (deviceAddress, notification) =>
        {

        }, (deviceAddress2, characteristic, data) =>
        {
            //Receive ROLL data
            byte[] dataRoll = { data[0], data[1], data[2], data[3] };
            euler[0] = System.BitConverter.ToInt32(dataRoll, 0) / 65536.0;   // 44100.0
            //Debug.Log("Roll = " + euler[0]);

            //Receive PITCH data
            byte[] dataPitch = { data[4], data[5], data[6], data[7] };
            euler[1] = System.BitConverter.ToInt32(dataPitch, 0) / 65536.0;   // 44100.0
            //Debug.Log("Pitch = " + euler[1]);

            //Receive YAW data
            byte[] datayaw = { data[8], data[9], data[10], data[11] };
            euler[2] = System.BitConverter.ToInt32(datayaw, 0) / 65536.0;   // 44100.0
            //Debug.Log("Yaw = " + euler[2]);

            // Debug.Log("ENVIOU BYTES : " + data);
            // Debug.Log("ENVIOU BYTES STRING : " + Encoding.ASCII.GetString(data));
            // Debug.Log("ENVIOU BYTES STRING : " + Encoding.Default.GetString(data));
            // Debug.Log("ENVIOU BYTES Lenght : " + data.Length);
        });
        Debug.Log("DEPOIS DO READ");
        return euler;
    }

    //Receive Rotation Matrix
    public static double[,] ReceiveRotationMatrix()
    {
        double[,] rotationMatrix = new double[3,3];
        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readRotationMatrixUUID), (deviceAddress, notification) =>
        {

        }, (deviceAddress2, characteristic, data) =>
        {
            //Receive ROLL data
            byte[] data00 = { data[0], data[1]};
            rotationMatrix[0,0] = System.BitConverter.ToInt32(data00, 0) / 65536.0;   // 44100.0
            Debug.Log("M00 = " + rotationMatrix[0,0]);

            //Receive PITCH data
            byte[] data01 = { data[2], data[3] };
            rotationMatrix[0,1] = System.BitConverter.ToInt32(data01, 0) / 65536.0;   // 44100.0
            Debug.Log("M01 = " + rotationMatrix[0,1]);

            //Receive YAW data
            byte[] data02 = { data[4], data[5] };
            rotationMatrix[0, 2] = System.BitConverter.ToInt32(data02, 0) / 65536.0;   // 44100.0
            Debug.Log("M02 = " + rotationMatrix[0, 2]);

            byte[] data10 = { data[6], data[7] };
            rotationMatrix[1, 0] = System.BitConverter.ToInt32(data10, 0) / 65536.0;   // 44100.0
            Debug.Log("M10 = " + rotationMatrix[1, 0]);

            byte[] data11 = { data[8], data[9] };
            rotationMatrix[1, 1] = System.BitConverter.ToInt32(data11, 0) / 65536.0;   // 44100.0
            Debug.Log("M11 = " + rotationMatrix[1, 1]);

            byte[] data12 = { data[10], data[11] };
            rotationMatrix[1, 2] = System.BitConverter.ToInt32(data12, 0) / 65536.0;   // 44100.0
            Debug.Log("M12 = " + rotationMatrix[1, 2]);

            byte[] data20 = { data[12], data[13] };
            rotationMatrix[2, 0] = System.BitConverter.ToInt32(data20, 0) / 65536.0;   // 44100.0
            Debug.Log("M20 = " + rotationMatrix[2, 0]);

            byte[] data21 = { data[14], data[15] };
            rotationMatrix[2, 1] = System.BitConverter.ToInt32(data21, 0) / 65536.0;   // 44100.0
            Debug.Log("M21 = " + rotationMatrix[2, 1]);

            byte[] data22 = { data[12], data[13] };
            rotationMatrix[2, 2] = System.BitConverter.ToInt32(data22, 0) / 65536.0;   // 44100.0
            Debug.Log("M22 = " + rotationMatrix[2, 2]);

            

        });
        Debug.Log("DEPOIS DO READ");
        return rotationMatrix;
    }


    //Receive Compass data
    public static void ReceiveCompass(DeviceObject bsnDevice)
    {
        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readCompassUUID), (deviceAddress, notification) =>
        {

        }, (deviceAddress2, characteristic, data) =>
        {
            //TODO RETORNAR BYTES ( data )
        });
        Debug.Log("DEPOIS DO READ");
    }


    //Receive Gravity Vector
    public static double[] ReceiveGravityVector()
    {
        double[] gravityVector = new double[3];
        Debug.Log("ENTROU NO RECEBER");
        Debug.Log("NOME = " + bsnDevice.Name);
        Debug.Log("ADRESS = " + bsnDevice.Address);
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readGravityVectorUUID), (deviceAddress, notification) =>
        {

        }, (deviceAddress2, characteristic, data) =>
        {
            //Receive X-AXYS data
            byte[] dataX = {data[0], data[1], data[2], data[3]};
            gravityVector[0] = System.BitConverter.ToSingle(dataX, 0);
            //Debug.Log("X = " + gravityVector[0]);

            //Receive Y-AXYS data
            byte[] dataY = { data[4], data[5], data[6], data[7] };
            gravityVector[1] = System.BitConverter.ToSingle(dataY, 0);
            //Debug.Log("Y = " + gravityVector[1]);

            //Receive Z-AXYS data
            byte[] dataZ = { data[8], data[9], data[10], data[11] };
            gravityVector[2] = System.BitConverter.ToSingle(dataZ, 0);
            //Debug.Log("Z = " + gravityVector[2]);

            // Debug.Log("ENVIOU BYTES : " + data);
            // Debug.Log("ENVIOU BYTES STRING : " + Encoding.ASCII.GetString(data));
            // Debug.Log("ENVIOU BYTES STRING : " + Encoding.Default.GetString(data));
            // Debug.Log("ENVIOU BYTES Lenght : " + data.Length);
        });
        Debug.Log("DEPOIS DO READ");
        return gravityVector;
    }

    //Receive Battery Status
    public static void ReceiveBatteryStatus(){
        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBatteryBSNUUID(_serviceBatteryUUID), FullBatteryBSNUUID(_readBatteryUUID), (deviceAddress, notification) =>
        {

        }, (deviceAddress2, characteristic, data) =>
        {
            int batteryStatus = data[0];
            Debug.Log("Status da Bateria = " + batteryStatus);

        });
        Debug.Log("DEPOIS DO READ");
    }


    //Receive steps previously calibrated in BSN
    public static void ReceiveSteps()
    {
        Debug.Log("ENTROU NO RECEBER");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(bsnDevice.Address, FullBSNUUID(_serviceReadUUID), FullBSNUUID(_readStepUUID), (deviceAddress, notification) =>
        {
        }, (deviceAddress2, characteristic, data) =>
        {
            int passo = data[0];
            Debug.Log("PASSOS = " + ((data[1] * 256) + data[0]));
            //TODO RETORNAR BYTES

        });
        Debug.Log("FIM DO RECEBER");
    }
	//calcula o complemento de 2 dos dados brutos obtidos da pulseiira
	public static int binTwosComplementToSignedDecimal(char[] binary,int significantBits) 
	{
		int power = (int)  Mathf.Pow(2,significantBits-1);
		int sum = 0;
		int i;
		for (i=0; i<binary.Length; ++i)
		{
			if ( i==0 && binary[i]!='0')
			{
				sum = power * -1;
			}
			else 
			{
				sum += (binary[i]-'0')*power;//The -0 is needed
			}
			power /= 2;
		}

		return sum;
	}

	private static double aRes, gRes, mRes;      // scale resolutions per LSB for the sensors
	//Desfinições para configuração dos sensores
	public enum Gscale {
		GFS_250DPS = 0,
		GFS_500DPS,
		GFS_1000DPS,
		GFS_2000DPS
	};

	public static double getGres(Gscale gscale) {
		
		switch (gscale)
		{
		// Possible gyro scales (and their register bit settings) are:
		// 250 DPS (00), 500 DPS (01), 1000 DPS (10), and 2000 DPS  (11). 
		// Here's a bit of an algorith to calculate DPS/(ADC tick) based on that 2-bit value:
		case Gscale.GFS_250DPS:
			gRes = 250.0/32768.0;
			break;
		case Gscale.GFS_500DPS:
			gRes = 500.0/32768.0;
			break;
		case Gscale.GFS_1000DPS:
			gRes = 1000.0/32768.0;
			break;
		case Gscale.GFS_2000DPS:
			gRes = 2000.0/32768.0;
			break;
		}
		return gRes;
	}

	public enum Ascale {
		AFS_2G = 0,
		AFS_4G,
		AFS_8G,
		AFS_16G
	};


	public static double getAres(Ascale ascale) {
		switch (ascale)
		{
		// Possible accelerometer scales (and their register bit settings) are:
		// 2 Gs (00), 4 Gs (01), 8 Gs (10), and 16 Gs  (11). 
		// Here's a bit of an algorith to calculate DPS/(ADC tick) based on that 2-bit value:
		case Ascale.AFS_2G:
			aRes = 2.0/32768.0;
			break;
		case Ascale.AFS_4G:
			aRes = 4.0/32768.0;
			break;
		case Ascale.AFS_8G:
			aRes = 8.0/32768.0;
			break;
		case Ascale.AFS_16G:
			aRes = 16.0/32768.0;
			break;
		}
		return aRes;
	}

	static List<double[]> accel_gyro_temp = new List<double[]>();
	public static void CalibrateGyroAccel (double[] gyroCali, double[] accelCali){
		//pegar 10 de cada sensor em int 32 bits, soma-los e pegar a media
		int accelsensitivity = 16384; //LSB/degrees/sec
		int gyrosensitinvity = 131; // LSB/g
		for(int i = 0; i < 10; i++){
			accel_gyro_temp = ReceiveRawData();

			accelCali[0] += accel_gyro_temp [0] [0];
			accelCali[1] += accel_gyro_temp [0] [1];
			accelCali[2] += accel_gyro_temp [0] [2];

			gyroCali[0] += accel_gyro_temp [1] [0];
			gyroCali[1] += accel_gyro_temp [1] [1];
			gyroCali[2] += accel_gyro_temp [1] [2];
			waiter ();
		}
		//normnalize as somas de acordo com for
		accelCali [0] /= 10;
		accelCali [1] /= 10;
		accelCali [2] /= 10;
		gyroCali [0] /= 10;
		gyroCali [1] /= 10;
		gyroCali [2] /= 10;

		//remove gravity from z-axis accelerometer bias calcualtion
		if(accelCali[2] > 0){
			accelCali [2] -= accelsensitivity;
		} else {
			accelCali [2] += accelsensitivity;
		}

		gyroCali[0] /= gyrosensitinvity;
		gyroCali[1] /= gyrosensitinvity;
		gyroCali[2] /= gyrosensitinvity;

		accelCali[0] /= accelsensitivity;
		accelCali[1] /= accelsensitivity;
		accelCali[2] /= accelsensitivity;
	}

	static IEnumerator waiter()
	{
		//Wait for 3 seconds
		yield return new WaitForSeconds(3);
		accel_gyro_temp = ReceiveRawData();
	}

}

