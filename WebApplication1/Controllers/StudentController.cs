using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private const string FilePath = "students.json";

        // ��ȡ����ѧ��
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            var students = ReadStudentsFromFile();
            return Ok(students);
        }

        

        // ���ѧ��
        [HttpPost]
        public ActionResult<Student> AddStudent(Student student)
        {
            var students = ReadStudentsFromFile();

            // �Զ�����ID
            student.ID = students.Count > 0 ? students.Max(s => s.ID) + 1 : 1;
            students.Add(student);

            WriteStudentsToFile(students);
            return CreatedAtAction(nameof(GetStudent), new { id = student.ID }, student);
        }
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var students = ReadStudentsFromFile();
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound($"ѧ�� ID {id} �����ڡ�");
            }
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student updatedStudent)
        {
            var students = ReadStudentsFromFile();
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound($"�޷����£�ѧ�� ID {id} �����ڡ�");
            }

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Class = updatedStudent.Class;

            WriteStudentsToFile(students);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var students = ReadStudentsFromFile();
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound($"�޷�ɾ����ѧ�� ID {id} �����ڡ�");
            }

            students.Remove(student);
            WriteStudentsToFile(students);
            return NoContent();
        }




        // ���ļ���ȡѧ������
        private List<Student> ReadStudentsFromFile()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return new List<Student>();
            }

            var json = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
        }

        // ��ѧ������д���ļ�
        private void WriteStudentsToFile(List<Student> students)
        {
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(FilePath, json);
        }
    }
}
