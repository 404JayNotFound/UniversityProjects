import lejos.hardware.Sound;
import lejos.hardware.lcd.LCD;
import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.hardware.sensor.NXTSoundSensor;
import lejos.robotics.subsumption.Behavior;
import lejos.utility.Delay;

/**
 * Behavior to respond to sound levels above a predefined threshold.
 * The robot reacts based on the detected sound level and the current state of the ultrasound behavior.
 */
public class soundBehavior implements Behavior {

    private boolean isActive;
    private NXTSoundSensor soundSensor;
    private int soundThreshold = 60;

    private NXTRegulatedMotor engineR;
    private NXTRegulatedMotor engineL;

    public SoundBehavior(NXTRegulatedMotor motorR, NXTRegulatedMotor motorL, NXTSoundSensor soundSensor) {
        this.soundSensor = soundSensor;
        this.engineR = motorR;
        this.engineL = motorL;
        this.isActive = false;
    }

    public void setSoundThreshold(int threshold) {
        this.soundThreshold = threshold;
    }


    @Override
    public void action() {
        isActive = true;
        LCD.clear();
        LCD.drawString("Behavior: Sound", 0, 5);

        while (isActive) {
            int soundLevel = getSoundLevel();

            if (soundLevel > soundThreshold) { 
                LCD.drawString("Sound detected!", 0, 2);

                if (BehaviorRobot.Ultrasound_State == 0) {
                    LCD.drawString("Ultrasound_State = 0", 0, 3);
                    rotateCounterClockwise();
                } else if (BehaviorRobot.Ultrasound_State == 1) {
                    LCD.drawString("Ultrasound_State = 1", 0, 3);
                    playVictoryDance();
                    terminateProgram();
                    break;
                }
            }

            Delay.msDelay(500);
        }
    }

    private int getSoundLevel() {
        float[] sample = new float[soundSensor.sampleSize()];
        soundSensor.fetchSample(sample, 0);
        return (int) (sample[0] * 100);
    }

    private void rotateCounterClockwise() {
        engineR.setSpeed(200);
        engineL.setSpeed(200);

        engineR.rotate(-180);
        engineL.rotate(180);
    }

    private void playVictoryDance() {
        LCD.clear();
        LCD.drawString("Victory Dance!", 0, 0);

        for (int i = 0; i < 3; i++) {
            Sound.playTone(440, 500);
            Delay.msDelay(200);
        }
    }

    private void terminateProgram() {
        LCD.clear();
        LCD.drawString("Terminating Program", 0, 0);
        System.exit(0);
    }

    @Override
    public void suppress() {
        isActive = false;
    }

    @Override
    public boolean takeControl() {
        int soundLevel = getSoundLevel();
        return soundLevel > soundThreshold;
    }
}
