using ClientDeviceSimulator;
using Microsoft.VisualBasic.FileIO;

class Program
{
    static int lastReadLine = 0;

    static void Main()
    {
        using (var monitoringMessageQueueService = new MonitoringMessageQueueService())
        {
            Console.WriteLine("Press any key to stop the application.");

            var timer = new Timer(SendMonitoringValueCallback, monitoringMessageQueueService, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            Console.ReadKey();
        }
    }

    static void SendMonitoringValueCallback(object state)
    {
        if (state is MonitoringMessageQueueService monitoringMessageQueueService)
        {
            string filePath = Path.Combine("..", "..", "..", "sensor.csv");
            double sensorValue = ReadFromCsv(filePath);

            var monitoringMessage = new MonitoringDto
            {
                DeviceId = Guid.Parse("1ED39EE7-F559-4CC8-1E30-08DBEFF362AB"),
                MeasurmentValue = sensorValue
            };

            monitoringMessageQueueService.SendMessage(monitoringMessage);
        }
    }

    static double ReadFromCsv(string filePath)
    {
        var sensorValue = 0.0;
        try
        {
            // Read the CSV file
            using (var reader = new StreamReader(filePath))
            {
                // Skip lines already read
                for (int i = 0; i < lastReadLine; i++)
                {
                    reader.ReadLine();
                }

                // Read and process the next line
                string line = reader.ReadLine();
                if (line != null)
                {
                    // Process the data from the first column (assuming values are separated by commas)
                    string[] values = line.Split(',');
                    string firstColumnValue = values[0];

                    // Print or process the value as needed
                    Console.WriteLine($"Read value: {firstColumnValue}");

                    double.TryParse(firstColumnValue, out sensorValue);

                    // Increment the last read line
                    lastReadLine++;
                }
                else
                {
                    Console.WriteLine("End of file reached. Resetting to the first line.");
                    lastReadLine = 1;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return sensorValue;
    }

}
