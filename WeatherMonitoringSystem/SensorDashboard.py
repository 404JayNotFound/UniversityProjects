import pandas as pd                                     
from dash import Dash, dcc, html                        
from dash.dependencies import Output, Input             
import plotly.express as px                             
import pickle                                           
import os                                               
from google_auth_oauthlib.flow import InstalledAppFlow  
from google.auth.transport.requests import Request      
from googleapiclient.discovery import build             
from googleapiclient.http import MediaIoBaseDownload    
import io                                               

SCOPES = ['https://www.googleapis.com/auth/drive.file']  # Required scope for Google Drive file access
CSV_FILENAME = 'NameOfTheCSVFile.csv'                    # Name of the CSV file to be downloaded
CREDENTIALS_FILE = 'credentials.json'                    # Path to the Google API credentials file
FILE_ID = 'InsertFileIDHere'                             # ID of the file on Google Drive to be downloaded


def authenticate_google_drive():
    """
    Authenticates the user for Google Drive access using OAuth2 and returns
    a service client for interacting with Google Drive API.
    
        - First checks if credentials exist in 'token.pickle'.
        - If credentials are expired or invalid, it triggers the authentication flow.
    """
    creds = None

    # Check if credentials already exist (token.pickle stores the credentials)
    if os.path.exists('token.pickle'):
        with open('token.pickle', 'rb') as token:
            creds = pickle.load(token)

    # If there are no valid credentials or the token has expired, re-authenticate
    if not creds or not creds.valid:
        if creds and creds.expired and creds.refresh_token:
            creds.refresh(Request())  # Refresh expired credentials if possible
        else:
            # Start the OAuth2 authentication flow to get new credentials
            flow = InstalledAppFlow.from_client_secrets_file(CREDENTIALS_FILE, scopes=SCOPES)
            creds = flow.run_local_server(port=0)

        # Save the credentials for future use (in 'token.pickle')
        with open('token.pickle', 'wb') as token:
            pickle.dump(creds, token)

    # Build and return the Google Drive API client
    return build('drive', 'v3', credentials=creds)


def download_csv(file_id, filename=CSV_FILENAME):
    """
    Downloads a CSV file from Google Drive using its file ID and saves it locally.
    Args:
    - file_id (str): The unique ID of the file on Google Drive.
    - filename (str): The name of the local file where the CSV will be saved (default is 'sensor_data.csv').
    
    Returns:
    - filename (str): The name of the saved file.
    """
    # Initialize Google Drive service to interact with the API
    drive_service = authenticate_google_drive()

    # Request the file from Google Drive
    request = drive_service.files().get_media(fileId=file_id)
    file = io.BytesIO()  # This will use an in-memory byte stream to download the file
    downloader = MediaIoBaseDownload(file, request)

    # Download the file in chunks until fully downloaded
    done = False
    while not done:
        status, done = downloader.next_chunk()

    # Save the downloaded content into a local CSV file
    file.seek(0)    # Move back to the start of the byte stream
    with open(filename, 'wb') as f:
        f.write(file.read())

    return filename


def load_data():
    """
    Loads the downloaded CSV file into a pandas DataFrame for further processing.

    Returns:
    - df (DataFrame): A pandas DataFrame containing the data from the CSV.
    """
    return pd.read_csv(CSV_FILENAME, encoding='utf-8')


def create_figure(df, x_col, y_col, title):
    """
    Creates a line plot figure using Plotly Express.
    Args:
    - df (DataFrame): The pandas DataFrame containing the sensor data.
    - x_col (str): The column name for the x-axis (typically 'Timestamp').
    - y_col (str): The column name for the y-axis (sensor readings like 'Light Level (%)').
    - title (str): The title of the plot.
    
    Returns:
    - fig (plotly.graph_objects.Figure): A Plotly figure object for displaying the chart.
    """
    return px.line(df, x=x_col, y=y_col, title=title)


# Initialize Dash 
app = Dash(__name__)

# This is defining the layout of the app
app.layout = html.Div([
    html.H1("Sensor Data Dashboard"),  # Title of the dashboard
    # Interval component to refresh the data every 60 seconds
    dcc.Interval(id='interval-component', interval=60*1000, n_intervals=0),

    # Graphs
    dcc.Graph(id='light-level-over-time'),
    dcc.Graph(id='temperature-over-time'),
    dcc.Graph(id='humidity-over-time'),
    dcc.Graph(id='gas-concentration-over-time')
])


@app.callback(
    [Output('light-level-over-time', 'figure'),
     Output('temperature-over-time', 'figure'),
     Output('humidity-over-time', 'figure'),
     Output('gas-concentration-over-time', 'figure')],
    [Input('interval-component', 'n_intervals')]
)


def update_graphs(n):
    """
    Callback function to update the graphs with the latest data every time
    the Interval component triggers (every 60 seconds).
    This function downloads the latest CSV, loads it into a DataFrame, 
    and updates the four graphs accordingly.
    """
    # Download the latest data file from Google Drive
    download_csv(FILE_ID)

    # Load the newly downloaded data into a pandas DataFrame
    df = load_data()

    # Create the four graphs based on the latest data
    light_fig = create_figure(df, 'Timestamp', 'Light Level (%)', "Light Level Over Time")
    temp_fig = create_figure(df, 'Timestamp', 'Temperature (C)', "Temperature Over Time")
    humidity_fig = create_figure(df, 'Timestamp', 'Humidity (%)', "Humidity Over Time")
    gas_fig = create_figure(df, 'Timestamp', 'Gas Concentration (MQ135)', "Gas Concentration Over Time")

    # Return the updated figures for each graph
    return light_fig, temp_fig, humidity_fig, gas_fig


# Run the Dash app server to launch the web dashboard
if __name__ == '__main__':
    app.run_server(debug=True, host="InsertTheIPForHostHere")
