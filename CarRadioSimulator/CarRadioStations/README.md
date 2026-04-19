# Car Radio Simulator — WPF / C#

A WPF application that simulates an FM car radio with full
Radio Data System (RDS) support.

---

## Quick Start

### 1 — Place the data file
Copy `StationsInformation.txt` to:
```
C:\StationsInfo\StationsInformation.txt
```

### 2 — Build & Run
Open `CarRadioSimulator.csproj` in Visual Studio 2022 (or later)
and press **F5**, *or* from the terminal:

```bash
dotnet run --project CarRadioSimulator.csproj
```

Requires **.NET 8 SDK** (Windows) with WPF workload.

---

## File Structure

```
CarRadioSimulator/
├── RadioStation.cs          # RDS data model (properties + validation)
├── StationLoader.cs         # File parser → Dictionary<double, RadioStation>
├── MainWindow.xaml          # Full WPF UI (dark radio-panel aesthetic)
├── MainWindow.xaml.cs       # All interaction logic & chart drawing
├── App.xaml / App.xaml.cs   # Application entry-point
├── CarRadioSimulator.csproj # SDK-style project file (.NET 8 WPF)
└── StationsInformation.txt  # Sample data (copy to C:\StationsInfo\)
```

---

## Architecture Decisions

### Data Model — `RadioStation`
Encapsulates all RDS fields with validated properties:
| Property | Type | Validation |
|---|---|---|
| `StationName` | `string` | Non-empty |
| `HasTravelInfo` | `bool` | — |
| `RadioText` | `string` | Trimmed |
| `BaseFrequency` | `double` | 88.0 – 108.0 MHz |
| `AlternativeFrequency` | `double` | 88.0 – 108.0 MHz |

### Data Container — `Dictionary<double, RadioStation>`
Keyed by **BaseFrequency** so that when the dial is turned the
application can look up the matching station in O(1) time, rather
than scanning a list on every slider-change event.

### Frequency Mapping Formula
The slider spans **0 – 200** (potentiometer equivalent).
The FM band spans **88.0 – 108.0 MHz**.

```
f(x) = FmMin + (x − SliderMin) / (SliderMax − SliderMin) × (FmMax − FmMin)
     = 88.0  + (x − 0)         / (200 − 0)               × (108.0 − 88.0)
     = 88.0  + x/200 × 20
```

This is a simple **linear interpolation** mapping [0,200] → [88,108].

A snap window of **±0.2 MHz** is used so that the user does not need to
position the slider with pixel-perfect precision to lock onto a station.

---

## UI Features

| Feature | Where |
|---|---|
| Load Stations button | Toolbar + Menu → File → Load Stations |
| Frequency dial (slider) | Tuner Dial panel |
| RDS info cards | Station / Radio Text / Base Freq / Alt Freq |
| TA travel-info LED | Green indicator, lights when station has travel info |
| Filter (From / To combos + Filter button) | Filter toolbar row |
| Filter-active checkbox | Auto-ticked by Filter button, read-only |
| Display Stations button | Refreshes list view + draws bar chart |
| Bar chart | Canvas inside scrollable pane, colour-coded by frequency |
| Clear Filter button | Resets to all stations |
| Menu bar | File → Load / Filter / Exit |
| Status bar | Live feedback on every action |

---

## StationsInformation.txt Format
```
[Station Name]
96FM
...
[Travel Information]
No
...
[Radio Text]
Joe Murphy Show
...
[Base Frequency]
96.0
...
[Alternative Frequency]
96.9
...
```
Each section header uses square-bracket tags.
All sections must contain the same number of entries.
