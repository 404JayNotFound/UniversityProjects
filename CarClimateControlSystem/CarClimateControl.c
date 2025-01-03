#include <stdio.h>

// Delay constants for simulating heating/cooling time
const int DELAY_OUTER = 1000000;
const int DELAY_INNER = 13000;

// Port B bit definitions for heater, fan, and sensor control as per project requirements
const int PORT_B_BIT_4 = 0x10;  // Heater (bit 4)
const int PORT_B_BIT_5 = 0x20;  // Fan (bit 5)
const int PORT_B_BIT_7 = 0x80;  // Sensor (bit 7)

// Temperature threshold for heater activation
const int HEATER_THRESHOLD = 10;

// Function Declarations
int readInteger(const char *prompt); 
void activateFan(int *portB, int currentTemperature, int thresholdTemperature);
void activateHeater(int *portB, int sensorStatus);
void printPortBBinary(int portB);
void delay(); 

int main() {
    int currentTemperature, thresholdTemperature;
    int portB = 0;  // Simulated Port B register for control

    // Input the temperature threshold, ensuring it is above the heater threshold
    do {
        thresholdTemperature = readInteger("Enter the threshold temperature (C): ");
        if (thresholdTemperature <= HEATER_THRESHOLD) {
            printf("Threshold temperature must be greater than %d°C.\n", HEATER_THRESHOLD);
        }
    } while (thresholdTemperature <= HEATER_THRESHOLD);

    while (1) {
        // Read the current temperature
        currentTemperature = readInteger("\nEnter the current temperature (C): ");

        // Control fan based on temperature comparison with threshold
        activateFan(&portB, currentTemperature, thresholdTemperature);
        
        // Simulate sensor input for heater control
        if (currentTemperature < HEATER_THRESHOLD) {
            portB |= PORT_B_BIT_7;  // Sensor high (activate heater)
        } else {
            portB &= ~PORT_B_BIT_7; // Sensor low (deactivate heater)
        }

        // Control heater based on sensor status
        activateHeater(&portB, (portB & PORT_B_BIT_7) != 0);

        // Display the current state of Port B in binary and hex
        printPortBBinary(portB);

        // Simulate delay for heating/cooling
        delay();
    }

    return 0;
}

// Prints the current state of Port B in binary and hexadecimal formats
void printPortBBinary(int portB) {
    printf("Current state of Port B: ");
    for (int i = 7; i >= 0; i--) {
        printf("%d", (portB >> i) & 1);
    }
    printf(" (0x%02X)\n", portB);
}

// Activates or deactivates the fan based on current temperature
void activateFan(int *portB, int currentTemperature, int thresholdTemperature) {
    if (currentTemperature > thresholdTemperature) {
        *portB |= PORT_B_BIT_5;  // Fan on
        printf("Fan activated\n");
    } else {
        *portB &= ~PORT_B_BIT_5; // Fan off
        printf("Fan deactivated\n");
    }
}

// Activates or deactivates the heater based on sensor status
void activateHeater(int *portB, int sensorStatus) {
    if (sensorStatus) {
        *portB |= PORT_B_BIT_4;  // Heater on
        printf("Heater activated\n");
    } else {
        *portB &= ~PORT_B_BIT_4; // Heater off
        printf("Heater deactivated\n");
    }
}

// Simulates delay to emulate heating/cooling time
void delay() {
    for (int i = 0; i < DELAY_OUTER; i++) {
        for (int j = 0; j < DELAY_INNER; j++); // Inner loop for additional delay
    }
}

// Reads and validates integer input from the user
int readInteger(const char *prompt) {
    int value;
    while (1) {
        printf("%s", prompt);
        if (scanf("%d", &value) == 1) {
            char c;
            if (scanf("%c", &c) == 1 && c != '\n') {
                printf("Invalid input. Please enter a valid integer.\n");
                while (getchar() != '\n');  // Clear the input buffer
            } else {
                return value;  // Return valid input
            }
        } else {
            printf("Invalid input. Please enter a valid integer.\n");
            while (getchar() != '\n');  // Clear the input buffer
        }
    }
}
