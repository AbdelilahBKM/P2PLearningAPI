using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using P2PLearningAPI.Interfaces;

public class UploadRepository : IUploadInterface
{
    private readonly string _storagePath;

    public UploadRepository()
    {
        // Configure the storage path (you can customize this)
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        // Ensure the directory exists
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        // Define the directory path in wwwroot where files will be stored
        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // Ensure the directory exists
        if (!Directory.Exists(uploadDirectory))
        {
            Directory.CreateDirectory(uploadDirectory);
        }

        // Generate a unique file name
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadDirectory, fileName);

        // Save the file to the directory
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the relative file path for public access
        return Path.Combine("uploads", fileName);  // Relative path for public URL
    }


    public async Task<bool> DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_storagePath, filePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }
}
