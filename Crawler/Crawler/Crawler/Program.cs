using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using static System.Collections.Specialized.BitVector32;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using Application.Features.Orders.Commands.Add;
using Domain.Enums;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System.Text;
using Application.Features.Products.Commands.Add;

Console.WriteLine("Crawler_Bot");


new DriverManager().SetUpDriver(new ChromeConfig());


using var httpClient = new HttpClient();

IWebDriver driver = new ChromeDriver();


    Console.Clear();
    
    Console.WriteLine("Giriş Yapıldı------------------------------------");
    driver.Navigate().GoToUrl("https://finalproject.dotnet.gg");
    ReadOnlyCollection<IWebElement> pagination = driver.FindElements(By.ClassName("page-item"));

    int a = 0; //ürün sayısı tutucu
    
    //yeni bir order oluşturup apiye istek yap
    var orderAddRequest = new OrderAddCommand()
    {
        Id=Guid.NewGuid(),
        ProductCrawlType=ProductCrawlType.All,
    };

    var orderAddResponse = await SendHttpPostRequest<OrderAddCommand, object>(httpClient, "https://localhost:7090/api/Orders/Add", orderAddRequest);
    Guid orderId = orderAddRequest.Id;
    
    
    

    for (int i=1; i<pagination.Count; i++)
    {
        driver.Navigate().GoToUrl($"https://finalproject.dotnet.gg/?currentPage={i}");
        Console.WriteLine($"{i}.Sayfa");
        Console.WriteLine();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        ReadOnlyCollection<IWebElement> productCard = driver.FindElements(By.CssSelector(".col.mb-5"));
        Console.WriteLine(productCard.Count);
        
        

    if (orderAddResponse != null)
    {
        for (int j = 1; j<=productCard.Count; j++)
        {
            string productName = string.Empty;
            string productImageURL = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/img")).GetAttribute("src");
            bool isDiscounted;


            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[2]"));
            if (elements.Count == 0)
            {
                isDiscounted = false;
               
            }
            else
            {
                isDiscounted = true;
                
            }
            
            decimal? discountedPrice = null;
            decimal originalPrice;
          
            if (isDiscounted)
            {
                productName = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/h5")).Text;
                discountedPrice =decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[2]")).Text.Remove(0, 1));
                originalPrice = decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[1]")).Text.Remove(0, 1));
                a++;
            }
            else
            {
                productName = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[1]/div/h5")).Text;
                originalPrice =decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[1]/div/span")).Text.Remove(0, 1));
                a++;
            }

            //Daha sonra silinecek
            Console.WriteLine("----------------------------");

            if (isDiscounted)
            {
                Console.WriteLine("Ürün Adı: " + productName);
                Console.WriteLine("İndirimli mi?: " + isDiscounted);
                Console.WriteLine("İndirimli Fiyat: " + discountedPrice);
                Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                Console.WriteLine("Ürün Resmi URL'si: " + productImageURL);
                Console.WriteLine("Ürün OrderId'si " + orderAddRequest.Id.ToString());
                var productAddRequest = new ProductAddCommand()
                {
                    
                    OrderId =orderAddRequest.Id,
                    Name = productName,
                    Picture=productImageURL,
                    IsOnSale = true,
                    Price=originalPrice,
                    SalePrice=discountedPrice,   

                };
               
                var productAddResponse =await SendHttpPostRequest<ProductAddCommand, object>(httpClient, "https://localhost:7090/api/Products/Add", productAddRequest);
                Console.WriteLine($"Product order ıd{productAddRequest.OrderId}");

            }
            else
            {
                Console.WriteLine("Ürün Adı: " + productName);
                Console.WriteLine("İndirimli mi?: " + isDiscounted);
                Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                Console.WriteLine("Ürün Resmi URL'si: " + productImageURL);

                var productAddRequest = new ProductAddCommand()
                {
                    // ProductAddCommand nesnesinin diğer özellikleri
                    OrderId =orderAddRequest.Id,
                    Name = productName,
                    IsOnSale = true,
                    Price=originalPrice,
                    SalePrice=discountedPrice,
                    Picture=productImageURL,

                };
                
                var productAddResponse =await SendHttpPostRequest<ProductAddCommand, object>(httpClient, "https://localhost:7090/api/Products/Add", productAddRequest);

            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
            
        
    }
    
    
    Console.WriteLine(a);
    driver.Dispose();


async Task<TResponse> SendHttpPostRequest<TRequest, TResponse>(HttpClient httpClient, string url, TRequest payload)
{
    var jsonPayload = JsonConvert.SerializeObject(payload);
    var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync(url, httpContent);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
    Console.WriteLine($"Response: {responseObject}");

    return responseObject;
}
