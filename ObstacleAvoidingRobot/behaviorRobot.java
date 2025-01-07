import lejos.hardware.lcd.LCD;
import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.hardware.port.MotorPort;
import lejos.hardware.port.SensorPort;
import lejos.hardware.sensor.NXTLightSensor;
import lejos.hardware.sensor.NXTSoundSensor;
import lejos.hardware.sensor.NXTTouchSensor;
import lejos.hardware.sensor.NXTUltrasonicSensor;
import lejos.robotics.subsumption.Arbitrator;
import lejos.robotics.subsumption.Behavior;

public class behaviorRobot {
    // Define sensor and motor objects needed by the robot
    NXTRegulatedMotor engineR = new NXTRegulatedMotor(MotorPort.B);
    NXTRegulatedMotor engineL = new NXTRegulatedMotor(MotorPort.C);
    NXTRegulatedMotor sensorMotor = new NXTRegulatedMotor(MotorPort.A);

    NXTTouchSensor bumperR = new NXTTouchSensor(SensorPort.S1);
    NXTLightSensor light = new NXTLightSensor(SensorPort.S2);
    NXTUltrasonicSensor uSensor = new NXTUltrasonicSensor(SensorPort.S3);
    NXTSoundSensor soundSensor = new NXTSoundSensor(SensorPort.S4);

    Arbitrator arby;
    Behavior[] behaviorList = new Behavior[6];

    IntHolder desiredDistance = new IntHolder(30);

    // Constants and state variables as per project requirements
    public static final int Wander_Speed = 350;
    public static int Ultrasound_State = 0;
    public static int Wall_Distance = 20;
    public static int Sound_Threshold = 60;
    public static final int SPEED = 360;

    // Constructor to initialize the robot and its behaviors
    BehaviorRobot() throws Exception {
        uSensor.setCurrentMode("Distance");
        desiredDistance.value = 30;

        // Initialize the list of behaviors
        behaviorList[0] = new MoveForward(uSensor, engineR, engineL, desiredDistance);
        behaviorList[1] = new SoundBehavior(engineL, engineR, soundSensor);
        behaviorList[2] = new LightBehavior(light, desiredDistance, sensorMotor);
        behaviorList[3] = new UltrasonicCollision(uSensor, engineR, engineL, sensorMotor);
        behaviorList[4] = new BumperCollision(bumperR, engineR, engineL);
        behaviorList[5] = new EmergencyStop(engineR, engineL);

        arby = new Arbitrator(behaviorList);
    }

    // Program entry point
    public static void main(String[] args) throws Exception {
        // Create an instance of the BehaviorRobot class
        BehaviorRobot SampleBehaviors = new BehaviorRobot();
        LCD.drawString("Arbitrating", 0, 0);
        // Start the arbitrator
        SampleBehaviors.arby.go();
    }
}
