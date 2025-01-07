import lejos.hardware.lcd.LCD;
import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.hardware.sensor.NXTLightSensor;
import lejos.robotics.SampleProvider;
import lejos.robotics.subsumption.Behavior;

/*********************************************************
 * This class implements a light-based behavior for the robot.
 * The behavior is triggered when the ambient light level exceeds
 * a specified threshold. When active, it rotates the ultrasound
 * sensor motor and displays light values on the EV3 screen.
 **********************************************************/
public class lightBehavior implements Behavior {

    // Constants for behavior thresholds
    private static final int LIGHT_THRESHOLD = 50;
    private static final int WALL_DISTANCE = 20;

    // Instance variables for state, sensors, and motors
    private boolean isActive;
    private NXTLightSensor lightSensor;
    private float[] lightValue = new float[1];
    private SampleProvider lightAmbient;
    private NXTRegulatedMotor ultrasoundMotor;

    // Constructor to initialize sensors, motors, and desired settings
    public LightBehavior(NXTLightSensor lightSensor, IntHolder desiredWallDistance, NXTRegulatedMotor ultrasoundMotor) {
        this.lightSensor = lightSensor;
        this.lightAmbient = lightSensor.getMode("Ambient");
        this.ultrasoundMotor = ultrasoundMotor;
        this.isActive = false;
    }

    @Override
    public void action() {
        isActive = true;

        LCD.clear();
        LCD.drawString("Light Behavior", 0, 5);

        BehaviorRobot.Ultrasound_State = 1;
        BehaviorRobot.Wall_Distance = WALL_DISTANCE;

        ultrasoundMotor.setSpeed(100);
        ultrasoundMotor.rotate(180);

        while (isActive) {
            lightAmbient.fetchSample(lightValue, 0);
            LCD.drawString("Light: " + lightValue[0], 0, 6);
        }
    }

    // Method to stop the behavior
    @Override
    public void suppress() {
        isActive = false;
    }

    @Override
    public boolean takeControl() {
        lightAmbient.fetchSample(lightValue, 0);
        return lightValue[0] > LIGHT_THRESHOLD;
    }
}
