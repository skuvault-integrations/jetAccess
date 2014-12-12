using LINQtoCSV;

namespace JetAccessTestsIntegration.TestEnvironment
{
    internal class FlatCsvLineAppKeys
    {
        public FlatCsvLineAppKeys()
        {
        }
        
        [CsvColumn(Name = "ApiUser", FieldIndex = 1)]
        public string ApiUser { get; set; }

        [CsvColumn(Name = "Secret", FieldIndex = 2)]
        public string Secret { get; set; }
    }
}