namespace Hosteland.EmailTemplate {
    public class ResetPasswordTemplate {

        public string Subject = "Hosteland - Đặt lại mật khẩu";

        public static string Get(string callbackUrl) {
            return @"
<html>
<head>
    <style>
    </style>
</head>
<body>
    <div style=""font-family: Nunito Sans,sans-serif; line-height: 1.5;"">
        <div style=""max-width: 600px;
            margin: 0 auto;
            padding: 0 0 40px 0;
            background-color: #f2f2f2;
            flex-direction: column;
            align-content: center;
            border: #999b6d solid 2px;
            border-radius: 6px;
            font-family: Nunito Sans,sans-serif;
            font-size: 14px;"">
            <img style=""width: 100%;border-radius: 4px 4px 0 0;""
                src=""https://blobcuakhoa.blob.core.windows.net/files/home-page-main-img.jpg?fbclid=IwAR0CGha88aPpre5j-lIhJE4ak_d76nZAnw0-Zineib2hvVIATX3nxedvYbg_aem_AWoelabb9l2Rly_laTCvKKYakHwe8fhwkOS5DgA6uLEdMRejR4megSN5JcBonDUOu9fi1aXRzW0JkeXK3k-KyGOs"">
          <div style="""">
            <h2 style=""
                       margin-top: 3%;
            max-width: fit-content;
  margin-left: auto;
  margin-right: auto;
            color: #999b6d;
            font-size: 32px;
            margin-bottom: 20px;"">Đặt lại mật khẩu Hosteland</h2>
                <div style=""color: black;
  max-width: fit-content;
  margin-left: auto;
  margin-right: auto;"">
                    Bạn vừa gửi yêu cầu đặt lại mật khẩu tài khoản Hosteland.<br>
    Nếu bạn không phải là người thực hiện thao tác này, <br> vui lòng hãy liên hệ với chúng tôi để báo cáo về vấn đề xác thực tài khoản.<br>
    Nếu có bất kì vấn đề hoặc sự cố nào, <br> đừng ngần ngại liên hệ với đội ngũ tư vấn khách hàng của chúng tôi để giải quyết.<br>
    Cám ơn vì đã sử dụng dịch vụ.<br><br>
    Mến chào,<br>
    Hosteland
                </div>
                <div style=""margin-top: 2%; color: black;
                             max-width: fit-content;
  margin-left: auto;
  margin-right: auto;
                            "">
                    Nhấn nút bên dưới để tiến hành đặt lại mật khẩu
                </div>
            <div style=""
  max-width: fit-content;
  margin-left: auto;
  margin-right: auto;
            margin-top: 4%;"">
                <a href=""" + callbackUrl + @""" style=""text-decoration: none;
            display: inline-block;
            background-color: #999b6d;
            color: #fff;
            padding: 10px 20px;
            border-radius: 50px;"">Đặt lại mật khẩu</a>
            </div>
          </div>
          </div>
        </div>
    </div>
</body>

</html>";
        }
    }
}
