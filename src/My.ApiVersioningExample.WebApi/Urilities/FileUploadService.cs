using Microsoft.AspNetCore.StaticFiles;
using My.ApiVersioningExample.Common.Responses;

namespace My.ApiVersioningExample.WebApi.Urilities
{
	public class FileUploadService
	{
		public static IWebHostEnvironment _env;

		public FileUploadService(IWebHostEnvironment env)
		{
			_env = env;
		}


		public string GetMimeType(string fileName)
		{
			// Make Sure Microsoft.AspNetCore.StaticFiles Nuget Package is installed
			var provider = new FileExtensionContentTypeProvider();
			string contentType;
			if (!provider.TryGetContentType(fileName, out contentType))
			{
				contentType = "application/octet-stream";
			}
			return contentType;
		}

		public async Task<FileResponse?> UploadFileLocalyAndGetUrl(IFormFile? file, string? directoryName)
		{
			FileResponse vwFileResponse = new FileResponse();
			if (file != null)
			{
				directoryName = directoryName ?? "Others";
				string ftpDestination = $"/File_Storage/Uploads/{directoryName.ToLower()}/";

				string fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";

				vwFileResponse.FileName = fileName;
				vwFileResponse.FilePath = $"{ftpDestination}{fileName}";
				vwFileResponse.FileSize = GetFileSizeString(file);
				vwFileResponse.FileType = Path.GetExtension(file.FileName);
				vwFileResponse.FileSizeInByte = file.Length;
				var p = $"{_env?.WebRootPath}/{ftpDestination}";

				if (!Directory.Exists($"{_env?.WebRootPath}/{ftpDestination}"))
				{
					Directory.CreateDirectory($"{_env?.WebRootPath}/{ftpDestination}");

				}

				using (var stream = new FileStream($"{_env?.WebRootPath}/{ftpDestination}/{fileName.Trim()}", FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

			}
			return vwFileResponse;
		}

		public static string GetFileSizeString(IFormFile file)
		{
			if (file is not null)
			{

				long fileSizeInBytes = file.Length;

				// Convert to kilobytes (KB)
				double fileSizeInKB = fileSizeInBytes / 1024.0;

				// Convert to megabytes (MB)
				double fileSizeInMB = fileSizeInKB / 1024.0;

				// Convert to gigabytes (GB)
				double fileSizeInGB = fileSizeInMB / 1024.0;

				if (fileSizeInGB >= 1)
				{
					return $"{fileSizeInGB:F3} GB";
				}
				else if (fileSizeInMB >= 1)
				{
					return $"{fileSizeInMB:F3} MB";
				}
				else
				{
					return $"{fileSizeInKB:F3} KB";
				}
			}
			return null!;
		}
		public void DeleteFile(string fileURL)
		{

			var path = _env?.WebRootPath + fileURL;
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

	}
}
