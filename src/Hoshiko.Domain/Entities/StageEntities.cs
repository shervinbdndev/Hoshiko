namespace Hoshiko.Domain.Entities
{
    public class Stage
    {
        public int Id {get;set;}
        public string Name {get;set;} = null!;

        public ICollection<Quiz> Quizzes {get;set;} = new List<Quiz>();
        public ICollection<Learn> Learns {get;set;} = new List<Learn>();
    }


    public class Quiz
    {
        public int Id {get;set;}
        public string Question {get;set;} = null!;
        public string OptionA {get;set;} = null!;
        public string OptionB {get;set;} = null!;
        public string OptionC {get;set;} = null!;
        public string OptionD {get;set;} = null!;
        public string CorrectOption {get;set;} = null!;

        public int StageId {get;set;}
        public Stage Stage {get;set;} = null!;
    }


    public class Learn
    {
        public int Id {get;set;}
        public string Title {get;set;} = null!;
        public string Content {get;set;} = null!;

        public int StageId {get;set;}
        public Stage Stage {get;set;} = null!;
    }


    public class UserStageProgress
    {
        public int Id {get;set;}
        public string UserId {get;set;} = null!;
        public int StageId {get;set;}
        public Stage Stage {get;set;} = null!;

        public bool IsLearnCompleted {get;set;} = false;
        public bool IsQuizCompleted {get;set;} = false;


        public int RetryCount {get;set;} = 0;
        public DateTime? LastRetryAt {get;set;}      
        public DateTime StartedAt {get;set;} = DateTime.UtcNow;
        public DateTime? CompletedAt {get;set;}
    }


    public class UserQuizAnswer
    {
        public int Id {get;set;}
        public string UserId {get;set;} = null!;
        public int QuizId {get;set;}
        public Quiz Quiz {get;set;} = null!;
        public string SelectedOption {get;set;} = null!;
        public bool IsCorrect => SelectedOption == Quiz.CorrectOption;
        public DateTime AnsweredAt {get;set;} = DateTime.UtcNow;
    }



    public class Certificate
    {
        public int Id {get;set;}
        public string UserId {get;set;} = null!;
        public string UserName {get;set;} = null!;
        public string FullName {get;set;} = null!;
        public DateTime IssuedAt {get;set;} = DateTime.UtcNow;
        public string CertificateCode {get;set;} = null!;
        public bool IsRevoked {get;set;}
    }
}