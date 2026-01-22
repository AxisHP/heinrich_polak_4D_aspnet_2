namespace Common.DTO
{
    public class CartOperationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static CartOperationResult SuccessResult()
        {
            return new CartOperationResult { Success = true };
        }

        public static CartOperationResult FailureResult(string errorMessage)
        {
            return new CartOperationResult 
            { 
                Success = false, 
                ErrorMessage = errorMessage 
            };
        }
    }
}
