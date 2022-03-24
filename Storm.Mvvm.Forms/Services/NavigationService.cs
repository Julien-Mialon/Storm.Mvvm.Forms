using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
    public class NavigationService : INavigationService
    {
        public async Task PushAsync(Page page, Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true)
        {
            if (page.BindingContext is ViewModelBase vm)
            {
                vm.Initialize(parameters ?? new Dictionary<string, object>());
            }

            NavigationPage navigationPage = DependencyService.Get<NavigationPage>();
            if (mode == NavigationMode.Replace)
            {
                Page currentPage = navigationPage.CurrentPage;
                
                await navigationPage.Navigation.PushAsync(page, animated);
                navigationPage.Navigation.RemovePage(currentPage);
            }
            else if (mode == NavigationMode.ReplaceAll)
            {
                await navigationPage.Navigation.PopToRootAsync(false);
                Page currentPage = navigationPage.CurrentPage;
                
                await navigationPage.Navigation.PushAsync(page, animated);
                navigationPage.Navigation.RemovePage(currentPage);
            }
            else if (mode == NavigationMode.Push)
            {
                await navigationPage.Navigation.PushAsync(page, animated);
            }
            else
            {
                await navigationPage.Navigation.PushModalAsync(page, animated);
            }
        }

        public Task PushAsync<TPage>(Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true) where TPage : Page, new()
        {
            return PushAsync(new TPage(), parameters, mode, animated);
        }

        public async Task PopAsync(bool animated = true)
        {
            NavigationPage navigationPage = DependencyService.Get<NavigationPage>();
            if (navigationPage.Navigation.ModalStack.Count > 0)
            {
                await navigationPage.Navigation.PopModalAsync(animated);
            }
            else
            {
                await navigationPage.Navigation.PopAsync(animated);
            }
        }
    }
}