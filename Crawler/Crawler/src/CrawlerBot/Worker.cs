using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using Application.Features.Orders.Commands.Add;
using Domain.Enums;
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.Products.Commands.Add;
using Application.Common.Models.SignalR;
using Newtonsoft.Json;
using System.Text;

namespace CrawlerBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _dataTransferHub = "https://localhost:7090/Hubs/DataTransferHub";
        private readonly HubConnection _DataTransferHubConnection;

        int customAmount;
        int selectedAmount;
        int requestedAmount;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            _DataTransferHubConnection = new HubConnectionBuilder()
                .WithUrl(_dataTransferHub)
                .WithAutomaticReconnect()
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // SignalR mesajlarýný bekleyin
            _DataTransferHubConnection.On<int, int>("ReceiveDataFromBlazor", (customAmount, selectedOption) =>
            {
                this.customAmount=customAmount;
                selectedAmount=selectedOption;
                Crawler();

            });

            await _DataTransferHubConnection.StartAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        async Task Crawler()
        {
            var orderAddRequest = new OrderAddCommand();
            using var httpClient = new HttpClient();
            requestedAmount=customAmount;

            if (selectedAmount == 0)
            {
                orderAddRequest = new OrderAddCommand()
                {
                    Id=Guid.NewGuid(),
                    //RequestedAmount=this.customAmount,
                    ProductCrawlType=ProductCrawlType.All
                };
            }
            if (selectedAmount == 1)
            {
                orderAddRequest = new OrderAddCommand()
                {
                    Id=Guid.NewGuid(),
                    ProductCrawlType=ProductCrawlType.OnDiscount
                };
            }

            if (selectedAmount == 2)
            {
                orderAddRequest = new OrderAddCommand()
                {
                    Id=Guid.NewGuid(),
                    ProductCrawlType=ProductCrawlType.NonDiscount
                };
            }

            var orderAddResponse = await SendHttpPostRequest<OrderAddCommand, object>(httpClient, "https://localhost:7090/api/Orders/Add", orderAddRequest);
            Guid orderId = orderAddRequest.Id;
            await SendLogNotification("New order generated");

            new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver driver = new ChromeDriver();
            Console.Clear();

            //Order event bot started oluþturulup signalR'a mesaj olarak geçildi.
            var orderEventAddRequest = new OrderEventAddCommand()
            {
                OrderId= orderId,
                Status=OrderStatus.BotStarted,
            };

            var orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, "https://localhost:7090/api/OrderEvents/Add", orderEventAddRequest);

            //SignalR ile verileri hub'a gönderme
            await SendLogNotification(orderEventAddRequest.Status.ToString());

            driver.Navigate().GoToUrl("https://4teker.net/");
            ReadOnlyCollection<IWebElement> pagination = driver.FindElements(By.ClassName("page-item"));

            int productCounter = 0; //ürün sayýsý tutucu


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

                        if (isDiscounted && (selectedAmount== 1 || selectedAmount== 0)  && productCounter<requestedAmount)
                        {
                            productName = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/h5")).Text;
                            discountedPrice =decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[2]")).Text.Remove(0, 1));
                            originalPrice = decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[2]/div/span[1]")).Text.Remove(0, 1));
                            Console.WriteLine("Ürün Adý: " + productName);
                            Console.WriteLine("Ýndirimli mi?: " + isDiscounted);
                            Console.WriteLine("Ýndirimli Fiyat: " + discountedPrice);
                            Console.WriteLine("Ýndirimsiz Fiyat: " + originalPrice);
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
                        else if (!isDiscounted && (selectedAmount== 2 || selectedAmount== 0) && productCounter<requestedAmount)
                        {
                            productName = driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[1]/div/h5")).Text;
                            originalPrice =decimal.Parse(driver.FindElement(By.XPath($"/html/body/section/div/div/div[{j}]/div/div[1]/div/span")).Text.Remove(0, 1));
                            Console.WriteLine("Ürün Adý: " + productName);
                            Console.WriteLine("Ýndirimli mi?: " + isDiscounted);
                            Console.WriteLine("Ýndirimsiz Fiyat: " + originalPrice);
                            Console.WriteLine("Ürün Resmi URL'si: " + productImageURL);
                            Console.WriteLine("----------------------------");

                            var productAddRequest = new ProductAddCommand()
                            {
                                // ProductAddCommand nesnesinin diðer özellikleri
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
            driver.Dispose();
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
            // 'CreateLog' metodu burada kullanýlarak bir günlük oluþturulabilir
            var log = CreateLog(logMessage);

            // HubConnection oluþturulmalý ve baþlatýlmalý
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7090/Hubs/UserLogHub") // Hub URL'sini burada belirtmelisiniz
                .WithAutomaticReconnect()
                .Build();

            try
            {
                await hubConnection.StartAsync(); // HubConnection'ý baþlatma
                await hubConnection.InvokeAsync("SendLogNotificationAsync", log); // Metodu çaðýrma
            }
            finally
            {
                await hubConnection.DisposeAsync(); // HubConnection'ý kapatma ve kaynaklarý temizleme
            }
        }
    }
}