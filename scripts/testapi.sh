URL=http://localhost:5000/meterreadings/importapi

curl -X POST -F 'File=@"/Users/Matt/Dev/exercises/dotnet_examples/ensek/docs/Meter_Reading.csv";type=text/csv' $URL
