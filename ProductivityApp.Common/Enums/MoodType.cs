using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Common.Enums
{
    public enum MoodType
    {
        [Display(Name = "😊 Happy")]
        Happy,

        [Display(Name = "😐 Neutral")]
        Neutral,

        [Display(Name = "🙁 Sad")]
        Sad,

        [Display(Name = "😣 Stressed")]
        Stressed,

        [Display(Name = "🤩 Excited")]
        Excited
    }
}
