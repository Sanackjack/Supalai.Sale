using System;
using System.Collections.Generic;
using System.Net;

namespace ClassifiedAds.CrossCuttingConcerns.Constants
{
    public class ResponseData
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public int HttpStatus { get; private set; }

        ResponseData(string code, string message, int httpStatus) => (Code, Message, HttpStatus) = (code, message, httpStatus);

        public static IEnumerable<ResponseData> Values
        {
            get
            {
                yield return SUCCESS;
                yield return DATA_CREATE;
                yield return DATA_REMOVE;
                yield return DATA_UPDATE;
                yield return AUTHENTICATION_FAIL;
                yield return INCORRECT_USERNAME_PASSWORD;
                yield return USER_LOCK;
                yield return USER_NOT_ACTIVE;
                yield return SIGNATURE_NOT_MATCH;
                yield return TOKEN_GENERATE_FAIL;
                yield return TOKEN_EXPIRED;
                yield return TOKEN_INVALID;
                yield return TOKEN_IS_NULL;
                yield return TOKEN_ERROR;
                yield return TOKEN_NOT_AUTHORIZE;
                yield return REFRESH_TOKEN_GENERATE_FAIL;
                yield return REFRESH_TOKEN_EXPIRED;
                yield return REFRESH_TOKEN_INVALID;
                yield return REFRESH_TOKEN_IS_NULL;
                yield return REFRESH_TOKEN_ERROR;
                yield return DATA_NOT_FOUND;
                yield return DATA_DUPLICATE;
                yield return DATA_SUSPENDED;
                yield return DATA_NOT_ACTIVE;
                yield return DATABASE_LOCK;
                yield return DATABASE_ERROR;
                yield return BAD_REQUEST_CONNECTION;
                yield return CONNECTION_ERROR;
                yield return CONNECTION_TIMED_OUT;
                yield return CONNECTION_WAF_LIMIT;
                yield return CONNECTION_RATE_LIMIT;
                yield return CONNECTION_WAS_BANNED;
                yield return CONNECTION_TOO_MANY_REQUESTS;
                yield return VALIDATION_REQUEST_PARAMETER_FAIL;
                yield return VALIDATION_REQUEST_HEADER_FAIL;
                yield return VALIDATION_REQUEST_UNSUPPORTED_MEDIA_TYPE;
                yield return VALIDATION_REQUEST_METHOD_FAIL;
                yield return VALIDATION_LOGIC_FAIL;
                yield return VALIDATION_BUSINESS_FAIL;
                yield return THIRD_PARTY_MAINTAIN;
                yield return THIRD_PARTY_SERVICE_UNAVAILABLE;
                yield return THIRD_PARTY_BAD_REQUEST;
                yield return THIRD_PARTY_BUSINESS_ERROR;
                yield return THIRD_PARTY_TRANSACTION_ERROR;
                yield return THIRD_PARTY_AUTHENTICATE_ERROR;
                yield return THIRD_PARTY_DATA_NOT_FOUND;
                yield return THIRD_PARTY_SYSTEM_ERROR;
                yield return THIRD_PARTY_CONNECT_TIMEOUT;
                yield return THIRD_PARTY_CONNECT_ERROR;
                yield return THIRD_PARTY_UNKNOWN_ERROR_CODE;
                yield return SERVICE_MAINTAIN;
                yield return SERVICE_UNAVAILABLE;
                yield return SYSTEM_ERROR;
                yield return INTERNAL_SERVER_ERROR;
                yield return UNKNOWN_ERROR;
            }
        }

        // 0 Case : Success
        public static readonly ResponseData SUCCESS = new ResponseData("0", "Success", (int)HttpStatusCode.OK);
        public static readonly ResponseData DATA_CREATE = new ResponseData("0", "Success", (int)HttpStatusCode.Created);
        public static readonly ResponseData DATA_REMOVE = new ResponseData("0", "Success", (int)HttpStatusCode.OK);
        public static readonly ResponseData DATA_UPDATE = new ResponseData("0", "Success", (int)HttpStatusCode.OK);

