namespace LMS.Core.Enums;

public enum ResultStatus
{
    Ok = 200,
    Created = 201,
    BadRequest = 400,
    Error = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    Invalid = 422,
    UnprocessableEntity = 422,
    CriticalError = 500,
    Unavailable = 503
}
