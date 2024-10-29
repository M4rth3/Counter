namespace Counter.Views;
using Counter.Models;
using System.Diagnostics.Metrics;

public partial class CounterView : ContentView
{
    string _fileName;
    private int count = 0;
    private Models.Counter_class counter_object;
    private List<string> CounterData = new List<string>();
    public string Name;

    public CounterView(string Name, int startValue, Color color)
    {
        this.Name = Name;
        string directory = Path.Combine(FileSystem.AppDataDirectory, Name);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        _fileName = Path.Combine(directory, "counter.txt");
        InitializeComponent();
        CreateCounter(Name, startValue, color);
    }

    public CounterView(string Name)
    {
        this.Name = Name;
        string directory = Path.Combine(FileSystem.AppDataDirectory, Name);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        _fileName = Path.Combine(directory, "counter.txt");
        InitializeComponent();
        counter_object = new Models.Counter_class { CounterName = Name };

        if (File.Exists(_fileName))
        {
            foreach (string line in File.ReadLines(_fileName))
            {
                CounterData.Add(line);
            }

            counter_object.StarterValue = int.Parse(CounterData[1]);
            counter_object.CurrValue = int.Parse(CounterData[2]);
            byte r = byte.Parse(CounterData[3]);
            byte g = byte.Parse(CounterData[4]);
            byte b = byte.Parse(CounterData[5]);
            counter_object.color = Color.FromRgb(r, g, b);
        }
        else
        {
            counter_object.CurrValue = -5;
        }
        count = counter_object.CurrValue;
        BindingContext = counter_object;
    }

    private void Save(string filename, List<string> data)
    {
        File.WriteAllLines(filename, data);
    }

    private void CreateCounter(string Name, int startValue, Color color)
    {
        counter_object = new Models.Counter_class
        {
            CounterName = Name,
            CurrValue = startValue,
            color = color
        };

        count = startValue;
        BindingContext = counter_object;

        CounterData = new List<string>
        {
            counter_object.CounterName,
            startValue.ToString(),
            counter_object.CurrValue.ToString(),
            ((int)(color.Red * 255)).ToString(),
            ((int)(color.Green * 255)).ToString(),
            ((int)(color.Blue * 255)).ToString()
        };

        Save(_fileName, CounterData);
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        if (sender == Counterplus)
        {
            count++;
            counter_object.CurrValue = count;
        }
        else if (sender == Counterminus)
        {
            count--;
            counter_object.CurrValue = count;
        }

        C_Label.Text = count.ToString();
        CounterData = new List<string>
            {
                counter_object.CounterName,
                counter_object.StarterValue.ToString(),
                counter_object.CurrValue.ToString(),
                ((int)(counter_object.color.Red * 255)).ToString(),
                ((int)(counter_object.color.Green * 255)).ToString(),
                ((int)(counter_object.color.Blue * 255)).ToString()
            };
        Save(_fileName, CounterData);
    }

    private void Suicide(object sender, EventArgs e)
    {
        var mainPage = this.Parent.Parent.Parent as MainPage;
        if (mainPage != null)
        {
            mainPage.DeleteCounter(this);
        }
        if (File.Exists(_fileName))
            File.Delete(_fileName);
    }
}
