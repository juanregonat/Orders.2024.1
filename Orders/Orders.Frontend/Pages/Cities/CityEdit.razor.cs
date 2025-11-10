using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Cities
{
    [Authorize(Roles = "Admin")]
    public partial class CityEdit
    {
        private City? city;
        private FormWithName<City>? cityForm; //es la representación del codigo razor en el codigo c#

        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [Parameter] public int CityId { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<City>($"/api/cities/{CityId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    Return();
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            city = responseHttp.Response;

        }


        private async Task SaveAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/cities", city);
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
            cityForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo($"/states/details/{city!.StateId}");
        }
    }
}