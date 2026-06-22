namespace Turbohesap.Api.Common;

/// <summary>İstenen kayıt bulunamadığında fırlatılır. Middleware 404'e çevirir.</summary>
public sealed class NotFoundException(string message) : Exception(message);

/// <summary>Çakışma (örn. yinelenen kod) durumunda fırlatılır. Middleware 409'a çevirir.</summary>
public sealed class ConflictException(string message) : Exception(message);

/// <summary>İş kuralı ihlalinde fırlatılır. Middleware 400'e çevirir.</summary>
public sealed class BusinessRuleException(string message) : Exception(message);

/// <summary>Kimlik doğrulama başarısız olduğunda fırlatılır. Middleware 401'e çevirir.</summary>
public sealed class AuthenticationFailedException(string message) : Exception(message);
