using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public class CsvLoader
    {
        public static IEnumerable<T> Load<T>(string csvFilePath, bool ignoreConvertError = true, bool ignoreIndexOutOfRange = true) where T : class, new()
        {
            if (!File.Exists(csvFilePath))
            {
                throw new FileNotFoundException($"File not found: {csvFilePath}", csvFilePath);
            }

            return File.ReadAllLines(csvFilePath).Skip(1).Select(x => Map<T>(x, ignoreConvertError, ignoreIndexOutOfRange)).ToList();
        }

        public static T Map<T>(string csvLine, bool ignoreConvertError = true, bool ignoreIndexOutOfRange = true) where T : class, new()
        {
            var values = csvLine.Split(',');
            var target = new T();

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.CanWrite)
                {
                    if (property.GetCustomAttributes(typeof(CsvColumnAttribute), false).FirstOrDefault() is CsvColumnAttribute attribute)
                    {
                        try
                        {
                            property.SetValue(target, Convert.ChangeType(values[attribute.Positon], property.PropertyType), null);
                        }
                        catch(IndexOutOfRangeException indexOutOfRangeException)
                        {
                            if (!ignoreIndexOutOfRange)
                            {
                                throw indexOutOfRangeException;
                            }
                        }
                        catch (FormatException formatException)
                        {
                            if (!ignoreConvertError)
                            {
                                throw formatException;
                            }
                        }
                    }
                }
            }

            return target;
        }
    }
}
