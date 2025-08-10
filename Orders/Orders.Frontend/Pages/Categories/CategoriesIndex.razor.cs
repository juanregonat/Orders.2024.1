using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
    public partial class CategoriesIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;

        //alertas personalizadas
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        // manejo de la navegación: ir de una pagina a otra
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        public List<Category>? Categories{ get; set; }


        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }


        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Category>>("api/categories");

            //si va al backend y hay un error:
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Categories = responseHttp.Response;
        }

        private async Task DeleteAsync(Category category)
        {
            //configuracion de alerta personalizada
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"Estas seguro de querer borrar la categoría: {category.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            //si ya confirma que lo quiere borrar, entonces así llamamos al delete del repository
            var responseHttp = await Repository.DeleteAsync<Category>($"api/categories/{category.Id}");

            //si lo trato de borrar y hay errores:
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
                return;
            }

            await LoadAsync();

            //aca agrega una tostada que dice "categoria eliminada"
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Categoría borrada con éxito");

        }
    }
}
