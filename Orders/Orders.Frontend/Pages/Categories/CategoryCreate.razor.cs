using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
    [Authorize(Roles = "Admin")]

    public partial class CategoryCreate
    {
        private Category category = new();
        private FormWithName<Category>? categoryForm; //es la representación del codigo razor en el codigo c#

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        //metodo que crea la categoria (solo cuando ya pasó las validaciones)
        private async Task CreateAsync()
        {
            //hago el post y si el backend me devuelve el error, lo levanto
            var responseHtpp = await Repository.PostAsync("api/categories", category);

            if (responseHtpp.Error)
            {
                var message = await responseHtpp.GetErrorMessageAsync();
                //le pasamos el error a sweetAlert
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();// si todo fue exitoso, se va a la pagina countries

            //aca agrega una tostada que dice "categoria agregada"
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
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
            categoryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/categories");
        }
    }
}