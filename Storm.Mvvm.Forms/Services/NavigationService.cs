using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Storm.Mvvm.Patterns;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class NavigationService : INavigationService
	{
		private readonly Stack<(Page, NavigationMode)> _pages = new Stack<(Page, NavigationMode)>();

		public event EventHandler<PagePushEventArgs> ViewPushed;
		public event EventHandler<PagePopEventArgs> ViewPopped;

		protected Page CurrentPage => LazySingletonInitializer<ICurrentPageService>.Value.CurrentPage;

		public Task PushAsync(Page page, Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true)
		{
			_pages.Push((page, mode));

			if (page.BindingContext is ViewModelBase vm)
			{
				vm.Initialize(parameters ?? new Dictionary<string, object>());
			}

			Task result;
			if (mode == NavigationMode.Push)
			{
				result = CurrentPage.Navigation.PushAsync(page, animated);
				// Push will be handled by the MvvmNavigationPage
			}
			else
			{
				result = CurrentPage.Navigation.PushModalAsync(page, animated);
				// Push will be handled by the MvvmApplication
			}

			return result;
		}

		public Task PushAsync<TPage>(Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true) where TPage : Page, new()
		{
			return PushAsync(new TPage(), parameters, mode, animated);
		}

		public Task<Page> PopAsync(bool animated = true)
		{
			(Page _, NavigationMode mode) = _pages.Pop();
			Task<Page> result;
			if (mode == NavigationMode.Push)
			{
				result = CurrentPage.Navigation.PopAsync(animated);
				// Pop will be handled by the MvvmNavigationPage
			}
			else
			{
				result = CurrentPage.Navigation.PopModalAsync(animated);
				// Pop will be handled by the MvvmApplication
			}
			return result;
		}

		public void OnPush(Page page, NavigationMode mode)
		{
			ViewPushed?.Invoke(this, new PagePushEventArgs(page, mode));
		}

		public void OnPop(Page page, NavigationMode mode)
		{
			ViewPopped?.Invoke(this, new PagePopEventArgs(page, mode));
		}
	}
}
