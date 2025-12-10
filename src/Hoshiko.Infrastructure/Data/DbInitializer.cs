using Hoshiko.Domain.Entities;

namespace Hoshiko.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (context.Stages.Any()) return;

            var stages = new List<Stage>
            {
    
                new Stage
                {
                    Name = "مرحله ۱: آشنایی اولیه",

                    Learns = new List<Learn>
                    {
                        new Learn { Title = "درس ۱", Content = "در این مرحله شما با مفاهیم اولیه بازی آشنا می‌شوید." },
                        new Learn { Title = "درس ۲", Content = "برای موفقیت در مراحل بعدی باید مفاهیم پایه را کامل یاد بگیرید." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz
                        {
                            Question = "هدف مرحله اول چیست؟",
                            OptionA = "آشنایی با مفاهیم پایه",
                            OptionB = "یادگیری مباحث پیشرفته",
                            OptionC = "شروع مرحله پایانی",
                            OptionD = "هیچ‌کدام",
                            CorrectOption = "A"
                        },
                        new Quiz
                        {
                            Question = "چرا مرحله اول مهم است؟",
                            OptionA = "چون ساده است",
                            OptionB = "چون پایه مراحل بعدی است",
                            OptionC = "چون امتیاز ندارد",
                            OptionD = "چون بازی را پایان می‌دهد",
                            CorrectOption = "B"
                        }
                    }
                },

    
                new Stage
                {
                    Name = "مرحله ۲: یادگیری مفاهیم جدید",

                    Learns = new List<Learn>
                    {
                        new Learn { Title = "درس ۱", Content = "یادگیری تدریجی باعث تمرکز بهتر شما می‌شود." },
                        new Learn { Title = "درس ۲", Content = "نسبت به مرحله قبل این مرحله کمی سخت‌تر است." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz
                        {
                            Question = "یادگیری تدریجی چه مزیتی دارد؟",
                            OptionA = "کاهش تمرکز",
                            OptionB = "تمرکز بهتر",
                            OptionC = "هیچ مزیتی ندارد",
                            OptionD = "کاهش سرعت",
                            CorrectOption = "B"
                        },
                        new Quiz
                        {
                            Question = "مرحله دوم نسبت به مرحله اول چگونه است؟",
                            OptionA = "آسان‌تر",
                            OptionB = "یکسان",
                            OptionC = "کمی سخت‌تر",
                            OptionD = "غیرممکن",
                            CorrectOption = "C"
                        }
                    }
                },

    
                new Stage
                {
                    Name = "مرحله ۳: چالش‌های ابتدایی",

                    Learns = new List<Learn>
                    {
                        new Learn { Title = "درس ۱", Content = "چالش‌ها برای ارزیابی مهارت‌های شما طراحی شده‌اند." },
                        new Learn { Title = "درس ۲", Content = "در این مرحله باید دقت بیشتری داشته باشید." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz
                        {
                            Question = "هدف چالش‌ها چیست؟",
                            OptionA = "سخت کردن بازی",
                            OptionB = "ارزیابی مهارت‌های شما",
                            OptionC = "طولانی کردن مرحله",
                            OptionD = "کم کردن امتیاز",
                            CorrectOption = "B"
                        },
                        new Quiz
                        {
                            Question = "در مرحله سوم باید چه کاری انجام دهید؟",
                            OptionA = "صرفاً بازی کنید",
                            OptionB = "بدون دقت پاسخ دهید",
                            OptionC = "با دقت بیشتری عمل کنید",
                            OptionD = "مرحله را رد کنید",
                            CorrectOption = "C"
                        }
                    }
                }
            };

            await context.Stages.AddRangeAsync(stages);
            await context.SaveChangesAsync();
        }
    }
}