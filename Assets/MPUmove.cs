﻿using System;
using UnityEngine;
using System.Collections;
using System.IO.Ports;
public class  MPUmove : MonoBehaviour
{
	public static SerialPort sp = new SerialPort ("COM3",57600,Parity.None,8,StopBits.One);
	float gz;
	float ax;
	float ay;
	float giroz;
	float aceleracionx;
	float aceleraciony;
	float aceleracion=0.00025f;
	float gyro = 1.0f / 32768.0f;
	float factor=30f;
	Vector3 rotation;
	// Start is called before the first frame update
	void Start()
	{
		Connection ();   
	}

	// Update is called once per frame
	void Update()
	{
		if (sp.IsOpen)
		{

			try
			{
				string serialInput=sp.ReadLine();
				string[] input = serialInput.Split(',');

				ax=Int32.Parse(input[3])*aceleracion;
				ay=Int32.Parse(input[4])*aceleracion;


				gz=Int32.Parse(input[5])*gyro;

				if (Mathf.Abs(ax) - 1 < 0) ax = 0;
				if (Mathf.Abs(ay) - 1 < 0) ay = 0;


				aceleracionx += ax;
				aceleraciony+= ay;


				//if (Mathf.Abs(gx) < 0.025f) gx = 0f;
				//if (Mathf.Abs(gy) < 0.025f) gy = 0f;
				if (Mathf.Abs(gz) < 0.025f) gz = 0f;
			
				//girox+=gx;
				//giroy+=gy;
				giroz+=gz;

				if(ax==0&&ay==0){
					aceleracionx=0f;
					aceleraciony=0f;
				}
					

				transform.Translate (new Vector3(aceleracionx*Time.deltaTime,0f, aceleraciony*Time.deltaTime));

				rotation= new Vector3(transform.eulerAngles.x,-giroz*factor,transform.eulerAngles.z);
				transform.rotation= Quaternion.Euler(rotation);

			}catch
			{

			}
		}
	}



	void Connection()
	{
		if (sp != null)
		{
			if (sp.IsOpen) {
				sp.Close ();
				Debug.Log ("Port Closed, because it was already open");
			} else
			{
				sp.Open ();
				sp.ReadTimeout = 20;
				Debug.Log ("Port Opened");
			}
		}
		else
		{
			if(sp.IsOpen)
			{
				Debug.Log("Port is already open");
			}
			else
			{
				Debug.Log("Port is null");
			}
		}
	}


} 
