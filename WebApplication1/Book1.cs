using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ApiClient
{
    // 与服务端相同的Book类
    public class Book1
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WebAPI 客户端测试");
            
            // 替换为你的WebAPI地址
            string baseUrl = "http://localhost:端口号/api/Book";
            
            // 1. GET 请求 - 获取所有图书
            Console.WriteLine("\n获取所有图书:");
            GetAllBooks(baseUrl);
            
            // 2. POST 请求 - 添加新图书
            Console.WriteLine("\n添加新图书:");
            Book1 newBook = new Book1 { Name = "WebRequest实战", Price = 45.5 };
            AddBook(baseUrl, newBook);
            
            // 再次获取所有图书，查看新添加的图书
            Console.WriteLine("\n添加后的所有图书:");
            GetAllBooks(baseUrl);
            
            // 3. PUT 请求 - 更新图书
            Console.WriteLine("\n更新图书(ID=1):");
            Book1 updatedBook = new Book1 { ID = 1, Name = "ASP.NET Core高级教程", Price = 99.9 };
            UpdateBook(baseUrl, updatedBook);
            
            // 4. GET 请求 - 获取单本图书
            Console.WriteLine("\n获取更新后的图书(ID=1):");
            GetBookById(baseUrl, 1);
            
            // 5. DELETE 请求 - 删除图书
            Console.WriteLine("\n删除图书(ID=2):");
            DeleteBook(baseUrl, 2);
            
            // 再次获取所有图书，确认删除
            Console.WriteLine("\n删除后的所有图书:");
            GetAllBooks(baseUrl);
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }

        // GET请求 - 获取所有图书
        static void GetAllBooks(string baseUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                request.Method = "GET";
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    Console.WriteLine(json);
                    
                    // 可以反序列化JSON并处理
                    // Book[] books = JsonSerializer.Deserialize<Book[]>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取图书失败: {ex.Message}");
            }
        }

        // GET请求 - 获取单本图书
        static void GetBookById(string baseUrl, int id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{baseUrl}/{id}");
                request.Method = "GET";
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    Console.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取图书失败: {ex.Message}");
            }
        }

        // POST请求 - 添加新图书
        static void AddBook(string baseUrl, Book1 book)
        {
            try
            {
                string json = JsonSerializer.Serialize(book);
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
                {
                    writer.Write(json);
                }
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string responseJson = reader.ReadToEnd();
                    Console.WriteLine($"添加成功: {responseJson}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加图书失败: {ex.Message}");
            }
        }

        // PUT请求 - 更新图书
        static void UpdateBook(string baseUrl, Book1 book)
        {
            try
            {
                string json = JsonSerializer.Serialize(book);
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{baseUrl}/{book.ID}");
                request.Method = "PUT";
                request.ContentType = "application/json";
                
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
                {
                    writer.Write(json);
                }
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"更新成功 (状态码: {(int)response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新图书失败: {ex.Message}");
            }
        }

        // DELETE请求 - 删除图书
        static void DeleteBook(string baseUrl, int id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{baseUrl}/{id}");
                request.Method = "DELETE";
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"删除成功 (状态码: {(int)response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除图书失败: {ex.Message}");
            }
        }
    }
}
