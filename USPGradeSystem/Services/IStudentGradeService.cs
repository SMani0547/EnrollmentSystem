using System.Collections.Generic;
using System.Threading.Tasks;

namespace USPEducation.Services
{
    public interface IStudentGradeService
    {
        /// <summary>
        /// Sends grade data to an external student system.
        /// </summary>
        /// <param name="studentId">The student's unique ID.</param>
        /// <param name="courseId">The course's unique ID.</param>
        /// <param name="grade">The student's grade.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        Task<bool> SendGradeDataAsync(int studentId, int courseId, string grade);
        Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId);
    }
}
