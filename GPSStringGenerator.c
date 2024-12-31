#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <ctype.h>

// Function declarations
char calculateChecksum(const char *gpsString);                                                                                                        // Calculates the checksum for a GPS string
int validateCoordinate(const char *input, int maxDegree, int isLatitude);                                                                             // Validates a GPS coordinate string
int isValidDirection(char dir, char type);                                                                                                            // Validates the direction character
void promptForInput(char *buffer, int bufferSize, const char *prompt, int (*validationFunc)(const char *, int, int), int maxDegree, int isLatitude);  // Prompts the user for input and validates it
char promptDirection(const char *prompt, char type);                                                                                                  // Prompts the user for a valid direction character

int main() {
    // Buffers to store latitude and longitude values and their directions
    char latitude[12], latDirection;
    char longitude[12], lonDirection;

    // Buffer for the formatted GPS string and checksum
    char gpsString[50];
    char checksum;

    // Prompt user for a valid latitude and direction
    promptForInput(latitude, sizeof(latitude), "Enter latitude (format: DDMM.MMMM): ", validateCoordinate, 90, 1);
    latDirection = promptDirection("Enter latitude direction (N/S): ", 'L');

    // Prompt user for a valid longitude and direction
    promptForInput(longitude, sizeof(longitude), "Enter longitude (format: DDDMM.MMMM): ", validateCoordinate, 180, 0);
    lonDirection = promptDirection("Enter longitude direction (E/W): ", 'G');

    // Construct the base GPS string using the fixed time and status values
    snprintf(gpsString, sizeof(gpsString), "$GPGLL,%s,%c,%s,%c,225444,A,", latitude, latDirection, longitude, lonDirection);

    // Calculate the checksum for the GPS string
    checksum = calculateChecksum(gpsString);

    // Append the checksum to the final GPS string
    char finalGpsString[80];
    snprintf(finalGpsString, sizeof(finalGpsString), "%s@%02X", gpsString, checksum);

    // Output the complete formatted GPS string
    printf("Formatted GPS String: %s\n", finalGpsString);

    return 0;
} /* End of main */


/**
 * @brief Calculates the checksum for a GPS string.
 *        The checksum is calculated via XOR-ing all characters
 *        in the string, starting after the initial '$'.
 * 
 * @param gpsString: The input GPS string (i.e. "$GPGLL,...").
 * @return The calculated checksum.
 */
char calculateChecksum(const char *gpsString) {
    char checksum = 0;
    int i;

    // Skips the '$' and XOR all characters
    for (i = 1; gpsString[i] != '\0'; i++) {
        checksum ^= gpsString[i];
    }
    return checksum;
} 


/**
 * @brief Validates the GPS coordinate string.
 *        Checks if the input format matches DDMM.MMMM or DDDMM.MMMM
 *        depending on whether it's latitude or longitude.
 * 
 * @param input: The coordinate string to validate.
 * @param maxDegree: The maximum allowable degree (90 for latitude, 180 for longitude).
 * @param isLatitude: 1 if validating latitude, 0 if longitude.
 * @return 1 if the coordinate is valid, 0 otherwise.
 */
int validateCoordinate(const char *input, int maxDegree, int isLatitude) {
    int degree, minute;
    float decimal;

    // Parse the input based on whether its latitude or longitude
    int parsed = isLatitude
        ? sscanf(input, "%2d%2d.%4f", &degree, &minute, &decimal)
        : sscanf(input, "%3d%2d.%4f", &degree, &minute, &decimal);

    // Ensure the input matches the expected format
    if (parsed != 3) return 0;

    // Validate degree range (0 to maxDegree)
    if (degree < 0 || degree > maxDegree) return 0;

    // Validate minute range (0 to 59)
    if (minute < 0 || minute >= 60) return 0;

    return 1;  // Valid coordinate
}


/**
 * @brief Validates the direction character for latitude or longitude.
 *        Acceptable directions:
 *        -> 'N' or 'S' for latitude
 *        -> 'E' or 'W' for longitude
 * 
 * @param dir: The direction character to validate.
 * @param type: 'L' for latitude direction, 'G' for longitude direction.
 * @return 1 if the direction is valid, 0 otherwise.
 */
int isValidDirection(char dir, char type) {
    dir = toupper(dir); // User input to uppercase

    // Validate based on the type (latitude or longitude)
    if (type == 'L') {
        return (dir == 'N' || dir == 'S');
    } else if (type == 'G') {
        return (dir == 'E' || dir == 'W');
    }

    return 0;  // Invalid direction
}


/**
 * @brief Prompts the user for input and validates it using the provided function.
 *        Repeatedly prompts until valid input is received.
 * 
 * @param buffer:         Buffer to store the validated input.
 * @param bufferSize:     Size of the buffer.
 * @param prompt:         The message to display to the user.
 * @param validationFunc: Pointer to the validation function.
 * @param maxDegree:      Maximum degree value (for validation).
 * @param isLatitude:     1 if validating latitude, 0 if longitude.
 */
void promptForInput(char *buffer, int bufferSize, const char *prompt, int (*validationFunc)(const char *, int, int), int maxDegree, int isLatitude) {
    while (1) {
        printf("%s", prompt);

        // Read input into the buffer
        if (scanf("%11s", buffer) == 1 && validationFunc(buffer, maxDegree, isLatitude)) {
            return;  // Valid input
        }

        printf("Invalid input. Please try again.\n");
    }
}


/**
 * @brief Prompts the user for a valid direction character (N/S or E/W).
 *        Repeatedly prompts until a valid direction is entered.
 * 
 * @param prompt: The message to display to the user.
 * @param type: 'L' for latitude direction, 'G' for longitude direction.
 * @return The valid direction character entered by the user.
 */
char promptDirection(const char *prompt, char type) {
    char direction;

    while (1) {
        printf("%s", prompt);

        // Read the direction input
        if (scanf(" %c", &direction) == 1) {
            direction = toupper(direction);  // Normalize to uppercase

            // Validate the direction
            if (isValidDirection(direction, type)) {
                return direction;  // Valid direction
            }
        }

        printf("Invalid direction. Please try again.\n");
    }
}
