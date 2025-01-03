# 🛠️ GPS String Generator

**Project Description**: <br>
This project generates a GPS data string in the `$GPGLL` format. The program prompts the user to enter both latitude and longitude coordinates, along with direction indicators (N/S for latitude and E/W for longitude), and constructs a GPS string that follows the NMEA standard.
The program does the following:
- Prompts the user to enter latitude and longitude in the formats:
  - Latitude: DDMM.MMMM (e.g., 3713.1234)
  - Longitude: DDDMM.MMMM (e.g., 12213.1234)
- Prompts the user to enter direction indicators:
  - Latitude direction: North or South
  - Longitude direction: East or West
- Constructs a GPS string in the following format:
  - `"$GPGLL,<latitude>,<N/S>,<longitude>,<E/W>,<time>,<status>,@<checksum>"`
  - Where time and status are fixed fields, and the checksum is calculated based on the data string.
 - Outputs the formatted GPS string with the checksum appended at the end.

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

This program accepts GPS coordinates from the user in a specific format and generates a properly formatted `$GPGLL` string. It includes user-friendly prompts and input validation to ensure the correctness of latitude, longitude, and direction values.

### The format of the generated GPS string is as follows: `$GPGLL,<latitude>,<N/S>,<longitude>,<E/W>,<time>,<status>,@<checksum>`


- **Latitude**: DDMM.MMMM (e.g., 3713.1234)
- **Longitude**: DDDMM.MMMM (e.g., 12213.1234)
- **Direction Indicators**: 
  - Latitude direction: North (N) or South (S)
  - Longitude direction: East (E) or West (W)
- **Time**: Fixed value for simulation.
- **Status**: Fixed value for simulation.
- **Checksum**: Calculated based on the data string.

---

## ⚙️ How It Works

1. **Input for Latitude and Longitude**: The program prompts the user to enter the latitude and longitude values in the format DDMM.MMMM or DDDMM.MMMM.
2. **Direction Validation**: The user is asked to specify whether the latitude is North (N) or South (S) and whether the longitude is East (E) or West (W).
3. **GPS String Construction**: Once valid inputs are received, the program constructs a GPS string with the format `$GPGLL,latitude,N/S,longitude,E/W,time,status,@checksum`.
4. **Checksum Calculation**: The program calculates the checksum for the GPS string by XOR-ing all characters in the string.
5. **Output**: The final GPS string with the checksum is displayed to the user.

---

## 💻 Technologies Used

- **C**: The program is written in C, utilizing basic input/output functions and string manipulation to generate the GPS string.
- **Standard Libraries**: The program uses standard libraries such as `stdio.h`, `string.h`, and `stdlib.h` for input/output operations, string formatting, and general functionality.

---

## 📋 Project Requirements

- A C compiler (e.g., GCC) to compile the code.
- Basic knowledge of GPS coordinate formatting and the NMEA standard.

---

## 🎯 Example Usage

Enter latitude (format: DDMM.MMMM): 3713.1234 <br>
Enter latitude direction (N/S): N <br>
Enter longitude (format: DDDMM.MMMM): 12213.1234 <br>
Enter longitude direction (E/W): W <br>
Formatted GPS String: $GPGLL,3713.1234,N,12213.1234,W,225444,A,@57

---

## 💬 Contact
Feel free to explore, or reach out for questions. You can contact me via GitHub or email for inquiries related to any specific project.

## Contributors
- [Jamie O'Connor](https://github.com/404JayNotFound)
