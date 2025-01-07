import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.hardware.sensor.NXTUltrasonicSensor;
import lejos.robotics.subsumption.Behavior;
import lejos.hardware.lcd.LCD;

/**
 * Behavior for ultrasonic collision detection.
 * The robot uses an ultrasonic sensor to detect obstacles, adjusts its motor speeds to follow or avoid walls,
 * and rotates the ultrasonic sensor motor to check surroundings.
 */
public class ultrasonicCollision implements Behavior {

    private final NXTRegulatedMotor engineR;
    private final NXTRegulatedMotor engineL;
    private final NXTUltrasonicSensor uSensor;
    private final NXTRegulatedMotor sensorMotor;
    private float[] distanceSample;
    private boolean isActive;

    public UltrasonicCollision(
        NXTUltrasonicSensor sensor, 
        NXTRegulatedMotor motorR, 
        NXTRegulatedMotor motorL, 
        NXTRegulatedMotor sensorMotor
    ) {
        this.engineR = motorR;
        this.engineL = motorL;
        this.uSensor = sensor;
        this.sensorMotor = sensorMotor;
        this.uSensor.setCurrentMode("Distance");
        this.distanceSample = new float[uSensor.sampleSize()];
        this.isActive = false;
    }

    @Override
    public void action() {
        isActive = true;

        LCD.clear();
        LCD.drawString("Behavior 1: Ultrasonic", 0, 5);

        engineR.setSpeed(BehaviorRobot.SPEED * 0.5F);
        engineL.setSpeed(BehaviorRobot.SPEED * 0.5F);

        if (BehaviorRobot.Ultrasound_State == 0) {
            sensorMotor.rotateTo(90);
            trackWall("right");
        } else if (BehaviorRobot.Ultrasound_State == 1) {
            sensorMotor.rotateTo(-90);
            trackWall("left");
        }

        engineR.stop(true);
        engineL.stop(true);
    }

    private void trackWall(String direction) {
        while (isActive) {
            uSensor.fetchSample(distanceSample, 0);

            float error = BehaviorRobot.Wall_Distance - (distanceSample[0] * 100);
            int adjustment = (int) (error * 5);

            if (direction.equals("right")) {
                engineR.setSpeed(BehaviorRobot.SPEED - adjustment);
                engineL.setSpeed(BehaviorRobot.SPEED + adjustment);
            } else if (direction.equals("left")) {
                engineR.setSpeed(BehaviorRobot.SPEED + adjustment);
                engineL.setSpeed(BehaviorRobot.SPEED - adjustment);
            }

            engineR.forward();
            engineL.forward();

            if (distanceSample[0] > 1.0) break;
        }
    }

    @Override
    public void suppress() {
        isActive = false;
    }

    @Override
    public boolean takeControl() {
        uSensor.fetchSample(distanceSample, 0);

        if (distanceSample[0] < 0.15) {
            BehaviorRobot.Ultrasound_State = 1;
        } else if (distanceSample[0] < 0.25) {
            BehaviorRobot.Ultrasound_State = 0;
        }
        return distanceSample[0] < 0.30;
    }
}
