# 🌡️ Car Climate Control System

**Project Description**: <br>
This project simulates a basic climate control system for a car. The system reads a **threshold temperature** (the temperature above which the fan will activate) and continuously monitors the **current temperature**. If the temperature exceeds the threshold, the fan is activated. If the temperature falls below a predefined low threshold, the system simulates a sensor reading and activates the heater. The fan remains on for 1 minute (simulated) after the temperature drops below the threshold to simulate a cooling period.

The program uses a simulated **Port B** to control the fan, heater, and sensor, and provides real-time feedback in both **binary** and **hexadecimal** formats.

## 📑 Table of Contents

1. [Overview](#-overview)
2. [How It Works](#-how-it-works)
3. [Technologies Used](#-technologies-used)
4. [Project Requirements](#-project-requirements)
5. [Example Usage](#-example-usage)
6. [Contact](#-contact)
7. [Contributors](#-contributors)

---

## 📝 Overview

The program simulates a basic climate control system for a car environment. It is designed to activate the **fan** when the ambient temperature exceeds a specified threshold and the **heater** when the temperature falls below a predefined low value. The system operates based on the following logic:

- **Threshold Temperature**: The user specifies a threshold temperature above which the fan is activated.
- **Fan Operation**: If the current temperature exceeds the threshold, the fan is activated by setting Port B bit 5 to high.
- **Heater Operation**: If the current temperature is below the predefined low threshold, **Port B bit 7** simulates a sensor input, and Port B bit 4 is set high to activate the heater.
- **Port B Monitoring**: The state of Port B is continuously displayed in both **binary** and **hexadecimal** formats, allowing for real-time feedback of the system’s control operations.

---

## ⚙️ How It Works

1. **Threshold Temperature Input**: The user is prompted to enter a **threshold temperature**, which is validated to ensure it is higher than the predefined minimum heater threshold.
2. **Current Temperature Monitoring**: After setting the threshold, the program enters a loop, asking the user to input the **current temperature** at regular intervals.
3. **Fan and Heater Activation**:
   - If the current temperature exceeds the threshold, the fan is activated by setting **Port B bit 5** high.
   - If the current temperature falls below the predefined threshold, **Port B bit 7** simulates a sensor input, and the heater is activated by setting **Port B bit 4** high.
   - The fan remains on for 1 minute (simulated by a delay) after the temperature drops below the threshold.
4. **Port B State Feedback**: After each temperature reading and control action, the state of **Port B** is displayed both in **binary** and **hexadecimal** formats, showing the activation status of the fan and heater.
5. **Delay Simulation**: The program includes a delay to simulate the cooling or heating process, making the system appear more realistic.

---

## 💻 Technologies Used

- **C**: The program is implemented in C, utilizing basic input/output functions to interact with the user and manage control logic.
- **Standard Libraries**: The program uses libraries like `stdio.h` for input/output operations.
- **Simulated Hardware Control**: The system uses a simulated **Port B** (represented as an integer) to manage the fan, heater, and sensor, making this a software simulation of an embedded system.

---

## 📋 Project Requirements

- **C Compiler**: A C compiler such as GCC is required to compile and run the code.
- **Basic Knowledge of Embedded Systems**: Understanding the logic of controlling bits in a port register and simulating hardware actions.
- **Standard Libraries**: The program depends on standard C libraries such as `stdio.h`, `stdlib.h`, and `string.h` for functionality.
- **System Input**: User inputs for threshold and current temperature, with input validation to ensure correct data entry.

---

## 🎯 Example Usage

```text
Enter the threshold temperature (C): 25
Enter the current temperature (C): 30
Fan activated 🌬️
Current state of Port B: 00000000 (0x00)

Enter the current temperature (C): 22
Fan deactivated ❄️
Current state of Port B: 00100000 (0x20)
Heater activated 🔥
Current state of Port B: 10100000 (0xA0)
```

---

## 💬 **Contact**

Feel free to explore, contribute, or reach out for questions regarding the project. You can contact me through GitHub or email for further inquiries.

## **Contributors**  
- Jamie O'Connor
