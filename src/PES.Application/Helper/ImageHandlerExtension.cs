using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public static class ImageHandlerExtension
{
    private static readonly Cloudinary _cloudinary;

    static ImageHandlerExtension()
    {
        DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
        _cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
        _cloudinary.Api.Secure = true;
    }

    public static async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        using var stream = imageFile.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(imageFile.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl?.ToString();
    }


    public static async Task<bool> RemoveImageAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
        return deletionResult.Result == "ok";
    }
}
