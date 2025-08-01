using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Reflection;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountryCreate
    {
        private Country country = new();
        private CountryForm? countryForm; //es la representación del codigo razor en el codigo c#

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        //metodo que crea el pais (solo cuando ya pasó las validaciones)
        private async Task CreateAsync()
        {
            //hago el post y si el backend me devuelve el error, lo levanto
            var responseHtpp = await repository.PostAsync("api/countries", country);

            if (responseHtpp.Error)
            {
                var message = await responseHtpp.GetErrorMessageAsync();
                //le pasamos el error a sweetAlert
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();// si todo fue exitoso, se va a la pagina countries

            //aca agrega una tostada que dice "pais agregado"
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado");
        }

        private void Return()
        {
            countryForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/countries");
        }
    }
}