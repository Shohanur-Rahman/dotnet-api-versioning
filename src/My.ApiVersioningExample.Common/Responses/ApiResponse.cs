namespace My.ApiVersioningExample.Common.Responses
{
	/// <summary>
	/// Represents a standardized API response wrapper that includes a success flag, message, and result data.
	/// </summary>
	/// <typeparam name="T">The type of data being returned in the response.</typeparam>
	public class ApiResponse<T>
	{
		/// <summary>
		/// Indicates whether the API operation was successful.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// A message providing additional context, such as an error description or a success note.
		/// </summary>
		public string? Message { get; set; }

		/// <summary>
		/// The result data returned by the API, if applicable.
		/// </summary>
		public T? Result { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
		/// </summary>
		public ApiResponse() { }

		/// <summary>
		/// Initializes a successful API response with result data and an optional message.
		/// </summary>
		/// <param name="data">The result data to return.</param>
		/// <param name="message">An optional message describing the result.</param>
		public ApiResponse(T data, string? message = null)
		{
			Success = true;
			Result = data;
			Message = message;
		}

		/// <summary>
		/// Initializes a failed API response with an error message.
		/// </summary>
		/// <param name="message">A message describing the error or failure.</param>
		public ApiResponse(string message)
		{
			Success = false;
			Message = message;
		}

		/// <summary>
		/// Creates a successful API response with the specified data and optional message.
		/// </summary>
		/// <param name="data">The result data to return.</param>
		/// <param name="message">An optional message describing the result.</param>
		/// <returns>An <see cref="ApiResponse{T}"/> indicating success.</returns>
		public static ApiResponse<T> Ok(T data, string? message = null) =>
			new ApiResponse<T>(data, message);

		/// <summary>
		/// Creates a failed API response with the specified error message.
		/// </summary>
		/// <param name="message">A message describing the failure.</param>
		/// <returns>An <see cref="ApiResponse{T}"/> indicating failure.</returns>
		public static ApiResponse<T> Fail(string message) =>
			new ApiResponse<T>(message);
	}

}
