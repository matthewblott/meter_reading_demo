# Meter Reading Demo

## Running

To run simply enter the following in a terminal and browse to ```http://localhost:5000```:

```
cd src/Ensek.WebUI/
dotnet run
```

You can also upload via ```curl```:

```
cd docs
curl -X POST -F 'File=@"Meter_Reading.csv";type=text/csv' http://localhost:5000/meterreadings/import
```

### Database

The ```ensek.sqlite``` used with the application is found in the ```db``` folder along with the ```log.sqlite``` database.


## Testing

To run the tests simply enter the following in a terminal:

```
cd src/Ensek.Tests
dotnet test
```

