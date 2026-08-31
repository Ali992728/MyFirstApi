namespace MyFirstApi.GenericResponse
{
    public class ResponseResult<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool status { get; set; } = false; 

        public static ResponseResult<T> Success(T? data,string Message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = Message,
                status = true
            };
        }

        public static ResponseResult<T> Failure(T? data, string Message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = Message,
                status = false
            };
        }
    }
}
