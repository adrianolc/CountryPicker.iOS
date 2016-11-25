using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using CountryPicker.iOS.Model;
using Foundation;
using UIKit;

namespace CountryPicker.iOS
{
	public class CountryPicker : UIPickerView, IUIPickerViewDelegate, IUIPickerViewDataSource
	{
		public event EventHandler<Country> OnSelectedCountry;

		readonly List<Country> _countries;

		public Country SelectedCountry { get; set; }

		public CountryPicker ()
		{
			_countries = InitCountries ().ToList();

			DataSource = this;
			Delegate = this;
		}

		public IEnumerable<Country> InitCountries ()
		{
			foreach (string code in NSLocale.ISOCountryCodes) {
				string name = NSLocale.CurrentLocale.GetCountryCodeDisplayName (code);
				yield return new Country { Code = code, Name = name };
			}
		}

		public void SetSelectedCountry (string code, bool animated)
		{
			SelectedCountry = _countries.FirstOrDefault (c => c.Code.Equals (code));

			if (SelectedCountry != null) {
				int index = _countries.IndexOf (SelectedCountry);
				Select (index, 0, animated);
			}
		}

		public nint GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public nint GetRowsInComponent (UIPickerView pickerView, nint component)
		{
			return _countries.Count ();
		}

		[Export ("pickerView:viewForRow:forComponent:reusingView:")]
		public UIView GetView (UIPickerView pickerView, nint row, nint component, UIView view)
		{
			if (view == null) {
				view = new UIView (new CGRect (0, 0, 280, 30));

				UILabel label = new UILabel (new CGRect (35, 3, 245, 24));
				label.BackgroundColor = UIColor.Clear;
				label.Tag = 1;

				view.AddSubview (label);

				UIImageView flag = new UIImageView (new CGRect (3, 3, 24, 24));
				flag.ContentMode = UIViewContentMode.ScaleAspectFit;
				flag.Tag = 2;

				view.AddSubview (flag);
			}

			var country = _countries[(int)row];

			((UILabel)view.ViewWithTag (1)).Text = country.Name;
			((UIImageView)view.ViewWithTag (2)).Image = UIImage.FromFile (string.Format ("{0}.png", country.Code));

			return view;
		}

		[Export ("pickerView:didSelectRow:inComponent:")]
		public void Selected (UIPickerView pickerView, nint row, nint component)
		{
			SelectedCountry = _countries[(int)row];

			if (OnSelectedCountry != null)
				OnSelectedCountry (this, SelectedCountry);
		}
	}
}
