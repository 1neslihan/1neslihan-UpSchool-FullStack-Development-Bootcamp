namespace UpSchool.Wasm.Common.Utilities
{
    public static class CssUtilities
    {
        public static string GetCssColorClassForPasswords(int length)
        {
            switch (length)
            {
                case var value when value >= 6 && value <= 12:
                    return "bg-danger";


                case var value when value > 12 && value <= 19:
                    return "bg-warning";


                case var value when value > 19 && value<=40:
                    return "bg-success";
                default:
                    throw new Exception(message: "Beklenmedik bir şifre uzunluğu geldi");
                    break;
            }
        }
    }
}
