namespace Web_API.dto
{
    public class ResponseDto<T>
    {
        public int statusCode {  get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
