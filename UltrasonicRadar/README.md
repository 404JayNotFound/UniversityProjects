# 🚀 **Ultrasonic Radar System**

## 🧭 **Overview**
The **Ultrasonic Radar System** is a real-time system designed to simulate radar functionality using an **Arduino Uno** microcontroller. By integrating an **HC-SR04 ultrasonic sensor**, a **micro servo motor**, and an **I2C LCD display**, the system is capable of scanning a 180° range, measuring distances to objects, and displaying the results in real-time.

The system operates by rotating the ultrasonic sensor using the micro servo, emitting ultrasonic pulses to detect nearby objects, and calculating distances using the time-of-flight method. This data is then displayed on an LCD and can be transmitted via serial communication for external visualization.

This project is an excellent platform for understanding principles such as **distance measurement**, **servo control**, and **sensor integration**. It can be applied in various fields including robotics, IoT, and automation.

<div align="center">

![NumPy](https://img.shields.io/badge/numpy-%23013243.svg?style=for-the-badge&logo=numpy&logoColor=white)
![Matplotlib](https://img.shields.io/badge/Matplotlib-%23ffffff.svg?style=for-the-badge&logo=Matplotlib&logoColor=black)
![Python](https://img.shields.io/badge/python-3670A0?style=for-the-badge&logo=python&logoColor=ffdd54) ![C++](https://img.shields.io/badge/c++-%2300599C.svg?style=for-the-badge&logo=c%2B%2B&logoColor=white) 
![Arduino](https://img.shields.io/badge/-Arduino-00979D?style=for-the-badge&logo=Arduino&logoColor=white)

</div>

---

## 📑 **Table of Contents**
1. [Overview](#overview)
2. [Features](#features)
3. [Hardware Components](#hardware-components)
4. [Software Requirements](#software-requirements)
5. [Circuit Design](#circuit-design)
6. [PCB Design](#pcb-design)
7. [Code Description](#code-description)
8. [Setup Instructions](#setup-instructions)
9. [Future Improvements](#future-improvements)
10. [Contact](#-contact)
11. [Contributors](#-contributors)

---

## 🌟 **Features**
- **Distance Measurement**: 
  - Uses the **HC-SR04 ultrasonic sensor** to measure distances with high accuracy. The sensor operates on the principle of emitting an ultrasonic wave and measuring the time it takes to reflect back after hitting an object.
  
- **Servo-Controlled Scanning**:
  - The **micro servo (9g)** is used to rotate the ultrasonic sensor across a 180° horizontal arc, allowing the system to scan its environment in a manner similar to a radar system.
  
- **Proximity Alerts**: 
  - An **LED** indicator is triggered when objects are detected within a user-defined proximity threshold, which is helpful for immediate feedback in object detection applications.
  
- **Real-Time Display**:
  - The distance readings, along with the corresponding scanning angle, are shown on an **I2C LCD (16x2)** display. This minimizes wiring complexity and provides real-time data feedback.
  
- **Compact Design**:
  - The system is designed with modularity in mind, allowing it to be easily assembled using an **Arduino shield** for streamlined hardware integration.

---

## 🛠️ **Hardware Components**
This project uses the following components to build the radar system:

- **Arduino Uno**:
  - The **ATmega328P** microcontroller on the **Arduino Uno** is responsible for controlling the servo, measuring distances with the ultrasonic sensor, processing data, and driving the display and LED indicators. The Uno operates at 16 MHz and provides adequate digital I/O and PWM pins for the system.
  
- **HC-SR04 Ultrasonic Sensor**:
  - The **HC-SR04** is a widely used ultrasonic sensor that emits a high-frequency sound wave and measures the time it takes for the wave to reflect off an object. The time delay is used to calculate the distance to the object. It provides measurements within the range of 2 cm to 400 cm, with a typical accuracy of ±3 mm.
  
- **Micro Servo (9g)**:
  - The **micro servo** is used to rotate the ultrasonic sensor between 0° and 180° with high precision. The servo is controlled using **Pulse Width Modulation (PWM)** signals from the Arduino, which defines its position within the rotational range.
  
- **I2C LCD (16x2)**:
  - The **I2C LCD** is a 16x2 character display that shows the current distance measurement and scanning angle. Using the I2C communication protocol, this module reduces wiring complexity and only requires two pins (SDA and SCL) for data transfer.
  
- **LED**:
  - The **LED** serves as an alert system, turning on when the system detects an object within a predefined proximity threshold. This provides immediate visual feedback for detection events.
  
- **Potentiometer** (Optional):
  - A **potentiometer** can be used to adjust the proximity threshold dynamically, or to fine-tune the servo’s speed or other parameters.
  
- **Push Button** (Optional):
  - A **push button** could be used for additional user input, such as manually resetting the system, pausing the scan, or switching modes.

### 💰 **Bill of Materials**
<div align="center">
  
  | Item                | Quantity | Description                  | Estimated Cost (EUR) |
  |---------------------|----------|------------------------------|-----------------------|
  | Arduino Uno         | 1        | Microcontroller Board        | 15 - 25              |
  | HC-SR04             | 1        | Ultrasonic Sensor            | 3 - 7                |
  | Micro Servo (9g)    | 1        | Motor for Rotation           | 6 - 8                |
  | I2C LCD (16x2)      | 1        | Display Module               | 4 - 6                |
  | LED                 | 1        | Through-Hole Indicator       | 0.05                 |
  | 220Ω Resistor       | 1        | For LED Current Limiting     | 0.02                 |
  | Potentiometer       | 1        | Adjustable Resistance        | 1.00                 |
  | Push Button         | 1        | Momentary Contact Switch     | 0.28                 |
  | 1Ω Resistor         | 1        | Used for Grounding LCD       | 0.05                 |
  
</div>

---

## 💻 **Software Requirements**
- **Arduino IDE**:
  - The **Arduino IDE** is used for writing the firmware for the **Arduino Uno**. The code is written in **C++** and uploaded to the microcontroller, where it handles the servo control, sensor data collection, and LCD display updates.
  
- **Python**:
  - If using a Python-based visualizer, the Python script reads the serial data sent from the Arduino and displays the radar-like plot using libraries such as **Matplotlib**.
  - The required libraries can be installed by running:
    
  ```
  pip install -r requirements.txt
  ```
    
---

## 🔌 **Circuit Design**
The **Servo Ultrasonic Radar System** integrates several components, and the circuit follows these key connections:

- **HC-SR04 Ultrasonic Sensor**:
  - **Trigger pin** to **D10** on Arduino.
  - **Echo pin** to **D9** on Arduino.
  
- **Micro Servo**:
  - Control pin connected to **D13** on Arduino.
  
- **LED**:
  - Connected to **D3** through a **220Ω current-limiting resistor**.
  
- **I2C LCD (16x2)**:
  - Uses **SDA** and **SCL** for data communication.
  
- **Potentiometer (Optional)**:
  - Connected to an analog input pin (e.g., **A2**) to adjust settings such as proximity threshold or servo rotation speed.

- **Push Button (Optional)**:
  - Connected to **D2** on Arduino for user input functionality.

---

## 🖥️ **PCB Design**

The **Servo Ultrasonic Radar System** can benefit from a custom-designed PCB (Printed Circuit Board) to streamline assembly, reduce wiring complexity, and improve reliability. Below is an overview of the PCB design and the key components integrated into the design.

### 📐 **Overview of PCB Design**

The PCB design for the Servo Ultrasonic Radar System incorporates the following key elements:

- **Arduino Uno Compatibility**: The design will include a socket or header pins that allow the Arduino Uno to be mounted on the PCB or connected via a header pin for easy assembly.
- **Ultrasonic Sensor Circuit**: The HC-SR04 ultrasonic sensor requires connections for its **Trigger** and **Echo** pins, which will be routed to the appropriate pins on the Arduino Uno.
- **Servo Motor Interface**: The PCB will include a dedicated header for the micro servo motor control pin. The servo operates using Pulse Width Modulation (PWM), and this signal will be routed directly to the appropriate Arduino pin.
- **I2C LCD Interface**: The PCB will provide pins for the **SDA** and **SCL** connections of the I2C LCD, as well as a connection for the **power** and **ground** lines.
- **LED Indicator**: The PCB will include a 220Ω current-limiting resistor for the **LED**, connected to a GPIO pin on the Arduino for proximity alerts.
- **Optional Components**: There will be headers for optional components such as the **potentiometer** for adjusting the proximity threshold and the **push button** for manual control.

<div align="center">
  
  ![Screenshot 2025-01-01 165128](https://github.com/user-attachments/assets/70411a95-9324-4fa0-9b60-1dc8829dd97a)
  ![Screenshot 2025-01-01 170439](https://github.com/user-attachments/assets/3fee31d4-aed2-4d40-9930-70c9ceeeb41f)

</div>
 
---

## 🖥️ **Code Description**

### 💡**Arduino Code**
- **Purpose**: The Arduino code controls the servo, measures distances, and manages outputs to the LCD and LED.
- **Key Functions**:
  - `setup()`: Initializes pin modes, LCD, and serial communication.
  - `loop()`: Rotates the servo, measures distances, and updates the LCD with distance readings.
  - `rotateAndMeasure()`: Rotates the sensor through a set range of angles, takes distance readings, and triggers the LED if within threshold.
  - `measureDistance()`: Uses the **HC-SR04** sensor to calculate distances.

### 🐍 **Python Code**
- The Python script reads the serial data from the Arduino and plots the scanned radar-like data using **Matplotlib**. This provides a graphical representation of detected objects, allowing further analysis or visualization.
![Screenshot 2024-12-31 220252](https://github.com/user-attachments/assets/76b57ad6-adc8-46f7-a219-e19d747061a7)

---

## ⚙️ **Setup Instructions**
1. **Hardware Assembly**:
   - Mount the components on a breadboard or a custom PCB.
   - Connect the ultrasonic sensor, servo motor, LED, and I2C LCD according to the circuit diagram provided.
   
2. **Software Upload**:
   - Open the Arduino code in the **Arduino IDE** (`UltrasonicRadar.ino`).
   - Select the appropriate board and port from the **Tools** menu in the IDE.
   - Upload the code to the **Arduino Uno**.
   
3. **Testing**:
   - Power the Arduino using USB or an external power source.
   - Ensure that the servo rotates, the ultrasonic sensor detects objects, and the LCD displays distance readings.
   - Observe the LED for proximity alerts.

4. **Python Visualizer**:
   - Run the optional Python script to visualize serial data. This will plot the distance and angle in a radar-like manner.

---

## 🔧 **Future Improvements**
- **Integration of Additional Sensors**:
  - Add sensors such as **PIR** for motion detection to enhance the system’s functionality.
  
- **Manual Control via Joystick**:
  - Introduce a joystick interface for manual control of the servo motor’s rotation.
  
- **Wireless Communication**:
  - Incorporate wireless communication modules (e.g., **Bluetooth** or **Wi-Fi**) for remote monitoring and control.
  
- **Energy Efficiency**:
  - Optimize power management in the system for battery-operated versions by adjusting the frequency of sensor readings or using low-power components.

---

## 💬 Contact
Feel free to explore, or reach out for questions. You can contact me via GitHub or email for inquiries related to any specific project.

## Contributors
- [Jamie O'Connor](https://github.com/404JayNotFound)
