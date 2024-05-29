using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
namespace PES.Application.Helper
{

    public record FireBaseFile
    {
        public string URL { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
    public static class StorageHandler
    {
        private static string API_KEY = "AIzaSyB-cdgxQD1Gwpl_vKSMGCaenYeFPMdFKM4";
        private static string Bucket = "ntassignment-518e1.appspot.com";
        private static string AuthEmail = "mandayngu@gmail.com";
        private static string AuthPassword = "0902388458Tr";
        private static FirebaseStorage _storage;
        public static async Task<FireBaseFile> UploadFileAsync(this IFormFile fileUpload, string folderName)
        {
            if (fileUpload.Length > 0)
            {
                var fs = fileUpload.OpenReadStream();
                var auth = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                var cancellation = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true

                    }
                    ).Child(folderName).Child(fileUpload.FileName)
                    .PutAsync(fs, CancellationToken.None);

                    
                try
                {
                    var result = await cancellation;

                    return new FireBaseFile
                    {
                        FileName = fileUpload.FileName,
                        URL = result
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }

            }
            else throw new Exception("File is not existed!");
        }

        public static async Task<FireBaseFile> UploadFileStream(this Stream fileUpload, string folderName, string fileName)
        {
            if (fileUpload.Length > 0)
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                var cancellation = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true

                    }
                    ).Child(folderName).Child(fileName)
                    .PutAsync(fileUpload, CancellationToken.None);
                try
                {
                    var result = await cancellation;

                    return new FireBaseFile
                    {
                        FileName = fileName,
                        URL = result
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }

            }
            else throw new Exception("File is not existed!");
        }




        public static async Task<bool> RemoveFileAsync(this string fileName, string FolderName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));
            var loginInfo = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var storage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(loginInfo.FirebaseToken),
                ThrowOnCancel = true
            });
            await storage.Child(FolderName).Child(fileName).DeleteAsync();
            return true;

        }
    }
}