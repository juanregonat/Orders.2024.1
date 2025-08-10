using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Frontend.Shared
{
    public partial class FormWithName <TModel> where TModel : IEntityWithName //le decimos que este formulario es del tipo Tmodel que implemente IEntityWithName
    {
        private EditContext editContext = null!; //el edit context es el contexto de edicion.

        //VIDEO 15: formulario generico. Basado en el formulario país, se tranforma en uno genérico

        //acá le pasala entidad a crear o editar. Con data notation se hace obligatorio que me pase un modelo
        [EditorRequired, Parameter] public TModel Model { get; set; } = default!;
        
        //acá le pasala la label del modelo
        [EditorRequired, Parameter] public string Label { get; set; } = null!;

        //cuando el formulario aprueba todas las validaciones, aca se pasa código para hacer algo (crear o editar)
        //cuando se reciben componentes Razor, se pasa renderFragment
        //pero cuando se pasa código, es un EventCallback
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }

        //boton de cancelar edicion:
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

        //errores personalizados. Se injecta el sweetAlert
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;

        //para evitar que los datos cargados se pierdan al ir a otra página (todo este bloque de codigo)
        public bool FormPostedSuccessfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(Model);
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
