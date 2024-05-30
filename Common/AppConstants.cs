using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Common
{
    public static class ErrorMessages
    {
        public const string Password_ConfirmPassword_Not_Match = "Password_ConfirmPassword_Not_Match";
        public const string OldPassword_Not_Match = "Please check the existing password";
        public const string Error_In_Creation = "Error_In_Creation";
        public const string Error_User_With_Id_Exists = "User with User Id Already Exists";
        public const string Error_With_Id_Exists = "Record with Id Already Exists";
        public const string Error_With_DeptCode_Exists = "Record with department short code already Exists";
        public const string Error_With_DistCode_Exists = "Record with district code already Exists";
        public const string Error_In_Update = "Error_In_Update";
        public const string Error_System_Exception = "System Exception";
        public const string Error_In_Activation = "Could not activate as similar active data already exists";
        public const string Error_Duplicate_Email = "User with this mail id already exists";
        public const string Login_failed = "Login failed";
        public const string User_Not_Found = "User not found";
        public const string Password_No_match = "Please check the password";
        public const string Data_Not_Found = "Data not found";
        public const string Invalid_token = "Invalid token";
        public const string UnAuthenticated = "UnAuthenticated";
        public const string Unauthorized_Acess = "Unauthorized Acess";
        public const string Validation_Failed = "Validation Failed";

        public const string Transaction_Not_Started = "Transaction_Not_Started";

        public const string No_valid_data_found_to_update = "No valid data found to update";
        public const string Record_not_found_to_update = "Record not found to update";
        public const string Record_Already_Exists = "Record already exists";
        public const string Incorrect_CAPTCHA = "Incorrect CAPTCHA!!! Enter valid letters shown in the image";
        public const string Incorrect_creden = "Bad Request";
    }

    public static class AppConstants
    {
        public const string Date_Format = "dd-MM-YYYY";
        public const string Time_Format = "h:mm:ss tt";

        public const string HeadrKeyAccessToken = "cat";
        public const string HeadrKeyRefreshToken = "crt";

        public const string DataFound = "Data collected successfully";
        public const string DataNotFound = "No data found in the record";
        public const string DataAdded = "Data added successfully";
        public const string MissingField = "Missing fields in form body";
        public const string Deleted = "Data deleted Successfully";
        public const string ApproveStatusDone = "Approve Status Updated Successfully";
        public const string ApproveStatusFail = "Approve Status Update Failed";
        public const string RejectStatusDone = "Rejection Status Updated Successfully";
        public const string RejectStatusFail = "Rejection Status Update Failed";
        public const string ReceiveStatusDone = "Receive Status Updated Successfully";
        public const string ReceiveStatusFail = "Receive Status Update Failed";
    }
}
