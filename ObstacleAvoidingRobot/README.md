# 🤖 Mobile Robotics System with LeJOS EV3 🚀

This project implements a **Mobile Robotics System** using the **Subsumption Architecture** on the **LEGO EV3 platform** with the LeJOS framework. The robot leverages modular behaviors and sensory inputs to dynamically interact with its environment. The system is a practical demonstration of hierarchical control systems for autonomous robots.

---

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Hardware Components](#hardware-components)
- [Software Components](#software-components)
- [System Architecture](#system-architecture)
  - [Subsumption Architecture](#subsumption-architecture)
  - [Arbitration](#arbitration)
- [Behavior Design](#behavior-design)
  - [Behavior 0: Forward Movement](#behavior-0-forward-movement)
  - [Behavior 1: Ultrasound Tracking](#behavior-1-ultrasound-tracking)
  - [Behavior 2: Sound Response](#behavior-2-sound-response)
  - [Behavior 3: Light Response](#behavior-3-light-response)
  - [Behavior 4: Touch Response](#behavior-4-touch-response)
  - [Behavior 5: Button Control](#behavior-5-button-control)
- [Implementation Details](#implementation-details)
- [Setup Instructions](#setup-instructions)
- [System Features](#system-features)
- [Future Enhancements](#future-enhancements)
- [License](#license)

---

## 🌍 Project Overview

This project is a comprehensive implementation of autonomous robotic behavior using the **Subsumption Architecture**, built on the **LeJOS EV3** framework. The robot integrates real-time sensory data and hierarchical behaviors to navigate and interact with its environment. 

The project is designed to:
1. **Demonstrate Autonomous Robotics**: Explore reactive and adaptive robotic control systems.
2. **Leverage Modular Programming**: Implement independent behavior modules for flexibility and scalability.
3. **Utilize Hierarchical Control**: Prioritize behaviors based on importance and environmental stimuli.

The robot's behaviors include navigating forward, detecting and avoiding obstacles, responding to sound and light triggers, and handling user termination commands.

---

## 🛠 Hardware Components

- **LEGO Mindstorms EV3 Brick**: The primary computational unit for executing control algorithms.
- **Ultrasonic Sensor**: Measures distances for obstacle detection and wall-following behaviors.
- **Touch Sensor**: Detects collisions for reactive response.
- **Light Sensor**: Monitors ambient light levels to trigger specific actions.
- **Sound Sensor**: Detects sound intensity for audio-triggered responses.
- **Large Motors**: Drive the robot's wheels for movement.
- **EV3 Battery Pack**: Provides power for sensors and motors.

### Hardware Configuration
- Sensors are mounted on the EV3 robot frame in specific orientations for optimal input:
  - Ultrasonic Sensor: Forward-facing for object detection.
  - Touch Sensor: Front-mounted for collision detection.
  - Light Sensor: Positioned downward to detect surface brightness.
  - Sound Sensor: Positioned unobstructed for accurate readings.

---

## 💻 Software Components

- **LeJOS EV3**: A Java-based alternative firmware for the EV3 brick, enabling advanced programming capabilities.
- **Eclipse IDE**: Used for developing and debugging the Java code.
- **LeJOS Subsumption Framework**: Facilitates modular behavior design and arbitration logic.
- **PC Tools**: For deploying code and managing communication with the EV3 brick.

---

## 🛠 System Architecture

### Subsumption Architecture 🧠
The robot's control system is based on **Subsumption Architecture**, which organizes behaviors into a hierarchy:
- **Layered Design**: Behaviors are grouped into layers based on priority.
- **Behavior Modules**: Each module corresponds to a specific task, such as movement, obstacle avoidance, or light response.
- **Triggering Events**: Sensors activate behaviors based on predefined thresholds.

![Screenshot 2025-01-08 151653](https://github.com/user-attachments/assets/7444043a-1b32-48fa-bb1c-edb64df3e35d)

---

### Arbitration
Arbitration resolves conflicts when multiple behaviors request control simultaneously:
- **Priority-Based Suppression**: Higher-priority behaviors override lower-priority ones.
- **Default Behavior**: Forward movement is the base behavior, active when no other behavior is triggered.
- **Dynamic Decision-Making**: The system evaluates sensory data in real time to determine the active behavior.

---

## ⚙️ Behavior Design

### Behavior 0: Forward Movement
- **Function**: Moves the robot forward at a constant speed.
- **Trigger**: Always active (default behavior).
- **Details**:
  - Provides the foundation for other behaviors.
  - Suppressed when higher-priority behaviors are activated.

---

### Behavior 1: Ultrasound Tracking
- **Function**: Tracks walls based on ultrasonic distance measurements.
- **Trigger**: Activated when an obstacle is detected within 25 cm.
- **Details**:
  - Tracks the wall on the left or right depending on the robot's state.
  - Adjusts motor speeds to maintain a consistent distance.

---

### Behavior 2: Sound Response
- **Function**: Reacts to sound intensity.
- **Trigger**: Sound level exceeds the `Sound_Threshold`.
- **Details**:
  - Rotates counterclockwise if `Ultrasound_State = 0`.
  - Executes a "Victory Dance" and ends the program if `Ultrasound_State = 1`.

---

### Behavior 3: Light Response
- **Function**: Adjusts the robot's state based on light levels.
- **Trigger**: Light intensity exceeds the `Light_Threshold`.
- **Details**:
  - Rotates the ultrasonic sensor for reverse wall tracking.
  - Updates internal state variables for new behavior context.

---

### Behavior 4: Touch Response
- **Function**: Handles physical collisions.
- **Trigger**: Activated when the touch sensor is pressed.
- **Details**:
  - Reverses 20 cm using a proportional controller.
  - Emits a "sorrowful beep" and pauses before resuming movement.

---

### Behavior 5: Button Control
- **Function**: Terminates the program gracefully.
- **Trigger**: Pressing the EV3 brick's Enter button.
- **Details**:
  - Plays an exit tune.
  - Safely halts motors and ends program execution.

---

## 📐 Implementation Details

- Each behavior is implemented as a separate class in Java, adhering to the **LeJOS Subsumption Framework**.
- Sensor data is processed in real time to trigger behaviors dynamically.
- Motor control uses proportional adjustments for smooth movement.
- The arbitration logic ensures seamless behavior transitions.

---

## ⚙️ Setup Instructions

### Hardware Setup
1. Assemble the robot with the sensors and motors connected to the appropriate ports on the EV3 brick.
2. Ensure the battery pack is fully charged.

### Software Setup
1. Install **LeJOS EV3** firmware on the EV3 brick.
2. Set up the **Eclipse IDE** with the LeJOS plugin.
3. Import the Java project and ensure all dependencies are correctly configured.
4. Deploy the program to the EV3 brick using a USB or Bluetooth connection.

---

## ✨ System Features

- **Dynamic Behavior Arbitration**: Automatically selects the most relevant behavior based on sensory input.
- **Modular Design**: Independent behavior modules for easy extension and debugging.
- **Real-Time Interaction**: Responds dynamically to environmental changes.

---

## 🚀 Future Enhancements

- **Advanced Sensors**: Incorporate cameras or gyroscopes for complex behaviors.
- **Mapping and Localization**: Implement SLAM for improved navigation.
- **Path Planning**: Introduce algorithms for efficient obstacle avoidance.

---

## 💬 Contact
Feel free to explore, or reach out for questions. You can contact me via GitHub or email for inquiries related to any specific project.

## Contributors
- [Jamie O'Connor](https://github.com/404JayNotFound)
