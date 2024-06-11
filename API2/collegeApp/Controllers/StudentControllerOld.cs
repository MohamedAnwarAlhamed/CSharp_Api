using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public string GetStudentName()
        {
            return "Student name 1";
        }

        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return List<Student>
                {
                new Student
                {
                    Id = 1,
                    StudentName = "Student 1",
                    Email = "studentemail1@gmail.com",
                    Address = "Hyd, INDIA"
                },
                new Student
                {
                    Id = 2,
                    StudentName = "Student 1",
                    Email = "studentemail1@gmail.com",
                    Address = "Hyd, INDIA"
                }
            };

        }
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return CollegeRepository.Students;
        }
        [HttpGet("{id:int}")]


        public Student GetStudentById(int id)
        {
            return CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
        }
        //=============================================================================
        //=============================================================================
        //=============================================================================
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]

        public IEnumerable<Student> GetStudents()
        {
            return CollegeRepository.Students;
        }
        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]

        public Student GetStudentById(int id)
        {
            return CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
        }
        [HttpGet("{name:alpha}", Name = "GetStudentByName")]

        public Student GetStudentByName(string name)
        {
            return CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();
        }
        [HttpDelete("{id}", Name = "DeleteStudentById")]
        public bool DeleteStudent(int id)
        {
            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault(); CollegeRepository.Students.Remove(student);
        }
        //=============================================================================
        //=============================================================================
        //=============================================================================
        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        // [ProducesResponseType(200, Type = typeof(Student))]
        // [Produces ResponseType(400)]
        // [Produces Response Type(404)]
        [ProducesResponseType(StatusCodes.Status2000K)]
        [ProducesResponseType(StatusCodes.Status400 Bad Request)]
        [ProducesResponseType(StatusCodes.Status404 Not Found)]
        [ProducesResponseType(StatusCodes.Status500Internal Server Error)]

        public ActionResult<Student> GetStudentById(int id)
        {
            //BadRequest 400 Badrequest Client error
            if (id <= 0)
                return BadRequest();
            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            //NotFound 404 NotFound Client error
            if (student == null) return NotFound($"The student with id {id} not found");
            //ок 200 Success 
            return Ok(student);
        }
        //=============================================================================
        //=============================================================================
        //=============================================================================
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            var students = new List<StudentDTO>();
            foreach (var item in CollegeRepository.Students)
            {
                StudentDTO obj = new StudentDTO()
                {
                    Id = item.Id,
                    StudentName = item.StudentName,
                    Address = item.Address,
                    Email = item.Email
                };
                students.Add(obj);
            }
            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Address = s.Address,
                Email = s.Email
            });
            //OK 200 Success 
            return Ok(students);
        }
        //=============================================================================
        //=============================================================================
        //=============================================================================
        [HttpPost]
        [Route("Create")]
        //api/student/create
        [Produces Response Type(StatusCodes.Status2000K)]
        [Produces Response Type(StatusCodes.Status400BadRequest)]
        [Produces Response Type(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model == null)
                return BadRequest();

            //if(model.AdmissionDate < DateTime.Now)
            //{
            //    //1. Directly adding error message to modelstate
            //    //2. Using custom attribute
            //    ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than or equal to todays date");
            //    return BadRequest(ModelState);
            //}     
            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email model.Email
            };
            CollegeRepository.Students.Add(student);
            model.Id = student.Id;
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
            //return Ok(model);
        }
        //===============================================================================
        //===============================================================================
        //===============================================================================
        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                BadRequest();

            var existingStudent = CollegeRepository.Students(s => s.Id == model.Id).FirstOrDefault;

            if (existingStudent == null)
                return NotFound();

            existingStudent.StudentName = model.StudentName;
            existingStudent.Email model.Email;
            existingStudent.Address = model.Address;

            return NoContent();


        }
        //===============================================================================
        //===============================================================================
        //===============================================================================
        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {

            if (patchDocument == null || id <= 0)
                BadRequest();

            var existingStudent = CollegeRepository.Students(s => s.Id == id).FirstOrDefault;

            if (existingStudent == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existing Student.Email,
                Address existingStudent.Address

            };

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;

            //204 - NoContent
            return NoContent();


        }
        //===============================================================================
        //===============================================================================
        //===============================================================================
        [HttpDelete("Delete/{id}", Name = "DeleteStudentById")]
        //api/student/delete/1
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(int id)
        {

            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students(s => s.Id == id).FirstOrDefault;
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with id {id} not found");

            CollegeRepository.Students.Remove(student);
            //OK - 200 - Success
            return Ok(true);
        }
        //===============================================================================
        //===============================================================================
        //===============================================================================
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;
        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext; // replace each CollegeRepository to _dbContext and method that save data to database
        }

        

    }
}

