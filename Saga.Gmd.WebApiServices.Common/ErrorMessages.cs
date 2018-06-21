namespace Saga.Gmd.WebApiServices.Common
{
    public static class ErrorMessages
    {
        public const string IWilSendInvalidValue = "Parameter 'IWillSend' value has invalid argument.";
        public const string IWillSendCanNotBeNull = "Parameter 'IWillSend' cannot be empty.";
        public const string PostcodeCannotBeEmpty = "Postcode cannot be empty.";
        public const string PostcodeIsInValid = "Postcode is invalid.";
        public const string NameAndAddressCannotBeEmpty = "NameAndAddress cannot be empty.";
        public const string KeyValueCannotBeEmpty = "KeyValue cannot be empty.";
        public const string KeyCannotBeEmpty = "Key cannot be empty.";
        public const string ProvidedKeyIsInvalid = "Provided Key is invalid.";
        public const string SSONValueMustBeGUID = "For SSON value must be GUID.";
        public const string ValueMustBeIntegerType = "Value must be an integer type.";
        public const string ValueCannotBeEmpty = "Value cannot be empty.";
        public const string ReturnMeCannotBeEmpty = "ReturnMe cannot be empty.";
        public const string ReturnMeIsInvalid = "ReturnMe is invalid.";
        public const string CustomerKeysCannotBeNullOrEmpty = "Customer keys cannot be null or empty.";
        public const string CustomerKeyIsInvalid = "Customer Key is Invalid.";
        public const string SurnameCannotBeEmpty = "Surname cannot be empty.";
        public const string SurnameLengthError = "Surname is too long.";
        public const string TitleLengthError = "Title is too long.";
        public const string FirstNameLengthError = "First name is too long.";
        public const string PhoneLengthError = "Phone number is too long.";
        public const string EmailLengthError = "Email is too long.";
        public const string EmailIsInvalid = "Email is invalid.";
        public const string Address1LengthError = "Address line1 is too long.";
        public const string Address2LengthError = "Address line2 is too long.";
        public const string Address3LengthError = "Address line3 is too long.";
        public const string Address4LengthError = "Address line4 is too long.";
        public const string DOBIsInValid = "Date of Birth is invalid.";
        public const string AddressCannotBeEmpty = "Address cannot be empty.";
        public const string ProvideAtleastOneLineOfAddress = "At least one line of address should be provided.";
        public const string ToDateIsInvalid = "To date is invalid.";
        public const string FromDateIsInvalid = "From date is invalid.";
        public const string ToDateIsGreaterThanFromDate = "ToDate must be greater than FromDate.";
        public const string Gender = "Gender has invalid value.";
        public const string SystemSourceCannotBeEmpty = "System Source cannot be empty.";
        public const string TitleCannotBeEmpty = "Title cannot be empty.";
        public const string Forename = "Forename value is too long.";
        public const string Surname = "Surname value is too long.";
        public const string SurNameRequried = "Surname cannot be empty.";
        public const string SystemSource = "SystemSource value is too long.";
        public const string Title = "Title value is too long.";
        public const string DateofBirth = "DateOfBirth value is not valid date.";
        public const string Telephone = "Telephone value is too long.";
        public const string Address = "Address cannot be empty.";
        public const string Address1 = "Address1 value is too long.";
        public const string Address2 = "Address2 value is too long.";
        public const string TownCity = "TownCity value is too long.";
        public const string County = "County value is too long.";
        public const string Postcode = "Postcode value is too long.";
        public const string PostcodeRequried = "Postcode cannot be empty.";
        public const string SystemIdCanntBeEmpty = "SystemId cannot be empty.";
        public const string MarketingSourceCanntobempty = "MarketingSource cannot be empty.";
        public const string TractionTypeCannotbeempty = "Transaction Type cannot be empty.";
        public const string TransactionBrandCannotbempty = "Transaction Brand cannot be empty";
        public const string GenederHasInvalidValue = "Gender has invalid characters.";

        //Permission Details
        public const string PermissionInvalidValue = "'Permssions' cannot be null or empty";
        public const string PermissionDetailsInvalidValue = "'Permssion Details' cannot be empty";

        //membership 
        public const string MemberObjectIsNull = "Member object cannot be null for the Post operation";
        public const string MemberDataInvalid = "Member data is Invalid - ActivationId and MembershipNo cannot both be zero";
        public const string DontHavePermission = "You don't have enough permission to retrive the data for '{0}'";


        //PermissionLoad
        public const string PermissionsId = "Permission Id cannot be empty";
        public const string Source = "Source cannot be empty";
        public const string HAC = "HAC cannot be empty";
        public const string ReConsentRequiredCore = "ReConsentRequiredCore cannot be empty";
        public const string LastUpdatedDate = "LastUpdatedDate cannot be empty";
        public const string LastUpdatedAgentName = "LastUpdatedAgentName cannot be empty";
        public const string QuestionId = "QuestionId cannot be empty";
        public const string Journey = "Journey cannot be empty";
        public const string JourneyType = "Journey Type cannot be empty";
        public const string PermissionCategory = "Please provide atleast one permission category";
        public const string PermissionCategoryStatus = "Please provide permission category status";
        public const string PermissionCategoryDisplayValue = "Please provide permission category display value";
        public const string ValidPostType = "Please provide either key value pair or name and address";
        public const string ValidCustomerKey = "Customer Key cannot be empty";
        public const string ValidCustomerValue = "Customer Key value cannot be empty";
        public const string PendingEnquiryInsertFailed = "Enquiry insert failed.";
        public const string CustomerNotFound = "Customer not found";
        public const string CustomerSummaryPermissionsNotFound = "Summary permissions for the customer not found.";
        public const string CustomerPermissionsNotFound = "Premissions not created for the customer";

        public const string NewSourceId = "NewSourceId cannot be null or empty.";
        public const string NewSourceKey = "NewSourceKey cannot be null or empty.";
        public const string OriginalSourceId = "OriginalSourceId cannot be null or empty.";
        public const string OriginalSourceKey = "OriginalSourceKey cannot be null or empty.";
        public const string ToMoveSourceId = "ToMoveSourceId cannot be null or empty.";
        public const string ToMoveSourceKey = "ToMoveSourceKey cannot be null or empty.";
    }

    public static class ErrorCodeInfo
    {
        public const string RequestOkButResultNotFound = "Request is OK, but prameters didn't fetch any result. Please provide the correct information and try again.";
        public const string NoDataFound = "Requested input didn't fetch any values.";
        public const string BadRequestInfo = "Please correct the information mentioned in the Errors property and try again.";
        public const string UnAuthorizedInfo = "Please provide valid token for the request, please check that correct scope/client id/client secret is used to request a token.";
        public const string InternalServerErrorInfo = "Something wrong in your request or at server , If you need more information on the error, please contact GMD Team with the error message displayed above.";
        public const string ForbiddenInfo = "You don't enough permission to perform this action. If you think you should have it. Please contact the GMD team.";
    }


    public static class ReturnMeTypeConstant
    {
        public const string CustomerKeys = "customerkeys";
        public const string MailingHistory = "mailinghistory";
        public const string Permissions = "permissions";
        public const string Membership = "membership";
        public const string CustomerMatch = "customermatch";
        public const string TravelSummary = "travelsummary";

    }

    public static class DatabaseMessage
    {
        public const string CustomerCreated = "Customer Created Successfully";
        public const string CustomerNotCreated = "Customer creation failed";
        public const string DatabaseException = "Oops something went wrong! Please contact GMD Team quoting id '{0}'";


        //Permissions
        public const string PermissionsCreated = "Permission Created Successfully";
        public const string NotValidPermissionsId = "PermissionsId is not valid";
        public const string PermissionsNotCreated = "Permission Creation Failed";
        public const string MembershipDataSaved = "Membership Data Updated successfully";
        public const string MembershipDataNotUpdated = "Membership Data Update failed";
    }
}