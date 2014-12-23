using System.Linq;
using JetAccess.Models;
using LINQtoCSV;

namespace JetAccessTestsIntegration.TestEnvironment
{
	public class TestDataReader
	{
		private readonly string _appKeysFilePath;
		private FlatCsvLineAppKeys _flatCsvLineAppKeys;

		public JetUserCredentials GetJetUserCredentials
		{
			get { return new JetUserCredentials( _flatCsvLineAppKeys.ApiUser, _flatCsvLineAppKeys.Secret ); }
		}

		public TestDataReader( string appKeysFilePath )
		{
			this._appKeysFilePath = appKeysFilePath;
			this.ReadData();
		}

		public void ReadData()
		{
			var cc = new CsvContext();
			this._flatCsvLineAppKeys = Enumerable.FirstOrDefault( cc.Read< FlatCsvLineAppKeys >( this._appKeysFilePath, new CsvFileDescription { FirstLineHasColumnNames = true } ) );
		}
	}
}