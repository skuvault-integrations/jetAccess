using System.Linq;
using JetAccess.Models;
using LINQtoCSV;

namespace JetAccessTests.TestEnvironment
{
	public class TestDataReader
	{
		private readonly string _appKeysFilePath;
		private FlatCsvLineAppKeys _flatCsvLineAppKeys;

		public JetUserCredentials GetJetUserCredentials => new JetUserCredentials( this._flatCsvLineAppKeys.ApiUser, this._flatCsvLineAppKeys.Secret );

		public TestDataReader( string appKeysFilePath )
		{
			this._appKeysFilePath = appKeysFilePath;
			this.ReadData();
		}

		public void ReadData()
		{
			var cc = new CsvContext();
			this._flatCsvLineAppKeys = cc.Read< FlatCsvLineAppKeys >( this._appKeysFilePath, new CsvFileDescription { FirstLineHasColumnNames = true } ).FirstOrDefault();
		}
	}
}