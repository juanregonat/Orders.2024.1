using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    [Authorize(Roles = "Admin")]
    public partial class CountryEdit
    {
        private Country? country;
        private FormWithName<Country>? countryForm; //es la representación del codigo razor en el codigo c#

        [Inject] private IRepository Repository { get; set; } = null!;
        //alertas personalizadas
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        // manejo de la navegación: ir de una pagina a otra
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Country>($"/api/countries/{Id}");
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
            else 
            {
                country = responseHttp.Response;
            }
        }
        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();// si todo fue exitoso, se va a la pagina countries

            //aca agrega una tostada que dice "pais agregado"
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados");
        }

        private void Return()
        {
            countryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/countries");
        }
    }
}