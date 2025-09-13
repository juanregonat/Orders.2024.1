using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountriesIndex
    {
        private int currentPage = 1;

        private int totalPages;

        [Inject] private IRepository Repository { get; set; } = null!;
        //alertas personalizadas
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        // manejo de la navegación: ir de una pagina a otra
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        public List<Country>? Countries { get; set; }


        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task SelectedPageAsync(int page)
        { 
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            var responseHttp = await Repository.GetAsync<List<Country>>($"api/countries?page={page}");

            //si va al backend y hay un error:
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }

            Countries = responseHttp.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            var responseHttp = await Repository.GetAsync<int>("api/countries/totalPages");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
        }

        private async Task DeleteAsync(Country country)
        {
            //configuracion de alerta personalizada
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"Estas seguro de querer borrar el país: {country.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            //si ya confirma que lo quiere borrar, entonces así llamamos al delete del repository
            var responseHttp = await Repository.DeleteAsync<Country>($"api/countries/{country.Id}");

            //si lo trato de borrar y hay errores:
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
                 return;
            }

            await LoadAsync();

            //aca agrega una tostada que dice "pais eliminado"
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Pais borrado con exito");       

        }
    }
}
