import numpy as np
import serial
import keyboard
import matplotlib
from matplotlib import pyplot as plt

def setup_serial_connection(port='COM4', baudrate=9600, timeout=2):
    """
    Sets up the serial communication with the given parameters.

    Returns:
        serial.Serial: A configured serial connection object.
    """
    return serial.Serial(port, baudrate=baudrate, bytesize=8, parity='N', stopbits=1, timeout=timeout)


def setup_plot():
    """
    Sets up the radar plot window with the required configurations.

    This function creates the figure, sets up polar axes, and initializes
    plot elements like the radar points and lines.

    Returns:
        tuple: Contains the figure, axis, plot objects, distance array, 
               angle array, and maximum radius for the radar.
    """
    # Use TkAgg backend for interactive plotting
    matplotlib.use('TkAgg')
    fig = plt.figure(facecolor='k')
    fig.canvas.toolbar.pack_forget()
    fig.canvas.manager.set_window_title('Radar Plot')
    
    # Maximize the plot window
    mng = plt.get_current_fig_manager()
    mng.window.state('zoomed')

    # Create polar axis
    ax = fig.add_subplot(1, 1, 1, polar=True, facecolor='#006b70')
    ax.tick_params(axis='both', colors='w')

    # Radar settings
    r_max = 100.0
    ax.set_ylim([0.0, r_max])                       # Set radius range
    ax.set_xlim([0.0, np.pi])                       # Set angle range (0 to 180 degrees in radians)
    ax.set_position([-0.05, -0.05, 1.1, 1.05])      # Adjust axis position for better appearance
    ax.set_rticks(np.linspace(0.0, r_max, 5))       # Set radial ticks
    ax.set_thetagrids(np.linspace(0, 180, 10))      # Set angular ticks
    ax.grid(color='w', alpha=0.4)                   # Grid style

    # Prepare for data plotting
    angles = np.arange(0, 181, 1)       # Angles from 0 to 180 degrees
    theta = angles * (np.pi / 180.0)    # Convert degrees to radians

    # Initialize plot objects for radar points and current angle line
    pols, = ax.plot([], linestyle='', marker='o', markerfacecolor='r', markeredgecolor='w',
                    markeredgewidth=1.0, markersize=3.0, alpha=0.9)
    line1, = ax.plot([], color='w', linewidth=4.0)

    # Initialize distance array (set to 1 as default)
    dists = np.ones((len(angles),))

    # Display initial figure
    fig.canvas.draw()
    fig.show()

    return fig, ax, pols, line1, dists, theta, r_max


def update_radar_plot(ax, fig, pols, line1, dists, angle, dist, theta, r_max):
    """
    Updates the radar plot with new data for a given angle and distance.

    Args:
        ax (matplotlib.axes._polar.Axes): The polar axis object.
        fig (matplotlib.figure.Figure): The figure object for the plot.
        pols (matplotlib.lines.Line2D): The plot object for radar points.
        line1 (matplotlib.lines.Line2D): The plot object for the angle line.
        dists (numpy.ndarray): Array holding distance data for all angles.
        angle (float): The current angle (in degrees) of the radar scan.
        dist (float): The measured distance at the given angle.
        theta (numpy.ndarray): Array of angle values (in radians).
        r_max (float): Maximum radius value for the plot.
    """
    # Update the distance array at the specified angle
    dists[int(angle)] = dist

    # Update the radar plot with new data
    pols.set_data(theta, dists)              # Update the scatter plot with distances
    fig.canvas.restore_region(axbackground)  # Restore the background for redrawing
    ax.draw_artist(pols)                     # Redraw the points on the plot

    # Draw a line for the current angle (from center to the maximum radius)
    line1.set_data(np.repeat(angle * (np.pi / 180), 2), np.linspace(0.0, r_max, 2))
    ax.draw_artist(line1)  # Draw the current angle line

    # Refresh the plot
    fig.canvas.blit(ax.bbox)   # Update the area with changes
    fig.canvas.flush_events()  # Ensure immediate update to the plot


def process_serial_data(data):
    """
    Processes the raw serial data, extracting angle and distance if valid.

    Args:
        data (bytes): Raw data from the serial port.

    Returns:
        tuple: The angle and distance as floats if valid, otherwise None, None.
    """
    decoded = data.decode().strip()  # Decode and strip newline characters
    if ',' in decoded:
        try:
            # Attempt to split the data into angle and distance, and convert to float
            angle, dist = map(float, decoded.split(','))
            if 0 < dist < 200:  # Only process distances in a valid range
                return angle, dist
        except ValueError:
            # Handle case where the data can't be converted to float
            print(f"Invalid data format: {decoded}")
    return None, None  # Return None if data is invalid


def main():
    """
    Main function to run the radar plot and serial data handling.
    
    This function initializes the serial connection, sets up the plot, 
    and enters a loop to continuously read and process data from the serial 
    port to update the radar plot. The user can exit the loop by pressing 
    the 'q' key.
    """
    # Set up serial connection
    ser = setup_serial_connection()

    # Set up the radar plot
    fig, ax, pols, line1, dists, theta, r_max = setup_plot()

    # Prepare for animation updates
    fig.canvas.blit(ax.bbox)
    fig.canvas.flush_events()
    axbackground = fig.canvas.copy_from_bbox(ax.bbox)

    while True:
        try:
            # Read data from the serial port
            data = ser.readline()
            
            # Process the serial data to extract angle and distance
            angle, dist = process_serial_data(data)

            if angle is not None and dist is not None:
                # Update the radar plot with the new data
                update_radar_plot(ax, fig, pols, line1, dists, angle, dist, theta, r_max)

            # Check if the user pressed 'q' to quit the application
            if keyboard.is_pressed('q'):
                plt.close('all')
                print("User quit the application")
                break

        except KeyboardInterrupt:
            # Handle keyboard interrupt to gracefully exit
            plt.close('all')
            print('Keyboard Interrupt')
            break

    exit()  # Ensure the program exits properly

if __name__ == "__main__":
    main()  # Run the main function when the script is executed
