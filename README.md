# PassportMD

A personal health tracking app built with .NET MAUI. Create a health profile, record vitals, and get instant feedback on your BMI, blood pressure, and heart rate with personalized tips and health flags.

## Features

- **Create Profile** -- Enter name, age, height, and weight
- **Enter Vitals** -- Record blood pressure (systolic/diastolic) and resting heart rate
- **Health Summary** -- View BMI, blood pressure, and heart rate categories with color-coded indicators and personalized tips
- **Health Flags** -- Automatic warnings when readings fall outside healthy ranges
- **Update Weight** -- Quick weight-only updates without re-entering your full profile
- **Save / Load** -- Persist passports to CSV files and reload them later
- **Clear Passport** -- Delete saved passports or clear the current session

## Tech Stack

- **Framework:** .NET 8 MAUI
- **Language:** C# 12
- **Platforms:** Android, iOS, Windows, macOS (Catalyst)
- **Storage:** CSV file persistence (app data directory)

## Project Structure

```
PassportMD/
├── Models/
│   ├── PersonProfile.cs    — Profile data + BMI calculation
│   ├── Vitals.cs           — BP + heart rate data + categories
│   └── HealthPassport.cs   — Container with health flags
├── Pages/
│   ├── CreateProfilePage   — Profile creation form
│   ├── VitalsPage          — Vitals entry form
│   ├── SummaryPage         — Full health summary display
│   └── UpdateWeightPage    — Weight-only update form
├── Services/
│   └── CsvRepository.cs    — CSV save/load/delete logic
├── MainPage                — Home menu with navigation
├── AppShell                — Shell navigation setup
└── App                     — Application entry point
```

## Building and Running

### Prerequisites

- .NET 8 SDK
- .NET MAUI workload: `dotnet workload install maui`
- Visual Studio 2022 17.8+ (recommended) or VS Code with MAUI extension

### Run on Windows

```bash
dotnet build -f net8.0-windows10.0.19041.0
dotnet run -f net8.0-windows10.0.19041.0
```

### Run on Android

```bash
dotnet build -f net8.0-android
```

Deploy to emulator or device via Visual Studio or `adb install`.

## Roadmap

- [ ] Vitals history — track readings over time with date stamps
- [ ] Charts — visualize BMI and vitals trends
- [ ] Multiple profiles — family health tracking
- [ ] Export to PDF — shareable health passport document
- [ ] Unit tests — model and service layer test coverage
- [ ] Dark mode support
- [ ] Localization and metric units option

## Origin

Originally built as a Java console application (FitTide), then ported to C# (.NET 8 console), and finally brought to life as a cross-platform mobile/desktop app with .NET MAUI.

## License

This project is for personal and educational use.
