using System.Collections.Generic;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class CurrentPageService: ICurrentPageService
	{
		public Page CurrentPage
		{
			get
			{
				NavigationPage navigationPage = DependencyService.Get<NavigationPage>();
				IReadOnlyList<Page> modalStack = navigationPage.Navigation.ModalStack;
				if (modalStack.Count > 0)
				{
					return modalStack[modalStack.Count - 1];
				}

				IReadOnlyList<Page> navStack = navigationPage.Navigation.NavigationStack;
				if (navStack.Count > 0)
				{
					return navStack[navStack.Count - 1];
				}

				return null;
			}
		}
	}
}
