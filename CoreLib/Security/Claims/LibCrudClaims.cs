namespace Library.Security.Claims;
public static class LibCrudClaims
{
    public const string READ_CLAIM_TYPE = "ReadClaim";
    public const string CREATE_CLAIM_TYPE = "CreateClaim";
    public const string UPDATE_CLAIM_TYPE = "UpdateClaim";
    public const string DELETE_CLAIM_TYPE = "DeleteClaim";

    public static readonly ClaimInfo Read = new(READ_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);
    public static readonly ClaimInfo Create = new(CREATE_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);
    public static readonly ClaimInfo Update = new(UPDATE_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);
    public static readonly ClaimInfo Delete = new(DELETE_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);

    public static IEnumerable<string> GetClaimTypes()
    {
        yield return READ_CLAIM_TYPE;
        yield return CREATE_CLAIM_TYPE;
        yield return UPDATE_CLAIM_TYPE;
        yield return DELETE_CLAIM_TYPE;
    }

    public static IEnumerable<ClaimInfo> GetClaims()
    {
        yield return Read;
        yield return Create;
        yield return Update;
        yield return Delete;
    }
}