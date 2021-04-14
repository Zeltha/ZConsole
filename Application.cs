using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZConsole
{
    public static class Application
    {
        public delegate T ArgumentParser<out T>(string value);
        
        private static Dictionary<string, string> _arguments;
        
        static Application()
        {
            ProcessCommandLineArguments();
        }

        private static void ProcessCommandLineArguments()
        {
            var args = Environment.GetCommandLineArgs();
            
            _arguments = new Dictionary<string, string>(args.Length);
            
            var r = new Regex("-([^\\s\\=]*)(?:(?:=\")(.+)(?:\")|=(.+)|$)");
            
            for (var i = 0; i < args.Length; i++)
            {
                if (r.IsMatch(args[i]))
                {
                    Match m = r.Match(args[i]);

                    switch (m.Groups.Count)
                    {
                        case 4:
                            _arguments[m.Groups[1].Value] = m.Groups[3].Value;
                            break;
                        case 3:
                            _arguments[m.Groups[1].Value] = m.Groups[2].Value;
                            break;
                        case 2:
                            _arguments[m.Groups[1].Value] = "True";
                            break;
                    }
                }
            }
        }

        public static void AddDefaultArgument(string key, string value)
        {
            if (_arguments.ContainsKey(key))
            {
                return;
            }
            
            _arguments.Add(key, value);
        }

        public static void AddDefaultArgument(params (string key, string value)[] defaults)
        {
            for (var i = 0; i < defaults.Length; i++)
            {
                AddDefaultArgument(defaults[i].key, defaults[i].value);
            }
        }

        public static string GetArgument(params string[] keys)
        {
            foreach (var t in keys)
            {
                if (_arguments.ContainsKey(t))
                {
                    return _arguments[t];
                }
            }

            return null;
        }

        public static string GetArgument(string key)
        {
            return _arguments.ContainsKey(key) ? _arguments[key] : null;
        }

        public static T GetArgument<T>(string key, ArgumentParser<T> parser)
        {
            return parser(_arguments[key]);
        }

        public static T GetArgument<T>(string key, ArgumentParser<T> parser, T defaultResult)
        {
            var temp = _arguments[key];
            return string.IsNullOrEmpty(temp) ? defaultResult : parser(temp);
        }
    }
}