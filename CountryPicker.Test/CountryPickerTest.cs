using System;
using System.Linq;
using NUnit.Framework;

namespace CountryPicker.Test
{
	[TestFixture]
	public class CountryPickerTest
	{
		[Test]
		public void InitCountries () 
		{
			var picker = new iOS.CountryPicker ();
			Assert.True (picker.InitCountries ().Count () > 0);
		}
	}
}
