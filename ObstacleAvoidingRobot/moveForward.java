import lejos.hardware.lcd.LCD;
import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.hardware.sensor.NXTUltrasonicSensor;
import lejos.robotics.subsumption.Behavior;
import lejos.utility.Delay;

/**
 * This class implements the robot's MoveForward behavior.
 * This behavior is always ready to take control as the lowest-priority behavior.
 * It moves the robot forward while maintaining a desired distance from objects using an ultrasonic sensor.
 */
public class moveForward implements Behavior {

    private final NXTRegulatedMotor engineR;
    private final NXTRegulatedMotor engineL;
    private final NXTUltrasonicSensor uSensor;
    private float[] distanceSample;

    // Configuration and control variables
    private int Vc;
    private int sampleInterval;
    private int distance;
    private final int noObject = 450;
    private int minPower = 200, maxPower = 500;
    private int leftSpeed = 0, rightSpeed = 0;
    private IntHolder desiredDistance;
    private float error, Pgain;
    private boolean isActive = false;
  
    // Constructor to initialize sensors, motors, and behavior parameters.
    MoveForward(NXTUltrasonicSensor sensor, NXTRegulatedMotor motorR, NXTRegulatedMotor motorL, IntHolder desiredWallDistance) {
        Pgain = 1.0f;
        Vc = 300;
        sampleInterval = 25;
        desiredDistance = desiredWallDistance;
        engineR = motorR;
        engineL = motorL;
        uSensor = sensor;

        distanceSample = new float[uSensor.sampleSize()];
        leftSpeed = rightSpeed = Vc;
    }

    @Override
    public void action() {
        isActive = true;
        LCD.clear();
        LCD.drawString("Behavior: MoveForward", 0, 5);

        while (isActive) {
            uSensor.fetchSample(distanceSample, 0);
            distance = (int) (distanceSample[0] * 100);
            LCD.drawInt(distance, 0, 5);

            if (Math.abs(distance) > noObject) {
                distance = desiredDistance.value + 40;
            }

            // Calculate error and adjust motor speeds using proportional control
            error = (distance - desiredDistance.value);
            rightSpeed = (int) (Vc - 0.5 * (Pgain * error));
            leftSpeed = (int) (Vc + 0.5 * (Pgain * error));

            rightSpeed = Math.min(rightSpeed, maxPower);
            leftSpeed = Math.min(leftSpeed, maxPower);
            rightSpeed = Math.max(rightSpeed, minPower);
            leftSpeed = Math.max(leftSpeed, minPower);

            engineL.setSpeed(leftSpeed);
            engineR.setSpeed(rightSpeed);

            LCD.drawInt(leftSpeed, 0, 5);
            LCD.drawInt(rightSpeed, 0, 6);

            engineR.forward();
            engineL.forward();

            Delay.msDelay(sampleInterval);
        }

        engineR.stop(true);
        engineL.stop(true);
    }

    @Override
    public void suppress() {
        isActive = false;
    }

    @Override
    public boolean takeControl() {
        return true;  // Always ready to take control
    }
}
