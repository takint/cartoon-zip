﻿using NewCartoonInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newCartoonImplementation
{
    public class LogService<T> : ILogService<T> where T:new()
    {
        public T GetObjectFromFile(string url)
        {
            T result;
            try
            {
                using (Stream stream = File.Open(url, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    result = ((T)bformatter.Deserialize(stream));
                }
            }
            catch(Exception e)
            {
                result = new T();
            }
            return result;
        }

        public void SaveObjectToFile(string url, T objToSave)
        {
            using (Stream stream = File.Open(url, FileMode.OpenOrCreate))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, objToSave);
            }
        }

        public async void WriteCurrentLog(string content)
        {
            string logFileName = $"{DateTime.Now.ToString(@"dd-MM-yyyy_HH-mm")}.txt";
            if (!File.Exists(logFileName))
                File.Create(logFileName);
            string currentContent = "";
            using (var reader = new StreamReader(logFileName))
            {
                currentContent = await reader.ReadToEndAsync();
            }
            currentContent = $"{currentContent} \r\n{content}";
            using (var writer = new StreamWriter(logFileName))
            {
                await writer.WriteAsync(content);
            }
        }
    }
}
