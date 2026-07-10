using System;
using System.Net;

namespace ScienceLogic.Em7.Api.Exceptions;

public class ApiException : Exception
{
	public ApiException(HttpStatusCode httpStatusCode, string message) : base(message)
	{
		HttpStatusCode = httpStatusCode;
	}

	public ApiException(HttpStatusCode httpStatusCode, string message, Exception innerException) : base(message, innerException)
	{
		HttpStatusCode = httpStatusCode;
	}

	public HttpStatusCode HttpStatusCode { get; }
}
