namespace Hosteland.EmailTemplate {
    public class ConfirmEmailTemplate {

        public string Subject = "Hosteland - Xác nhận email";

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
          <div style="""">
            <h2 style=""
                       margin-top: 3%;
            max-width: fit-content;
  margin-left: auto;
  margin-right: auto;
            color: #999b6d;
            font-size: 32px;
            margin-bottom: 20px;"">Chào mừng đến với Hosteland</h2>
                <div style=""color: black;
  max-width: fit-content;
  margin-left: auto;
  margin-right: auto;"">
                    Chỉ còn một xíu nữa thôi là bạn đã hoàn thành rồi 🎉
                </div>
                <div style=""margin-top: 2%; color: black;
                             max-width: fit-content;
  margin-left: auto;
  margin-right: auto;
                            "">
                    Vui lòng xác nhận thông tin bằng cách nhấn vào nút bên dưới 
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
            border-radius: 50px;"">Xác nhận Email ✉</a>
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
