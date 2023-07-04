using Application.Common.Models.SignalR;
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.Orders.Commands.Add;
using Application.Features.Products.Commands.Add;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
using System.Text;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


bool Continue=false;

using var httpClient = new HttpClient();

int requestedAmount;


//var hubConnection=new HubConnectionBuilder()
//    .WithUrl($"https://localhost:7090/Hubs/UserLogHub")
//    .WithAutomaticReconnect()
//    .Build();

//await hubConnection.StartAsync();


//new DriverManager().SetUpDriver(new ChromeConfig());

while (!Continue)
{

    Console.WriteLine("Crawler_Bot");
    var orderAddRequest = new OrderAddCommand();
    ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();



    bool validChoice = false;

    while (!validChoice)
    {
        Console.WriteLine("Kaç ürün kazımak istiyorsunuz?");
        requestedAmount=Convert.ToInt32(Console.ReadLine()); 
        Console.WriteLine("Hangi tip ürünleri kazımak istiyorsunuz?");
        Console.WriteLine("1- Hepsi");
        Console.WriteLine("2- İndirimdekiler");
        Console.WriteLine("3- İndirimde olmayanlar");
        keyInfo = Console.ReadKey();
        Console.WriteLine();

        switch (keyInfo.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                orderAddRequest= new OrderAddCommand()
                {
                    Id=Guid.NewGuid(),
                    ProductCrawlType=ProductCrawlType.All,
                };
                validChoice=true;
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                orderAddRequest= new OrderAddCommand()
                {
                    Id=Guid.NewGuid(),
                    ProductCrawlType=ProductCrawlType.OnDiscount,
                };
                validChoice=true;
                break;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                orderAddRequest= new OrderAddCommand()
                {
                    Id=Guid.NewGuid(),
                    ProductCrawlType=ProductCrawlType.NonDiscount,
                };
                validChoice=true;
                break;
            default:
                Console.WriteLine("Geçersiz seçenek");
                Thread.Sleep(1500);
                Console.Clear();
                break;


        }





        var orderAddResponse = await SendHttpPostRequest<OrderAddCommand, object>(httpClient, "https://localhost:7090/api/Orders/Add", orderAddRequest);
        Guid orderId = orderAddRequest.Id;
        //await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("New order generated"));
        await SendLogNotification("New order generated");

        new DriverManager().SetUpDriver(new ChromeConfig());
        IWebDriver driver = new ChromeDriver();
        Console.Clear();

        //Order event bot started oluşturulup signalR'a mesaj olarak geçildi.
        var orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId= orderId,
            Status=OrderStatus.BotStarted,
        };

        var orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);

        //SignalR ile verileri hub'a gönderme
        //await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(orderEventAddRequest.Status.ToString()));
        await SendLogNotification(orderEventAddRequest.Status.ToString());




        driver.Navigate().GoToUrl("https://4teker.net/");
        ReadOnlyCollection<IWebElement> pagination = driver.FindElements(By.ClassName("page-item"));

        int productCounter = 0; //ürün sayısı tutucu


        orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId= orderId,
            Status=OrderStatus.CrawlingStarted,
        };

        orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);
        await SendLogNotification(orderEventAddRequest.Status.ToString());

        for (int i = 1; i<pagination.Count; i++)
        {
            driver.Navigate().GoToUrl($"https://4teker.net/?currentPage={i}");
            Console.WriteLine($"{i}.Sayfa");
            Console.WriteLine();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            ReadOnlyCollection<IWebElement> productCard = driver.FindElements(By.CssSelector(".col.mb-5"));
            //Console.WriteLine(productCard.Count);
            int howMany = 0;


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

                    if (isDiscounted && (keyInfo.Key==ConsoleKey.D1 || keyInfo.Key==ConsoleKey.NumPad1 || keyInfo.Key== ConsoleKey.D2 || keyInfo.Key==ConsoleKey.NumPad2) && productCounter<requestedAmount)
                    {
                        productName = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/h5")).Text;
                        discountedPrice =decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[2]")).Text.Remove(0, 1));
                        originalPrice = decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[1]")).Text.Remove(0, 1));
                        Console.WriteLine("Ürün Adı: " + productName);
                        Console.WriteLine("İndirimli mi?: " + isDiscounted);
                        Console.WriteLine("İndirimli Fiyat: " + discountedPrice);
                        Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                        Console.WriteLine("Ürün Resmi URL'si: " + productImageURL);
                        Console.WriteLine("Ürün OrderId'si " + orderAddRequest.Id.ToString());
                        Console.WriteLine("----------------------------");
                        var productAddRequest = new ProductAddCommand()
                        {

                            OrderId =orderAddRequest.Id,
                            Name = productName,
                            Picture=productImageURL,
                            IsOnSale = true,
                            Price=originalPrice,
                            SalePrice=discountedPrice,

                        };

                        var productAddResponse = await SendHttpPostRequest<ProductAddCommand, object>(httpClient, "https://localhost:7090/api/Products/Add", productAddRequest);
                        productCounter++;
                        howMany++;
                    }
                    else if (!isDiscounted && (keyInfo.Key==ConsoleKey.D1 || keyInfo.Key==ConsoleKey.NumPad1 || keyInfo.Key==ConsoleKey.D3 || keyInfo.Key==ConsoleKey.NumPad3) && productCounter<requestedAmount)
                    {
                        productName = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[1]/div/h5")).Text;
                        originalPrice =decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[1]/div/span")).Text.Remove(0, 1));
                        Console.WriteLine("Ürün Adı: " + productName);
                        Console.WriteLine("İndirimli mi?: " + isDiscounted);
                        Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                        Console.WriteLine("Ürün Resmi URL'si: " + productImageURL);
                        Console.WriteLine("----------------------------");

                        var productAddRequest = new ProductAddCommand()
                        {
                            // ProductAddCommand nesnesinin diğer özellikleri
                            OrderId =orderAddRequest.Id,
                            Name = productName,
                            IsOnSale = false,
                            Price=originalPrice,
                            SalePrice=discountedPrice,
                            Picture=productImageURL,

                        };

                        var productAddResponse = await SendHttpPostRequest<ProductAddCommand, object>(httpClient, "https://localhost:7090/api/Products/Add", productAddRequest);
                        productCounter++;
                        howMany++;
                    }





                }
                Console.WriteLine($"{productCard.Count} element found. {howMany} of them is scrapped");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                orderEventAddRequest = new OrderEventAddCommand()
                {
                    OrderId= orderId,
                    Status=OrderStatus.CrawlingFailed,
                };
                orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);

                await SendLogNotification(orderEventAddRequest.Status.ToString());
            }

            if (requestedAmount==productCounter)
            {
                break;
            }

        }
        Console.WriteLine($"Found product: {productCounter}");
        driver.Dispose();
        Console.Clear();
        orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId= orderId,
            Status=OrderStatus.CrawlingCompleted,
        };
        orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);

        await SendLogNotification(orderEventAddRequest.Status.ToString());


        orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId= orderId,
            Status=OrderStatus.OrderCompleted,
        };
        orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);
        await SendLogNotification(orderEventAddRequest.Status.ToString());

        Console.WriteLine("Do you want to continue scrapping? (y/n)");
        ConsoleKeyInfo ContinueScrapping = Console.ReadKey();
        //if (ContinueScrapping.Key==ConsoleKey.Y)
        //{
        //    driver.Dispose();
        //    Console.WriteLine();
        //    Console.WriteLine();


        //}
        if (ContinueScrapping.Key==ConsoleKey.N)
        {
            orderEventAddRequest = new OrderEventAddCommand()
            {
                OrderId= orderId,
                Status=OrderStatus.BotClosed,
            };
            orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);
            await SendLogNotification(orderEventAddRequest.Status.ToString());
            driver.Dispose();
            httpClient.Dispose();
            Continue=true;

        }


    }


    async Task<TResponse> SendHttpPostRequest<TRequest, TResponse>(HttpClient httpClient, string url, TRequest payload)
    {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, httpContent);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
        //Console.WriteLine($"Response: {responseObject}");

        return responseObject;
    }

    UserLogDto CreateLog(string message) => new UserLogDto(message);

    async Task SendLogNotification(string logMessage)
    {
        // 'CreateLog' metodu burada kullanılarak bir günlük oluşturulabilir
        var log = CreateLog(logMessage);

        // HubConnection oluşturulmalı ve başlatılmalı
        var hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7090/Hubs/UserLogHub") // Hub URL'sini burada belirtmelisiniz
            .WithAutomaticReconnect()
            .Build();

        try
        {
            await hubConnection.StartAsync(); // HubConnection'ı başlatma
            await hubConnection.InvokeAsync("SendLogNotificationAsync", log); // Metodu çağırma
        }
        finally
        {
            await hubConnection.DisposeAsync(); // HubConnection'ı kapatma ve kaynakları temizleme
        }
    }
}



