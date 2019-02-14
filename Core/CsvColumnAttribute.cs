using System;

namespace Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvColumnAttribute : Attribute
    {
        public int Positon { get; private set; }
        public CsvColumnAttribute(int position)
        {
            Positon = position;
        }
    }
}
