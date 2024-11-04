using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HLS.Topup.Dto;

namespace HLS.Topup.Files
{
    public interface IFileCommonAppService
    {
        FileDto GetFileExcel<T>(List<T> data, string fileSourceName, string sourceName, string outputFileName);
        byte[] CompressFile(FileDto file);
        byte[] CompressFiles(List<FileDto> files);
    }
}
