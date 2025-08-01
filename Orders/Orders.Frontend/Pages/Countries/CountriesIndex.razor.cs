using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountriesIndex
    {
        [Inject] private IRepository repository { get; set; } = null!;

        //alertas personalizadas
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        // manejo de la navegación: ir de una pagina a otra
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        public List<Country>? Countries { get; set; }


        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }


        private async Task LoadAsync()
        {
            var responseHttp = await repository.GetAsync<List<Country>>("api/countries");

            //si va al backend y hay un error:
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Countries = responseHttp.Response;            
        }

        private async Task DeleteAsync(Country country)
        {
            //configuracion de alerta personalizada
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
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
            var responseHttp = await repository.DeleteAsync<Country>($"api/countries/{country.Id}");

            //si lo trato de borrar y hay errores:
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/countries");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
                 return;
            }

            await LoadAsync();

            //aca agrega una tostada que dice "pais eliminado"
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
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
