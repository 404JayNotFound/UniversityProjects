#  Temperature Sensor Response Time Comparison in LabVIEW

##  Project Brief

> Design a LabVIEW program that compares the speed of response of the LM35 temperature sensor installed on the sensor board with the thermistor supplied to you. This is an NTC, 22k, (Farnell:1672386). Your system should use the thermistor in a potential divider. The program should display both the voltage produced by the sensor on pins 1 of J2 and the thermistor voltage. It should then ask the user to hold both sensors and display the changing output voltages on a graph. Your program should indicate which sensor reading started to rise first and the rate of change of each voltage.

---

##  Overview

This project involves designing a **LabVIEW** program to compare the **response speed** of two temperature sensors:

-  **LM35 temperature sensor** (mounted on the sensor board)
-  **NTC Thermistor** (22k, Farnell: 1672386)

The goal is to measure and analyze how quickly each sensor responds to a change in temperature (e.g., when held by a user) by evaluating their voltage outputs and rates of change.

---

##  Table of Contents

- [Project Brief](#-project-brief)
- [Overview](#-overview)
- [Hardware Setup](#️-hardware-setup)
- [Program Features](#-program-features)
- [Getting Started](#-getting-started)
- [Requirements](#-requirements)
- [Contributors](#contributors)

---

##  Hardware Setup

###  Sensors
- **LM35 Sensor**
  - Output voltage measured on **pin 1 of J2**
  - Provides a linear output voltage proportional to temperature

- **NTC Thermistor**
  - Resistance: 22kΩ
  - Configured in a **potential divider** circuit
  - Voltage measured across the thermistor

###  Signal Acquisition
- Both sensor voltages are read using a DAQ (Data Acquisition) interface
- Ensure correct scaling and filtering of input voltages as needed

---

##  Program Features

###  Display
- Real-time voltage output from **both sensors**
- Dynamic **graphical plot** of voltage changes over time

###  User Interaction
- User is prompted to **hold both sensors**
- Program visually tracks changing voltage outputs in real-time

###  Analysis
- Automatically detects:
  -  **Which sensor's output voltage rises first**

###  Indicators
- Highlight of the **faster-responding sensor**
- Display of **calculated rates of change**

---

##  Getting Started

1. **Connect the sensors** as follows:
   - LM35 to **pin 1 of J2**
   - Thermistor in a **potential divider** configuration

2. **Open the LabVIEW VI** provided in the project folder

3. **Run the program** and follow the instructions:
   - Hold both sensors when prompted
   - Watch real-time graphs and sensor readings
   - Review results on sensor response speed

---

##  Requirements

- **LabVIEW** 2018 or later
- **DAQ device** compatible with LabVIEW for analog voltage input
- **LM35 sensor** and **22k NTC thermistor** (Farnell: 1672386)
- Basic electronic components (resistors, breadboard, jumper wires)

---
##  Contact
Feel free to explore, or reach out for questions. You can contact me via GitHub or email for inquiries related to any specific project.

## Contributors
- [Jamie O'Connor](https://github.com/404JayNotFound)
