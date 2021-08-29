# Meter Reading Demo



## Running

To run simply enter the following in a terminal and browse to ```http://localhost:5000```:

```
cd src/Ensek.WebUI/
dotnet run
```

You can also upload via ```curl```:

```
curl -X POST -F 'File=@"Meter_Reading.csv";type=text/csv' http://localhost:5000/meterreadings/
```

## Testing

To run the tests simply enter the following in a terminal:

```
cd src/Ensek.Tests
dotnet test
```

You can also 

