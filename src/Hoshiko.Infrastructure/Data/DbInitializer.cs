using Hoshiko.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Hoshiko.Infrastructure.Helpers;
using Hoshiko.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Hoshiko.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            await context.Database.EnsureCreatedAsync();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminUserName = "modabernia";
            var adminPassword = "Modabernia@123!";

            var admin = await userManager.FindByNameAsync(adminUserName);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = adminUserName,
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Modabernia"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            var stages = new List<Stage>
            {
                new Stage
                {
                    Name = "مرحله ۱: هوش مصنوعی چیست؟",
                    Slug = SlugHelper.GenerateSlug("مرحله ۱: هوش مصنوعی چیست؟"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "هوش مصنوعی به زبان ساده", Slug = SlugHelper.GenerateSlug("هوش مصنوعی به زبان ساده"), Content = "هوش مصنوعی یعنی کامپیوترها بتوانند فکر کنند و یاد بگیرند." },
                        new Learn { Title = "مثال‌های ساده", Slug = SlugHelper.GenerateSlug("مثال‌های ساده"), Content = "وقتی گوشی قفلش با چهره باز می‌شود، از هوش مصنوعی استفاده می‌کند." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "هوش مصنوعی یعنی چه؟", OptionA = "کامپیوتر باهوش", OptionB = "تلویزیون", OptionC = "مداد", OptionD = "کتاب", CorrectOption = "A" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۲: کامپیوترها چطور دستور می‌گیرند؟",
                    Slug = SlugHelper.GenerateSlug("مرحله ۲: کامپیوترها چطور دستور می‌گیرند؟"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "دستور چیست؟", Slug = SlugHelper.GenerateSlug("دستور چیست؟"), Content = "کامپیوتر فقط کاری را انجام می‌دهد که به آن بگوییم." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "کامپیوتر چطور تصمیم می‌گیرد؟", OptionA = "با احساس", OptionB = "با دستور", OptionC = "با خواب", OptionD = "با شانس", CorrectOption = "B" },
                        new Quiz { Question = "اگر بگوییم بپر، چه می‌شود؟", OptionA = "هیچ", OptionB = "دستور اجرا می‌شود", OptionC = "خاموش می‌شود", OptionD = "می‌خوابد", CorrectOption = "B" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۳: یادگیری ماشین",
                    Slug = SlugHelper.GenerateSlug("مرحله ۳: یادگیری ماشین"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "یاد گرفتن با مثال", Slug = SlugHelper.GenerateSlug("یاد گرفتن با مثال"), Content = "ماشین‌ها با دیدن مثال‌های زیاد یاد می‌گیرند." },
                        new Learn { Title = "تمرین زیاد", Slug = SlugHelper.GenerateSlug("تمرین زیاد"), Content = "هرچه تمرین بیشتر باشد، نتیجه بهتر می‌شود." },
                        new Learn { Title = "مثل بچه‌ها", Slug = SlugHelper.GenerateSlug("مثل بچه‌ها"), Content = "بچه‌ها هم با تمرین یاد می‌گیرند." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "ماشین‌ها چگونه یاد می‌گیرند؟", OptionA = "با مثال", OptionB = "با خواب", OptionC = "با ترس", OptionD = "با شانس", CorrectOption = "A" },
                        new Quiz { Question = "تمرین چه کمکی می‌کند؟", OptionA = "هیچ", OptionB = "یادگیری بهتر", OptionC = "خرابی", OptionD = "کند شدن", CorrectOption = "B" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۴: تشخیص تصویر",
                    Slug = SlugHelper.GenerateSlug("مرحله ۴: تشخیص تصویر"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "دیدن عکس‌ها", Slug = SlugHelper.GenerateSlug("دیدن عکس‌ها"), Content = "کامپیوتر می‌تواند عکس‌ها را بشناسد." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "تشخیص تصویر یعنی چه؟", OptionA = "شنیدن صدا", OptionB = "دیدن و شناخت عکس", OptionC = "نوشتن", OptionD = "دویدن", CorrectOption = "B" },
                        new Quiz { Question = "کدام مثال درست است؟", OptionA = "شناخت چهره", OptionB = "نوشتن", OptionC = "دوچرخه", OptionD = "کفش", CorrectOption = "A" },
                        new Quiz { Question = "گوشی چطور چهره را می‌شناسد؟", OptionA = "هوش مصنوعی", OptionB = "جادو", OptionC = "شانس", OptionD = "باتری", CorrectOption = "A" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۵: تشخیص صدا",
                    Slug = SlugHelper.GenerateSlug("مرحله ۵: تشخیص صدا"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "شنیدن صدا", Slug = SlugHelper.GenerateSlug("شنیدن صدا"), Content = "هوش مصنوعی می‌تواند صدای انسان را بفهمد." },
                        new Learn { Title = "مثال صوتی", Slug = SlugHelper.GenerateSlug("مثال صوتی"), Content = "دستیار صوتی نمونه‌ای از تشخیص صداست." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "کدام از صدا استفاده می‌کند؟", OptionA = "دستیار صوتی", OptionB = "مداد", OptionC = "کتاب", OptionD = "کفش", CorrectOption = "A" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۶: ربات‌ها",
                    Slug = SlugHelper.GenerateSlug("مرحله ۶: ربات‌ها"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "ربات چیست؟", Slug = SlugHelper.GenerateSlug("ربات چیست؟"), Content = "ربات‌ها ماشین‌هایی هستند که کار انجام می‌دهند." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "ربات‌ها چه کاری می‌کنند؟", OptionA = "کمک", OptionB = "بازی", OptionC = "هیچ", OptionD = "خرابی", CorrectOption = "A" },
                        new Quiz { Question = "ربات‌ها با چه چیزی کار می‌کنند؟", OptionA = "هوش مصنوعی", OptionB = "خواب", OptionC = "ترس", OptionD = "مداد", CorrectOption = "A" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۷: بازی‌های هوشمند",
                    Slug = SlugHelper.GenerateSlug("مرحله ۷: بازی‌های هوشمند"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "بازی با فکر", Slug = SlugHelper.GenerateSlug("بازی با فکر"), Content = "بعضی بازی‌ها با هوش مصنوعی ساخته شده‌اند." },
                        new Learn { Title = "دشمن باهوش", Slug = SlugHelper.GenerateSlug("دشمن باهوش"), Content = "دشمنان بازی می‌توانند فکر کنند." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "دشمن بازی چگونه حرکت می‌کند؟", OptionA = "تصادفی", OptionB = "با هوش مصنوعی", OptionC = "خاموش", OptionD = "با شانس", CorrectOption = "B" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۸: کمک در زندگی",
                    Slug = SlugHelper.GenerateSlug("مرحله ۸: کمک در زندگی"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "کمک در خانه", Slug = SlugHelper.GenerateSlug("کمک در خانه"), Content = "هوش مصنوعی می‌تواند به ما کمک کند." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "کدام کمک روزانه است؟", OptionA = "پیشنهاد فیلم", OptionB = "دویدن", OptionC = "خواب", OptionD = "نقاشی", CorrectOption = "A" },
                        new Quiz { Question = "هوش مصنوعی کجا کمک می‌کند؟", OptionA = "خانه", OptionB = "مدرسه", OptionC = "هر دو", OptionD = "هیچ‌جا", CorrectOption = "C" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۹: مراقب باشیم",
                    Slug = SlugHelper.GenerateSlug("مرحله ۹: مراقب باشیم"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "استفاده درست", Slug = SlugHelper.GenerateSlug("استفاده درست"), Content = "باید از هوش مصنوعی درست استفاده کنیم." },
                        new Learn { Title = "ایمنی", Slug = SlugHelper.GenerateSlug("ایمنی"), Content = "اطلاعات شخصی را نباید به همه بدهیم." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "چه کاری درست است؟", OptionA = "مراقبت", OptionB = "بی‌دقتی", OptionC = "نادیده گرفتن", OptionD = "ترسیدن", CorrectOption = "A" }
                    }
                },
                new Stage
                {
                    Name = "مرحله ۱۰: آینده هوش مصنوعی",
                    Slug = SlugHelper.GenerateSlug("مرحله ۱۰: آینده هوش مصنوعی"),
                    Learns = new List<Learn>
                    {
                        new Learn { Title = "آینده خوب", Slug = SlugHelper.GenerateSlug("آینده خوب"), Content = "هوش مصنوعی می‌تواند دنیا را بهتر کند." }
                    },
                    Quizzes = new List<Quiz>
                    {
                        new Quiz { Question = "آینده هوش مصنوعی چگونه است؟", OptionA = "کمک‌کننده", OptionB = "بد", OptionC = "بی‌فایده", OptionD = "ترسناک", CorrectOption = "A" },
                        new Quiz { Question = "هوش مصنوعی چه کمکی می‌کند؟", OptionA = "زندگی بهتر", OptionB = "هیچ", OptionC = "خرابی", OptionD = "خاموشی", CorrectOption = "A" },
                        new Quiz { Question = "آیا می‌توانیم از آن درست استفاده کنیم؟", OptionA = "بله", OptionB = "نه", OptionC = "نمی‌دانم", OptionD = "شاید", CorrectOption = "A" }
                    }
                }
            };

            if (!context.Stages.Any())
            {
                await context.Stages.AddRangeAsync(stages);
                await context.SaveChangesAsync();
            }
        }
    }
}