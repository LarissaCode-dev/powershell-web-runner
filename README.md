# PowerShell Web Runner

A .NET Core web application that executes PowerShell scripts and displays results in a web interface. Built with ASP.NET Core MVC, this application provides a clean interface to execute and view PowerShell script results.

## Features

- Execute PowerShell scripts through a web interface
- Filter process results by name
- Clean and responsive Bootstrap UI
- Cross-platform compatible (Windows/macOS/Linux)
- Optional debug logging
- RESTful design with POST-REDIRECT-GET pattern

## Requirements

- .NET 8.0
- PowerShell Core (pwsh)
- Web browser

## Setup

1. Clone the repository:
```bash
git clone https://github.com/YOUR_USERNAME/powershell-web-runner.git
```

2. Navigate to the project directory:
```bash
cd powershell-web-runner
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Run the application:
```bash
dotnet run
```

5. Open your browser and navigate to:
```
https://localhost:5001
```

## Usage

1. Access the web interface
2. (Optional) Enter a process name to filter results
3. Click "Search" to execute the PowerShell script
4. View results in the table below

## Project Structure

- `/Controllers` - MVC Controllers
- `/Models` - Data models
- `/Views` - Razor views
- `/Services` - PowerShell execution service
- `/Scripts` - PowerShell scripts

## Contributing

Feel free to submit issues and enhancement requests!
