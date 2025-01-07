import lejos.hardware.Sound;
import lejos.hardware.lcd.LCD;
import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.hardware.sensor.NXTTouchSensor;
import lejos.robotics.subsumption.Behavior;

/**
 * Behavior 4: Triggered when the touch sensor is pressed.
 * The robot reverses for a set distance using proportional control,
 * beeps sorrowfully, pauses for 3 seconds, and resumes.
 */
public class bumperCollision implements Behavior {

    // Class variables accessible to all methods
    private final NXTRegulatedMotor engineR;
    private final NXTRegulatedMotor engineL;
    private final NXTTouchSensor tSensorR;

    private float[] rBumperSample;
    private boolean bRightBumper = false;
    private boolean isActive = false;

    private final float SPEEDFACTOR = 0.3F;
    private final float Kp = 2.0F;
    private final int TARGET_DISTANCE = 10;
    private final float WHEEL_DIAMETER = 4.0F;
    private final float PI = 3.14159F;

    // Constructor to initialize the motors and touch sensor.
    public BumperCollision(NXTTouchSensor touchR, NXTRegulatedMotor motorR, NXTRegulatedMotor motorL) {
        engineR = motorR;
        engineL = motorL;
        tSensorR = touchR;
        tSensorR.setCurrentMode("Touch");
        rBumperSample = new float[tSensorR.sampleSize()];
    }

    private void resetBoolean() {
        bRightBumper = false;
    }


    @Override
    public void action() {
        isActive = true;

        LCD.clear();
        LCD.drawString("Behavior 4", 0, 0);

        float wheelCircumference = PI * WHEEL_DIAMETER;
        int rotationsNeeded = (int)((TARGET_DISTANCE / wheelCircumference) * 360);
        engineR.resetTachoCount();
        engineL.resetTachoCount();

        engineR.setSpeed(engineR.getMaxSpeed() * SPEEDFACTOR);
        engineL.setSpeed(engineL.getMaxSpeed() * SPEEDFACTOR);

        engineR.backward();
        engineL.backward();

        while (isActive) {
            int tachoCountR = Math.abs(engineR.getTachoCount());
            int tachoCountL = Math.abs(engineL.getTachoCount());
            int tachoCount = Math.min(tachoCountR, tachoCountL);

            // Calculate the distance traveled so far
            float currentDistance = (tachoCount / 360.0F) * wheelCircumference;
            LCD.drawString("Distance: " + currentDistance, 0, 1);

            if (currentDistance >= TARGET_DISTANCE) {
                engineR.stop(true);
                engineL.stop(true);
                break;
            }

            float error = TARGET_DISTANCE - currentDistance;
            float adjustment = Math.abs(Kp * error);
            engineR.setSpeed(Math.max(50, Math.min(adjustment, engineR.getMaxSpeed() * SPEEDFACTOR)));
            engineL.setSpeed(Math.max(50, Math.min(adjustment, engineL.getMaxSpeed() * SPEEDFACTOR)));

            try {
                Thread.sleep(50);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
      
        engineR.stop(true);
        engineL.stop(true);
        LCD.drawString("Beeping...", 0, 2);
        Sound.playTone(440, 500);
        Sound.playTone(220, 500);

        try {
            Thread.sleep(3000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        LCD.drawString("Resuming...", 0, 3);
    }

    @Override
    public void suppress() {
        isActive = false;
        engineR.stop(true);
        engineL.stop(true);
    }

    @Override
    public boolean takeControl() {
        resetBoolean();
        tSensorR.fetchSample(rBumperSample, 0);
        bRightBumper = (rBumperSample[0] == 1.0);
        return bRightBumper;
    }
}
