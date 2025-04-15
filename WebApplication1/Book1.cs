using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ApiClient
{
    // ��������ͬ��Book��
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
            Console.WriteLine("WebAPI �ͻ��˲���");
            
            // �滻Ϊ���WebAPI��ַ
            string baseUrl = "http://localhost:�˿ں�/api/Book";
            
            // 1. GET ���� - ��ȡ����ͼ��
            Console.WriteLine("\n��ȡ����ͼ��:");
            GetAllBooks(baseUrl);
            
            // 2. POST ���� - �����ͼ��
            Console.WriteLine("\n�����ͼ��:");
            Book1 newBook = new Book1 { Name = "WebRequestʵս", Price = 45.5 };
            AddBook(baseUrl, newBook);
            
            // �ٴλ�ȡ����ͼ�飬�鿴����ӵ�ͼ��
            Console.WriteLine("\n��Ӻ������ͼ��:");
            GetAllBooks(baseUrl);
            
            // 3. PUT ���� - ����ͼ��
            Console.WriteLine("\n����ͼ��(ID=1):");
            Book1 updatedBook = new Book1 { ID = 1, Name = "ASP.NET Core�߼��̳�", Price = 99.9 };
            UpdateBook(baseUrl, updatedBook);
            
            // 4. GET ���� - ��ȡ����ͼ��
            Console.WriteLine("\n��ȡ���º��ͼ��(ID=1):");
            GetBookById(baseUrl, 1);
            
            // 5. DELETE ���� - ɾ��ͼ��
            Console.WriteLine("\nɾ��ͼ��(ID=2):");
            DeleteBook(baseUrl, 2);
            
            // �ٴλ�ȡ����ͼ�飬ȷ��ɾ��
            Console.WriteLine("\nɾ���������ͼ��:");
            GetAllBooks(baseUrl);
            
            Console.WriteLine("\n��������˳�...");
            Console.ReadKey();
        }

        // GET���� - ��ȡ����ͼ��
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
                    
                    // ���Է����л�JSON������
                    // Book[] books = JsonSerializer.Deserialize<Book[]>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"��ȡͼ��ʧ��: {ex.Message}");
            }
        }

        // GET���� - ��ȡ����ͼ��
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
                Console.WriteLine($"��ȡͼ��ʧ��: {ex.Message}");
            }
        }

        // POST���� - �����ͼ��
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
                    Console.WriteLine($"��ӳɹ�: {responseJson}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"���ͼ��ʧ��: {ex.Message}");
            }
        }

        // PUT���� - ����ͼ��
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
                    Console.WriteLine($"���³ɹ� (״̬��: {(int)response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"����ͼ��ʧ��: {ex.Message}");
            }
        }

        // DELETE���� - ɾ��ͼ��
        static void DeleteBook(string baseUrl, int id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{baseUrl}/{id}");
                request.Method = "DELETE";
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"ɾ���ɹ� (״̬��: {(int)response.StatusCode})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ɾ��ͼ��ʧ��: {ex.Message}");
            }
        }
    }
}
