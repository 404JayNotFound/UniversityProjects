#include <DHT.h>
#include <Wire.h>
#include <RTClib.h>

const int photoResistorPin = A0;
const int mq135Pin = A2;
const int dhtPin = 4;

const int photoResistorLEDPin = 7;
const int mq135LEDPin = 5;
const int dhtLEDPin = 6;

const int ledBrightness = 200;

// DHT sensor type
#define DHTTYPE DHT11
DHT dht(dhtPin, DHTTYPE);

RTC_DS1307 rtc;

void setup() {
    Serial.begin(9600);
    dht.begin();

    pinMode(photoResistorLEDPin, OUTPUT);
    pinMode(mq135LEDPin, OUTPUT);
    pinMode(dhtLEDPin, OUTPUT);

    delay(1000); // This delay is for sensor stabilization

    if (!rtc.begin()) {
        Serial.println("RTC module not found. Please check the connection.");
    } else {
        rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));
    }
}

void loop() {
    // Read data from sensors
    int lightValue = analogRead(photoResistorPin);
    float lightLevel = lightValue / 1023.0 * 100;
    int mq135Value = analogRead(mq135Pin);
    float humidity = dht.readHumidity();
    float temperature = dht.readTemperature();

    // Fetch the current date and time from RTC
    DateTime now = rtc.now();

    if (!isnan(humidity) && !isnan(temperature)) {
        // Print timestamp and sensor data
        Serial.print(now.year(), DEC);
        Serial.print("/");
        Serial.print(now.month(), DEC);
        Serial.print("/");
        Serial.print(now.day(), DEC);
        Serial.print(" ");
        Serial.print(now.hour(), DEC);
        Serial.print(":");
        Serial.print(now.minute(), DEC);
        Serial.print(":");
        Serial.print(now.second(), DEC);
        Serial.print(",");
        Serial.print(lightLevel);
        Serial.print(",");
        Serial.print(mq135Value);
        Serial.print(",");
        Serial.print(humidity);
        Serial.print(",");
        Serial.println(temperature);

        // Indicate readings using LEDs
        analogWrite(photoResistorLEDPin, ledBrightness);
        analogWrite(mq135LEDPin, ledBrightness);
        analogWrite(dhtLEDPin, ledBrightness);
        delay(100);
        analogWrite(photoResistorLEDPin, 0);
        analogWrite(mq135LEDPin, 0);
        analogWrite(dhtLEDPin, 0);
    } else {
        Serial.println("Error reading DHT11 sensor!");
    }

    delay(30000); // 30-second delay between readings
}
