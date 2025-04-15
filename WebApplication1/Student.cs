using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApiClient
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Class { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:5000/api/Student");

            Console.WriteLine("1. ��ȡ����ѧ����Ϣ");
            await GetAllStudents();

            Console.WriteLine("\n2. �����ѧ��");
            await AddStudent(new Student { Name = "����", Age = 20, Class = "�������ѧ" });

            Console.WriteLine("\n3. ��ȡ����ѧ����Ϣ");
            await GetStudentById(1);

            Console.WriteLine("\n4. ����ѧ����Ϣ");
            await UpdateStudent(1, new Student { ID = 1, Name = "����", Age = 21, Class = "�������" });

            Console.WriteLine("\n5. ɾ��ѧ��");
            await DeleteStudent(1);

            Console.WriteLine("\n������ɣ�");
        }

        // ��ȡ����ѧ��
        private static async Task GetAllStudents()
        {
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var students = JsonSerializer.Deserialize<List<Student>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach (var student in students)
                {
                    Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Class: {student.Class}");
                }
            }
            else
            {
                Console.WriteLine($"��ȡѧ��ʧ��: {response.StatusCode}");
            }
        }

        // ��ȡ����ѧ��
        private static async Task GetStudentById(int id)
        {
            var response = await client.GetAsync($"/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var student = JsonSerializer.Deserialize<Student>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Class: {student.Class}");
            }
            else
            {
                Console.WriteLine($"��ȡѧ��ʧ��: {response.StatusCode}");
            }
        }

        // ���ѧ��
        private static async Task AddStudent(Student student)
        {
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("ѧ����ӳɹ���");
            }
            else
            {
                Console.WriteLine($"���ѧ��ʧ��: {response.StatusCode}");
            }
        }

        // ����ѧ��
        private static async Task UpdateStudent(int id, Student student)
        {
            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("ѧ�����³ɹ���");
            }
            else
            {
                Console.WriteLine($"����ѧ��ʧ��: {response.StatusCode}");
            }
        }

        // ɾ��ѧ��
        private static async Task DeleteStudent(int id)
        {
            var response = await client.DeleteAsync($"/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("ѧ��ɾ���ɹ���");
            }
            else
            {
                Console.WriteLine($"ɾ��ѧ��ʧ��: {response.StatusCode}");
            }
        }
    }
}
