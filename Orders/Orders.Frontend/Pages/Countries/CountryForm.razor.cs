using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    // video 8
    public partial class CountryForm //este form es para crear y para editar paises
    {
        private EditContext editContext = null!;
        //el edit context es el contexto de edicion.

        //acá le pasa el pais a crear o editar. Con data notation se hace obligatorio el pase del pais
        [EditorRequired, Parameter] public Country Country { get; set; } = null!;

        //cuando el formulario aprueba todas las validaciones, aca se pasa código para hacer algo (crear o editar)
        //cuando se reciben componentes Razor, se pasa renderFragment
        //pero cuando se pasa código, es un EventCallback
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }

        //boton de cancelar edicion:
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

        //errores personalizados. Se injecta
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;

        //para evitar que los datos cargados se pierdan al ir a otra página (todo este bloque de codigo)
        public bool FormPostedSuccessfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(Country);
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            //el formulario tuvo modificaciones?
            var formWasEdited = editContext.IsModified();

            //el formulario tuvo modificaciones o fue posteado? Salimos
            if (!formWasEdited || FormPostedSuccessfully)
            {
                return;
            }

            //configuracion de alerta personalizada
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "Deseas abandonar la página y perder los cambios?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });
            
            //para lanzar la alerta, debemos confirmar. 
            var confirm = string.IsNullOrEmpty(result.Value);

            //si el usuario acepta perder los cambios, salimos
            if (confirm)
            {
                return;
            }
            //si el usario no quiere perderlos, no puede salir de la pág.
            context.PreventNavigation();
        }
    }
}