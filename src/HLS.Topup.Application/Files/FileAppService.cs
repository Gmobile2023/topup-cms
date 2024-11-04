using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.Authorization.Users.Profile.Dto;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dto;
using HLS.Topup.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Files
{
    [AbpAuthorize]
    public class FileAppService : TopupAppServiceBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ILogger<FileAppService> _logger;

        public FileAppService(IWebHostEnvironment hostingEnvironment, ITempFileCacheManager tempFileCacheManager,
            ILogger<FileAppService> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _tempFileCacheManager = tempFileCacheManager;
            _logger = logger;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
        }

        /// <summary>
        /// Upload file:
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<List<string>> UploadFiles(IEnumerable<IFormFile> files)
        {
            var urls = new List<string>();
            var i = 0;
            foreach (var file in files)
            {
                i++;
                var extension = Path.GetExtension(file.FileName)?.ToLower();
                ValidateFile(file);
                var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + i + extension;
                if (file.Length <= 0) continue;
                var pathReturn = UploadFileToServer(file, fileName);
                if (pathReturn == null)
                    throw new UserFriendlyException("Upload file không thành công");
                urls.Add(pathReturn);
            }

            return urls;
        }

        /// <summary>
        /// Upload file:
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null)
                    throw new UserFriendlyException("File not found");
                _logger.LogInformation($"UploadFile request:{file.FileName}-{file.ContentType}");
                if (file.Length <= 0)
                    throw new UserFriendlyException("File not found");
                var extension = Path.GetExtension(file.FileName)?.ToLower();
                ValidateFile(file);
                var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + extension;
                var pathReturn = UploadFileToServer(file, fileName);
                if (pathReturn == null)
                    throw new UserFriendlyException("Upload file không thành công");
                return pathReturn;
            }
            catch (Exception e)
            {
                _logger.LogError($"UploadFile error:{e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file.Length <= 0)
                throw new UserFriendlyException("File not found");
            _logger.LogInformation($"UploadImage request: {file.FileName}");
            var extension = Path.GetExtension(file.FileName)?.ToLower();
            //ValidateFile(file);
            var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + extension;
            var pathReturn = UploadFileToServer(file, fileName);
            if (pathReturn == null)
                throw new UserFriendlyException("Upload file không thành công");
            return pathReturn;
        }

        public async Task<UploadProfilePictureOutput> UploadProfileAvatar(IFormFile file)
        {
            try
            {
                _logger.LogInformation($"UploadProfileAvatar request:{file.FileName}");
                if (file.Length <= 0)
                    throw new UserFriendlyException("File not found");
                var extension = Path.GetExtension(file.FileName)?.ToLower();
                //ValidateFile(file);
                var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + extension;
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                // if (!GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                // {
                //     throw new Exception(L("IncorrectImageFormat"));
                // }
                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, fileBytes);
                _logger.LogInformation($"UploadProfileAvatar success:{file.FileName}");

                // using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                // {
                // }
                return new UploadProfilePictureOutput
                {
                    FileToken = fileToken,
                    FileName = fileName,
                    FileType = extension,
                    //Width = bmpImage.Width,
                    //Height = bmpImage.Height
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Upload avatar error:{e}");
                throw new UserFriendlyException("Upload ảnh không thành công");
            }
        }

        public async Task<UploadFileResult> DetectImage(IFormFile file)
        {
            try
            {
                if (file.Length <= 0)
                    throw new UserFriendlyException("File not found");
                _logger.LogInformation($"DetectImage request:{file.FileName}");
                var saveFilePath = await UploadImage(file);
                _logger.LogInformation($"UploadImage sucess:{file.FileName}");
                if (saveFilePath == null) throw new UserFriendlyException("Tải ảnh không thành công");

                var rs = new UploadFileResult
                {
                    FilePath = saveFilePath
                };
                try
                {
                    byte[] fileBytes;
                    using (var stream = file.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }

                    var img = Convert.ToBase64String(fileBytes);
                    var json = (new { image = img }).ToJson();
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var host = _appConfiguration["sol:host"];
                    var apiKey = _appConfiguration["sol:apiKey"];
                    var recognitionUrl = _appConfiguration["sol:recognition"];
                    var client = new HttpClient
                    {
                        BaseAddress = new Uri(host),
                    };
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("api-key", apiKey);
                    _logger.LogInformation($"DetectImage process:{file.FileName}");
                    var response = await client.PostAsync(recognitionUrl, data);
                    _logger.LogInformation($"DetectImage return:{response.ToJson()}");
                    if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        rs.DetectImageInfo = result.FromJson<DetectImageInfo>();
                        rs.DetectImageInfo.DocumentType = rs.DetectImageInfo.Document == "NEW ID"
                            ? CommonConst.IdType.CitizenIdentity
                            : rs.DetectImageInfo.Document == "OLD ID"
                                ? CommonConst.IdType.IdentityCard
                                : rs.DetectImageInfo.Document == "PASSPORT"
                                    ? CommonConst.IdType.Passport
                                    : CommonConst.IdType.IdentityCard;

                        _logger.LogInformation($"DetectImage return:{rs.ToJson()}");
                        if (string.IsNullOrEmpty(rs.DetectImageInfo.ResultMessage) ||
                            !string.Equals(rs.DetectImageInfo.ResultMessage, "Nhận dạng thành công",
                                StringComparison.CurrentCultureIgnoreCase))
                        {
                            throw new UserFriendlyException(
                                "Không nhận diện được ảnh. Vui lòng chụp ảnh rõ nét và không lẫn các hình ảnh khác");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Detect img error:{e}");
                    throw new UserFriendlyException(
                        "Không nhận diện được ảnh. Vui lòng chụp ảnh rõ nét và không lẫn các hình ảnh khác");
                }

                return rs;
            }
            catch (Exception e)
            {
                _logger.LogError($"Detect img error:{e}");
                throw new UserFriendlyException(
                    "Không nhận diện được ảnh. Vui lòng chụp ảnh rõ nét và không lẫn các hình ảnh khác");
            }
        }


        private bool ValidateFile(IFormFile file)
        {
            var extensionTypes = _appConfiguration["App:FileExtentions"].Split(',').ToList();
            var extension = Path.GetExtension(file.FileName)?.ToLower();

            if (!extensionTypes.Contains(extension))
            {
                throw new UserFriendlyException("File không hợp lệ");
            }

            var knownTypes = _appConfiguration["App:FileKnownTypes"].Split(',').ToList();
            if (file.Length <= 0) throw new UserFriendlyException("File không hợp lệ");
            if (!knownTypes.Contains(file.ContentType.ToLower()))
            {
                throw new UserFriendlyException("File không đúng định dạng");
            }

            if (file.Length >= 20000000)
            {
                throw new UserFriendlyException("File quá lớn");
            }

            return true;
        }

        private static ImageFormat GetRawImageFormat(byte[] fileBytes)
        {
            using (var ms = new MemoryStream(fileBytes))
            {
                var fileImage = Image.FromStream(ms);
                return fileImage.RawFormat;
            }
        }
        private string UploadFileLocal(IFormFile file, string fileName)
        {
            var folder = DateTime.Now.ToString("ddMMyyyy");
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/" + folder);
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);
            var extension = Path.GetExtension(file.FileName)?.ToLower();
            ValidateFile(file);
            //var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + extension;
            var path = $"/Uploads/{folder}/" + fileName;
            if (file.Length <= 0) return path;
            var filePath = Path.Combine(uploads, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return path;
        }
        private string UploadFileToServer(IFormFile file, string fileName)
        {
            try
            {
                _logger.LogInformation($"UploadFileToServer request: {file.FileName}-{fileName}");
                //Check tao thư muc
                var folder =  DateTime.Now.ToString("ddMMyyyy");
                var serverPath = $"/Uploads/{folder}/";
                var createFolder = CreateFtpDirectory(_appConfiguration["FtpServer:Url"] + serverPath);
                if (!createFolder) return null;
                var fileUrl = serverPath + "/" + fileName;
                var pathServer = _appConfiguration["FtpServer:Url"] + fileUrl;
                var request = (FtpWebRequest)WebRequest.Create(pathServer);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_appConfiguration["FtpServer:Username"],
                    _appConfiguration["FtpServer:Password"]);
                request.UsePassive = true;
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                request.ContentLength = fileBytes.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(fileBytes, 0, fileBytes.Length);
                requestStream.Close();
                var response = (FtpWebResponse)request.GetResponse();
                _logger.LogInformation("Upload File To FTP Complete, status {0}", response.StatusDescription);
                Console.WriteLine("Upload File To FTP Complete, status {0}", response.StatusDescription);
                response.Close();
                return _appConfiguration["FtpServer:UrlViewFile"] + fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError("Upload file to FTP servie error: " + ex);
                Console.WriteLine("Upload file to FTP servie error: {0}", ex);
                return null;
            }
        }

        private bool CreateFtpDirectory(string directory)
        {
            try
            {
                //create the directory
                _logger.LogInformation($"CreateFtpDirectory request: {directory}");
                var requestDir = (FtpWebRequest)WebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(_appConfiguration["FtpServer:Username"],
                    _appConfiguration["FtpServer:Password"]);
                requestDir.UsePassive = true;
                // requestDir.UseBinary = true;
                // requestDir.KeepAlive = false;
                var checkExits = DoesFtpDirectoryExist(directory);
                if (checkExits) return true;
                var response = (FtpWebResponse)requestDir.GetResponse();
                var ftpStream = response.GetResponseStream();
                ftpStream?.Close();
                response.Close();
                _logger.LogInformation($"CreateFtpDirectory return: {response.StatusDescription}-{directory}");
                return true;
            }
            catch (WebException ex)
            {
                _logger.LogInformation($"CreateFtpDirectory {directory} is exsit");
                var response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }

                response.Close();
                return false;
            }
        }

        private bool DoesFtpDirectoryExist(string directory)
        {
            try
            {
                _logger.LogInformation($"Process DoesFtpDirectoryExist request: {directory}");
                var requestDir = (FtpWebRequest)WebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.ListDirectory;
                requestDir.Credentials = new NetworkCredential(_appConfiguration["FtpServer:Username"],
                    _appConfiguration["FtpServer:Password"]);
                requestDir.UsePassive = true;
                _logger.LogInformation($"Process DoesFtpDirectoryExist begin: {directory}");
                var response = (FtpWebResponse)requestDir.GetResponse();
                var ftpStream = response.GetResponseStream();
                ftpStream?.Close();
                response.Close();
                _logger.LogInformation($"DoesFtpDirectoryExist return: {response.StatusDescription}-{directory}");
                return true;
            }
            catch (WebException ex)
            {
                _logger.LogInformation($"DoesFtpDirectoryExist is exist: {directory}");
                return false;
            }
        }
    }
}
