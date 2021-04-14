using System.Collections.Generic;
using System.Collections.Specialized;

namespace ApplicationAssistant
{
    public class DataGrid
    {
        private readonly Dictionary<string, string> _data;

        public DataGrid(Dictionary<string, string> data)
        {
            _data = data;
        }
        
        public string this[string key]
        {
            get => _data[key];
            set
            {
                _data[key] = value;

                UpdateDisplay(GetIndex(key), key);
            }
        }

        private void UpdateDisplay(int index, string key)
        {
            System.Console.CursorTop = index;
            System.Console.CursorLeft = 23;
            
            Console.Write(_data[key].PadRight(60));
        }

        private int GetIndex(string key)
        {
            int index = 0;
            foreach (var keyValuePair in _data)
            {
                if (!keyValuePair.Key.Equals(key))
                    index++;
                else
                    return index;
            }

            return -1;
        }

        public void Display()
        {
            System.Console.Clear();
            
            foreach (var keyValuePair in _data)
            {
                System.Console.Write(keyValuePair.Key.PadLeft(20));
                System.Console.Write(" | ");
                System.Console.WriteLine(keyValuePair.Value.PadLeft(60));
            }
        }
        
        public void ParseDisplay()
        {
            System.Console.Clear();
            
            foreach (var keyValuePair in _data)
            {
                Console.Write(keyValuePair.Key.PadLeft(20));
                Console.Write(" | ");
                Console.WriteLine(keyValuePair.Value.PadRight(60));
            }
        }
    }
}