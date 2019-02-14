using Core;

namespace Entities
{
    public class Product
    {
        [CsvColumn(0)]
        public string Id { get; set; }
        [CsvColumn(1)]
        public string Name { get; set; }
        [CsvColumn(2)]
        public float Price { get; set; }
        [CsvColumn(3)]
        public float OriginalPrice { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
