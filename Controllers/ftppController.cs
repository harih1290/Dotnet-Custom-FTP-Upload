using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Text;

namespace ftp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ftppController : ControllerBase
{
    private readonly ILogger<ftppController> _logger;
    public ftppController(ILogger<ftppController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public string UploadFile(string FtpUrl, string fileName, string userName, string password,string
    UploadDirectory="")
    {
        try{
        string PureFileName = new FileInfo(fileName).Name;
        String uploadUrl = String.Format("{0}/{1}", FtpUrl,PureFileName);
        FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(uploadUrl);
        req.Proxy = null;
        req.Method = WebRequestMethods.Ftp.UploadFile;
        req.Credentials = new NetworkCredential(userName,password);
        req.UseBinary = true;
        req.UsePassive = true;
        //byte[] data = Enumerable.Repeat((byte)0x20,100).ToArray();
        string author = "Withinloop coolpage biz";

        byte[] data = Encoding.ASCII.GetBytes(author);
        req.ContentLength = data.Length;
        Stream stream =  req.GetRequestStream();
        stream.Write(data, 0, data.Length);
        stream.Close();
        FtpWebResponse res = (FtpWebResponse)req.GetResponse();
        return "okay";
        }catch(Exception ex){
        _logger.LogInformation("FTP upload Error:"+DateTimeOffset.UtcNow+":"+ex.ToString());
         return "something went wrong";
        }
    }
}
