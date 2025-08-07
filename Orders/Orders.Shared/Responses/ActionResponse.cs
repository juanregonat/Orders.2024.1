namespace Orders.Shared.Responses
{
    public class ActionResponse<T>
    {
        //si la operacion es exitosa, da un resultado, sino devuelve un mensaje (de error)
        public bool WasSuccess { get; set; }

        public string? Messagge { get; set; }

        public T? Result { get; set; }
    }
}