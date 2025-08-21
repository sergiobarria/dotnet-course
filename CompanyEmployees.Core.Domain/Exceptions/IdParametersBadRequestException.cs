namespace CompanyEmployees.Core.Domain.Exceptions;

public sealed class IdParametersBadRequestException() : BadRequestException("Parameter ids is null");

public sealed class CollectionByIdsBadRequestException()
    : BadRequestException("Collection count mismatch comparing to ids.");