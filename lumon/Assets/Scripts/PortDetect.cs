using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;
using System;

public class SerialPortSelector : MonoBehaviour
{
    void Start()
    {
        Debug.Log("hell0");
        RefreshPortList();
    }

    private void RefreshPortList()
    {
        try
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (var t in ports)
            {
                Debug.Log(t);
            }
        } catch (Exception e){
            Debug.Log(e); 
        }
    }

   
}
