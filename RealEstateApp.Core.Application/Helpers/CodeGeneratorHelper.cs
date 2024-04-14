using System.Text;

namespace RealEstateApp.Core.Application.Helpers
{
    public static class CodeGeneratorHelper
    {
        public static int GeneratePropertyCode()
        {
            Random random = new Random();
            StringBuilder propertyCode = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                propertyCode.Append(random.Next(0, 10)); // Genera un dígito aleatorio entre 0 y 9
            }

            return int.Parse(propertyCode.ToString());
        }
    }
}
