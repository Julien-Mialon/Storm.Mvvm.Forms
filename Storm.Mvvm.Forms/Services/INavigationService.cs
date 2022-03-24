using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public interface INavigationService
	{
		Task PushAsync(Page page, Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true);

		Task PushAsync<TPage>(Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true) where TPage : Page, new();

		Task PopAsync(bool animated = true);
	}
}
