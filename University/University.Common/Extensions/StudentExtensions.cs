namespace University.Common;

public static class StudentExtensions
{
  // Метод розширення
  public static void AddBonusPoints(this Student student, int points)
  {
    student.LastGrade += points;
  }
}