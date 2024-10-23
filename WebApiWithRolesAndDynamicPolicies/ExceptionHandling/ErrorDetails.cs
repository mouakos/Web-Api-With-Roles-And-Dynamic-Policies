using System.Text.Json;

namespace WebApiWithRoles.ExceptionHandling
{
    internal class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}