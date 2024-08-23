namespace dem2.hhh
{
    public class GoogleReCaptchaHelper
    {
        private readonly string _secretKey;

        public GoogleReCaptchaHelper(string secretKey)
        {
            _secretKey = secretKey;
        }

        public async Task<bool> ValidateCaptchaAsync(string token)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={token}", null);
                var result = await response.Content.ReadAsStringAsync();
                return result.Contains("\"success\": true");
            }
        }
    }
}
