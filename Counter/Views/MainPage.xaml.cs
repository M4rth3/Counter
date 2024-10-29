using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Counter.Views
{
    public partial class MainPage : ContentPage
    {
        List<ContentView> counters = new List<ContentView>();
        List<string> counters_names = new List<string>();

        public MainPage()
        {
            InitializeComponent();

            string filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.txt");
            if (File.Exists(filePath))
            {
                counters_names = File.ReadAllLines(filePath).ToList();
            }

             for (int i = 0; i < counters_names.Count; i++)
             {
                 RecreateCounter(counters_names[i]);
             }


        }

        private void AddCounter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SValueTextBox.Text) ||
                string.IsNullOrWhiteSpace(RTextBox.Text) ||
                string.IsNullOrWhiteSpace(GTextBox.Text) ||
                string.IsNullOrWhiteSpace(BTextBox.Text))
            {
                ErrorNotAll.IsVisible = true;
            }
            else
            {
                ErrorNotAll.IsVisible = false;
                if (!int.TryParse(SValueTextBox.Text, out int startValue))
                {
                    ErrorNumber.IsVisible = true;
                    return;
                }
                else
                {
                    ErrorNumber.IsVisible = false;

                    if (!int.TryParse(RTextBox.Text, out int r) || r < 0 || r > 255 ||
                        !int.TryParse(GTextBox.Text, out int g) || g < 0 || g > 255 ||
                        !int.TryParse(BTextBox.Text, out int b) || b < 0 || b > 255)
                    {
                        ErrorColor.IsVisible = true;
                    }
                    else
                    {
                        if (!counters_names.Contains(NameTextBox.Text))
                        {
                            ErrorName.IsVisible = false;
                            counters_names.Add(NameTextBox.Text);
                            ErrorColor.IsVisible = false;
                            Color color = new Color(r, g, b);
                            var counterView = new CounterView(NameTextBox.Text, startValue, color);
                            MainPageLayout.Children.Add(counterView);
                            counters.Add(counterView);
                            Save();
                        }
                        else
                        {
                            ErrorName.IsVisible = true; 
                            return;
                        }
                    }
                        
                }
            }
        }
        private void RecreateCounter(string name)
        {
            var counterView = new CounterView(name);
            MainPageLayout.Children.Add(counterView);
            counters.Add(counterView);
        }

        private void DeleteLastCounter(object sender, EventArgs e)
        {
            var counterView = counters.LastOrDefault();
            if (counterView != null)
            {
                MainPageLayout.Children.Remove(counterView);
                counters.Remove(counterView);

                counters_names.RemoveAt(counters_names.Count - 1);
                Save();
            }
        }

        public void DeleteCounter(CounterView counter)
        {
            MainPageLayout.Children.Remove(counter);
            counters.Remove(counter);
            counters_names.Remove(counter.Name);
            Save();
        }

        private void Save()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.txt");
            File.WriteAllLines(filePath, counters_names);
        }
    }
}
