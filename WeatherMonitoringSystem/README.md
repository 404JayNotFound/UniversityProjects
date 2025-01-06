## 🌱 Environmental Monitoring System with Arduino & Google Drive Integration 🌍

This project involves an **Environmental Monitoring System** that collects data from various sensors and stores it in a CSV file. The data is then automatically uploaded to **Google Drive** for easy access and future analysis. The system is built using **Arduino**, **Python**, **Dash**, and **Google API**. The sensors used are a **Photoresistor**, **MQ135 gas sensor**, and **DHT11 temperature & humidity sensor**.

---
## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Hardware Components](#hardware-components)
- [Software Components](#software-components)
- [How It Works](#how-it-works)
  - [Data Collection](#data-collection)
  - [Data Logging](#data-logging)
  - [Google Drive Upload](#google-drive-upload)
  - [Real-Time Monitoring](#real-time-monitoring)
- [Setup Instructions](#setup-instructions)
  - [Arduino Setup](#arduino-setup)
  - [Python Setup](#python-setup)
  - [Dash Setup](#dash-setup)
  - [Set up Automatic Start of Python Script Using systemd](#set-up-automatic-start-of-python-script-using-systemd)
- [Monitoring Dashboard](#monitoring-dashboard)
- [Troubleshooting](#troubleshooting)
  - [No Data Displayed on Dashboard](#no-data-displayed-on-dashboard)
  - [Serial Communication Error](#serial-communication-error)
  - [Unable to Install Python Libraries on the Pi](#unable-to-install-python-libraries-on-the-pi)
- [Proof of Concept](#proof-of-concept)
- [Purchase Links for Components Used](purchase-links-for-components-used)
- [Contact](#contact)
- [Contributors](#contributors)

--- 
## 📍 Project Overview

This project integrates data collection, storage, and real-time monitoring using various **Arduino** sensors. The Arduino reads data from sensors (light, air quality, temperature, and humidity), logs the data into a CSV file, and uploads it to **Google Drive** using Python. The **Dash** web application then displays real-time data on an interactive online dashboard.

---
## 🛠 Hardware Components

- **Raspberry Pi**
- **Arduino UNO**
- **DHT11 Temperature and Humidity Sensor**
- **Photoresistor (LDR)**
- **MQ135 Air Quality Sensor**
- **RTC DS1307 Real Time Clock (RTC) Module**
- Jumper wires and breadboard

---
## 💻 Software Components

- **Arduino IDE** for writing and uploading code to the Arduino
- **Python 3.x** for running scripts to log data and interact with Google Drive
- **Dash by Plotly** for creating interactive web dashboards
- **Google Drive API** for cloud storage integration

---
## 🧑‍💻 How It Works

Below shows the basic system architecture, which illustrates the flow of data from the sensors through the Arduino, processed and logged by the Python script, and then stored on Google Drive for real-time monitoring and visualization via the Dash application. 

![Screenshot 2025-01-06 195802](https://github.com/user-attachments/assets/2620b366-df36-4a94-9185-6aaba7b9a749)

### Data Collection

- The **Arduino** continuously collects data from the **Photoresistor**, **MQ135**, and **DHT11** sensors.
- The data is timestamped using the **RTC module** and sent to the **serial monitor**.

### Data Logging

- The **Python script** listens to the **serial port** for incoming sensor data.
- It parses the data and logs it into a **CSV file**.

### Google Drive Upload

- The **Python script** authenticates with the **Google Drive API** and uploads the latest **CSV file** to your **Google Drive** for cloud storage.

### Real-Time Monitoring

- The **Dash application** continuously pulls the latest data from **Google Drive** and plots it in **real-time graphs** for easy visualization as seen in the example below.

![Screenshot 2025-01-06 202436](https://github.com/user-attachments/assets/67397d5d-e181-456b-be03-25a13b383fa6)


---
## 🌐 Google Drive Credentials

To use the Google Drive API within the Python code for storing sensor data, you need to set up credentials in the Google Developer Console.

### Steps to Obtain the Credentials File:

#### 1. **Create a Project in the Google Developer Console**:
   - Go to the [Google Developer Console](https://console.developers.google.com/).
   - Sign in with your Google account.
   - Click on **Select a Project** in the top-right corner and then click **New Project**.
   - Name your project and click **Create**.

#### 2. **Enable the Google Drive API**:
   - Navigate to the **Library** section on the left panel.
   - In the search bar, type "Google Drive API" and select it from the results.
   - Click **Enable** to activate the Google Drive API for your project.

#### 3. **Create OAuth 2.0 Credentials**:
   - Go to the **Credentials** section in the left sidebar.
   - Click on **Create Credentials** and choose **OAuth client ID**.
   - You will be prompted to configure the **OAuth consent screen**. Fill in the necessary fields (e.g., app name, email).
   - Under **Application type**, select **Desktop App** (since this is for local use).
   - After configuring the consent screen, click **Create**.
   - Once created, you will see a pop-up with your **Client ID** and **Client Secret**.
   - Click **Download** to download the **credentials.json** file.

---

## 🛠 Setup Instructions

## Arduino Setup

1. Connect the sensors to the **Arduino** as per the following pin connections:
   - **Photoresistor** to analog pin `A0`
   - **MQ135** to analog pin `A2`
   - **DHT11** to digital pin `4`
   - **RTC DS1307** to I2C pins (SCL to `A5`, SDA to `A4`)
2. Upload the provided Arduino code to the board via the **Arduino IDE**.
3. Make sure the correct **serial port** is selected in the **Arduino IDE**.

## Python Setup

1. Install Python (version 3.x) and necessary libraries:
    ```bash
    pip install pyserial pandas plotly dash google-auth google-auth-oauthlib google-auth-httplib2 google-api-python-client
    ```
2. Place your `credentials.json` file (from Google Developer Console) in the project directory.
3. Run the Python script, `DataToGoogleDrive.py` to start logging data from the Arduino and upload it to **Google Drive**.

## Dash Setup

1. Install the necessary Dash libraries:
    ```bash
    pip install dash
    ```
2. Set up the Dash application by running the `SensorDashboard.py` script. This will start a local server to host the real-time monitoring dashboard.

## Set up Automatic Start of Python Script Using systemd

1. **Create a systemd Service File**

    To run the Python script automatically when the Raspberry Pi boots up, you'll need to create a `systemd` service file. Follow these steps:

- Open the terminal on your Raspberry Pi.
- Create a new service file in `/etc/systemd/system/` by running the following command:

  ```
  sudo nano /etc/systemd/system/sensor-data.service

- Add the following content to the file (replace /path/to/your/script.py with the actual path to your Python script):

```
  [Unit]
  Description=Sensor Data Collection and Upload Script
  After=network.target
  
  [Service]
  ExecStart=/usr/bin/python3 /path/to/your/script.py
  WorkingDirectory=/path/to/your/
  StandardOutput=inherit
  StandardError=inherit
  Restart=always
  User=pi
  Group=pi
  
  [Install]
  WantedBy=multi-user.target
```

2. **Explanation of the Service File:**
  - Description: A short description of the service.
  - After=network.target: Ensures the service starts after the network is up.
  - ExecStart: The command to start the Python script. Make sure to use the full path to the Python interpreter (/usr/bin/python3) and the full path to your Python script.
  - WorkingDirectory: Sets the working directory for the script.
  - Restart=always: Ensures that the script restarts automatically if it crashes.
  - User=pi and Group=pi: The service runs under the "pi" user and group. If you're using a different user, replace pi with the appropriate username.

3. **Enable the Service**

    Once you’ve saved and closed the file, you need to enable and start the service. Run the following commands:

    To reload the systemd manager to recognize the new service:
    ````
    sudo systemctl daemon-reload
    ````
    
    To enable the service to start automatically on boot:
    ````
    sudo systemctl enable sensor-data.service
    ````
    
    To start the service immediately:
    ````
    sudo systemctl start sensor-data.service
    ````

4. **Verify the Service is Running**

   To check if the service is running properly, you can use the following command:
    ````
    sudo systemctl status sensor-data.service
    ````
    You should see output indicating that the service is active and running.

5. **Optional: Stopping the Service**

   To stop the service:
    ````
    sudo systemctl stop sensor-data.service
    ````
    
---
## 📊 Monitoring Dashboard

The **Dash** web application provides a real-time graphical interface to monitor the sensor readings. The dashboard displays:

- **Light Level (%)**
- **Temperature (°C)**
- **Humidity (%)**
- **Gas Concentration (MQ135)**

The graphs are automatically updated every **60 seconds** with the latest data collected from the sensors.

---
## 🐞 Troubleshooting

### No Data Displayed on Dashboard

- Ensure that the **Python scripts** are running and uploading data to either the **CSV** or to the **Google Drive**.
- Ensure the Raspberry Pi has a **valid Internet Connection**.
- Check that the correct **File ID** is set in the **dashboard.py** script.
- Verify your **Google Drive credentials** and **OAuth tokens** pathed correctly with the rest of the files.

### Serial Communication Error

- Check the **Serial Port** and **Baud Rate** settings in the **Arduino** and **Python** scripts.
- Ensure the **Arduino** is correctly connected to the Pi.

### Unable to Install Python Libraries on the Pi

If you're unable to install Python libraries on the Raspberry Pi, follow these steps to troubleshoot the issue.

**1: Update Your Raspberry Pi**

Make sure your Raspberry Pi OS is up to date. Running the following commands will update the OS and its package repositories:
```
sudo apt update
sudo apt upgrade -y
````
**2: Install Python 3 and Pip**

Ensure that you have Python 3 and pip installed. Use the following commands to install them if they are not already installed:
````
sudo apt install python3 python3-pip
````
**3: Install Virtual Environment Package**
A virtual environment will allow you to install Python packages isolated from the system-wide Python installation. This will help avoid package conflicts.

Install the venv package using the following command:
````
sudo apt install python3-venv
````
Now that python3-venv is installed, create a new virtual environment:
````
python3 -m venv myenv
````
Activate the virtual environment to start using it:
````
source myenv/bin/activate
````
Once activated, your terminal prompt should change to show the name of the environment, like so: (myenv) pi@raspberrypi:~$

Now that you are inside the virtual environment, install the required libraries by using the pip command.
````
pip install dash pandas plotly google-auth google-auth-oauthlib google-api-python-client
````

---
## 💡 Proof of Concept

This project serves as a proof of concept for integrating **Arduino sensors** with cloud storage and real-time data visualization. The system demonstrates how sensor data can be collected, logged, and uploaded to **Google Drive**, and how that data can be dynamically visualized in a web-based dashboard using Dash.

While the current implementation showcases the core functionality, there is significant potential to extend this project. For example, **machine learning models** could be integrated into the system to train and predict future sensor data based on historical patterns. By using the sensor data collected over time, it would be possible to develop a **predictive model** that can forecast future values for variables like temperature, humidity, light level, or gas concentration. The system could display not only the current readings but also **predicted future values**, helping users anticipate trends.

---
## 🛠 Purchase Links for Components Used

The following hardware components are used for this project. You can purchase them from the provided links:

| Component                        | Description                                        | Purchase Link                                                                                                                                                 |
|----------------------------------|----------------------------------------------------|---------------------------------------------------------------------------------------------------------------------|
| **Arduino Uno**                  | The microcontroller used to collect sensor data.    | [Arduino Uno - Amazon](https://www.amazon.com/dp/B08GQL5WRH)                                                                                                 |
| **DHT11 Sensor**                 | A basic temperature and humidity sensor.            | [DHT11 Sensor - Amazon](https://www.amazon.com/HiLetgo-Temperature-Humidity-Digital-3-3V-5V/dp/B01DKC2GQ0)                                                                                                |
| **MQ135 Gas Sensor**             | Measures air quality (NH3, NOx, alcohol, CO2).      | [MQ135 Sensor - Amazon](https://www.amazon.com/Ximimark-Quality-Hazardous-Detection-Arduino/dp/B07L73VTTY)                                                                                                |
| **Photoresistor (LDR)**          | A sensor to measure light intensity.                | [Photoresistor (LDR) - Amazon](https://www.amazon.com/eBoot-Photoresistor-Sensitive-Resistor-Dependent/dp/B01N7V536K)                                                                                         |
| **RTC DS3231 Module**            | Real-time clock module for time-stamping data.      | [DS3231 RTC Module - Amazon](https://www.amazon.co.uk/WINGONEER-DS3231-AT24C32-Precision-Arduino/dp/B01H5NAFUY)                                                                                          |
| **Jumper Wires**                 | Wires to connect the components together.           | [Jumper Wires - Amazon](https://www.amazon.com/California-JOS-Breadboard-Optional-Multicolored/dp/B0BRTJXND9)                                                                                              |
| **Breadboard**                   | Used for prototyping circuit connections.           | [Breadboard - Amazon](https://www.amazon.com/Rindion-Breadboard-Distribution-Connection-Prototype/dp/B0DBQ8ML2T)                                                                                                 |
| **Raspberry Pi 3B**               | The computing platform to run the Python scripts.   | [Raspberry Pi 3B - Amazon](https://www.amazon.com/Raspberry-Pi-Model-Board-Plus/dp/B0BNJPL4MW)                                                                                             |
| **MicroSD Card (32GB)**          | Storage for the Raspberry Pi OS and files.          | [MicroSD Card - Amazon](https://www.amazon.com/Amazon-Basics-microSDXC-Memory-Adapter/dp/B08TJRVWV1)                                                                                                |

> **Note:** Please verify the specifications and compatibility before purchasing. Always check the most updated product page for current availability.


---
## 💬 Contact
Feel free to explore, or reach out for questions. You can contact me via GitHub or email for inquiries related to any specific project.

## Contributors
- [Jamie O'Connor](https://github.com/404JayNotFound)

