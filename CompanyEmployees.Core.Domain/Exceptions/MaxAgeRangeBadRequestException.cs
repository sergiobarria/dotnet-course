namespace CompanyEmployees.Core.Domain.Exceptions;

public class MaxAgeRangeBadRequestException() : BadRequestException("Max age can't be less than min age.");