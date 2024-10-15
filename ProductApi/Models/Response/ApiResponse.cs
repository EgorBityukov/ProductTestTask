namespace ProductApi.Models.Response
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public ApiResponse(bool isSuccess, T? data, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static ApiResponse<T> Success(T data) => new ApiResponse<T>(true, data);

        public static ApiResponse<T> Fail(string errorMessage) => new ApiResponse<T>(false, default, errorMessage);
    }
}
