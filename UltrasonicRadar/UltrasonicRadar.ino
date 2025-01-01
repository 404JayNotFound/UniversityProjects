#include <Wire.h>
#include <LiquidCrystal.h>
#include <Servo.h>

// Pin Definitions
#define TRIG_PIN  6  // HC-SR04 Trigger Pin
#define ECHO_PIN  5  // HC-SR04 Echo Pin
#define LED_PIN   4  // LED Pin for proximity indication

// Create Servo object to control the servo motor
Servo myservo;

// Create LCD object for display control (using pins 13, 12, 8, 9, 10, 11)
LiquidCrystal lcd(13, 12, 8, 9, 10, 11);

// Setup function: Runs once when the program starts
void setup() {
  Serial.begin(9600);          // Start serial communication at 9600 baud rate
  myservo.attach(7);           // Attach the servo to pin 7
  pinMode(TRIG_PIN, OUTPUT);   // Set TRIG_PIN as an output (to send pulse)
  pinMode(ECHO_PIN, INPUT);    // Set ECHO_PIN as an input (to receive echo pulse)
  pinMode(LED_PIN, OUTPUT);    // Set LED_PIN as an output (to control the LED)
  lcd.begin(16, 2);            // Initialize the LCD (16 columns, 2 rows)
}

// Main loop function: Runs repeatedly to rotate the servo and measure distance
void loop() {
  // Rotate the servo from 0 to 180 degrees, and measure distance at each position
  rotateAndMeasure(0, 180);  
  // Rotate the servo back from 180 to 0 degrees, and measure distance at each position
  rotateAndMeasure(180, 0);  
}

// Function to rotate the servo from start to end angle and measure distance at each position
void rotateAndMeasure(int start, int end) {
  // Determine whether to rotate clockwise or counterclockwise
  int increment = (start < end) ? 1 : -1;

  // Rotate the servo from the start angle to the end angle
  for (int pos = start; pos != end + increment; pos += increment) {
    myservo.write(pos);           // Move the servo to the current position
    delay(5);                     // Short delay for smoother movement and stability

    int distance = measureDistance();  // Measure distance while the servo is moving

    // Only process valid distance measurements
    if (distance < 200 && distance > 0) {
      // Send the current servo angle and the measured distance over serial
      Serial.print(pos);        // Print the current servo angle
      Serial.print(",");
      Serial.println(distance); // Print the measured distance

      // Display the measured distance on the LCD
      lcd.clear();              // Clear any previous display
      lcd.setCursor(0, 0);      // Set cursor to top-left corner of the LCD
      lcd.print("Distance: ");
      lcd.print(distance);
      lcd.print(" cm");

      // Control LED based on distance: turn on if less than 20 cm
      if (distance < 20) {
        digitalWrite(LED_PIN, HIGH);  // Turn on LED (proximity indicator)
      } else {
        digitalWrite(LED_PIN, LOW);   // Turn off LED
      }
    }
  }
}

// Function to measure the distance using the HC-SR04 ultrasonic sensor
int measureDistance() {
  // Send a 10-microsecond pulse to trigger the sensor
  digitalWrite(TRIG_PIN, LOW); 
  delayMicroseconds(2);           // Wait for sensor to stabilize
  digitalWrite(TRIG_PIN, HIGH);   
  delayMicroseconds(10);          // Send the trigger pulse
  digitalWrite(TRIG_PIN, LOW);

  // Measure the duration of the echo pulse
  long duration = pulseIn(ECHO_PIN, HIGH);

  // Calculate and return the distance in centimeters
  int distance = duration * 0.034 / 2;  // Formula: distance = (duration * speed of sound) / 2
  return distance;  // Return the calculated distance
}