        // 1xxx Case : Authenticate logic error
        public static readonly ResponseData AUTHENTICATION_FAIL = new ResponseData("1001", "Authentication fail.", (int)HttpStatusCode.Unauthorized);
        public static readonly ResponseData INCORRECT_USERNAME_PASSWORD = new ResponseData("1002", "Username or Password is incorrect.", (int)HttpStatusCode.Unauthorized);
        public static readonly ResponseData USER_LOCK = new ResponseData("1003", "Username is lock.", (int)HttpStatusCode.Unauthorized);
        public static readonly ResponseData USER_NOT_ACTIVE = new ResponseData("1004", "Username is not active.", (int)HttpStatusCode.Unauthorized);
        public static readonly ResponseData SIGNATURE_NOT_MATCH = new ResponseData("1005", "Signature does not match or invalid.", (int)HttpStatusCode.Unauthorized);

        // 2xxx Case : Token logic error
        public static readonly ResponseData TOKEN_GENERATE_FAIL = new ResponseData("2001", "Token generate fail.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData TOKEN_EXPIRED = new ResponseData("2002", "Token is Expired.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData TOKEN_INVALID = new ResponseData("2003", "Token is invalid.", (int)HttpStatusCode.Unauthorized);
        public static readonly ResponseData TOKEN_IS_NULL = new ResponseData("2004", "Token is null.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData TOKEN_ERROR = new ResponseData("2005", "Token error or exception.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData TOKEN_NOT_AUTHORIZE = new ResponseData("2006", "Not authorize for is Token.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData REFRESH_TOKEN_GENERATE_FAIL = new ResponseData("2007", "Refresh token generate fail.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData REFRESH_TOKEN_EXPIRED = new ResponseData("2008", "Refresh token is Expired.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData REFRESH_TOKEN_INVALID = new ResponseData("2009", "Refresh token is invalid.", (int)HttpStatusCode.Unauthorized);
        public static readonly ResponseData REFRESH_TOKEN_IS_NULL = new ResponseData("2010", "Refresh token is null.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData REFRESH_TOKEN_ERROR = new ResponseData("2011", "Refresh token error or exception.", (int)HttpStatusCode.InternalServerError);

        // 3xxx Case : Data or Database error
        public static readonly ResponseData DATA_NOT_FOUND = new ResponseData("3001", "Data not found.", (int)HttpStatusCode.NoContent);
        public static readonly ResponseData DATA_DUPLICATE = new ResponseData("3002", "Data duplicate.", (int)HttpStatusCode.Conflict);
        public static readonly ResponseData DATA_SUSPENDED = new ResponseData("3003", "Data was block or suspend.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData DATA_NOT_ACTIVE = new ResponseData("3004", "Data is not active.", (int)HttpStatusCode.NoContent);
        public static readonly ResponseData DATABASE_LOCK = new ResponseData("3998", "Database is lock.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData DATABASE_ERROR = new ResponseData("3999", "Database was error or exception.", (int)HttpStatusCode.InternalServerError);

        // 4xxx Case : Client connection error
        public static readonly ResponseData BAD_REQUEST_CONNECTION = new ResponseData("4001", "Client request is invalid.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData CONNECTION_ERROR = new ResponseData("4002", "Client connection is error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData CONNECTION_TIMED_OUT = new ResponseData("4003", "Client connection timeout.", (int)HttpStatusCode.RequestTimeout);
        public static readonly ResponseData CONNECTION_WAF_LIMIT = new ResponseData("4004", "Client connection WAF limit.", (int)HttpStatusCode.TooManyRequests);
        public static readonly ResponseData CONNECTION_RATE_LIMIT = new ResponseData("4005", "Client connection rate limit.", (int)HttpStatusCode.TooManyRequests);
        public static readonly ResponseData CONNECTION_WAS_BANNED = new ResponseData("4006", "Client connection was banned.", (int)HttpStatusCode.Forbidden);
        public static readonly ResponseData CONNECTION_TOO_MANY_REQUESTS = new ResponseData("4007", "Client connection too many request.", (int)HttpStatusCode.TooManyRequests);

        // 5xxx Case : Validate client request error
        public static readonly ResponseData VALIDATION_REQUEST_PARAMETER_FAIL = new ResponseData("5001", "Validate request parameter fail.", (int)HttpStatusCode.BadRequest);
        public static readonly ResponseData VALIDATION_REQUEST_HEADER_FAIL = new ResponseData("5002", "Validate request header fail.", (int)HttpStatusCode.BadRequest);
        public static readonly ResponseData VALIDATION_REQUEST_UNSUPPORTED_MEDIA_TYPE = new ResponseData("5003", "Validate request type fail.", (int)HttpStatusCode.UnsupportedMediaType);
        public static readonly ResponseData VALIDATION_REQUEST_METHOD_FAIL = new ResponseData("5004", "Method Failure.", (int)HttpStatusCode.MethodNotAllowed);
        public static readonly ResponseData VALIDATION_LOGIC_FAIL = new ResponseData("5005", "Validate logic fail. ", (int)HttpStatusCode.BadRequest);
        public static readonly ResponseData VALIDATION_BUSINESS_FAIL = new ResponseData("5006", "Validate business fail. ", (int)HttpStatusCode.BadRequest);

        // 6xxx Case : Third-party or external connection error
        public static readonly ResponseData THIRD_PARTY_MAINTAIN = new ResponseData("6000", "Third party service on maintain.", (int)HttpStatusCode.ServiceUnavailable);
        public static readonly ResponseData THIRD_PARTY_SERVICE_UNAVAILABLE = new ResponseData("6001", "Third party service not available.", (int)HttpStatusCode.ServiceUnavailable);
        public static readonly ResponseData THIRD_PARTY_BAD_REQUEST = new ResponseData("6002", "Third party bad request.", (int)HttpStatusCode.BadRequest);
        public static readonly ResponseData THIRD_PARTY_BUSINESS_ERROR = new ResponseData("6003", "Third party business error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData THIRD_PARTY_TRANSACTION_ERROR = new ResponseData("6004", "Third party transaction error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData THIRD_PARTY_AUTHENTICATE_ERROR = new ResponseData("6005", "Third party authentication error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData THIRD_PARTY_DATA_NOT_FOUND = new ResponseData("6006", "Third party data not found.", (int)HttpStatusCode.NoContent);
        public static readonly ResponseData THIRD_PARTY_SYSTEM_ERROR = new ResponseData("6996", "Third party system error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData THIRD_PARTY_CONNECT_TIMEOUT = new ResponseData("6997", "Third party connection timeout error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData THIRD_PARTY_CONNECT_ERROR = new ResponseData("6998", "Third party connection connection error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData THIRD_PARTY_UNKNOWN_ERROR_CODE = new ResponseData("6999", "Third party connection unknown error.", (int)HttpStatusCode.InternalServerError);

        // 9xxx Case : System error
        public static readonly ResponseData SERVICE_MAINTAIN = new ResponseData("9000", "Service on maintain.", (int)HttpStatusCode.ServiceUnavailable);
        public static readonly ResponseData SERVICE_UNAVAILABLE = new ResponseData("9001", "Service not available.", (int)HttpStatusCode.ServiceUnavailable);
        public static readonly ResponseData SYSTEM_ERROR = new ResponseData("9997", "System error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData INTERNAL_SERVER_ERROR = new ResponseData("9998", "Internal Server error.", (int)HttpStatusCode.InternalServerError);
        public static readonly ResponseData UNKNOWN_ERROR = new ResponseData("9999", "Unknown error.", (int)HttpStatusCode.InternalServerError);
    }
}
