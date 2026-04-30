# Circle From Arc Calculator

A WPF calculator for finding the full circle radius and diameter from:

- `AB`: chord length in millimeters
- `CD`: sagitta height in millimeters, measured at 90 degrees from the chord midpoint to the arc

## Formula

Let:

```text
a = AB / 2
s = CD
```

Then:

```text
radius = (a^2 + s^2) / (2s)
diameter = 2 * radius
```

Equivalent radius form:

```text
radius = (AB^2 / (8 * CD)) + (CD / 2)
```

## Build

```powershell
$env:DOTNET_CLI_HOME=(Join-Path (Get-Location) '.dotnet-home')
dotnet build CircleFromArcCalculator.slnx
```

## Run

```powershell
$env:DOTNET_CLI_HOME=(Join-Path (Get-Location) '.dotnet-home')
dotnet run --project .\CircleFromArcCalculator\CircleFromArcCalculator.csproj
```
