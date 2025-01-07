import lejos.hardware.Button;
import lejos.hardware.Sound;
import lejos.hardware.lcd.LCD;
import lejos.hardware.motor.NXTRegulatedMotor;
import lejos.robotics.subsumption.Behavior;

/*********************************************************
 * This class implements the robot's emergency stop function.
 * When the ENTER button is pressed, the robot stops its movement
 * immediately and waits for further button input.
 * - RIGHT  -> The robot resumes its movement.
 * - ESCAPE -> The robot's control program exits.
 **********************************************************/
public class emergencyStop implements Behavior 
{
    protected final NXTRegulatedMotor engineR;
    protected final NXTRegulatedMotor engineL;
    EmergencyStop(NXTRegulatedMotor motorR, NXTRegulatedMotor motorL)
    {
        engineR = motorR;
        engineL = motorL;
    }
    
    // Method executed when this behavior gains control of the robot
    public void action() 
    {
        engineR.stop(true);
        engineL.stop(true);
        
        int buttonID;

        LCD.clear();
        LCD.drawString("Behavior: Emergency Stop", 0, 1);
        LCD.drawString("Arbitration halted", 0, 2);
        LCD.drawString("RIGHT to Continue", 0, 3);
        LCD.drawString("ESCAPE to Exit", 0, 4);
        Button.LEDPattern(0);   // Set the robot's LED to indicate a halt state

        do 
        {
            buttonID = Button.waitForAnyPress(); 
        } while (buttonID != Button.ID_RIGHT && buttonID != Button.ID_ESCAPE);

        if (buttonID == Button.ID_ESCAPE) 
        {   
            
            LCD.clear();
            LCD.drawString("Exiting Program", 0, 0);
            LCD.drawString("Goodbye!", 0, 1);
            Sound.playTone(440, 500);
            Sound.playTone(220, 500);
            System.exit(1);
        }
        else 
        {
            LCD.clear();
            LCD.drawString("Resuming...", 0, 0);
            // The motors will be reactivated by the arbitrator's next behavior
        }
    }
    
    public void suppress() {}

    public boolean takeControl() 
    {
        return Button.ENTER.isDown();
    }
}
